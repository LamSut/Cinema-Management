namespace Cinema.Views.Admin
{
    partial class ShowsForm
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
            this.comboBoxMovieTitle = new System.Windows.Forms.ComboBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtShowID = new System.Windows.Forms.TextBox();
            this.comboBoxTheaterName = new System.Windows.Forms.ComboBox();
            this.lblShowID = new System.Windows.Forms.Label();
            this.lblMovieTitle = new System.Windows.Forms.Label();
            this.lblTheaterName = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(371, 108);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "List of Showtimes";
            // 
            // btnAddShow
            // 
            this.btnAddShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.btnAddShow.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddShow.Location = new System.Drawing.Point(133, 489);
            this.btnAddShow.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddShow.Name = "btnAddShow";
            this.btnAddShow.Size = new System.Drawing.Size(194, 35);
            this.btnAddShow.TabIndex = 2;
            this.btnAddShow.Text = "Add New Show";
            this.btnAddShow.UseVisualStyleBackColor = false;
            // 
            // btnDeleteShow
            // 
            this.btnDeleteShow.BackColor = System.Drawing.Color.DarkRed;
            this.btnDeleteShow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteShow.ForeColor = System.Drawing.Color.White;
            this.btnDeleteShow.Location = new System.Drawing.Point(894, 99);
            this.btnDeleteShow.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteShow.Name = "btnDeleteShow";
            this.btnDeleteShow.Size = new System.Drawing.Size(195, 42);
            this.btnDeleteShow.TabIndex = 4;
            this.btnDeleteShow.Text = "Delete This Show";
            this.btnDeleteShow.UseVisualStyleBackColor = false;
            // 
            // dataGridViewShows
            // 
            this.dataGridViewShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShows.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewShows.Location = new System.Drawing.Point(377, 157);
            this.dataGridViewShows.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewShows.Name = "dataGridViewShows";
            this.dataGridViewShows.RowHeadersWidth = 62;
            this.dataGridViewShows.RowTemplate.Height = 28;
            this.dataGridViewShows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewShows.ShowRowErrors = false;
            this.dataGridViewShows.Size = new System.Drawing.Size(712, 367);
            this.dataGridViewShows.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dateTimePickerShowTime
            // 
            this.dateTimePickerShowTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerShowTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerShowTime.Location = new System.Drawing.Point(133, 334);
            this.dateTimePickerShowTime.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerShowTime.Name = "dateTimePickerShowTime";
            this.dateTimePickerShowTime.Size = new System.Drawing.Size(194, 29);
            this.dateTimePickerShowTime.TabIndex = 7;
            this.dateTimePickerShowTime.Value = new System.DateTime(2025, 3, 25, 20, 6, 51, 0);
            // 
            // comboBoxMovieTitle
            // 
            this.comboBoxMovieTitle.DisplayMember = "movieID";
            this.comboBoxMovieTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMovieTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMovieTitle.FormattingEnabled = true;
            this.comboBoxMovieTitle.Location = new System.Drawing.Point(133, 189);
            this.comboBoxMovieTitle.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMovieTitle.Name = "comboBoxMovieTitle";
            this.comboBoxMovieTitle.Size = new System.Drawing.Size(194, 29);
            this.comboBoxMovieTitle.TabIndex = 8;
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.Location = new System.Drawing.Point(133, 413);
            this.txtPrice.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(194, 29);
            this.txtPrice.TabIndex = 9;
            // 
            // txtShowID
            // 
            this.txtShowID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShowID.Location = new System.Drawing.Point(133, 115);
            this.txtShowID.Margin = new System.Windows.Forms.Padding(2);
            this.txtShowID.Name = "txtShowID";
            this.txtShowID.ReadOnly = true;
            this.txtShowID.Size = new System.Drawing.Size(194, 29);
            this.txtShowID.TabIndex = 10;
            // 
            // comboBoxTheaterName
            // 
            this.comboBoxTheaterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheaterName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.comboBoxTheaterName.FormattingEnabled = true;
            this.comboBoxTheaterName.Location = new System.Drawing.Point(133, 260);
            this.comboBoxTheaterName.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxTheaterName.Name = "comboBoxTheaterName";
            this.comboBoxTheaterName.Size = new System.Drawing.Size(194, 29);
            this.comboBoxTheaterName.TabIndex = 11;
            // 
            // lblShowID
            // 
            this.lblShowID.AutoSize = true;
            this.lblShowID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblShowID.Location = new System.Drawing.Point(13, 115);
            this.lblShowID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblShowID.Name = "lblShowID";
            this.lblShowID.Size = new System.Drawing.Size(91, 25);
            this.lblShowID.TabIndex = 14;
            this.lblShowID.Text = "Show ID:";
            // 
            // lblMovieTitle
            // 
            this.lblMovieTitle.AutoSize = true;
            this.lblMovieTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMovieTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblMovieTitle.Location = new System.Drawing.Point(13, 189);
            this.lblMovieTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMovieTitle.Name = "lblMovieTitle";
            this.lblMovieTitle.Size = new System.Drawing.Size(115, 25);
            this.lblMovieTitle.TabIndex = 15;
            this.lblMovieTitle.Text = "Movie Title:";
            // 
            // lblTheaterName
            // 
            this.lblTheaterName.AutoSize = true;
            this.lblTheaterName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTheaterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblTheaterName.Location = new System.Drawing.Point(11, 260);
            this.lblTheaterName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTheaterName.Name = "lblTheaterName";
            this.lblTheaterName.Size = new System.Drawing.Size(85, 25);
            this.lblTheaterName.TabIndex = 16;
            this.lblTheaterName.Text = "Theater:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDate.Location = new System.Drawing.Point(13, 334);
            this.lblDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(60, 25);
            this.lblDate.TabIndex = 17;
            this.lblDate.Text = "Time:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblPrice.Location = new System.Drawing.Point(13, 413);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(61, 25);
            this.lblPrice.TabIndex = 18;
            this.lblPrice.Text = "Price:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(453, 50);
            this.label2.TabIndex = 19;
            this.label2.Text = "Showtimes Management";
            // 
            // AdminShowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1100, 560);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTheaterName);
            this.Controls.Add(this.lblMovieTitle);
            this.Controls.Add(this.lblShowID);
            this.Controls.Add(this.comboBoxTheaterName);
            this.Controls.Add(this.txtShowID);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.comboBoxMovieTitle);
            this.Controls.Add(this.dateTimePickerShowTime);
            this.Controls.Add(this.dataGridViewShows);
            this.Controls.Add(this.btnDeleteShow);
            this.Controls.Add(this.btnAddShow);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdminShowsForm";
            this.Text = "AdminShowsForm";
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
        private System.Windows.Forms.ComboBox comboBoxMovieTitle;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtShowID;
        private System.Windows.Forms.ComboBox comboBoxTheaterName;
        private System.Windows.Forms.Label lblShowID;
        private System.Windows.Forms.Label lblMovieTitle;
        private System.Windows.Forms.Label lblTheaterName;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label label2;
    }
}