﻿namespace wiquotes
{
    partial class UpdaterForm
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
            this.url_bar = new System.Windows.Forms.TextBox();
            this.list_http = new System.Windows.Forms.ListBox();
            this.added_list = new System.Windows.Forms.ListBox();
            this.add_list_button = new System.Windows.Forms.Button();
            this.add_item = new System.Windows.Forms.Button();
            this.delete_item = new System.Windows.Forms.Button();
            this.extract_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // url_bar
            // 
            this.url_bar.Location = new System.Drawing.Point(12, 45);
            this.url_bar.Name = "url_bar";
            this.url_bar.Size = new System.Drawing.Size(246, 20);
            this.url_bar.TabIndex = 0;
            // 
            // list_http
            // 
            this.list_http.FormattingEnabled = true;
            this.list_http.Location = new System.Drawing.Point(12, 110);
            this.list_http.Name = "list_http";
            this.list_http.Size = new System.Drawing.Size(120, 95);
            this.list_http.TabIndex = 1;
            // 
            // added_list
            // 
            this.added_list.FormattingEnabled = true;
            this.added_list.Location = new System.Drawing.Point(138, 110);
            this.added_list.Name = "added_list";
            this.added_list.Size = new System.Drawing.Size(120, 95);
            this.added_list.TabIndex = 2;
            // 
            // add_list_button
            // 
            this.add_list_button.Location = new System.Drawing.Point(12, 6);
            this.add_list_button.Name = "add_list_button";
            this.add_list_button.Size = new System.Drawing.Size(120, 33);
            this.add_list_button.TabIndex = 3;
            this.add_list_button.Text = "button1";
            this.add_list_button.UseVisualStyleBackColor = true;
            this.add_list_button.Click += new System.EventHandler(this.add_list_button_Click);
            // 
            // add_item
            // 
            this.add_item.Location = new System.Drawing.Point(12, 211);
            this.add_item.Name = "add_item";
            this.add_item.Size = new System.Drawing.Size(120, 36);
            this.add_item.TabIndex = 4;
            this.add_item.Text = "add_item";
            this.add_item.UseVisualStyleBackColor = true;
            this.add_item.Click += new System.EventHandler(this.add_item_Click);
            // 
            // delete_item
            // 
            this.delete_item.Location = new System.Drawing.Point(138, 211);
            this.delete_item.Name = "delete_item";
            this.delete_item.Size = new System.Drawing.Size(120, 36);
            this.delete_item.TabIndex = 5;
            this.delete_item.Text = "delete_item";
            this.delete_item.UseVisualStyleBackColor = true;
            this.delete_item.Click += new System.EventHandler(this.delete_item_Click);
            // 
            // extract_button
            // 
            this.extract_button.Location = new System.Drawing.Point(12, 253);
            this.extract_button.Name = "extract_button";
            this.extract_button.Size = new System.Drawing.Size(75, 23);
            this.extract_button.TabIndex = 6;
            this.extract_button.Text = "extract";
            this.extract_button.UseVisualStyleBackColor = true;
            this.extract_button.Click += new System.EventHandler(this.extract_button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 71);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(246, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.extract_button);
            this.Controls.Add(this.delete_item);
            this.Controls.Add(this.add_item);
            this.Controls.Add(this.add_list_button);
            this.Controls.Add(this.added_list);
            this.Controls.Add(this.list_http);
            this.Controls.Add(this.url_bar);
            this.Name = "UpdaterForm";
            this.Text = "UpdaterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox url_bar;
        private System.Windows.Forms.ListBox list_http;
        private System.Windows.Forms.ListBox added_list;
        private System.Windows.Forms.Button add_list_button;
        private System.Windows.Forms.Button add_item;
        private System.Windows.Forms.Button delete_item;
        private System.Windows.Forms.Button extract_button;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}