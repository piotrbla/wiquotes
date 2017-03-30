using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Net;
using System.IO;
using System.Linq;

namespace wiquotes
{
    public partial class UpdaterForm : Form
    {
        public UpdaterForm()
        {
            InitializeComponent();
        }

        public void add_list_button_Click(object sender, EventArgs e)
        {

            var Url = "http://bossa.pl/pub/futures/omega/omegafut.zip";
            list_ftp.Items.Clear();
            if (url_bar.Text == String.Empty)
                MessageBox.Show("PODAJ ADRES");
            else
            {
                WebClient WebClient = new WebClient();
                Stream Data = WebClient.OpenRead(url_bar.Text);
                
                foreach (var Temp in ObjectFromLibrary.FromStream(Data))
                { 
                    list_ftp.Items.Add(Temp.Name);
                }

            }
        }
        
        private void add_item_Click(object sender, EventArgs e)
        {
            string select = list_ftp.SelectedItem.ToString();
            added_list.Items.Add(select);
        }

        private void delete_item_Click(object sender, EventArgs e)
        {
            var selected = added_list.SelectedItem;
            added_list.Items.Remove(selected);
        }

        private void extract_button_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            Stream data = webClient.OpenRead(url_bar.Text);
            
            foreach (string file in added_list.Items)
            {
              ObjectFromLibrary.FromStream(url_bar.Text, file);
            }
        }
    }

    public class ObjectFromLibrary
    {
        public string Name;
        public int Size;

        public ObjectFromLibrary(string Name, int Size)
        {
            this.Name = Name;
            this.Size = Size;
        }

        static public List<ZipEntry> FromStream(Stream zipStream)
        {
            List<ZipEntry> entries = new List<ZipEntry>();
            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            while (zipEntry != null)
            {
                entries.Add(zipEntry);
                zipEntry = zipInputStream.GetNextEntry();
            }
            return entries;
        }

       static public void FromStream(string url, string file1)
        {

            using (var client = new System.Net.Http.HttpClient())
            using (var stream = client.GetStreamAsync(url).Result) // tutaj podaje adres 
            {
                //var path = Application.StartupPath; // Path.GetTempPath()
                var basepath = Path.Combine(Application.StartupPath);
                System.IO.Directory.CreateDirectory(basepath);

                var ar = new System.IO.Compression.ZipArchive(stream, System.IO.Compression.ZipArchiveMode.Read);
                var entry = ar.Entries.FirstOrDefault(e => e.FullName.EndsWith(file1)); // jakiego pliku szuka
                if (entry == null)
                    return;

                var path = Path.Combine(basepath, entry.FullName);

                if (string.IsNullOrEmpty(entry.Name))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
                     
                }

                using (var entryStream = entry.Open())
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (var file = File.Create(path))
                    {
                        entryStream.CopyTo(file);
                    }
                }
            }
        }
    }
}
