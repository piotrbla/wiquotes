namespace Exchange_charts
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.diagramsListBox = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(138, 12);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(434, 338);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // diagramsListBox
            // 
            this.diagramsListBox.BackColor = System.Drawing.SystemColors.Control;
            this.diagramsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.diagramsListBox.FormattingEnabled = true;
            this.diagramsListBox.Items.AddRange(new object[] {
            "diagram0",
            "diagram1",
            "diagram2",
            "diagram3"});
            this.diagramsListBox.Location = new System.Drawing.Point(12, 12);
            this.diagramsListBox.Name = "diagramsListBox";
            this.diagramsListBox.Size = new System.Drawing.Size(120, 180);
            this.diagramsListBox.TabIndex = 1;
            this.diagramsListBox.SelectedIndexChanged += new System.EventHandler(this.diagramsListBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.diagramsListBox);
            this.Controls.Add(this.chart);
            this.Name = "Form1";
            this.Text = "Exchange Charts";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.CheckedListBox diagramsListBox;
    }
}

