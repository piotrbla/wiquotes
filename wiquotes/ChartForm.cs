using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace wiquotes
{
    public partial class ChartForm : Form
    {
        private bool is_constructed = false;
        private List<DiagramData> TEST_CHART_DATA;
        private List<Diagram> diagrams;
        private int charts_quantity;

        public ChartForm()
        {
            InitializeComponent();
            diagrams = new List<Diagram>();
            TEST_CHART_DATA = new List<DiagramData>();
            is_constructed = true;
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            TEST_CHART_DATA.Add(new DiagramData("WIG20,19940414,1000.00,1000.00,1000.00,1000.00,35800.000"));
            TEST_CHART_DATA.Add(new DiagramData("WIG20,19940418,1050.50,1050.50,1050.50,1050.50,49975.000"));
            TEST_CHART_DATA.Add(new DiagramData("WIG20,19940419,1124.90,1124.90,1124.90,1124.90,69029.500"));
            TEST_CHART_DATA.Add(new DiagramData("WIG20,19940421,1304.80,1304.80,1304.80,1304.80,77075.500"));
            TEST_CHART_DATA.Add(new DiagramData("WIG20,19940425,1350.10,1350.10,1350.10,1350.10,114219.000"));
        }

        private void Add_chart_Click(object sender, EventArgs e)
        {
            diagrams.Add(new Diagram(TEST_CHART_DATA));
            this.Controls.Add(diagrams[diagrams.Count - 1].Dchart);
            resize_window(sender, e);
        }

        private void resize_window(object sender, EventArgs e)
        {
            if (is_constructed)
                resize_diagrams();
        }

        private void resize_diagrams()
        {
            int x_pos = 100;
            int x_end_pos = Size.Width - 28;     //Width of app window

            int start_y_pos = 12;
            int stop_y_pos = Size.Height - 50;   //Height of app window
            int area = stop_y_pos - start_y_pos;    //Window minimum size is defined

            int diagram_y_size = area / diagrams.Count;
            int diagram_x_size = x_end_pos - x_pos;

            int current_position = start_y_pos;
            foreach (Diagram D_i in diagrams)
            {
                var location = new Point(x_pos, current_position);
                D_i.Dchart.Location = location;

                var size = new Size(diagram_x_size, diagram_y_size);
                D_i.Dchart.Size = size;

                current_position += diagram_y_size;
            }

        }

    }
    class Diagram
    {
        private List<DiagramData> Ddata;
        public Chart Dchart;

        public Diagram(List<DiagramData> list)
        {
            Ddata = list;
            Dchart = new Chart();

            var are = "Area1";
            var ser = "Series1";
            var leg = "Legend1";

            Dchart.ChartAreas.Add(are);
            Dchart.Series.Add(ser);
            Dchart.Legends.Add(leg);

            Dchart.Series[ser].ChartArea = are;
            Dchart.Series[ser].Legend = leg;
            Dchart.Legends[leg].DockedToChartArea = are;
            Dchart.Legends[leg].IsDockedInsideChartArea = false;

            for (int i = 0; i < list.Count; i++)
            {
                Dchart.Series["Series1"].Points.AddXY(i, list[i].high);
            }
        }
    }

    class DiagramData
    {
        public string name;
        public string date;
        public double open;
        public double high;
        public double low;
        public double close;
        public double volume;

        //public double moving_average;

        public string all_data;

        public DiagramData(string line)
        {
            string[] tab = line.Split(new char[] { ',' });

            name = tab[0];
            date = tab[1];
            open = Double.Parse(tab[2], System.Globalization.CultureInfo.InvariantCulture);
            high = Double.Parse(tab[3], System.Globalization.CultureInfo.InvariantCulture);
            low = Double.Parse(tab[4], System.Globalization.CultureInfo.InvariantCulture);
            close = Double.Parse(tab[5], System.Globalization.CultureInfo.InvariantCulture);
            volume = Double.Parse(tab[6], System.Globalization.CultureInfo.InvariantCulture);

            all_data = line;
        }
    }
}
