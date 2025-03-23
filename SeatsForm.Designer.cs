namespace Cinema
{
    partial class SeatsForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panelMovieSelection = new System.Windows.Forms.Panel();
            this.lblTheater = new System.Windows.Forms.Label();
            this.cboTheaters = new System.Windows.Forms.ComboBox();
            this.lblPriceValue = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboShowTimes = new System.Windows.Forms.ComboBox();
            this.lblShowTime = new System.Windows.Forms.Label();
            this.cboMovies = new System.Windows.Forms.ComboBox();
            this.lblMovieTitle = new System.Windows.Forms.Label();
            this.panelScreen = new System.Windows.Forms.Panel();
            this.lblScreen = new System.Windows.Forms.Label();
            this.panelSeats = new System.Windows.Forms.Panel();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.panelSeatCategories = new System.Windows.Forms.Panel();
            this.lblSeatCategory = new System.Windows.Forms.Label();
            this.cboSeatCategory = new System.Windows.Forms.ComboBox();
            this.chkShowAllCategories = new System.Windows.Forms.CheckBox();
            this.panelSelectedSeats = new System.Windows.Forms.Panel();
            this.lblTotalPriceValue = new System.Windows.Forms.Label();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.lblSelectedSeats = new System.Windows.Forms.Label();
            this.lblSelectedSeatsTitle = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.panelMovieInfo = new System.Windows.Forms.Panel();
            this.pictureBoxPoster = new System.Windows.Forms.PictureBox();
            this.panelActions = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelMovieSelection.SuspendLayout();
            this.panelScreen.SuspendLayout();
            this.panelSeatCategories.SuspendLayout();
            this.panelSelectedSeats.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMovieInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPoster)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.panelHeader.Controls.Add(this.lblHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1200, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1200, 60);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Luxury Cinema - Ticket Booking";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMovieSelection
            // 
            this.panelMovieSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.panelMovieSelection.Controls.Add(this.lblTheater);
            this.panelMovieSelection.Controls.Add(this.cboTheaters);
            this.panelMovieSelection.Controls.Add(this.lblPriceValue);
            this.panelMovieSelection.Controls.Add(this.lblPrice);
            this.panelMovieSelection.Controls.Add(this.btnRefresh);
            this.panelMovieSelection.Controls.Add(this.cboShowTimes);
            this.panelMovieSelection.Controls.Add(this.lblShowTime);
            this.panelMovieSelection.Controls.Add(this.cboMovies);
            this.panelMovieSelection.Controls.Add(this.lblMovieTitle);
            this.panelMovieSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMovieSelection.Location = new System.Drawing.Point(0, 60);
            this.panelMovieSelection.Name = "panelMovieSelection";
            this.panelMovieSelection.Padding = new System.Windows.Forms.Padding(20);
            this.panelMovieSelection.Size = new System.Drawing.Size(1200, 120);
            this.panelMovieSelection.TabIndex = 1;
            // 
            // lblTheater
            // 
            this.lblTheater.AutoSize = true;
            this.lblTheater.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTheater.ForeColor = System.Drawing.Color.White;
            this.lblTheater.Location = new System.Drawing.Point(640, 29);
            this.lblTheater.Name = "lblTheater";
            this.lblTheater.Size = new System.Drawing.Size(80, 32);
            this.lblTheater.TabIndex = 7;
            this.lblTheater.Text = "Theater:";
            // 
            // cboTheaters
            // 
            this.cboTheaters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.cboTheaters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheaters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTheaters.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTheaters.ForeColor = System.Drawing.Color.White;
            this.cboTheaters.FormattingEnabled = true;
            this.cboTheaters.Location = new System.Drawing.Point(730, 25);
            this.cboTheaters.Name = "cboTheaters";
            this.cboTheaters.Size = new System.Drawing.Size(250, 40);
            this.cboTheaters.TabIndex = 8;
            this.cboTheaters.SelectedIndexChanged += new System.EventHandler(this.cboTheaters_SelectedIndexChanged);
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblPriceValue.Location = new System.Drawing.Point(706, 74);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new System.Drawing.Size(42, 32);
            this.lblPriceValue.TabIndex = 6;
            this.lblPriceValue.Text = "$0";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.ForeColor = System.Drawing.Color.White;
            this.lblPrice.Location = new System.Drawing.Point(640, 74);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(70, 32);
            this.lblPrice.TabIndex = 5;
            this.lblPrice.Text = "Price:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(1042, 40);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(136, 40);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboShowTimes
            // 
            this.cboShowTimes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.cboShowTimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboShowTimes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboShowTimes.ForeColor = System.Drawing.Color.White;
            this.cboShowTimes.FormattingEnabled = true;
            this.cboShowTimes.Location = new System.Drawing.Point(160, 70);
            this.cboShowTimes.Name = "cboShowTimes";
            this.cboShowTimes.Size = new System.Drawing.Size(460, 40);
            this.cboShowTimes.TabIndex = 3;
            this.cboShowTimes.SelectedIndexChanged += new System.EventHandler(this.cboShowTimes_SelectedIndexChanged);
            // 
            // lblShowTime
            // 
            this.lblShowTime.AutoSize = true;
            this.lblShowTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShowTime.ForeColor = System.Drawing.Color.White;
            this.lblShowTime.Location = new System.Drawing.Point(20, 74);
            this.lblShowTime.Name = "lblShowTime";
            this.lblShowTime.Size = new System.Drawing.Size(125, 32);
            this.lblShowTime.TabIndex = 2;
            this.lblShowTime.Text = "Showtime:";
            // 
            // cboMovies
            // 
            this.cboMovies.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.cboMovies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMovies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMovies.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMovies.ForeColor = System.Drawing.Color.White;
            this.cboMovies.FormattingEnabled = true;
            this.cboMovies.Location = new System.Drawing.Point(160, 25);
            this.cboMovies.Name = "cboMovies";
            this.cboMovies.Size = new System.Drawing.Size(460, 40);
            this.cboMovies.TabIndex = 1;
            this.cboMovies.SelectedIndexChanged += new System.EventHandler(this.cboMovies_SelectedIndexChanged);
            // 
            // lblMovieTitle
            // 
            this.lblMovieTitle.AutoSize = true;
            this.lblMovieTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMovieTitle.ForeColor = System.Drawing.Color.White;
            this.lblMovieTitle.Location = new System.Drawing.Point(20, 29);
            this.lblMovieTitle.Name = "lblMovieTitle";
            this.lblMovieTitle.Size = new System.Drawing.Size(86, 32);
            this.lblMovieTitle.TabIndex = 0;
            this.lblMovieTitle.Text = "Movie:";
            // 
            // panelScreen
            // 
            this.panelScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.panelScreen.Controls.Add(this.lblScreen);
            this.panelScreen.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScreen.Location = new System.Drawing.Point(0, 220);
            this.panelScreen.Name = "panelScreen";
            this.panelScreen.Size = new System.Drawing.Size(900, 60);
            this.panelScreen.TabIndex = 2;
            // 
            // lblScreen
            // 
            this.lblScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScreen.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreen.ForeColor = System.Drawing.Color.Silver;
            this.lblScreen.Location = new System.Drawing.Point(0, 0);
            this.lblScreen.Name = "lblScreen";
            this.lblScreen.Size = new System.Drawing.Size(900, 60);
            this.lblScreen.TabIndex = 0;
            this.lblScreen.Text = "SCREEN";
            this.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSeatCategories
            // 
            this.panelSeatCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(65)))));
            this.panelSeatCategories.Controls.Add(this.lblSeatCategory);
            this.panelSeatCategories.Controls.Add(this.cboSeatCategory);
            this.panelSeatCategories.Controls.Add(this.chkShowAllCategories);
            this.panelSeatCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeatCategories.Location = new System.Drawing.Point(0, 180);
            this.panelSeatCategories.Name = "panelSeatCategories";
            this.panelSeatCategories.Size = new System.Drawing.Size(1200, 40);
            this.panelSeatCategories.TabIndex = 8;
            // 
            // lblSeatCategory
            // 
            this.lblSeatCategory.AutoSize = true;
            this.lblSeatCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeatCategory.ForeColor = System.Drawing.Color.White;
            this.lblSeatCategory.Location = new System.Drawing.Point(20, 7);
            this.lblSeatCategory.Name = "lblSeatCategory";
            this.lblSeatCategory.Size = new System.Drawing.Size(133, 28);
            this.lblSeatCategory.TabIndex = 0;
            this.lblSeatCategory.Text = "Seat Category:";
            // 
            // cboSeatCategory
            // 
            this.cboSeatCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.cboSeatCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeatCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSeatCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSeatCategory.ForeColor = System.Drawing.Color.White;
            this.cboSeatCategory.FormattingEnabled = true;
            this.cboSeatCategory.Location = new System.Drawing.Point(160, 4);
            this.cboSeatCategory.Name = "cboSeatCategory";
            this.cboSeatCategory.Size = new System.Drawing.Size(240, 36);
            this.cboSeatCategory.TabIndex = 1;
            this.cboSeatCategory.SelectedIndexChanged += new System.EventHandler(this.cboSeatCategory_SelectedIndexChanged);
            // 
            // chkShowAllCategories
            // 
            this.chkShowAllCategories.AutoSize = true;
            this.chkShowAllCategories.Checked = true;
            this.chkShowAllCategories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAllCategories.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowAllCategories.ForeColor = System.Drawing.Color.White;
            this.chkShowAllCategories.Location = new System.Drawing.Point(420, 6);
            this.chkShowAllCategories.Name = "chkShowAllCategories";
            this.chkShowAllCategories.Size = new System.Drawing.Size(190, 32);
            this.chkShowAllCategories.TabIndex = 2;
            this.chkShowAllCategories.Text = "Show All Categories";
            this.chkShowAllCategories.UseVisualStyleBackColor = true;
            this.chkShowAllCategories.CheckedChanged += new System.EventHandler(this.chkShowAllCategories_CheckedChanged);
            // 
            // panelSeats
            // 
            this.panelSeats.AutoScroll = true;
            this.panelSeats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(65)))));
            this.panelSeats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSeats.Location = new System.Drawing.Point(0, 280);
            this.panelSeats.Name = "panelSeats";
            this.panelSeats.Padding = new System.Windows.Forms.Padding(20);
            this.panelSeats.Size = new System.Drawing.Size(900, 260);
            this.panelSeats.TabIndex = 3;
            // 
            // panelLegend
            // 
            this.panelLegend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLegend.Location = new System.Drawing.Point(0, 540);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(900, 60);
            this.panelLegend.TabIndex = 4;
            // 
            // panelSelectedSeats
            // 
            this.panelSelectedSeats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.panelSelectedSeats.Controls.Add(this.lblTotalPriceValue);
            this.panelSelectedSeats.Controls.Add(this.lblTotalPrice);
            this.panelSelectedSeats.Controls.Add(this.lblSelectedSeats);
            this.panelSelectedSeats.Controls.Add(this.lblSelectedSeatsTitle);
            this.panelSelectedSeats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSelectedSeats.Location = new System.Drawing.Point(0, 600);
            this.panelSelectedSeats.Name = "panelSelectedSeats";
            this.panelSelectedSeats.Size = new System.Drawing.Size(900, 80);
            this.panelSelectedSeats.TabIndex = 5;
            // 
            // lblTotalPriceValue
            // 
            this.lblTotalPriceValue.AutoSize = true;
            this.lblTotalPriceValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPriceValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.lblTotalPriceValue.Location = new System.Drawing.Point(811, 28);
            this.lblTotalPriceValue.Name = "lblTotalPriceValue";
            this.lblTotalPriceValue.Size = new System.Drawing.Size(42, 32);
            this.lblTotalPriceValue.TabIndex = 3;
            this.lblTotalPriceValue.Text = "$0";
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.AutoSize = true;
            this.lblTotalPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrice.ForeColor = System.Drawing.Color.White;
            this.lblTotalPrice.Location = new System.Drawing.Point(669, 28);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(128, 32);
            this.lblTotalPrice.TabIndex = 2;
            this.lblTotalPrice.Text = "Total Price:";
            // 
            // lblSelectedSeats
            // 
            this.lblSelectedSeats.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedSeats.ForeColor = System.Drawing.Color.White;
            this.lblSelectedSeats.Location = new System.Drawing.Point(199, 28);
            this.lblSelectedSeats.Name = "lblSelectedSeats";
            this.lblSelectedSeats.Size = new System.Drawing.Size(425, 32);
            this.lblSelectedSeats.TabIndex = 1;
            // 
            // lblSelectedSeatsTitle
            // 
            this.lblSelectedSeatsTitle.AutoSize = true;
            this.lblSelectedSeatsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedSeatsTitle.ForeColor = System.Drawing.Color.White;
            this.lblSelectedSeatsTitle.Location = new System.Drawing.Point(20, 28);
            this.lblSelectedSeatsTitle.Name = "lblSelectedSeatsTitle";
            this.lblSelectedSeatsTitle.Size = new System.Drawing.Size(173, 32);
            this.lblSelectedSeatsTitle.TabIndex = 0;
            this.lblSelectedSeatsTitle.Text = "Selected Seats:";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.panelButtons.Controls.Add(this.btnBook);
            this.panelButtons.Controls.Add(this.btnBack);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 680);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1200, 70);
            this.panelButtons.TabIndex = 6;
            // 
            // btnBook
            // 
            this.btnBook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(200)))), ((int)(((byte)(70)))));
            this.btnBook.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBook.Enabled = false;
            this.btnBook.FlatAppearance.BorderSize = 0;
            this.btnBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBook.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBook.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.btnBook.Location = new System.Drawing.Point(997, 14);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(180, 45);
            this.btnBook.TabIndex = 1;
            this.btnBook.Text = "Book Tickets";
            this.btnBook.UseVisualStyleBackColor = false;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(23, 14);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 45);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // panelMovieInfo
            // 
            this.panelMovieInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(55)))));
            this.panelMovieInfo.Controls.Add(this.pictureBoxPoster);
            this.panelMovieInfo.Controls.Add(this.panelActions);
            this.panelMovieInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMovieInfo.Location = new System.Drawing.Point(900, 180);
            this.panelMovieInfo.Name = "panelMovieInfo";
            this.panelMovieInfo.Padding = new System.Windows.Forms.Padding(15);
            this.panelMovieInfo.Size = new System.Drawing.Size(300, 500);
            this.panelMovieInfo.TabIndex = 7;
            // 
            // pictureBoxPoster
            // 
            this.pictureBoxPoster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPoster.Location = new System.Drawing.Point(15, 15);
            this.pictureBoxPoster.Name = "pictureBoxPoster";
            this.pictureBoxPoster.Size = new System.Drawing.Size(270, 380);
            this.pictureBoxPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPoster.TabIndex = 0;
            this.pictureBoxPoster.TabStop = false;
            // 
            // panelActions
            // 
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(15, 405);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(270, 80);
            this.panelActions.TabIndex = 1;
            // 
            // SeatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.panelSeats);
            this.Controls.Add(this.panelLegend);
            this.Controls.Add(this.panelSelectedSeats);
            this.Controls.Add(this.panelScreen);
            this.Controls.Add(this.panelSeatCategories);
            this.Controls.Add(this.panelMovieInfo);
            this.Controls.Add(this.panelMovieSelection);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelButtons);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.Name = "SeatsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Luxury Cinema - Ticket Booking";
            this.Load += new System.EventHandler(this.SeatsForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelMovieSelection.ResumeLayout(false);
            this.panelMovieSelection.PerformLayout();
            this.panelScreen.ResumeLayout(false);
            this.panelSeatCategories.ResumeLayout(false);
            this.panelSeatCategories.PerformLayout();
            this.panelSelectedSeats.ResumeLayout(false);
            this.panelSelectedSeats.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMovieInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPoster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panelMovieSelection;
        private System.Windows.Forms.Label lblPriceValue;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cboShowTimes;
        private System.Windows.Forms.Label lblShowTime;
        private System.Windows.Forms.ComboBox cboMovies;
        private System.Windows.Forms.Label lblMovieTitle;
        private System.Windows.Forms.Panel panelScreen;
        private System.Windows.Forms.Label lblScreen;
        private System.Windows.Forms.Panel panelSeats;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Panel panelSelectedSeats;
        private System.Windows.Forms.Label lblTotalPriceValue;
        private System.Windows.Forms.Label lblTotalPrice;
        private System.Windows.Forms.Label lblSelectedSeats;
        private System.Windows.Forms.Label lblSelectedSeatsTitle;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel panelMovieInfo;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.PictureBox pictureBoxPoster;
        private System.Windows.Forms.Label lblTheater;
        private System.Windows.Forms.ComboBox cboTheaters;
        private System.Windows.Forms.Panel panelSeatCategories;
        private System.Windows.Forms.Label lblSeatCategory;
        private System.Windows.Forms.ComboBox cboSeatCategory;
        private System.Windows.Forms.CheckBox chkShowAllCategories;
    }
}