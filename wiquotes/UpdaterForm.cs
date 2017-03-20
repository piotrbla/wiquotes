using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Net;
using System.IO;

namespace wiquotes
{
    public partial class UpdaterForm : Form
    {
        public UpdaterForm()
        {
            InitializeComponent();
        }

        private void add_list_button_Click(object sender, EventArgs e)
        {

                // chyba brakuje zabezpieczenia 
                string url = url_bar.Text;
                list_ftp.Items.Clear();
                if (url_bar.Text == String.Empty)
                {
                    MessageBox.Show("PODAJ ADRES");
                }
                else
                {
                    ObjectFromLibrary OBJ = new ObjectFromLibrary();
                    WebClient webClient = new WebClient();
                    Stream data = webClient.OpenRead(url);

                    foreach (var temp in OBJ.UnzipFromStream(data, @"c:\temp"))
                    {
                        list_ftp.Items.Add(temp);

                    }
                }
            }
        }
    }

    public class ObjectFromLibrary
    {
        public List<string> Name;
        public ObjectFromLibrary() { }

        public List<string> UnzipFromStream(Stream zipStream, string outFolder)
        {
            List<string> LIB = new List<string>();
            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            while (zipEntry != null)
            {
                //  String entryFileName = zipEntry.Name;
                LIB.Add(zipEntry.Name);
                zipEntry = zipInputStream.GetNextEntry();
        }
            return LIB;
        }
          
        }
        
