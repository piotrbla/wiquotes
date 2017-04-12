using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace wiquotes
{
    public partial class PreferencesForm : Form
    {

        Dictionary<string,TextBox> textboxes = new Dictionary<string,TextBox>();
        SQLiteConnection databaseConnection;
        Dictionary<string, decimal> vals = new Dictionary<string, decimal>();

        public object NavigationService { get; private set; }

        public PreferencesForm()
        {
            InitializeComponent();
            databaseConnection = new SQLiteConnection("Data Source=Preferences.sqlite;Version=3;");
            databaseConnection.Open();
            createTable("kwoty", "amount", "DOUBLE");
            createTable("colors", "color", "VARCHAR (30)");
            createTable("favourites", "fav", "VARCHAR (30)");

        }


        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            TabPage kwoty = newTab("Kwoty");
            newLabel(kwoty,"Kwota 1: ", 100, 50, 50, 20);
            NumericUpDown spin1 = newSpinBox(kwoty,50, 200, 200, 20);
            vals.Add("kwota1", spin1.Value);

            newLabel(kwoty, "Kwota 2: ", 100, 50, 50, 120);
            NumericUpDown spin2 = newSpinBox(kwoty,50, 200, 200, 120);
            vals.Add("kwota2", spin2.Value);

            newLabel(kwoty,"Odsetki: ", 100, 50, 50, 220);
            NumericUpDown spin3 = newSpinBox(kwoty,50, 200, 200, 220);
            vals.Add("odsetki", spin3.Value);

            Button butK = newButton(kwoty, "Accept", 50, 30, 355, 55);
            butK.Click += new EventHandler(butKHandler);
            
            TabPage favs = newTab("Favourites");
            newLabel(favs,"Enter your favourite websites:", 200, 30, 50, 20);
            TextBox txt = newTextBox(favs,300, 30, 50, 60);
            Button but = newButton(favs,"Accept", 50, 30, 355, 55);
            but.Click += new EventHandler(buttonHandler);

            TabPage colors = newTab("Colors");
            newLabel(colors,"Graph: ", 100, 50, 50, 20);
            Button graph = newButton(colors,"Change color", 150, 30, 200, 20);

            newLabel(colors,"Mesh: ", 100, 50, 50, 120);
            Button mesh = newButton(colors, "Change color", 150, 30, 200, 120);

            newLabel(colors,"Background: ", 100, 50, 50, 220);
            Button background = newButton(colors,"Change color", 150, 30, 200, 220);

            graph.Click += new EventHandler(colorButClicked);
            mesh.Click += new EventHandler(colorButClicked);
            background.Click += new EventHandler(colorButClicked);

            TabPage analysis = newTab("Analysis");
            //nazwa + kod analizyS

            calabazka();
        }
        private void calabazka()
        {
            string sql = "select * from kwoty";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                MessageBox.Show("Name: " + reader["name"] + "\tAmount: " + reader["amount"]);

            sql = "select * from colors";
            command = new SQLiteCommand(sql, databaseConnection);
            reader = command.ExecuteReader();
            while (reader.Read())
                MessageBox.Show("Name: " + reader["name"] + "\tColor: " + reader["color"]);


        }
        private void createTable(string table, string var2, string type)
        {
            string sql = "CREATE TABLE " + table + "(name VARCHAR(20), " + var2 + ' ' + type + " )";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }
        private void insertIntoTable(string table, string type1, string type2, string val1, string val2)
        {

            string sql = "insert into " + table + " (" + type1 + "," + type2 + ") values ('" + val1 + "'," + val2 + ")";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }
        private void insertIntoTableStr(string table, string type1, string type2, string val1, string val2)
        {
            
            string sql = "insert into " + table + " (" + type1 + "," + type2 + ") values ('" + val1 + "', '" + val2 + "' )";
            MessageBox.Show(sql);
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }
        private void buttonHandler(object sender, EventArgs e)
        {
          //  string txt = ((Button)sender).Text;
          //  textboxes.Add("FAV", txt);
        }

        private void butKHandler(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, decimal> kvp in vals)
            { insertIntoTable("kwoty", "name", "amount", Convert.ToString(kvp.Key), Convert.ToString(kvp.Value));

                MessageBox.Show(kvp.Key);
                MessageBox.Show(Convert.ToString(kvp.Value));
            }
        }

        private void colorButClicked(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            ColorDialog colorPicker = new ColorDialog();
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                if (colorPicker.Color == Color.FromName("Black"))
                    but.ForeColor = Color.FromName("White");
                but.BackColor = colorPicker.Color;
            }
            if (but != null)
            {
                string x = but.Name;
                insertIntoTableStr("colors", "VARCHAR(20)", "VARCHAR(20)", x, Convert.ToString(colorPicker.Color));
            }

          
        }

        private Button newButton(TabPage tab, string name, int sizex, int sizey, int locationx, int locationy)
        {
            Button button = new Button();
            button.Text = name;
            button.Size = new Size(sizex, sizey);
            button.Location = new Point(locationx, locationy);
            tab.Controls.Add(button);
            return button;
        }

        private TextBox newTextBox(TabPage tab, int sizex, int sizey, int locationx, int locationy)
        {
            TextBox box = new TextBox();
            box.Size = new Size(sizex, sizey);
            box.Location = new Point(locationx, locationy);
            tab.Controls.Add(box);

            return box;
        }

        private NumericUpDown newSpinBox(TabPage tab,int sizex, int sizey, int locationx, int locationy)
        {
            NumericUpDown spin = new NumericUpDown();
            spin.Size = new Size(sizex, sizey);
            spin.Location = new Point(locationx, locationy);
            spin.DecimalPlaces = 2;
            spin.Increment = 0.1m;
            tab.Controls.Add(spin);
            return spin;
        }

        private Label newLabel(TabPage tab, string name, int sizex, int sizey, int locationx, int locationy)
        {
            Label lab = new Label();
            lab.Text = name;
            lab.Size = new Size(sizex, sizey);
            lab.Font = new Font(lab.Font.FontFamily, lab.Font.Size + 1.0f, lab.Font.Style);
            lab.Location = new Point(locationx, locationy);
            tab.Controls.Add(lab);
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
