using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace Exchange_charts
{
    public partial class Form1 : Form
    {
        private List<DiagramData> charttest;
        public Form1()
        {
            InitializeComponent();
            charttest = new List<DiagramData>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            charttest.Add(new DiagramData("WIG20,19940414,1000.00,1000.00,1000.00,1000.00,35800.000"));
            charttest.Add(new DiagramData("WIG20,19940418,1050.50,1050.50,1050.50,1050.50,49975.000"));
            charttest.Add(new DiagramData("WIG20,19940419,1124.90,1124.90,1124.90,1124.90,69029.500"));
            charttest.Add(new DiagramData("WIG20,19940421,1304.80,1304.80,1304.80,1304.80,77075.500"));
            charttest.Add(new DiagramData("WIG20,19940425,1350.10,1350.10,1350.10,1350.10,114219.000"));
        }

        private void Display()
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            for (int i = 0; i < diagramsListBox.Items.Count; i++)
            {
                if (diagramsListBox.GetItemChecked(i))
                {
                    var are = "Area" + i.ToString();
                    var ser = "Serie" + i.ToString();
                    var leg = "Legend" + i.ToString();

                    chart.ChartAreas.Add(are);
                    chart.Series.Add(ser);
                    chart.Legends.Add(leg);

                    chart.Series[ser].ChartArea = are;
                    chart.Series[ser].Legend = leg;
                    chart.Legends[leg].DockedToChartArea = are;
                    chart.Legends[leg].IsDockedInsideChartArea = false;

                    for (int j = 0; j < charttest.Count; j++)
                    {
                        chart.Series["Serie" + i.ToString()].Points.AddXY(j, charttest[j].high);
                    }
                }
            }
        }

        private void diagramsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
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
