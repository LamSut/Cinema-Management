namespace Cinema
{
    partial class AdminFilmsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnAddMovie = new System.Windows.Forms.Button();
            this.btnDeleteMovie = new System.Windows.Forms.Button();
            this.dataGridViewMovies = new System.Windows.Forms.DataGridView();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.numericDuration = new System.Windows.Forms.NumericUpDown();
            this.txtMovieID = new System.Windows.Forms.TextBox();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.txtDirector = new System.Windows.Forms.TextBox();
            this.txtPosterURL = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.dateTimePickerRelease = new System.Windows.Forms.DateTimePicker();
            this.lblMovieID = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblPosterURL = new System.Windows.Forms.Label();
            this.lblDirector = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMovies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(852, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = "List of Movies";
            // 
            // btnAddMovie
            // 
            this.btnAddMovie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.btnAddMovie.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddMovie.Location = new System.Drawing.Point(170, 757);
            this.btnAddMovie.Name = "btnAddMovie";
            this.btnAddMovie.Size = new System.Drawing.Size(288, 60);
            this.btnAddMovie.TabIndex = 1;
            this.btnAddMovie.Text = "Add New Movie";
            this.btnAddMovie.UseVisualStyleBackColor = false;
            this.btnAddMovie.Click += new System.EventHandler(this.btnAddFilm_Click);
            // 
            // btnDeleteMovie
            // 
            this.btnDeleteMovie.BackColor = System.Drawing.Color.DarkRed;
            this.btnDeleteMovie.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMovie.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMovie.Location = new System.Drawing.Point(1348, 105);
            this.btnDeleteMovie.Name = "btnDeleteMovie";
            this.btnDeleteMovie.Size = new System.Drawing.Size(275, 57);
            this.btnDeleteMovie.TabIndex = 3;
            this.btnDeleteMovie.Text = "Delete This Movie";
            this.btnDeleteMovie.UseVisualStyleBackColor = false;
            this.btnDeleteMovie.Click += new System.EventHandler(this.btnDeleteFilm_Click);
            // 
            // dataGridViewMovies
            // 
            this.dataGridViewMovies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMovies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewMovies.Location = new System.Drawing.Point(489, 192);
            this.dataGridViewMovies.Name = "dataGridViewMovies";
            this.dataGridViewMovies.RowHeadersWidth = 62;
            this.dataGridViewMovies.RowTemplate.Height = 28;
            this.dataGridViewMovies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMovies.ShowRowErrors = false;
            this.dataGridViewMovies.Size = new System.Drawing.Size(1134, 412);
            this.dataGridViewMovies.TabIndex = 4;
            this.dataGridViewMovies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtTitle.Location = new System.Drawing.Point(170, 149);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(286, 39);
            this.txtTitle.TabIndex = 5;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // numericDuration
            // 
            this.numericDuration.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.numericDuration.Location = new System.Drawing.Point(170, 222);
            this.numericDuration.Name = "numericDuration";
            this.numericDuration.Size = new System.Drawing.Size(141, 39);
            this.numericDuration.TabIndex = 6;
            this.numericDuration.ValueChanged += new System.EventHandler(this.numericDuration_ValueChanged);
            // 
            // txtMovieID
            // 
            this.txtMovieID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtMovieID.Location = new System.Drawing.Point(170, 78);
            this.txtMovieID.Name = "txtMovieID";
            this.txtMovieID.ReadOnly = true;
            this.txtMovieID.Size = new System.Drawing.Size(140, 39);
            this.txtMovieID.TabIndex = 7;
            this.txtMovieID.TextChanged += new System.EventHandler(this.txtMovieID_TextChanged);
            // 
            // txtGenre
            // 
            this.txtGenre.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtGenre.Location = new System.Drawing.Point(171, 525);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(284, 39);
            this.txtGenre.TabIndex = 8;
            this.txtGenre.TextChanged += new System.EventHandler(this.txtGenre_TextChanged);
            // 
            // txtDirector
            // 
            this.txtDirector.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtDirector.Location = new System.Drawing.Point(170, 374);
            this.txtDirector.Name = "txtDirector";
            this.txtDirector.Size = new System.Drawing.Size(286, 39);
            this.txtDirector.TabIndex = 9;
            this.txtDirector.TextChanged += new System.EventHandler(this.txtDirector_TextChanged);
            // 
            // txtPosterURL
            // 
            this.txtPosterURL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtPosterURL.Location = new System.Drawing.Point(170, 298);
            this.txtPosterURL.Name = "txtPosterURL";
            this.txtPosterURL.Size = new System.Drawing.Size(286, 39);
            this.txtPosterURL.TabIndex = 10;
            this.txtPosterURL.TextChanged += new System.EventHandler(this.txtPosterURL_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtDescription.Location = new System.Drawing.Point(170, 597);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(286, 130);
            this.txtDescription.TabIndex = 11;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // dateTimePickerRelease
            // 
            this.dateTimePickerRelease.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.dateTimePickerRelease.Location = new System.Drawing.Point(170, 449);
            this.dateTimePickerRelease.Name = "dateTimePickerRelease";
            this.dateTimePickerRelease.Size = new System.Drawing.Size(286, 39);
            this.dateTimePickerRelease.TabIndex = 12;
            this.dateTimePickerRelease.ValueChanged += new System.EventHandler(this.dateTimePickerRelease_ValueChanged);
            // 
            // lblMovieID
            // 
            this.lblMovieID.AutoSize = true;
            this.lblMovieID.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMovieID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblMovieID.Location = new System.Drawing.Point(16, 78);
            this.lblMovieID.Name = "lblMovieID";
            this.lblMovieID.Size = new System.Drawing.Size(47, 32);
            this.lblMovieID.TabIndex = 13;
            this.lblMovieID.Text = "ID:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 149);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(71, 32);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "Title:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDuration.Location = new System.Drawing.Point(15, 222);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(123, 32);
            this.lblDuration.TabIndex = 15;
            this.lblDuration.Text = "Duration:";
            // 
            // lblPosterURL
            // 
            this.lblPosterURL.AutoSize = true;
            this.lblPosterURL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosterURL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblPosterURL.Location = new System.Drawing.Point(15, 298);
            this.lblPosterURL.Name = "lblPosterURL";
            this.lblPosterURL.Size = new System.Drawing.Size(66, 32);
            this.lblPosterURL.TabIndex = 16;
            this.lblPosterURL.Text = "URL:";
            // 
            // lblDirector
            // 
            this.lblDirector.AutoSize = true;
            this.lblDirector.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDirector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDirector.Location = new System.Drawing.Point(16, 374);
            this.lblDirector.Name = "lblDirector";
            this.lblDirector.Size = new System.Drawing.Size(116, 32);
            this.lblDirector.TabIndex = 17;
            this.lblDirector.Text = "Director:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDate.Location = new System.Drawing.Point(16, 449);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(74, 32);
            this.lblDate.TabIndex = 18;
            this.lblDate.Text = "Date:";
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblGenre.Location = new System.Drawing.Point(16, 525);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(89, 32);
            this.lblGenre.TabIndex = 19;
            this.lblGenre.Text = "Genre:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblDescription.Location = new System.Drawing.Point(16, 597);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(153, 32);
            this.lblDescription.TabIndex = 20;
            this.lblDescription.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label2.Location = new System.Drawing.Point(568, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(504, 65);
            this.label2.TabIndex = 21;
            this.label2.Text = "Movies Management";
            // 
            // AdminFilmsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1673, 862);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblDirector);
            this.Controls.Add(this.lblPosterURL);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMovieID);
            this.Controls.Add(this.dateTimePickerRelease);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtPosterURL);
            this.Controls.Add(this.txtDirector);
            this.Controls.Add(this.txtGenre);
            this.Controls.Add(this.txtMovieID);
            this.Controls.Add(this.numericDuration);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.dataGridViewMovies);
            this.Controls.Add(this.btnDeleteMovie);
            this.Controls.Add(this.btnAddMovie);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AdminFilmsForm";
            this.Text = "AdminFilmsForm";
            this.Load += new System.EventHandler(this.AdminFilmsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMovies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnAddMovie;
        private System.Windows.Forms.Button btnDeleteMovie;
        private System.Windows.Forms.DataGridView dataGridViewMovies;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.NumericUpDown numericDuration;
        private System.Windows.Forms.TextBox txtMovieID;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.TextBox txtDirector;
        private System.Windows.Forms.TextBox txtPosterURL;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DateTimePicker dateTimePickerRelease;
        private System.Windows.Forms.Label lblMovieID;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblPosterURL;
        private System.Windows.Forms.Label lblDirector;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label2;
    }
}