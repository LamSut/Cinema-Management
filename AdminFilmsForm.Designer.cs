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
            this.btnAddFilm = new System.Windows.Forms.Button();
            this.btnDeleteFilm = new System.Windows.Forms.Button();
            this.dataGridViewMovies = new System.Windows.Forms.DataGridView();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.numericDuration = new System.Windows.Forms.NumericUpDown();
            this.txtMovieID = new System.Windows.Forms.TextBox();
            this.txtGenre = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMovies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(480, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "List of films";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAddFilm
            // 
            this.btnAddFilm.Location = new System.Drawing.Point(81, 434);
            this.btnAddFilm.Name = "btnAddFilm";
            this.btnAddFilm.Size = new System.Drawing.Size(87, 34);
            this.btnAddFilm.TabIndex = 1;
            this.btnAddFilm.Text = "Add";
            this.btnAddFilm.UseVisualStyleBackColor = true;
            this.btnAddFilm.Click += new System.EventHandler(this.btnAddFilm_Click);
            // 
            // btnDeleteFilm
            // 
            this.btnDeleteFilm.Location = new System.Drawing.Point(878, 126);
            this.btnDeleteFilm.Name = "btnDeleteFilm";
            this.btnDeleteFilm.Size = new System.Drawing.Size(87, 34);
            this.btnDeleteFilm.TabIndex = 3;
            this.btnDeleteFilm.Text = "Delete";
            this.btnDeleteFilm.UseVisualStyleBackColor = true;
            this.btnDeleteFilm.Click += new System.EventHandler(this.btnDeleteFilm_Click);
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
            this.dataGridViewMovies.Size = new System.Drawing.Size(617, 304);
            this.dataGridViewMovies.TabIndex = 4;
            this.dataGridViewMovies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(27, 224);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(286, 26);
            this.txtTitle.TabIndex = 5;
            this.txtTitle.Text = "Please inpute new titlte";
            // 
            // numericDuration
            // 
            this.numericDuration.Location = new System.Drawing.Point(27, 296);
            this.numericDuration.Name = "numericDuration";
            this.numericDuration.Size = new System.Drawing.Size(141, 26);
            this.numericDuration.TabIndex = 6;
            // 
            // txtMovieID
            // 
            this.txtMovieID.Location = new System.Drawing.Point(27, 148);
            this.txtMovieID.Name = "txtMovieID";
            this.txtMovieID.ReadOnly = true;
            this.txtMovieID.Size = new System.Drawing.Size(141, 26);
            this.txtMovieID.TabIndex = 7;
            // 
            // txtGenre
            // 
            this.txtGenre.Location = new System.Drawing.Point(27, 373);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(286, 26);
            this.txtGenre.TabIndex = 8;
            // 
            // AdminFilmsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 654);
            this.Controls.Add(this.txtGenre);
            this.Controls.Add(this.txtMovieID);
            this.Controls.Add(this.numericDuration);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.dataGridViewMovies);
            this.Controls.Add(this.btnDeleteFilm);
            this.Controls.Add(this.btnAddFilm);
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
        private System.Windows.Forms.Button btnAddFilm;
        private System.Windows.Forms.Button btnDeleteFilm;
        private System.Windows.Forms.DataGridView dataGridViewMovies;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.NumericUpDown numericDuration;
        private System.Windows.Forms.TextBox txtMovieID;
        private System.Windows.Forms.TextBox txtGenre;
    }
}