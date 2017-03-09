using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace wiquotes
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.Text = "WI Quotes Version: " + assembly.GetName().Version.ToString();
        }
    }
}
