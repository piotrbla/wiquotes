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

            chart.Controls.Add(newLabel("Kwota 2: ", 100, 50, 50, 120));
            chart.Controls.Add(newSpinBox(50, 200, 200, 120));

            chart.Controls.Add(newLabel("Odsetki: ", 100, 50, 50, 220));
            chart.Controls.Add(newSpinBox(50, 200, 200, 220));
            
            TabPage favs = newTab("Favourites");
            favs.Controls.Add(newLabel("Enter your favourite websites:", 200, 30, 50, 20));
            favs.Controls.Add(newTextBox(300, 30, 50, 60));
            Button but = newButton("Accept", 50, 30, 355, 55);
            favs.Controls.Add(but);
            but.Click += new EventHandler(buttonHandler);

            TabPage colors = newTab("Colors");
            colors.Controls.Add(newLabel("Graph: ", 100, 50, 50, 20));
            Button colorBut = (newButton("Change color", 50, 200, 200, 20));
            colors.Controls.Add(colorBut);

            colors.Controls.Add(newLabel("Mesh: ", 100, 50, 50, 120));
            colorBut = (newButton("Change color", 50, 200, 200, 20));
            colors.Controls.Add(colorBut);

            colors.Controls.Add(newLabel("Background: ", 100, 50, 50, 220));
            colorBut = newButton("Change color", 50, 200, 200, 220);
            colors.Controls.Add(colorBut);
            colorBut.Click += new EventHandler(colorButClicked);

            TabPage analysis = newTab("Analysis"); 
            //nazwa + kod analizyS


        }
        
        private void buttonHandler(object sender, EventArgs e)
        {           
            //zapisywanie adresow w liscie
        }
        private void colorButClicked(object sender, EventArgs e)
        {
            //zapisywanie koloru do bazy
        }

        private Button newButton(string name, int sizex, int sizey, int locationx, int locationy)
        {
            Button button = new Button();
            button.Text = name;
            button.Size = new Size(sizex, sizey);
            button.Location = new Point(locationx, locationy);

            return button;
        }

        private TextBox newTextBox(int sizex, int sizey, int locationx, int locationy)
        {
            TextBox box = new TextBox();
            box.Size = new Size(sizex, sizey);
            box.Location = new Point(locationx, locationy);

            return box;
        }

        private NumericUpDown newSpinBox(int sizex, int sizey, int locationx, int locationy)
        {
            NumericUpDown spin = new NumericUpDown();
            spin.Size = new Size(sizex, sizey);
            spin.Location = new Point(locationx, locationy);
            spin.DecimalPlaces = 2;
            spin.Increment = 0.1m;
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
