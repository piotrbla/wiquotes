using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wiquotes
{
    public partial class PreferencesForm : Form
    {
        public PreferencesForm()
        {
            InitializeComponent();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            TabPage chart = newTab("Kwoty");
            chart.Controls.Add(newLabel("Kwota 1: ", 100, 50, 50, 20));
            chart.Controls.Add(newSpinBox(50, 200, 200, 20));
            chart.Controls.Add(newSpinBox(50, 200, 260, 20));
            chart.Controls.Add(newLabel(",", 20, 20, 250, 20));

            chart.Controls.Add(newLabel("Kwota 2: ", 100, 50, 50, 120));
            chart.Controls.Add(newSpinBox(50, 200, 200, 120));
            chart.Controls.Add(newSpinBox(50, 200, 260, 120));
            chart.Controls.Add(newLabel(",", 20, 20, 250, 120));

            chart.Controls.Add(newLabel("Odsetki: ", 100, 50, 50, 220));
            chart.Controls.Add(newSpinBox(50, 200, 200, 220));
            chart.Controls.Add(newSpinBox(50, 200, 260, 220));
            chart.Controls.Add(newLabel(",", 20, 20, 250, 220));
        }

        private NumericUpDown newSpinBox(int sizex, int sizey, int locationx, int locationy)
        {
            NumericUpDown spin = new NumericUpDown();
            spin.Size = new Size(sizex, sizey);
            spin.Location = new Point(locationx, locationy);
            return spin;
        }
        private Label newLabel(string name, int sizex, int sizey, int locationx, int locationy)
        {
            Label lab = new Label();
            lab.Text = name;
            lab.Size = new Size(sizex, sizey);
            lab.Font = new Font(lab.Font.FontFamily, lab.Font.Size + 1.0f, lab.Font.Style);
            lab.Location = new Point(locationx, locationy);

            return lab;
        }
        private TabPage newTab(string name)
        {
            TabPage page = new TabPage();
            page.Text = name;
           tabControl1.TabPages.Add(page);
            return page;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}
