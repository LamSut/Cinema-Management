﻿namespace Cinema
{
    partial class AdminShowsForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddShow = new System.Windows.Forms.Button();
            this.btnDeleteShow = new System.Windows.Forms.Button();
            this.dataGridViewShows = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dateTimePickerShowTime = new System.Windows.Forms.DateTimePicker();
            this.comboBoxMovieID = new System.Windows.Forms.ComboBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtShowID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(456, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "List of shows";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAddShow
            // 
            this.btnAddShow.Location = new System.Drawing.Point(81, 443);
            this.btnAddShow.Name = "btnAddShow";
            this.btnAddShow.Size = new System.Drawing.Size(87, 34);
            this.btnAddShow.TabIndex = 2;
            this.btnAddShow.Text = "Add";
            this.btnAddShow.UseVisualStyleBackColor = true;
            this.btnAddShow.Click += new System.EventHandler(this.btnAddFilm_Click);
            // 
            // btnDeleteShow
            // 
            this.btnDeleteShow.Location = new System.Drawing.Point(860, 147);
            this.btnDeleteShow.Name = "btnDeleteShow";
            this.btnDeleteShow.Size = new System.Drawing.Size(87, 34);
            this.btnDeleteShow.TabIndex = 4;
            this.btnDeleteShow.Text = "Delete";
            this.btnDeleteShow.UseVisualStyleBackColor = true;
            this.btnDeleteShow.Click += new System.EventHandler(this.btnDeleteFilm_Click);
            // 
            // dataGridViewShows
            // 
            this.dataGridViewShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShows.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewShows.Location = new System.Drawing.Point(338, 200);
            this.dataGridViewShows.Name = "dataGridViewShows";
            this.dataGridViewShows.RowHeadersWidth = 62;
            this.dataGridViewShows.RowTemplate.Height = 28;
            this.dataGridViewShows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewShows.ShowRowErrors = false;
            this.dataGridViewShows.Size = new System.Drawing.Size(609, 277);
            this.dataGridViewShows.TabIndex = 5;
            this.dataGridViewShows.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellContentClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dateTimePickerShowTime
            // 
            this.dateTimePickerShowTime.Location = new System.Drawing.Point(23, 326);
            this.dateTimePickerShowTime.Name = "dateTimePickerShowTime";
            this.dateTimePickerShowTime.Size = new System.Drawing.Size(219, 26);
            this.dateTimePickerShowTime.TabIndex = 7;
            // 
            // comboBoxMovieID
            // 
            this.comboBoxMovieID.DisplayMember = "movieID";
            this.comboBoxMovieID.FormattingEnabled = true;
            this.comboBoxMovieID.Location = new System.Drawing.Point(23, 260);
            this.comboBoxMovieID.Name = "comboBoxMovieID";
            this.comboBoxMovieID.Size = new System.Drawing.Size(219, 28);
            this.comboBoxMovieID.TabIndex = 8;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(23, 388);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(219, 26);
            this.txtPrice.TabIndex = 9;
            this.txtPrice.Text = "Please input new price";
            this.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtShowID
            // 
            this.txtShowID.Location = new System.Drawing.Point(23, 200);
            this.txtShowID.Name = "txtShowID";
            this.txtShowID.ReadOnly = true;
            this.txtShowID.Size = new System.Drawing.Size(219, 26);
            this.txtShowID.TabIndex = 10;
            // 
            // AdminShowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 654);
            this.Controls.Add(this.txtShowID);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.comboBoxMovieID);
            this.Controls.Add(this.dateTimePickerShowTime);
            this.Controls.Add(this.dataGridViewShows);
            this.Controls.Add(this.btnDeleteShow);
            this.Controls.Add(this.btnAddShow);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminShowsForm";
            this.Text = "AdminShowsForm";
            this.Load += new System.EventHandler(this.AdminShowsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddShow;
        private System.Windows.Forms.Button btnDeleteShow;
        private System.Windows.Forms.DataGridView dataGridViewShows;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DateTimePicker dateTimePickerShowTime;
        private System.Windows.Forms.ComboBox comboBoxMovieID;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtShowID;
    }
}