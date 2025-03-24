namespace Cinema
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
            this.comboBoxTheaterID = new System.Windows.Forms.ComboBox();
            this.lblShowID = new System.Windows.Forms.Label();
            this.lblMovieID = new System.Windows.Forms.Label();
            this.lblTheaterID = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(376, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "List of Shows";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAddShow
            // 
            this.btnAddShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.btnAddShow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddShow.Location = new System.Drawing.Point(692, 551);
            this.btnAddShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddShow.Name = "btnAddShow";
            this.btnAddShow.Size = new System.Drawing.Size(196, 43);
            this.btnAddShow.TabIndex = 2;
            this.btnAddShow.Text = "Add New Show";
            this.btnAddShow.UseVisualStyleBackColor = false;
            this.btnAddShow.Click += new System.EventHandler(this.btnAddFilm_Click);
            // 
            // btnDeleteShow
            // 
            this.btnDeleteShow.BackColor = System.Drawing.Color.DarkRed;
            this.btnDeleteShow.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteShow.ForeColor = System.Drawing.Color.White;
            this.btnDeleteShow.Location = new System.Drawing.Point(899, 86);
            this.btnDeleteShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDeleteShow.Name = "btnDeleteShow";
            this.btnDeleteShow.Size = new System.Drawing.Size(260, 52);
            this.btnDeleteShow.TabIndex = 4;
            this.btnDeleteShow.Text = "Delete This Show";
            this.btnDeleteShow.UseVisualStyleBackColor = false;
            this.btnDeleteShow.Click += new System.EventHandler(this.btnDeleteFilm_Click);
            // 
            // dataGridViewShows
            // 
            this.dataGridViewShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShows.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewShows.Location = new System.Drawing.Point(364, 160);
            this.dataGridViewShows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewShows.Name = "dataGridViewShows";
            this.dataGridViewShows.RowHeadersWidth = 62;
            this.dataGridViewShows.RowTemplate.Height = 28;
            this.dataGridViewShows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewShows.ShowRowErrors = false;
            this.dataGridViewShows.Size = new System.Drawing.Size(794, 384);
            this.dataGridViewShows.TabIndex = 5;
            this.dataGridViewShows.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewShows_CellContentClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // dateTimePickerShowTime
            // 
            this.dateTimePickerShowTime.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerShowTime.Location = new System.Drawing.Point(20, 372);
            this.dateTimePickerShowTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimePickerShowTime.Name = "dateTimePickerShowTime";
            this.dateTimePickerShowTime.Size = new System.Drawing.Size(328, 29);
            this.dateTimePickerShowTime.TabIndex = 7;
            this.dateTimePickerShowTime.ValueChanged += new System.EventHandler(this.dateTimePickerShowTime_ValueChanged);
            // 
            // comboBoxMovieID
            // 
            this.comboBoxMovieID.DisplayMember = "movieID";
            this.comboBoxMovieID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMovieID.FormattingEnabled = true;
            this.comboBoxMovieID.Location = new System.Drawing.Point(20, 230);
            this.comboBoxMovieID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxMovieID.Name = "comboBoxMovieID";
            this.comboBoxMovieID.Size = new System.Drawing.Size(258, 36);
            this.comboBoxMovieID.TabIndex = 8;
            this.comboBoxMovieID.SelectedIndexChanged += new System.EventHandler(this.comboBoxMovieID_SelectedIndexChanged);
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrice.Location = new System.Drawing.Point(20, 438);
            this.txtPrice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(258, 34);
            this.txtPrice.TabIndex = 9;
            this.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPrice.TextChanged += new System.EventHandler(this.txtPrice_TextChanged);
            // 
            // txtShowID
            // 
            this.txtShowID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtShowID.Location = new System.Drawing.Point(20, 160);
            this.txtShowID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtShowID.Name = "txtShowID";
            this.txtShowID.ReadOnly = true;
            this.txtShowID.Size = new System.Drawing.Size(258, 34);
            this.txtShowID.TabIndex = 10;
            // 
            // comboBoxTheaterID
            // 
            this.comboBoxTheaterID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.comboBoxTheaterID.FormattingEnabled = true;
            this.comboBoxTheaterID.Location = new System.Drawing.Point(20, 301);
            this.comboBoxTheaterID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxTheaterID.Name = "comboBoxTheaterID";
            this.comboBoxTheaterID.Size = new System.Drawing.Size(258, 36);
            this.comboBoxTheaterID.TabIndex = 11;
            // 
            // lblShowID
            // 
            this.lblShowID.AutoSize = true;
            this.lblShowID.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblShowID.Location = new System.Drawing.Point(15, 130);
            this.lblShowID.Name = "lblShowID";
            this.lblShowID.Size = new System.Drawing.Size(95, 28);
            this.lblShowID.TabIndex = 14;
            this.lblShowID.Text = "Show ID:";
            // 
            // lblMovieID
            // 
            this.lblMovieID.AutoSize = true;
            this.lblMovieID.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMovieID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblMovieID.Location = new System.Drawing.Point(15, 199);
            this.lblMovieID.Name = "lblMovieID";
            this.lblMovieID.Size = new System.Drawing.Size(103, 28);
            this.lblMovieID.TabIndex = 15;
            this.lblMovieID.Text = "Movie ID:";
            // 
            // lblTheaterID
            // 
            this.lblTheaterID.AutoSize = true;
            this.lblTheaterID.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTheaterID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblTheaterID.Location = new System.Drawing.Point(15, 270);
            this.lblTheaterID.Name = "lblTheaterID";
            this.lblTheaterID.Size = new System.Drawing.Size(117, 28);
            this.lblTheaterID.TabIndex = 16;
            this.lblTheaterID.Text = "Theater ID:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDate.Location = new System.Drawing.Point(15, 340);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(117, 28);
            this.lblDate.TabIndex = 17;
            this.lblDate.Text = "Theater ID:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblPrice.Location = new System.Drawing.Point(15, 406);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(64, 28);
            this.lblPrice.TabIndex = 18;
            this.lblPrice.Text = "Price:";
            // 
            // AdminShowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1217, 624);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTheaterID);
            this.Controls.Add(this.lblMovieID);
            this.Controls.Add(this.lblShowID);
            this.Controls.Add(this.comboBoxTheaterID);
            this.Controls.Add(this.txtShowID);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.comboBoxMovieID);
            this.Controls.Add(this.dateTimePickerShowTime);
            this.Controls.Add(this.dataGridViewShows);
            this.Controls.Add(this.btnDeleteShow);
            this.Controls.Add(this.btnAddShow);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.ComboBox comboBoxTheaterID;
        private System.Windows.Forms.Label lblShowID;
        private System.Windows.Forms.Label lblMovieID;
        private System.Windows.Forms.Label lblTheaterID;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblPrice;
    }
}