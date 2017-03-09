using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace wiquotes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            this.Text = "WI Quotes Version: " + assembly.GetName().Version.ToString();
        }
    }
}
