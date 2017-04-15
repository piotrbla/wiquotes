using System;
using System.Net;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.BZip2;
using System.Collections;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Checksums;

namespace RemoteZip
{
    /// <summary>
    /// Summary description for ZipDownloader.
    /// </summary>
    public class RemoteZipFile : IEnumerable
    {
        ZipEntry[] entries;
        string baseUrl;
        int MaxFileOffset;

        public RemoteZipFile()
        {
        }

        /*
		end of central dir signature  	4 bytes (0x06054b50)
		number of this disk 	2 bytes
		number of the disk with the start of the central directory 	2 bytes
		total number of entries in the central directory on this disk 	2 bytes
		total number of entries in the central directory 	2 bytes
		size of the central directory 	4 bytes
		offset of start of central directory
		with respect to the starting disk number 	4 bytes
		.ZIP file comment length 	2 bytes
		.ZIP file comment 	(variable size)
		 */

        /// <summary>
        /// TODO: case when the whole file is smaller than 64K
        /// TODO: handle non HTTP case
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Load(string url)
        {
            int CentralOffset, CentralSize;
            int TotalEntries;
            if (!FindCentralDirectory(url, out CentralOffset, out CentralSize, out TotalEntries))
                return false;

            MaxFileOffset = CentralOffset;

            // now retrieve the Central Directory
            baseUrl = url;
            entries = new ZipEntry[TotalEntries];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AddRange(CentralOffset, CentralOffset + CentralSize);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream s = res.GetResponseStream();
            try
            {
                // code taken from SharpZipLib with modification for not seekable stream
                // and adjustement for Central Directory entry
                for (int i = 0; i < TotalEntries; i++)
                {
                    if (ReadLeInt(s) != ZipConstants.CENSIG)
                    {
                        throw new ZipException("Wrong Central Directory signature");
                    }

                    // skip 6 bytes: version made (W), version ext (W), flags (W)
                    ReadLeInt(s);
                    ReadLeShort(s);
                    int method = ReadLeShort(s);
                    int dostime = ReadLeInt(s);
                    int crc = ReadLeInt(s);
                    int csize = ReadLeInt(s);
                    int size = ReadLeInt(s);
                    int nameLen = ReadLeShort(s);
                    int extraLen = ReadLeShort(s);
                    int commentLen = ReadLeShort(s);
                    // skip 8 bytes: disk number start, internal file attribs, external file attribs (DW)
                    ReadLeInt(s);
                    ReadLeInt(s);
                    int offset = ReadLeInt(s);

                    byte[] buffer = new byte[Math.Max(nameLen, commentLen)];

                    ReadAll(buffer, 0, nameLen, s);
                    string name = ZipConstants.ConvertToString(buffer);

                    ZipEntry entry = new ZipEntry(name);
                    entry.CompressionMethod = (CompressionMethod)method;
                    entry.Crc = crc & 0xffffffffL;
                    entry.Size = size & 0xffffffffL;
                    entry.CompressedSize = csize & 0xffffffffL;
                    entry.DosTime = (uint)dostime;
                    if (extraLen > 0)
                    {
                        byte[] extra = new byte[extraLen];
                        ReadAll(extra, 0, extraLen, s);
                        entry.ExtraData = extra;
                    }
                    if (commentLen > 0)
                    {
                        ReadAll(buffer, 0, commentLen, s);
                        entry.Comment = ZipConstants.ConvertToString(buffer);
                    }
                    entry.ZipFileIndex = i;
                    entry.Offset = offset;
                    entries[i] = entry;
                    OnProgress((i * 100) / TotalEntries);
                }
            }
            finally
            {
                s.Close();
                res.Close();
            }
            OnProgress(100);


            return true;
        }

        /// <summary>
        /// OnProgress during Central Header loading
        /// </summary>
        /// <param name="pct"></param>
        public virtual void OnProgress(int pct)
        {

        }

        /// <summary>
        /// Checks if there is a local header at the current position in the stream and skips it
        /// </summary>
        /// <param name="baseStream"></param>
        /// <param name="entry"></param>
        void SkipLocalHeader(Stream baseStream, ZipEntry entry)
        {
            lock (baseStream)
            {
                if (ReadLeInt(baseStream) != ZipConstants.LOCSIG)
                {
                    throw new ZipException("Wrong Local header signature");
                }

                Skip(baseStream, 10 + 12);
                int namelen = ReadLeShort(baseStream);
                int extralen = ReadLeShort(baseStream);
                Skip(baseStream, namelen + extralen);
            }
        }

        /// <summary>
        /// Finds the Central Header in the Zip file. We can minimize the number of requests and
        /// the bytes taken
        /// 
        /// Actually we do: 256, 1024, 65536
        /// </summary>
        /// <param name="baseurl"></param>
        /// <returns></returns>
        bool FindCentralDirectory(string url, out int Offset, out int Size, out int Entries)
        {
            int currentLength = 256;
            Entries = 0;
            Size = 0;
            Offset = -1;

            while (true)
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.AddRange(-(currentLength + 22));
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                // copy the buffer because we need a back seekable buffer
                byte[] bb = new byte[res.ContentLength];
                int endSize = ReadAll(bb, 0, (int)res.ContentLength, res.GetResponseStream());
                res.Close();

                // scan for the central block. The position of the central block
                // is end-comment-22
                //<
                // 50 4B 05 06
                int pos = endSize - 22;
                int state = 0;
                while (pos >= 0)
                {
                    if (bb[pos] == 0x50)
                    {
                        if (bb[pos + 1] == 0x4b && bb[pos + 2] == 0x05 && bb[pos + 3] == 0x06)
                            break; // found!!
                        pos -= 4;
                    }
                    else
                        pos--;
                }

                if (pos < 0)
                {
                    if (currentLength == 65536)
                        break;

                    if (currentLength == 1024)
                        currentLength = 65536;
                    else if (currentLength == 256)
                        currentLength = 1024;
                    else
                        break;
                }
                else
                {
                    // found it!! so at offset pos+3*4 there is Size, and pos+4*4
                    // BinaryReader is so elegant but now it's too much
                    Size = MakeInt(bb, pos + 12);
                    Offset = MakeInt(bb, pos + 16);
                    Entries = MakeShort(bb, pos + 10);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get a Stream for reading the specified entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public Stream GetInputStream(ZipEntry entry)
        {
            if (entry.Size == 0)
                return null;

            if (entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }
            
            long index = entry.ZipFileIndex;
            if (index < 0 || index >= entries.Length || entries[index].Name != entry.Name)
            {
                throw new IndexOutOfRangeException();
            }

            // WARNING
            // should parse the Local Header to get the data address
            // Maximum Size of the Local Header is ... 16+64K*2
            //
            // So the HTTP request should ask for the big local header, but actually the
            // additional data is not downloaded.
            // Optionally use an additional Request to be really precise
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(baseUrl);

            int limit = (int)(entry.Offset + entry.CompressedSize + 16 + 65536 * 2);
            if (limit >= MaxFileOffset)
                limit = MaxFileOffset - 1;
            req.AddRange((int)entry.Offset, limit);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream baseStream = res.GetResponseStream();

            // skips all the header
            SkipLocalHeader(baseStream, entries[index]);
            CompressionMethod method = entries[index].CompressionMethod;

            Stream istr = new PartialInputStream(baseStream, res, entries[index].CompressedSize);
            switch (method)
            {
                case CompressionMethod.Stored:
                    return istr;
                case CompressionMethod.Deflated:
                    return new InflaterInputStream(istr, new Inflater(true));
                case (CompressionMethod)12:
                    return new BZip2InputStream(istr);
                default:
                    throw new ZipException("Unknown compression method " + method);
            }
        }


        /// <summary>
        /// Read an unsigned short in little endian byte order.
        /// </summary>
        /// <exception name="System.IO.IOException">
        /// if a i/o error occured.
        /// </exception>
        /// <exception name="System.IO.EndOfStreamException">
        /// if the file ends prematurely
        /// </exception>
        int ReadLeShort(Stream s)
        {
            return s.ReadByte() | s.ReadByte() << 8;
        }

        /// <summary>
        /// Read an int in little endian byte order.
        /// </summary>
        /// <exception name="System.IO.IOException">
        /// if a i/o error occured.
        /// </exception>
        /// <exception name="System.IO.EndOfStreamException">
        /// if the file ends prematurely
        /// </exception>
        int ReadLeInt(Stream s)
        {
            return ReadLeShort(s) | ReadLeShort(s) << 16;
        }

        static void Skip(Stream s, int n)
        {
            for (int i = 0; i < n; i++)
                s.ReadByte();
        }

        static int ReadAll(byte[] bb, int p, int sst, Stream s)
        {
            int ss = 0;
            while (ss < sst)
            {
                int r = s.Read(bb, p, sst - ss);
                if (r <= 0)
                    return ss;
                ss += r;
                p += r;
            }
            return ss;
        }

        public static int MakeInt(byte[] bb, int pos)
        {
            return bb[pos + 0] | (bb[pos + 1] << 8) | (bb[pos + 2] << 16) | (bb[pos + 3] << 24);
        }

        public static int MakeShort(byte[] bb, int pos)
        {
            return bb[pos + 0] | (bb[pos + 1] << 8);
        }

        public int Size
        {
            get { return entries == null ? 0 : entries.Length; }
        }

        /// <summary>
        /// Returns an IEnumerator of all Zip entries in this Zip file.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            if (entries == null)
            {
                throw new InvalidOperationException("ZipFile has closed");
            }

            return new ZipEntryEnumeration(entries);
        }

        public ZipEntry this[int index]
        {
            get { return entries[index]; }
        }


        class ZipEntryEnumeration : IEnumerator
        {
            ZipEntry[] array;
            int ptr = -1;

            public ZipEntryEnumeration(ZipEntry[] arr)
            {
                array = arr;
            }

            public object Current
            {
                get
                {
                    return array[ptr];
                }
            }

            public void Reset()
            {
                ptr = -1;
            }

            public bool MoveNext()
            {
                return (++ptr < array.Length);
            }
        }

        class PartialInputStream : InflaterInputStream
        {
            Stream baseStream;
            long filepos;
            long end;
            HttpWebResponse request;



            public PartialInputStream(Stream baseStream, HttpWebResponse request, long len) : base(baseStream)
            {
                this.baseStream = baseStream;
                filepos = 0;
                end = len;
                this.request = request;
            }

            public override int Available
            {
                get
                {
                    long amount = end - filepos;
                    if (amount > Int32.MaxValue)
                    {
                        return Int32.MaxValue;
                    }

                    return (int)amount;
                }
            }

            public override int ReadByte()
            {
                if (filepos == end)
                {
                    return -1;
                }

                lock (baseStream)
                {
                    filepos++;
                    return baseStream.ReadByte();
                }
            }

            public override int Read(byte[] b, int off, int len)
            {
                if (len > end - filepos)
                {
                    len = (int)(end - filepos);
                    if (len == 0)
                    {
                        return 0;
                    }
                }
                lock (baseStream)
                {
                    int count = ReadAll(b, off, len, baseStream);
                    if (count > 0)
                    {
                        filepos += len;
                    }
                    return count;
                }
            }

            public long SkipBytes(long amount)
            {
                if (amount < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (amount > end - filepos)
                {
                    amount = end - filepos;
                }
                filepos += amount;
                for (int i = 0; i < amount; i++)
                    baseStream.ReadByte();
                return amount;
            }


            public override void Close()
            {
                request.Close();
                baseStream.Close();
            }
        }

    }


    /// <summary>
    /// This is a FilterOutputStream that writes the files into a zip
    /// archive one after another.  It has a special method to start a new
    /// zip entry.  The zip entries contains information about the file name
    /// size, compressed size, CRC, etc.
    /// 
    /// It includes support for STORED and DEFLATED and BZIP2 entries.
    /// This class is not thread safe.
    /// 
    /// author of the original java version : Jochen Hoenicke
    /// </summary>
    /// <example> This sample shows how to create a zip file
    /// <code>
    /// using System;
    /// using System.IO;
    /// 
    /// using NZlib.Zip;
    /// 
    /// class MainClass
    /// {
    /// 	public static void Main(string[] args)
    /// 	{
    /// 		string[] filenames = Directory.GetFiles(args[0]);
    /// 		
    /// 		ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));
    /// 		
    /// 		s.SetLevel(5); // 0 - store only to 9 - means best compression
    /// 		
    /// 		foreach (string file in filenames) {
    /// 			FileStream fs = File.OpenRead(file);
    /// 			
    /// 			byte[] buffer = new byte[fs.Length];
    /// 			fs.Read(buffer, 0, buffer.Length);
    /// 			
    /// 			ZipEntry entry = new ZipEntry(file);
    /// 			
    /// 			s.PutNextEntry(entry);
    /// 			
    /// 			s.Write(buffer, 0, buffer.Length);
    /// 			
    /// 		}
    /// 		
    /// 		s.Finish();
    /// 		s.Close();
    /// 	}
    /// }	
    /// </code>
    /// </example>
    public class ZipOutputStream : DeflaterOutputStream
    {
        private ArrayList entries = new ArrayList();
        private Crc32 crc = new Crc32();
        private ZipEntry curEntry = null;

        private long startPosition = 0;
        private Stream additionalStream = null;

        private CompressionMethod curMethod;
        private int size;
        private int offset = 0;

        private byte[] zipComment = new byte[0];
        private int defaultMethod = DEFLATED;

        /// <summary>
        /// Our Zip version is hard coded to 1.0 resp. 2.0
        /// </summary>
        private const int ZIP_STORED_VERSION = 10;
        private const int ZIP_DEFLATED_VERSION = 20;

        /// <summary>
        /// Compression method.  This method doesn't compress at all.
        /// </summary>
        public const int STORED = 0;

        /// <summary>
        /// Compression method.  This method uses the Deflater.
        /// </summary>
        public const int DEFLATED = 8;

        public const int BZIP2 = 12;

        /// <summary>
        /// Creates a new Zip output stream, writing a zip archive.
        /// </summary>
        /// <param name="baseOutputStream">
        /// the output stream to which the zip archive is written.
        /// </param>
        public ZipOutputStream(Stream baseOutputStream) : base(baseOutputStream, new Deflater(Deflater.DEFAULT_COMPRESSION, true))
        {
        }

        /// <summary>
        /// Set the zip file comment.
        /// </summary>
        /// <param name="comment">
        /// the comment.
        /// </param>
        /// <exception name ="ArgumentException">
        /// if UTF8 encoding of comment is longer than 0xffff bytes.
        /// </exception>
        public void SetComment(string comment)
        {
            byte[] commentBytes = ZipConstants.ConvertToArray(comment);
            if (commentBytes.Length > 0xffff)
            {
                throw new ArgumentException("Comment too long.");
            }
            zipComment = commentBytes;
        }
    }
}