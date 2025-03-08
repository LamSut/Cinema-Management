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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddFilm = new System.Windows.Forms.Button();
            this.btnEditFilm = new System.Windows.Forms.Button();
            this.btnDeleteShow = new System.Windows.Forms.Button();
            this.dataGridViewShows = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShows)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(255, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "List of shows";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnAddFilm
            // 
            this.btnAddFilm.Location = new System.Drawing.Point(40, 88);
            this.btnAddFilm.Name = "btnAddFilm";
            this.btnAddFilm.Size = new System.Drawing.Size(87, 34);
            this.btnAddFilm.TabIndex = 2;
            this.btnAddFilm.Text = "Add";
            this.btnAddFilm.UseVisualStyleBackColor = true;
            // 
            // btnEditFilm
            // 
            this.btnEditFilm.Location = new System.Drawing.Point(569, 88);
            this.btnEditFilm.Name = "btnEditFilm";
            this.btnEditFilm.Size = new System.Drawing.Size(87, 34);
            this.btnEditFilm.TabIndex = 3;
            this.btnEditFilm.Text = "Edit";
            this.btnEditFilm.UseVisualStyleBackColor = true;
            // 
            // btnDeleteShow
            // 
            this.btnDeleteShow.Location = new System.Drawing.Point(685, 88);
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
            this.dataGridViewShows.Location = new System.Drawing.Point(40, 149);
            this.dataGridViewShows.Name = "dataGridViewShows";
            this.dataGridViewShows.RowHeadersWidth = 62;
            this.dataGridViewShows.RowTemplate.Height = 28;
            this.dataGridViewShows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewShows.ShowRowErrors = false;
            this.dataGridViewShows.Size = new System.Drawing.Size(732, 304);
            this.dataGridViewShows.TabIndex = 5;
            // 
            // AdminShowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 515);
            this.Controls.Add(this.dataGridViewShows);
            this.Controls.Add(this.btnDeleteShow);
            this.Controls.Add(this.btnEditFilm);
            this.Controls.Add(this.btnAddFilm);
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
        private System.Windows.Forms.Button btnAddFilm;
        private System.Windows.Forms.Button btnEditFilm;
        private System.Windows.Forms.Button btnDeleteShow;
        private System.Windows.Forms.DataGridView dataGridViewShows;
    }
}