using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Net;
using System.IO;
using System.Linq;
using RemoteZip;


namespace wiquotes
{
    public partial class UpdaterForm : Form
    {
        //public ProgressBar Bar = new ProgressBar();


        public void ReturnProgress()
        {
            progressBar1.PerformStep();
        }

        public UpdaterForm()
        {
            InitializeComponent();
        }


        CreateZipFile NewFile;

        public void add_list_button_Click(object sender, EventArgs e)
        {
            NewFile = new CreateZipFile();
            list_http.Items.Clear();
            url_bar.Text = "http://bossa.pl/pub/futures/omega/omegafut.zip";


            if (url_bar.Text == String.Empty)
                MessageBox.Show("PODAJ ADRES");
            else
            {

                foreach (var Zip in NewFile.LoadFile(returnURL()))
                {
                    list_http.Items.Add(Zip);
                }
            }
        }

        public string returnURL()
        {
            return url_bar.Text;
        }

        private void add_item_Click(object sender, EventArgs e)
        {
            string select = list_http.SelectedItem.ToString();
            added_list.Items.Add(select);
        }

        private void delete_item_Click(object sender, EventArgs e)
        {
            var selected = added_list.SelectedItem;
            added_list.Items.Remove(selected);
        }

        private void extract_button_Click(object sender, EventArgs e)
        {
            List<int> Indexy = new List<int>();
            foreach (string File in added_list.Items)
            {
                if (NewFile.zipedFileList.Contains(File))
                {
                    NewFile.TotalLinesAllFile(NewFile.zipedFileList.IndexOf(File));
                    Indexy.Add(NewFile.zipedFileList.IndexOf(File));
                }
            }
            progressBar1.Maximum = NewFile.TotalCount;

            foreach (var Licznik in Indexy)
            {
                NewFile.SaveFile(Licznik);
            }
        }


    }

    public class InfoAboutZip
    {
        public string Name;
        public long Index;
        public string ZipFileName;

        public InfoAboutZip()
        {
        }
    }

    public class CreateZipFile : UpdaterForm
    {
        public int TotalCount = 0;
        public CreateZipFile()
        {
            
            
        }
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            public RemoteZipFile zip = new RemoteZipFile();
            public List<string> zipedFileList = new List<string>();
    
    

    public List<string> LoadFile(string Url)
        {
            if (zip.Load(Url))
            {
                
                foreach (ZipEntry zipe in zip)
                {
                    zipedFileList.Add(zipe.Name);
                    InfoAboutZip ZipInfo = new InfoAboutZip();
                    ZipInfo.Name = zipe.Name;
                    ZipInfo.ZipFileName = Url;
                    ZipInfo.Index = zipe.ZipFileIndex;
                }
            }
            return zipedFileList;
        }
        public List<string> ReturnListFile()
        {
            return zipedFileList;
        }
        public void SaveFile(int IndexInZip)
        {
            
            ZipEntry zipe = zip[IndexInZip];
            string txtTmp = "";
            string key = "";
            string filePath = System.Windows.Forms.Application.StartupPath + "../../../data/";
            int i = 1;
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = filePath + zipe.Name;
            List<decimal> columnValues = new List<decimal>();
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            Stream s = zip.GetInputStream(zipe);
            if (s != null)
            {
               
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(fs);
                
                sw.AutoFlush = true;
                string txt;
                while ((txt = sr.ReadLine()) != null)
                {
                    txtTmp = txtTmp + txt + "\r\n";
                    sb.AppendLine(txt);
                    ReturnProgress();
                    
                    
                }
                sw.Write(txtTmp);
                sw.Close();
                s.Close();
            }
            
        }

    public int TotalLines(StreamReader r)
        {
            int i = 0;
            while (r.ReadLine() != null) { i++; }
            return i; 
        }

    public void TotalLinesAllFile(int Index)
        {
                ZipEntry zipe = zip[Index];
                Stream s = zip.GetInputStream(zipe);
                StreamReader sr = new StreamReader(s);
                TotalCount += TotalLines(sr);
        }
    
     }
}