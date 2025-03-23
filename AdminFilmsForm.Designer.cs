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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMovies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(651, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "List of Movies";
            // 
            // btnAddMovie
            // 
            this.btnAddMovie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.btnAddMovie.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddMovie.Location = new System.Drawing.Point(29, 735);
            this.btnAddMovie.Name = "btnAddMovie";
            this.btnAddMovie.Size = new System.Drawing.Size(220, 60);
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
            this.btnDeleteMovie.Location = new System.Drawing.Point(1075, 101);
            this.btnDeleteMovie.Name = "btnDeleteMovie";
            this.btnDeleteMovie.Size = new System.Drawing.Size(252, 57);
            this.btnDeleteMovie.TabIndex = 3;
            this.btnDeleteMovie.Text = "Delete This Movie";
            this.btnDeleteMovie.UseVisualStyleBackColor = false;
            this.btnDeleteMovie.Click += new System.EventHandler(this.btnDeleteFilm_Click);
            // 
            // dataGridViewMovies
            // 
            this.dataGridViewMovies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMovies.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewMovies.Location = new System.Drawing.Point(348, 178);
            this.dataGridViewMovies.Name = "dataGridViewMovies";
            this.dataGridViewMovies.RowHeadersWidth = 62;
            this.dataGridViewMovies.RowTemplate.Height = 28;
            this.dataGridViewMovies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMovies.ShowRowErrors = false;
            this.dataGridViewMovies.Size = new System.Drawing.Size(979, 456);
            this.dataGridViewMovies.TabIndex = 4;
            this.dataGridViewMovies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtTitle.Location = new System.Drawing.Point(11, 196);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(286, 39);
            this.txtTitle.TabIndex = 5;
            this.txtTitle.Text = "Please inpute new titlte";
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // numericDuration
            // 
            this.numericDuration.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.numericDuration.Location = new System.Drawing.Point(11, 261);
            this.numericDuration.Name = "numericDuration";
            this.numericDuration.Size = new System.Drawing.Size(141, 39);
            this.numericDuration.TabIndex = 6;
            this.numericDuration.ValueChanged += new System.EventHandler(this.numericDuration_ValueChanged);
            // 
            // txtMovieID
            // 
            this.txtMovieID.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtMovieID.Location = new System.Drawing.Point(12, 123);
            this.txtMovieID.Name = "txtMovieID";
            this.txtMovieID.ReadOnly = true;
            this.txtMovieID.Size = new System.Drawing.Size(140, 39);
            this.txtMovieID.TabIndex = 7;
            this.txtMovieID.TextChanged += new System.EventHandler(this.txtMovieID_TextChanged);
            // 
            // txtGenre
            // 
            this.txtGenre.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtGenre.Location = new System.Drawing.Point(11, 528);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(164, 39);
            this.txtGenre.TabIndex = 8;
            this.txtGenre.Text = "Genre";
            this.txtGenre.TextChanged += new System.EventHandler(this.txtGenre_TextChanged);
            // 
            // txtDirector
            // 
            this.txtDirector.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtDirector.Location = new System.Drawing.Point(11, 389);
            this.txtDirector.Name = "txtDirector";
            this.txtDirector.Size = new System.Drawing.Size(215, 39);
            this.txtDirector.TabIndex = 9;
            this.txtDirector.Text = "Director";
            this.txtDirector.TextChanged += new System.EventHandler(this.txtDirector_TextChanged);
            // 
            // txtPosterURL
            // 
            this.txtPosterURL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtPosterURL.Location = new System.Drawing.Point(11, 317);
            this.txtPosterURL.Name = "txtPosterURL";
            this.txtPosterURL.Size = new System.Drawing.Size(286, 39);
            this.txtPosterURL.TabIndex = 10;
            this.txtPosterURL.Text = "URL: ";
            this.txtPosterURL.TextChanged += new System.EventHandler(this.txtPosterURL_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.txtDescription.Location = new System.Drawing.Point(11, 596);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(286, 103);
            this.txtDescription.TabIndex = 11;
            this.txtDescription.Text = "Des:";
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // dateTimePickerRelease
            // 
            this.dateTimePickerRelease.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.dateTimePickerRelease.Location = new System.Drawing.Point(11, 458);
            this.dateTimePickerRelease.Name = "dateTimePickerRelease";
            this.dateTimePickerRelease.Size = new System.Drawing.Size(286, 39);
            this.dateTimePickerRelease.TabIndex = 12;
            this.dateTimePickerRelease.ValueChanged += new System.EventHandler(this.dateTimePickerRelease_ValueChanged);
            // 
            // AdminFilmsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(1377, 807);
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
    }
}