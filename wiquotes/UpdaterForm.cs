using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
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
                var url = url_bar.Text;
                list_ftp.Items.Clear();
                if (url_bar.Text == String.Empty)
                {
                    MessageBox.Show("PODAJ ADRES");
                }
                else
                {
                    ObjectFromLibrary zipProxy = new ObjectFromLibrary();
                    WebClient webClient = new WebClient();
                    Stream data = webClient.OpenRead(url);

                    foreach (var temp in zipProxy.UnzipFromStream(data, @"c:\temp"))
                    {
                        list_ftp.Items.Add(temp);
                    }
                }
            }

        private void add_item_Click(object sender, EventArgs e)
        {
            var select = list_ftp.SelectedItem;
            added_list.Items.Add(select);
        }

        private void delete_item_Click(object sender, EventArgs e)
        {
            var selected = added_list.SelectedItem;
            added_list.Items.Remove(selected);
        }
    }
    }

    public class ObjectFromLibrary
    {
        public List<string> Name;

        public List<string> UnzipFromStream(Stream zipStream, string outFolder)
        {
            List<string> entries = new List<string>();
            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();
            while (zipEntry != null)
            {
                //  String entryFileName = zipEntry.Name;
                entries.Add(zipEntry.Name);
                zipEntry = zipInputStream.GetNextEntry();
            }
            return entries;
        }
          
    }
        
