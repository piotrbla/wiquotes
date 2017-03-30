using System;
using System.Diagnostics;
using System.Windows.Forms;
using wiquotes.Properties;

namespace wiquotes
{
    public partial class MainWindow : Form
    {
        private readonly DatabaseManager database;
        public MainWindow()
        {
            InitializeComponent();
            DatabaseManager.InitFile();
            database = new DatabaseManager();
            database.CreatTables();
            
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.Text = Resources.MainWindow_MainWindow_Load_WI_Quotes_Version__ + assembly.GetName().Version.ToString();
        }

        private void chartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var chartWindow = new ChartForm {MdiParent = this};
            chartWindow.Show();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var updaterWindow = new UpdaterForm {MdiParent = this};
            updaterWindow.Show();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var preferencesWindow = new PreferencesForm {MdiParent = this};//TODO: pass database
            preferencesWindow.Show();
        }
    }
}
