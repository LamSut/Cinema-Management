using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Cinema
{
    public partial class SeatsForm : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CinemaDB;Integrated Security=True";

        private string selectedShowID = "";
        private decimal ticketPrice = 0;
        private Dictionary<string, Button> seatButtons = new Dictionary<string, Button>();
        private List<string> selectedSeats = new List<string>();
        private List<string> bookedSeats = new List<string>();

        private string movieTitle = "";
        private string movieGenre = "";
        private int movieDuration = 0;
        private string moviePosterURL = "";
        private string movieDescription = "";

        private const int ROWS = 8;
        private const string ROW_LETTERS = "ABCDEFGH";
        private const int SEATS_PER_ROW = 16;
        private const int LEFT_SECTION = 4;
        private const int CENTER_SECTION = 8;
        private const int RIGHT_SECTION = 4;

        private readonly Color primaryColor = Color.FromArgb(20, 30, 60);
        private readonly Color secondaryColor = Color.FromArgb(175, 35, 35);
        private readonly Color accentColor = Color.FromArgb(240, 200, 70);
        private readonly Color availableSeatColor = Color.FromArgb(100, 160, 190);
        private readonly Color selectedSeatColor = Color.FromArgb(240, 200, 70);
        private readonly Color bookedSeatColor = Color.FromArgb(175, 35, 35);
        private readonly Color textColor = Color.White;

        private Label lblMovieInfoTitle;
        private Label lblGenreTitle;
        private Label lblGenreValue;
        private Label lblDurationTitle;
        private Label lblDurationValue;
        private Label lblSynopsisTitle;
        private TextBox txtSynopsis;
        private GroupBox groupBoxTicketSummary;
        private Label lblTicketCount;
        private Label lblTicketPrice;
        private Label lblTicketTotal;
        private Label lblDiscountTitle;
        private ComboBox cboDiscount;
        private Label lblFinalTotal;
        private Label lblPaymentMethodTitle;
        private ComboBox cboPaymentMethod;

        public SeatsForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void SeatsForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblScreen.Visible = false;
                InitializeAdditionalControls();
                InitializeSeatLayout();
                bool connected = TryConnectToDatabase();

                if (connected)
                {
                    LoadMovies();

                    if (cboShowTimes.DataSource != null)
                    {
                        ((DataTable)cboShowTimes.DataSource).Clear();
                    }
                    else
                    {
                        cboShowTimes.Items.Clear();
                    }
                }
                else
                {
                    LoadSampleData();
                    MessageBox.Show("Running in demo mode with sample data. Database connection unavailable.",
                        "Demo Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                btnBook.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during form initialization: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeAdditionalControls()
        {
            foreach (Control ctrl in panelMovieInfo.Controls)
            {
                if (ctrl != pictureBoxPoster)
                {
                    panelMovieInfo.Controls.Remove(ctrl);
                }
            }

            if (pictureBoxPoster != null)
            {
                pictureBoxPoster.Location = new Point(15, 15);
                pictureBoxPoster.Size = new Size(270, 280);
                pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxPoster.BorderStyle = BorderStyle.FixedSingle;
                pictureBoxPoster.Visible = true;
            }

            lblMovieInfoTitle = new Label();
            lblMovieInfoTitle.Location = new Point(15, 300);
            lblMovieInfoTitle.Size = new Size(270, 30);
            lblMovieInfoTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblMovieInfoTitle.ForeColor = accentColor;
            lblMovieInfoTitle.Text = "";
            lblMovieInfoTitle.AutoEllipsis = true;
            panelMovieInfo.Controls.Add(lblMovieInfoTitle);

            lblGenreTitle = new Label();
            lblGenreTitle.Location = new Point(15, 340);
            lblGenreTitle.Size = new Size(60, 25);
            lblGenreTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblGenreTitle.ForeColor = textColor;
            lblGenreTitle.Text = "Genre:";
            panelMovieInfo.Controls.Add(lblGenreTitle);

            lblGenreValue = new Label();
            lblGenreValue.Location = new Point(80, 340);
            lblGenreValue.Size = new Size(205, 25);
            lblGenreValue.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblGenreValue.ForeColor = textColor;
            lblGenreValue.Text = "";
            lblGenreValue.AutoEllipsis = true;
            panelMovieInfo.Controls.Add(lblGenreValue);

            lblDurationTitle = new Label();
            lblDurationTitle.Location = new Point(15, 365);
            lblDurationTitle.Size = new Size(70, 25);
            lblDurationTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDurationTitle.ForeColor = textColor;
            lblDurationTitle.Text = "Duration:";
            panelMovieInfo.Controls.Add(lblDurationTitle);

            lblDurationValue = new Label();
            lblDurationValue.Location = new Point(90, 365);
            lblDurationValue.Size = new Size(195, 25);
            lblDurationValue.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblDurationValue.ForeColor = textColor;
            lblDurationValue.Text = "";
            panelMovieInfo.Controls.Add(lblDurationValue);

            lblSynopsisTitle = new Label();
            lblSynopsisTitle.Location = new Point(15, 395);
            lblSynopsisTitle.Size = new Size(270, 25);
            lblSynopsisTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSynopsisTitle.ForeColor = textColor;
            lblSynopsisTitle.Text = "Synopsis:";
            panelMovieInfo.Controls.Add(lblSynopsisTitle);

            txtSynopsis = new TextBox();
            txtSynopsis.Location = new Point(15, 420);
            txtSynopsis.Size = new Size(270, 100);
            txtSynopsis.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            txtSynopsis.ForeColor = textColor;
            txtSynopsis.BackColor = Color.FromArgb(30, 40, 70);
            txtSynopsis.BorderStyle = BorderStyle.FixedSingle;
            txtSynopsis.Multiline = true;
            txtSynopsis.ReadOnly = true;
            txtSynopsis.ScrollBars = ScrollBars.Vertical;
            txtSynopsis.Text = "";
            panelMovieInfo.Controls.Add(txtSynopsis);
            panelActions.Controls.Clear();

            groupBoxTicketSummary = new GroupBox();
            groupBoxTicketSummary.Location = new Point(15, 525);
            groupBoxTicketSummary.Size = new Size(270, 155);
            groupBoxTicketSummary.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            groupBoxTicketSummary.ForeColor = accentColor;
            groupBoxTicketSummary.Text = "Ticket Summary";
            groupBoxTicketSummary.BackColor = Color.FromArgb(30, 40, 70);
            panelActions.Controls.Add(groupBoxTicketSummary);

            lblTicketCount = new Label();
            lblTicketCount.Location = new Point(10, 25);
            lblTicketCount.Size = new Size(250, 25);
            lblTicketCount.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketCount.ForeColor = textColor;
            lblTicketCount.Text = "Selected Seats: 0";
            groupBoxTicketSummary.Controls.Add(lblTicketCount);

            lblTicketPrice = new Label();
            lblTicketPrice.Location = new Point(10, 50);
            lblTicketPrice.Size = new Size(250, 25);
            lblTicketPrice.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketPrice.ForeColor = textColor;
            lblTicketPrice.Text = "Price per Ticket: $0.00";
            groupBoxTicketSummary.Controls.Add(lblTicketPrice);

            lblTicketTotal = new Label();
            lblTicketTotal.Location = new Point(10, 75);
            lblTicketTotal.Size = new Size(250, 25);
            lblTicketTotal.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketTotal.ForeColor = textColor;
            lblTicketTotal.Text = "Subtotal: $0.00";
            groupBoxTicketSummary.Controls.Add(lblTicketTotal);

            lblDiscountTitle = new Label();
            lblDiscountTitle.Location = new Point(10, 100);
            lblDiscountTitle.Size = new Size(70, 25);
            lblDiscountTitle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblDiscountTitle.ForeColor = textColor;
            lblDiscountTitle.Text = "Discount:";
            groupBoxTicketSummary.Controls.Add(lblDiscountTitle);

            cboDiscount = new ComboBox();
            cboDiscount.Location = new Point(85, 100);
            cboDiscount.Size = new Size(175, 25);
            cboDiscount.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            cboDiscount.ForeColor = Color.Black;
            cboDiscount.BackColor = Color.White;
            cboDiscount.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDiscount.Items.AddRange(new object[] {
                "None (0%)",
                "Student (10%)",
                "Senior (15%)",
                "Member (20%)"
            });
            cboDiscount.SelectedIndex = 0;
            cboDiscount.SelectedIndexChanged += CboDiscount_SelectedIndexChanged;
            groupBoxTicketSummary.Controls.Add(cboDiscount);

            lblFinalTotal = new Label();
            lblFinalTotal.Location = new Point(10, 125);
            lblFinalTotal.Size = new Size(250, 25);
            lblFinalTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFinalTotal.ForeColor = accentColor;
            lblFinalTotal.Text = "Total: $0.00";
            groupBoxTicketSummary.Controls.Add(lblFinalTotal);

            lblPaymentMethodTitle = new Label();
            lblPaymentMethodTitle.Location = new Point(15, 690);
            lblPaymentMethodTitle.Size = new Size(120, 25);
            lblPaymentMethodTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPaymentMethodTitle.ForeColor = textColor;
            lblPaymentMethodTitle.Text = "Payment Method:";
            panelActions.Controls.Add(lblPaymentMethodTitle);

            cboPaymentMethod = new ComboBox();
            cboPaymentMethod.Location = new Point(140, 690);
            cboPaymentMethod.Size = new Size(145, 25);
            cboPaymentMethod.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            cboPaymentMethod.ForeColor = Color.Black;
            cboPaymentMethod.BackColor = Color.White;
            cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPaymentMethod.Items.AddRange(new object[] {
                "Credit Card",
                "Debit Card",
                "Cash",
                "Mobile Payment"
            });
            cboPaymentMethod.SelectedIndex = 0;
            panelActions.Controls.Add(cboPaymentMethod);
            if (pictureBoxPoster != null)
            {
                pictureBoxPoster.BringToFront();
            }
        }

        private void CboDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        private bool TryConnectToDatabase()
        {
            string[] possibleConnectionStrings = new string[]
            {
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=localhost;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=.\SQLEXPRESS;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=127.0.0.1;Initial Catalog=CinemaDB;Integrated Security=True"
            };

            foreach (string connStr in possibleConnectionStrings)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connStr))
                    {
                        connection.Open();
                        connectionString = connStr;
                        return true;
                    }
                }
                catch
                {
                    continue;
                }
            }

            return false;
        }

        private void LoadMovies()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MovieID, Title FROM Movie WHERE Status = 'Active' ORDER BY Title";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow newRow = dataTable.NewRow();
                        newRow["MovieID"] = DBNull.Value;
                        newRow["Title"] = "-- Select a movie --";
                        dataTable.Rows.InsertAt(newRow, 0);

                        cboMovies.DataSource = dataTable;
                        cboMovies.DisplayMember = "Title";
                        cboMovies.ValueMember = "MovieID";

                        cboMovies.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No movies found in the database.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSampleData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading movies: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadSampleData();
                }
            }
        }

        private void LoadMovieDetails(string movieID)
        {
            if (string.IsNullOrEmpty(movieID) || movieID == DBNull.Value.ToString())
            {
                lblMovieInfoTitle.Text = "";
                lblGenreValue.Text = "";
                lblDurationValue.Text = "";
                txtSynopsis.Text = "";
                pictureBoxPoster.Image = null;
                return;
            }

            try
            {
                Console.WriteLine($"Loading movie details for MovieID: {movieID}");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT Title, Genre, Duration, Description, PosterURL
                        FROM Movie
                        WHERE MovieID = @MovieID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            movieTitle = reader["Title"].ToString();
                            movieGenre = reader["Genre"].ToString();
                            movieDuration = Convert.ToInt32(reader["Duration"]);
                            movieDescription = reader["Description"].ToString();
                            moviePosterURL = reader["PosterURL"] != DBNull.Value ? reader["PosterURL"].ToString() : "";

                            Console.WriteLine($"Movie found: {movieTitle}");
                            Console.WriteLine($"Poster URL: {moviePosterURL}");

                            lblMovieInfoTitle.Text = movieTitle;
                            lblGenreValue.Text = movieGenre;
                            lblDurationValue.Text = movieDuration + " min";
                            txtSynopsis.Text = movieDescription;

                            lblMovieInfoTitle.Visible = true;
                            lblGenreValue.Visible = true;
                            lblDurationValue.Visible = true;
                            txtSynopsis.Visible = true;
                            LoadMoviePoster(moviePosterURL);
                        }
                        else
                        {
                            Console.WriteLine($"Movie not found in database, loading sample data");
                            LoadSampleMovieDetails(movieID);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL error loading movie details: {sqlEx.Message}");
                LoadSampleMovieDetails(movieID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error loading movie details: {ex.Message}");
                LoadSampleMovieDetails(movieID);
            }
        }

        private void LoadMoviePoster(string posterURL)
        {
            try
            {
                if (!string.IsNullOrEmpty(posterURL))
                {
                    Console.WriteLine($"Attempting to load poster from URL: {posterURL}");

                    using (WebClientWithTimeout client = new WebClientWithTimeout())
                    {
                        client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
                        client.Timeout = 10000;

                        byte[] data = client.DownloadData(posterURL);
                        Console.WriteLine($"Downloaded {data.Length} bytes of poster data");

                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            Image posterImage = Image.FromStream(ms);
                            pictureBoxPoster.Image = new Bitmap(posterImage);
                            pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;

                            Console.WriteLine("Poster loaded successfully");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No poster URL provided, creating placeholder");
                    CreatePlaceholderImage();
                }
            }
            catch (WebException webEx)
            {
                Console.WriteLine($"Network error loading poster: {webEx.Message}");
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    Console.WriteLine("Connection timed out");
                }
                CreatePlaceholderImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading poster: {ex.Message}");
                CreatePlaceholderImage();
            }
        }

        private void CreatePlaceholderImage()
        {
            try
            {
                Bitmap bmp = new Bitmap(270, 380);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(30, 40, 70));
                    using (Pen pen = new Pen(Color.FromArgb(50, 60, 100), 2))
                    {
                        for (int i = 0; i < bmp.Width; i += 20)
                        {
                            g.DrawLine(pen, i, 0, i, bmp.Height);
                        }
                        for (int i = 0; i < bmp.Height; i += 20)
                        {
                            g.DrawLine(pen, 0, i, bmp.Width, i);
                        }
                    }

                    using (Font iconFont = new Font("Segoe UI Symbol", 48))
                    using (StringFormat sf = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    })
                    {
                        g.DrawString("🎬", iconFont, Brushes.LightGray,
                            new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
                    }
                }

                pictureBoxPoster.Image = bmp;
                pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
                Console.WriteLine("Placeholder image created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating placeholder image: {ex.Message}");
                pictureBoxPoster.Image = null;
            }
        }

        private void LoadShowTimes(string movieID)
        {
            if (string.IsNullOrEmpty(movieID) || movieID == DBNull.Value.ToString())
            {
                if (cboShowTimes.DataSource != null)
                {
                    ((DataTable)cboShowTimes.DataSource).Clear();
                }
                else
                {
                    cboShowTimes.Items.Clear();
                }

                ticketPrice = 0;
                lblPriceValue.Text = "$0.00";
                lblTicketPrice.Text = "Price per Ticket: $0.00";

                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT s.ShowID, 
                               CONVERT(VARCHAR, s.ShowTime, 103) + ' ' + CONVERT(VARCHAR, s.ShowTime, 108) AS ShowTimeDisplay,
                               s.Price,
                               t.TheaterName
                        FROM Show s
                        JOIN Theater t ON s.TheaterID = t.TheaterID
                        WHERE s.MovieID = @MovieID AND s.Status = 'Active' AND s.ShowTime > GETDATE()
                        ORDER BY s.ShowTime";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow newRow = dataTable.NewRow();
                        newRow["ShowID"] = DBNull.Value;
                        newRow["ShowTimeDisplay"] = "-- Select a showtime --";
                        newRow["Price"] = 0;
                        newRow["TheaterName"] = "";
                        dataTable.Rows.InsertAt(newRow, 0);

                        cboShowTimes.DataSource = dataTable;
                        cboShowTimes.DisplayMember = "ShowTimeDisplay";
                        cboShowTimes.ValueMember = "ShowID";

                        cboShowTimes.SelectedIndex = 0;
                    }
                    else
                    {
                        LoadSampleShowtimes(movieID);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading showtimes: " + ex.Message);
                    LoadSampleShowtimes(movieID);
                }
            }
        }

        private void LoadSampleData()
        {
            DataTable moviesTable = new DataTable();
            moviesTable.Columns.Add("MovieID", typeof(string));
            moviesTable.Columns.Add("Title", typeof(string));

            moviesTable.Rows.Add(DBNull.Value, "-- Select a movie --");
            moviesTable.Rows.Add("M001", "Inception");
            moviesTable.Rows.Add("M002", "Avatar: The Way of Water");
            moviesTable.Rows.Add("M003", "The Batman");
            moviesTable.Rows.Add("M004", "Interstellar");
            moviesTable.Rows.Add("M005", "John Wick: Chapter 4");
            moviesTable.Rows.Add("M007", "Parasite");

            cboMovies.DataSource = moviesTable;
            cboMovies.DisplayMember = "Title";
            cboMovies.ValueMember = "MovieID";

            cboMovies.SelectedIndex = 0;
        }

        private void LoadSampleShowtimes(string movieID)
        {
            DataTable showtimesTable = new DataTable();
            showtimesTable.Columns.Add("ShowID", typeof(string));
            showtimesTable.Columns.Add("ShowTimeDisplay", typeof(string));
            showtimesTable.Columns.Add("Price", typeof(decimal));
            showtimesTable.Columns.Add("TheaterName", typeof(string));

            showtimesTable.Rows.Add(DBNull.Value, "-- Select a showtime --", 0, "");

            DateTime today = DateTime.Today;

            if (movieID == "M001")
            {
                showtimesTable.Rows.Add("S001", today.AddDays(1).ToString("dd/MM/yyyy") + " 10:00", 12.50m, "Theater 1");
                showtimesTable.Rows.Add("S002", today.AddDays(1).ToString("dd/MM/yyyy") + " 14:30", 15.00m, "Theater 1");
                showtimesTable.Rows.Add("S003", today.AddDays(1).ToString("dd/MM/yyyy") + " 19:45", 18.00m, "Theater 1");
            }
            else if (movieID == "M002")
            {
                showtimesTable.Rows.Add("S004", today.AddDays(1).ToString("dd/MM/yyyy") + " 11:30", 12.50m, "Theater 2");
                showtimesTable.Rows.Add("S005", today.AddDays(1).ToString("dd/MM/yyyy") + " 15:00", 15.00m, "Theater 2");
                showtimesTable.Rows.Add("S006", today.AddDays(1).ToString("dd/MM/yyyy") + " 20:15", 18.00m, "Theater 2");
            }
            else if (movieID == "M003")
            {
                showtimesTable.Rows.Add("S007", today.AddDays(1).ToString("dd/MM/yyyy") + " 12:15", 12.50m, "Theater 3");
                showtimesTable.Rows.Add("S008", today.AddDays(1).ToString("dd/MM/yyyy") + " 16:30", 15.00m, "Theater 3");
                showtimesTable.Rows.Add("S009", today.AddDays(1).ToString("dd/MM/yyyy") + " 21:00", 18.00m, "Theater 3");
            }
            else if (movieID == "M004")
            {
                showtimesTable.Rows.Add("S010", today.AddDays(1).ToString("dd/MM/yyyy") + " 13:00", 12.50m, "Theater 4");
                showtimesTable.Rows.Add("S011", today.AddDays(1).ToString("dd/MM/yyyy") + " 17:30", 15.00m, "Theater 4");
                showtimesTable.Rows.Add("S012", today.AddDays(1).ToString("dd/MM/yyyy") + " 21:45", 18.00m, "Theater 4");
            }
            else if (movieID == "M007")
            {
                showtimesTable.Rows.Add("S016", today.AddDays(1).ToString("dd/MM/yyyy") + " 11:30", 12.50m, "Theater 7");
                showtimesTable.Rows.Add("S017", today.AddDays(1).ToString("dd/MM/yyyy") + " 16:00", 15.00m, "Theater 7");
                showtimesTable.Rows.Add("S018", today.AddDays(1).ToString("dd/MM/yyyy") + " 20:15", 17.50m, "Theater 7");
            }
            else
            {
                showtimesTable.Rows.Add("S013", today.AddDays(1).ToString("dd/MM/yyyy") + " 14:15", 12.50m, "Theater 5");
                showtimesTable.Rows.Add("S014", today.AddDays(1).ToString("dd/MM/yyyy") + " 18:00", 15.00m, "Theater 5");
                showtimesTable.Rows.Add("S015", today.AddDays(1).ToString("dd/MM/yyyy") + " 22:30", 18.00m, "Theater 5");
            }

            cboShowTimes.DataSource = showtimesTable;
            cboShowTimes.DisplayMember = "ShowTimeDisplay";
            cboShowTimes.ValueMember = "ShowID";

            cboShowTimes.SelectedIndex = 0;
        }

        private void LoadSampleMovieDetails(string movieID)
        {
            Console.WriteLine($"Loading sample movie details for MovieID: {movieID}");

            if (movieID == "M001")
            {
                movieTitle = "Inception";
                movieGenre = "Science Fiction, Action";
                movieDuration = 148;
                movieDescription = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg";
            }
            else if (movieID == "M002")
            {
                movieTitle = "Avatar: The Way of Water";
                movieGenre = "Adventure, Sci-Fi";
                movieDuration = 192;
                movieDescription = "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora. Once a familiar threat returns to finish what was previously started, Jake must work with Neytiri and the army of the Na'vi race to protect their home.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg";
            }
            else if (movieID == "M003")
            {
                movieTitle = "The Batman";
                movieGenre = "Action, Crime, Drama";
                movieDuration = 176;
                movieDescription = "When the Riddler, a sadistic serial killer, begins murdering key political figures in Gotham, Batman is forced to investigate the city's hidden corruption and question his family's involvement.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/74xTEgt7R36Fpooo50r9T25onhq.jpg";
            }
            else if (movieID == "M004")
            {
                movieTitle = "Interstellar";
                movieGenre = "Adventure, Drama, Sci-Fi";
                movieDuration = 169;
                movieDescription = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg";
            }
            else if (movieID == "M007")
            {
                movieTitle = "Parasite";
                movieGenre = "Drama, Thriller";
                movieDuration = 132;
                movieDescription = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg";
            }
            else
            {
                movieTitle = "John Wick: Chapter 4";
                movieGenre = "Action, Crime, Thriller";
                movieDuration = 169;
                movieDescription = "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, he must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.";
                moviePosterURL = "https://image.tmdb.org/t/p/w500/vZloFAK7NmvMGKE7VkF5UHaz0I.jpg";
            }

            Console.WriteLine($"Sample movie: {movieTitle}, Poster URL: {moviePosterURL}");

            lblMovieInfoTitle.Text = movieTitle;
            lblGenreValue.Text = movieGenre;
            lblDurationValue.Text = movieDuration + " min";
            txtSynopsis.Text = movieDescription;

            lblMovieInfoTitle.Visible = true;
            lblGenreValue.Visible = true;
            lblDurationValue.Visible = true;
            txtSynopsis.Visible = true;
            LoadMoviePoster(moviePosterURL);
        }

        private void InitializeSeatLayout()
        {
            panelSeats.Controls.Clear();
            seatButtons.Clear();

            int SEAT_SIZE = 35;
            int SEAT_MARGIN = 5;
            int AISLE_WIDTH = 20;
            int TOP_MARGIN = 30;
            int ROW_LABEL_WIDTH = 25;
            int totalSeatsWidth = SEATS_PER_ROW * (SEAT_SIZE + SEAT_MARGIN) + 2 * AISLE_WIDTH + ROW_LABEL_WIDTH;

            Panel mainContainer = new Panel();
            mainContainer.Size = new Size(totalSeatsWidth, ROWS * (SEAT_SIZE + SEAT_MARGIN) + TOP_MARGIN * 2);
            mainContainer.Location = new Point((panelSeats.Width - totalSeatsWidth) / 2, 10);
            mainContainer.BackColor = Color.Transparent;
            panelSeats.Controls.Add(mainContainer);

            Label screenLabel = new Label();
            screenLabel.Text = "SCREEN";
            screenLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            screenLabel.ForeColor = Color.Silver;
            screenLabel.Size = new Size(totalSeatsWidth, TOP_MARGIN);
            screenLabel.Location = new Point(0, 0);
            screenLabel.TextAlign = ContentAlignment.MiddleCenter;
            mainContainer.Controls.Add(screenLabel);

            Panel screenBar = new Panel();
            screenBar.BackColor = Color.Silver;
            screenBar.Size = new Size(totalSeatsWidth * 3 / 4, 3);
            screenBar.Location = new Point(totalSeatsWidth / 8, TOP_MARGIN - 5);
            mainContainer.Controls.Add(screenBar);
            int leftSectionCount = LEFT_SECTION;
            int centerSectionCount = CENTER_SECTION;

            for (int row = 0; row < ROWS; row++)
            {
                char rowLetter = ROW_LETTERS[row];

                Label lblRow = new Label();
                lblRow.Text = rowLetter.ToString();
                lblRow.Size = new Size(ROW_LABEL_WIDTH, SEAT_SIZE);
                lblRow.Location = new Point(5, TOP_MARGIN + 10 + row * (SEAT_SIZE + SEAT_MARGIN));
                lblRow.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRow.ForeColor = Color.White;
                lblRow.TextAlign = ContentAlignment.MiddleCenter;
                mainContainer.Controls.Add(lblRow);

                Color seatColor = (row < 3) ?
                    Color.FromArgb(120, 180, 200) : // Premium
                    Color.FromArgb(100, 160, 190);  // Standard

                string categoryID = (row < 3) ? "SC002" : "SC001";

                int xPos = ROW_LABEL_WIDTH + 10;

                for (int col = 1; col <= leftSectionCount; col++)
                {
                    string seatID = rowLetter.ToString() + col.ToString("00");
                    CreateSeatButton(mainContainer, seatID, xPos,
                        TOP_MARGIN + 10 + row * (SEAT_SIZE + SEAT_MARGIN),
                        seatColor, categoryID, SEAT_SIZE);
                    xPos += SEAT_SIZE + SEAT_MARGIN;
                }

                xPos += AISLE_WIDTH;
                for (int col = leftSectionCount + 1; col <= leftSectionCount + centerSectionCount; col++)
                {
                    string seatID = rowLetter.ToString() + col.ToString("00");
                    CreateSeatButton(mainContainer, seatID, xPos,
                        TOP_MARGIN + 10 + row * (SEAT_SIZE + SEAT_MARGIN),
                        seatColor, categoryID, SEAT_SIZE);
                    xPos += SEAT_SIZE + SEAT_MARGIN;
                }

                xPos += AISLE_WIDTH;
                for (int col = leftSectionCount + centerSectionCount + 1; col <= SEATS_PER_ROW; col++)
                {
                    string seatID = rowLetter.ToString() + col.ToString("00");
                    CreateSeatButton(mainContainer, seatID, xPos,
                        TOP_MARGIN + 10 + row * (SEAT_SIZE + SEAT_MARGIN),
                        seatColor, categoryID, SEAT_SIZE);
                    xPos += SEAT_SIZE + SEAT_MARGIN;
                }
            }
            UpdateSeatLegend();
        }

        private void CreateSeatButton(Panel container, string seatID, int x, int y, Color baseColor, string categoryID, int seatSize)
        {
            Button btnSeat = new Button();
            btnSeat.Name = "btnSeat_" + seatID;
            btnSeat.Text = seatID;
            btnSeat.Size = new Size(seatSize, seatSize);
            btnSeat.Location = new Point(x, y);
            btnSeat.BackColor = baseColor;
            btnSeat.FlatStyle = FlatStyle.Flat;
            btnSeat.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 70);
            btnSeat.FlatAppearance.BorderSize = 1;
            btnSeat.ForeColor = Color.Black;
            btnSeat.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnSeat.Tag = categoryID;
            btnSeat.Cursor = Cursors.Hand;
            btnSeat.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, seatSize + 1, seatSize + 1, 5, 5));

            btnSeat.Click += BtnSeat_Click;

            container.Controls.Add(btnSeat);
            seatButtons.Add(seatID, btnSeat);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void UpdateSeatLegend()
        {
            panelLegend.Controls.Clear();

            string[] legendTypes = new string[] { "Available", "Already Booked", "Currently Selected" };
            Color[] legendColors = new Color[] { availableSeatColor, bookedSeatColor, selectedSeatColor };

            int legendWidth = panelLegend.Width / legendTypes.Length;
            int buttonSize = 30;
            int yPos = (panelLegend.Height - buttonSize) / 2;

            for (int i = 0; i < legendTypes.Length; i++)
            {
                Button btnSample = new Button();
                btnSample.Size = new Size(buttonSize, buttonSize);
                btnSample.Location = new Point(legendWidth * i + legendWidth / 2 - buttonSize - 40, yPos);
                btnSample.BackColor = legendColors[i];
                btnSample.FlatStyle = FlatStyle.Flat;
                btnSample.FlatAppearance.BorderColor = Color.DarkGray;
                btnSample.Enabled = false;
                btnSample.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, buttonSize + 1, buttonSize + 1, 5, 5));
                panelLegend.Controls.Add(btnSample);

                Label lblLegend = new Label();
                lblLegend.Text = legendTypes[i];
                lblLegend.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                lblLegend.ForeColor = Color.White;
                lblLegend.AutoSize = true;
                lblLegend.Location = new Point(btnSample.Right + 10, btnSample.Top + (btnSample.Height - lblLegend.Height) / 2);
                panelLegend.Controls.Add(lblLegend);
            }
        }

        private void BtnSeat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedShowID) || selectedShowID == DBNull.Value.ToString())
            {
                MessageBox.Show("Please select a movie and showtime first!",
                    "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Button btnSeat = (Button)sender;
            string seatID = btnSeat.Name.Replace("btnSeat_", "");

            if (selectedSeats.Contains(seatID))
            {
                selectedSeats.Remove(seatID);
                btnSeat.BackColor = availableSeatColor;
                btnSeat.ForeColor = Color.Black;
            }
            else
            {
                selectedSeats.Add(seatID);
                btnSeat.BackColor = selectedSeatColor;
                btnSeat.ForeColor = Color.Black;
            }

            UpdateSelectedSeatsInfo();
            UpdateSummary();
        }

        private void LoadBookedSeats(string showID)
        {
            if (string.IsNullOrEmpty(showID) || showID == DBNull.Value.ToString()) return;
            foreach (KeyValuePair<string, Button> seat in seatButtons)
            {
                seat.Value.BackColor = availableSeatColor;
                seat.Value.Enabled = true;
                seat.Value.ForeColor = Color.Black;
            }

            bookedSeats.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT sb.SeatID 
                           FROM SeatBooking sb
                           JOIN Booking b ON sb.BookingID = b.BookingID
                           WHERE b.ShowID = @ShowID AND b.Status = 'Active'";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShowID", showID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string seatID = reader["SeatID"].ToString();
                        bookedSeats.Add(seatID);

                        if (seatButtons.ContainsKey(seatID))
                        {
                            seatButtons[seatID].BackColor = bookedSeatColor;
                            seatButtons[seatID].Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading booked seats: " + ex.Message);
                    Random rnd = new Random();
                    int numSeatsToMark = rnd.Next(10, 30);

                    for (int i = 0; i < numSeatsToMark; i++)
                    {
                        char row = ROW_LETTERS[rnd.Next(ROWS)];
                        int col = rnd.Next(1, SEATS_PER_ROW + 1);
                        string seatID = row.ToString() + col.ToString("00");

                        if (!bookedSeats.Contains(seatID) && seatButtons.ContainsKey(seatID))
                        {
                            bookedSeats.Add(seatID);
                            seatButtons[seatID].BackColor = bookedSeatColor;
                            seatButtons[seatID].Enabled = false;
                        }
                    }
                }
            }
        }

        private void UpdateSelectedSeatsInfo()
        {
            selectedSeats.Sort();
            Dictionary<char, List<string>> seatsByRow = new Dictionary<char, List<string>>();

            foreach (string seat in selectedSeats)
            {
                char row = seat[0];
                if (!seatsByRow.ContainsKey(row))
                {
                    seatsByRow[row] = new List<string>();
                }
                seatsByRow[row].Add(seat);
            }

            List<string> formattedSeats = new List<string>();
            foreach (var row in seatsByRow.Keys.OrderBy(k => k))
            {
                List<string> rowSeats = seatsByRow[row];
                rowSeats.Sort();
                formattedSeats.Add(string.Join(", ", rowSeats));
            }

            lblSelectedSeats.Text = string.Join(", ", formattedSeats);
            decimal totalPrice = selectedSeats.Count * ticketPrice;
            lblTotalPriceValue.Text = "$" + totalPrice.ToString("0.00");
            btnBook.Enabled = selectedSeats.Count > 0;
        }

        private void UpdateSummary()
        {
            lblTicketCount.Text = "Selected Seats: " + selectedSeats.Count;
            decimal subtotal = selectedSeats.Count * ticketPrice;
            lblTicketTotal.Text = "Subtotal: $" + subtotal.ToString("0.00");
            decimal discountPercent = 0;
            switch (cboDiscount.SelectedIndex)
            {
                case 1:
                    discountPercent = 0.10m;
                    break;
                case 2:
                    discountPercent = 0.15m;
                    break;
                case 3:
                    discountPercent = 0.20m;
                    break;
                default:
                    discountPercent = 0;
                    break;
            }

            decimal discountAmount = subtotal * discountPercent;
            decimal finalTotal = subtotal - discountAmount;

            lblFinalTotal.Text = "Total: $" + finalTotal.ToString("0.00");
        }

        private void ResetSelectedSeats()
        {
            selectedSeats.Clear();
            lblSelectedSeats.Text = "";
            lblTotalPriceValue.Text = "$0.00";

            foreach (string seatID in seatButtons.Keys)
            {
                if (seatButtons[seatID].BackColor == selectedSeatColor && seatButtons[seatID].Enabled)
                {
                    seatButtons[seatID].BackColor = availableSeatColor;
                    seatButtons[seatID].ForeColor = Color.Black;
                }
            }

            UpdateSummary();
            btnBook.Enabled = false;
        }

        private void ResetAllFormData()
        {
            selectedSeats.Clear();
            lblSelectedSeats.Text = "";
            lblTotalPriceValue.Text = "$0.00";
            ticketPrice = 0;
            lblPriceValue.Text = "$0.00";

            if (cboMovies.DataSource != null)
            {
                cboMovies.SelectedIndexChanged -= cboMovies_SelectedIndexChanged;
                ((DataTable)cboMovies.DataSource).Clear();
                cboMovies.DataSource = null;
                cboMovies.Items.Clear();
                cboMovies.SelectedIndexChanged += cboMovies_SelectedIndexChanged;
            }

            if (cboShowTimes.DataSource != null)
            {
                cboShowTimes.SelectedIndexChanged -= cboShowTimes_SelectedIndexChanged;
                ((DataTable)cboShowTimes.DataSource).Clear();
                cboShowTimes.DataSource = null;
                cboShowTimes.Items.Clear();
                cboShowTimes.SelectedIndexChanged += cboShowTimes_SelectedIndexChanged;
            }

            selectedShowID = "";
            lblMovieInfoTitle.Text = "";
            lblGenreValue.Text = "";
            lblDurationValue.Text = "";
            txtSynopsis.Text = "";
            pictureBoxPoster.Image = null;

            lblTicketCount.Text = "Selected Seats: 0";
            lblTicketPrice.Text = "Price per Ticket: $0.00";
            lblTicketTotal.Text = "Subtotal: $0.00";
            lblFinalTotal.Text = "Total: $0.00";
            cboDiscount.SelectedIndex = 0;
            btnBook.Enabled = false;

            foreach (Button btn in seatButtons.Values)
            {
                btn.BackColor = availableSeatColor;
                btn.Enabled = true;
                btn.ForeColor = Color.Black;
            }

            bookedSeats.Clear();
        }

        private void cboMovies_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetSelectedSeats();

                if (cboMovies.SelectedValue != null && cboMovies.SelectedValue != DBNull.Value && cboMovies.SelectedIndex > 0)
                {
                    string movieId = cboMovies.SelectedValue.ToString();
                    LoadShowTimes(movieId);
                    LoadMovieDetails(movieId);
                }
                else
                {
                    if (cboShowTimes.DataSource != null)
                    {
                        ((DataTable)cboShowTimes.DataSource).Clear();
                    }
                    else
                    {
                        cboShowTimes.Items.Clear();
                    }

                    lblMovieInfoTitle.Text = "";
                    lblGenreValue.Text = "";
                    lblDurationValue.Text = "";
                    txtSynopsis.Text = "";
                    pictureBoxPoster.Image = null;
                    ticketPrice = 0;
                    lblPriceValue.Text = "$0.00";
                    lblTicketPrice.Text = "Price per Ticket: $0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting movie: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboShowTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetSelectedSeats();

                if (cboShowTimes.SelectedValue != null && cboShowTimes.SelectedValue != DBNull.Value && cboShowTimes.SelectedIndex > 0)
                {
                    selectedShowID = cboShowTimes.SelectedValue.ToString();

                    if (cboShowTimes.SelectedItem is DataRowView selectedRow)
                    {
                        ticketPrice = Convert.ToDecimal(selectedRow["Price"]);
                        lblPriceValue.Text = "$" + ticketPrice.ToString("0.00");
                        lblTicketPrice.Text = "Price per Ticket: $" + ticketPrice.ToString("0.00");
                        LoadBookedSeats(selectedShowID);
                    }
                }
                else
                {
                    selectedShowID = "";
                    ticketPrice = 0;
                    lblPriceValue.Text = "$0.00";
                    lblTicketPrice.Text = "Price per Ticket: $0.00";

                    foreach (Button btn in seatButtons.Values)
                    {
                        btn.BackColor = availableSeatColor;
                        btn.Enabled = true;
                        btn.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting showtime: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ResetAllFormData();
                if (TryConnectToDatabase())
                {
                    LoadMovies();
                }
                else
                {
                    LoadSampleData();
                }

                InitializeSeatLayout();

                MessageBox.Show("All data has been refreshed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedSeats.Count == 0)
                {
                    MessageBox.Show("Please select at least one seat!",
                        "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string totalPrice = lblFinalTotal.Text.Replace("Total: $", "");
                string paymentMethod = cboPaymentMethod.SelectedItem.ToString();

                // Confirm booking
                DialogResult result = MessageBox.Show(
                    $"Confirm booking for the following details:\n\n" +
                    $"Movie: {movieTitle}\n" +
                    $"Showtime: {cboShowTimes.Text}\n" +
                    $"Seats: {string.Join(", ", selectedSeats)}\n" +
                    $"Total Price: ${totalPrice}\n" +
                    $"Payment Method: {paymentMethod}\n\n" +
                    $"Proceed with booking?",
                    "Confirm Booking",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    BookTickets();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking tickets: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BookTickets()
        {
            bool isConnected = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    isConnected = true;
                }
                catch
                {
                    isConnected = false;
                }
            }

            if (isConnected)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        conn.Open();
                        transaction = conn.BeginTransaction();

                        string bookingID = "B" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        string totalPriceString = lblFinalTotal.Text.Replace("Total: $", "");
                        decimal totalAmount = decimal.Parse(totalPriceString);

                        string paymentMethodID = "PM00" + (cboPaymentMethod.SelectedIndex + 1);
                        string discountID = "D00" + (cboDiscount.SelectedIndex + 1);

                        string insertBookingQuery = @"
           INSERT INTO Booking (BookingID, ShowID, BookingTime, DiscountID, PaymentMethodID, TotalAmount)
           VALUES (@BookingID, @ShowID, @BookingTime, @DiscountID, @PaymentMethodID, @TotalAmount)";

                        SqlCommand cmdBooking = new SqlCommand(insertBookingQuery, conn, transaction);
                        cmdBooking.Parameters.AddWithValue("@BookingID", bookingID);
                        cmdBooking.Parameters.AddWithValue("@ShowID", selectedShowID);
                        cmdBooking.Parameters.AddWithValue("@BookingTime", DateTime.Now);
                        cmdBooking.Parameters.AddWithValue("@DiscountID", discountID);
                        cmdBooking.Parameters.AddWithValue("@PaymentMethodID", paymentMethodID);
                        cmdBooking.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmdBooking.ExecuteNonQuery();

                        foreach (string seatID in selectedSeats)
                        {
                            SqlCommand cmdSeat = new SqlCommand("Book_Seat", conn, transaction);
                            cmdSeat.CommandType = CommandType.StoredProcedure;
                            cmdSeat.Parameters.AddWithValue("@BookingID", bookingID);
                            cmdSeat.Parameters.AddWithValue("@SeatID", seatID);
                            cmdSeat.Parameters.AddWithValue("@Price", ticketPrice);

                            int result = cmdSeat.ExecuteNonQuery();
                            if (result < 0)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Seat {seatID} has already been booked by someone else. Please refresh and select again!",
                                    "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LoadBookedSeats(selectedShowID);
                                return;
                            }
                        }

                        transaction.Commit();

                        TicketForm ticketForm = new TicketForm(bookingID);
                        ticketForm.ShowDialog();

                        ResetSelectedSeats();
                        LoadBookedSeats(selectedShowID);
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Booking successful! (Demo Mode)\n\nIn a real application, your booking would be saved to the database.",
                    "Demo Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (string seatID in selectedSeats)
                {
                    if (seatButtons.ContainsKey(seatID))
                    {
                        bookedSeats.Add(seatID);
                        seatButtons[seatID].BackColor = bookedSeatColor;
                        seatButtons[seatID].Enabled = false;
                    }
                }

                ResetSelectedSeats();
            }
        }


        public class WebClientWithTimeout : WebClient
        {
            private int _timeout = 10000;

            public int Timeout
            {
                get { return _timeout; }
                set { _timeout = value; }
            }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest request = base.GetWebRequest(uri);
                if (request != null)
                {
                    request.Timeout = _timeout;
                }
                return request;
            }
        }
    }
}