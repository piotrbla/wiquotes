﻿namespace wiquotes
{
    partial class ChartForm
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
            this.Add_chart = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Add_chart
            // 
            this.Add_chart.Location = new System.Drawing.Point(18, 15);
            this.Add_chart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Add_chart.Name = "Add_chart";
            this.Add_chart.Size = new System.Drawing.Size(112, 35);
            this.Add_chart.TabIndex = 3;
            this.Add_chart.Text = "Dodaj";
            this.Add_chart.UseVisualStyleBackColor = true;
            this.Add_chart.Click += new System.EventHandler(this.Add_chart_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 60);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "Odejmij";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 557);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Add_chart);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(439, 278);
            this.Name = "ChartForm";
            this.Text = "ChartForm";
            this.Load += new System.EventHandler(this.ChartForm_Load);
            this.Resize += new System.EventHandler(this.resize_window);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Add_chart;
        private System.Windows.Forms.Button button2;
    }
}