using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema.Views.Staff
{
    public partial class BookingForm : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CinemaDB;Integrated Security=True";

        private string selectedShowID = "";
        private string selectedTheaterId = "";
        private string selectedMovieId = "";
        private decimal ticketPrice = 0;
        private Dictionary<string, Button> seatButtons = new Dictionary<string, Button>();
        private List<string> selectedSeats = new List<string>();
        private List<string> bookedSeats = new List<string>();
        private string selectedCategoryId = "";
        private Dictionary<string, SeatCategoryInfo> seatCategories = new Dictionary<string, SeatCategoryInfo>();
        private Dictionary<string, string> seatCategoryMapping = new Dictionary<string, string>();

        private string movieTitle = "";
        private string movieGenre = "";
        private int movieDuration = 0;
        private string moviePosterURL = "";
        private string movieDescription = "";

        private DataTable allTheatersTable;
        private DataTable allShowtimesTable;

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
        private readonly Color standardSeatColor = Color.FromArgb(100, 160, 190);
        private readonly Color premiumSeatColor = Color.FromArgb(60, 179, 113);
        private readonly Color vipSeatColor = Color.FromArgb(180, 120, 240);

        private Label lblMovieInfoTitle;
        private Label lblGenreTitle;
        private Label lblGenreValue;
        private Label lblDurationTitle;
        private Label lblDurationValue;
        private Label lblDescriptionTitle;
        private TextBox txtDescription;
        private GroupBox groupBoxTicketSummary;
        private Label lblTicketCount;
        private Label lblTicketPrice;
        private Label lblTicketTotal;
        private Label lblDiscountTitle;
        private ComboBox cboDiscount;
        private Label lblFinalTotal;
        private Label lblPaymentMethodTitle;
        private ComboBox cboPaymentMethod;
        private Label lblTheaterTitle;
        private Label lblTheaterValue;

        private class SeatCategoryInfo
        {
            public string CategoryID { get; set; }
            public string CategoryName { get; set; }
            public decimal PriceMultiplier { get; set; }
            public Color SeatColor { get; set; }

            public SeatCategoryInfo(string id, string name, decimal multiplier, Color color)
            {
                CategoryID = id;
                CategoryName = name;
                PriceMultiplier = multiplier;
                SeatColor = color;
            }
        }

        private class MovieInfo
        {
            public string Title { get; set; } = "";
            public string Genre { get; set; } = "";
            public int Duration { get; set; } = 0;
            public string Description { get; set; } = "";
            public string PosterURL { get; set; } = "";
        }

        public BookingForm()
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
                InitializeSeatCategories();
                InitializeSeatLayout();

                Task.Run(async () =>
                {
                    bool connected = await TryConnectToDatabaseAsync();
                    if (connected)
                    {
                        await LoadSeatCategoriesAsync();
                        await LoadAllTheatersAsync();
                        await LoadMoviesAsync();
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            LoadSampleData();
                            MessageBox.Show("Running in demo mode with sample data. Database connection unavailable.",
                            "Demo Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });
                    }
                });

                btnBook.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during form initialization: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class LoadingForm : Form
        {
            public LoadingForm()
            {
                this.Size = new Size(200, 70);
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = Color.FromArgb(20, 30, 60);

                Label lblLoading = new Label();
                lblLoading.Text = "Loading...";
                lblLoading.ForeColor = Color.White;
                lblLoading.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblLoading.Dock = DockStyle.Fill;
                lblLoading.TextAlign = ContentAlignment.MiddleCenter;

                this.Controls.Add(lblLoading);
                this.ShowInTaskbar = false;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(240, 200, 70), ButtonBorderStyle.Solid);
            }
        }

        private void InitializeSeatCategories()
        {
            seatCategories.Clear();
            seatCategories.Add("SC001", new SeatCategoryInfo("SC001", "Standard", 1.0m, standardSeatColor));
            seatCategories.Add("SC002", new SeatCategoryInfo("SC002", "Premium", 1.2m, premiumSeatColor));
            seatCategories.Add("SC003", new SeatCategoryInfo("SC003", "VIP", 1.5m, vipSeatColor));
        }

        private void InitializeAdditionalControls()
        {
            foreach (Control ctrl in panelMovieInfo.Controls)
            {
                if (ctrl != pictureBoxPoster && ctrl != panelActions)
                {
                    panelMovieInfo.Controls.Remove(ctrl);
                }
            }

            panelMovieInfo.Padding = new Padding(15);
            panelMovieInfo.AutoScroll = true;

            if (pictureBoxPoster != null)
            {
                pictureBoxPoster.Location = new Point(15, 15);
                pictureBoxPoster.Size = new Size(210, 180);
                pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxPoster.BorderStyle = BorderStyle.FixedSingle;
                pictureBoxPoster.Visible = true;
            }

            Panel infoPanel = new Panel();
            infoPanel.Location = new Point(240, 15);
            infoPanel.Size = new Size(130, 180);
            infoPanel.BackColor = Color.Transparent;
            panelMovieInfo.Controls.Add(infoPanel);

            int labelHeight = 25;
            int spacing = 5;
            int currentY = 0;

            lblMovieInfoTitle = new Label();
            lblMovieInfoTitle.Location = new Point(0, currentY);
            lblMovieInfoTitle.Size = new Size(130, labelHeight);
            lblMovieInfoTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblMovieInfoTitle.ForeColor = accentColor;
            lblMovieInfoTitle.Text = "";
            lblMovieInfoTitle.AutoEllipsis = true;
            infoPanel.Controls.Add(lblMovieInfoTitle);
            currentY += labelHeight + spacing;

            lblGenreTitle = new Label();
            lblGenreTitle.Location = new Point(0, currentY);
            lblGenreTitle.Size = new Size(50, labelHeight);
            lblGenreTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblGenreTitle.ForeColor = textColor;
            lblGenreTitle.Text = "Genre:";
            infoPanel.Controls.Add(lblGenreTitle);

            lblGenreValue = new Label();
            lblGenreValue.Location = new Point(50, currentY);
            lblGenreValue.Size = new Size(80, labelHeight);
            lblGenreValue.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblGenreValue.ForeColor = textColor;
            lblGenreValue.Text = "";
            lblGenreValue.AutoEllipsis = true;
            infoPanel.Controls.Add(lblGenreValue);
            currentY += labelHeight + spacing;

            lblDurationTitle = new Label();
            lblDurationTitle.Location = new Point(0, currentY);
            lblDurationTitle.Size = new Size(70, labelHeight);
            lblDurationTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblDurationTitle.ForeColor = textColor;
            lblDurationTitle.Text = "Duration:";
            infoPanel.Controls.Add(lblDurationTitle);

            lblDurationValue = new Label();
            lblDurationValue.Location = new Point(70, currentY);
            lblDurationValue.Size = new Size(60, labelHeight);
            lblDurationValue.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblDurationValue.ForeColor = textColor;
            lblDurationValue.Text = "";
            infoPanel.Controls.Add(lblDurationValue);
            currentY += labelHeight + spacing;

            lblTheaterTitle = new Label();
            lblTheaterTitle.Location = new Point(0, currentY);
            lblTheaterTitle.Size = new Size(60, labelHeight);
            lblTheaterTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTheaterTitle.ForeColor = textColor;
            lblTheaterTitle.Text = "Theater:";
            infoPanel.Controls.Add(lblTheaterTitle);

            lblTheaterValue = new Label();
            lblTheaterValue.Location = new Point(60, currentY);
            lblTheaterValue.Size = new Size(70, labelHeight);
            lblTheaterValue.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTheaterValue.ForeColor = textColor;
            lblTheaterValue.Text = "";
            infoPanel.Controls.Add(lblTheaterValue);
            currentY += labelHeight + spacing;

            lblDescriptionTitle = new Label();
            lblDescriptionTitle.Location = new Point(15, 210);
            lblDescriptionTitle.Size = new Size(370, 20);
            lblDescriptionTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDescriptionTitle.ForeColor = textColor;
            lblDescriptionTitle.Text = "Description:";
            panelMovieInfo.Controls.Add(lblDescriptionTitle);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(15, 235);
            txtDescription.Size = new Size(370, 60);
            txtDescription.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            txtDescription.ForeColor = textColor;
            txtDescription.BackColor = Color.FromArgb(30, 40, 70);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Multiline = true;
            txtDescription.ReadOnly = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.WordWrap = true;
            txtDescription.Text = "";
            panelMovieInfo.Controls.Add(txtDescription);

            GroupBox ticketGroupBox = new GroupBox();
            ticketGroupBox.Location = new Point(15, 305);
            ticketGroupBox.Size = new Size(370, 150);
            ticketGroupBox.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            ticketGroupBox.ForeColor = accentColor;
            ticketGroupBox.Text = "Ticket Information";
            ticketGroupBox.BackColor = Color.FromArgb(25, 35, 65);
            panelMovieInfo.Controls.Add(ticketGroupBox);

            int gbY = 25;
            int gbX = 15;
            int gbLabelWidth = 120;
            int gbValueWidth = 225;

            Label lblSeatsTitle = new Label();
            lblSeatsTitle.Location = new Point(gbX, gbY);
            lblSeatsTitle.Size = new Size(gbLabelWidth, 20);
            lblSeatsTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSeatsTitle.ForeColor = textColor;
            lblSeatsTitle.Text = "Selected Seats:";
            ticketGroupBox.Controls.Add(lblSeatsTitle);

            lblTicketCount = new Label();
            lblTicketCount.Location = new Point(gbX + gbLabelWidth, gbY);
            lblTicketCount.Size = new Size(gbValueWidth, 20);
            lblTicketCount.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketCount.ForeColor = textColor;
            lblTicketCount.Text = "0";
            ticketGroupBox.Controls.Add(lblTicketCount);
            gbY += 25;

            Label lblTicketPriceTitle = new Label();
            lblTicketPriceTitle.Location = new Point(gbX, gbY);
            lblTicketPriceTitle.Size = new Size(gbLabelWidth, 20);
            lblTicketPriceTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTicketPriceTitle.ForeColor = textColor;
            lblTicketPriceTitle.Text = "Price per Ticket:";
            ticketGroupBox.Controls.Add(lblTicketPriceTitle);

            lblTicketPrice = new Label();
            lblTicketPrice.Location = new Point(gbX + gbLabelWidth, gbY);
            lblTicketPrice.Size = new Size(gbValueWidth, 20);
            lblTicketPrice.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketPrice.ForeColor = textColor;
            lblTicketPrice.Text = "$0.00";
            ticketGroupBox.Controls.Add(lblTicketPrice);
            gbY += 25;

            Label lblSubtotalTitle = new Label();
            lblSubtotalTitle.Location = new Point(gbX, gbY);
            lblSubtotalTitle.Size = new Size(gbLabelWidth, 20);
            lblSubtotalTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSubtotalTitle.ForeColor = textColor;
            lblSubtotalTitle.Text = "Subtotal:";
            ticketGroupBox.Controls.Add(lblSubtotalTitle);

            lblTicketTotal = new Label();
            lblTicketTotal.Location = new Point(gbX + gbLabelWidth, gbY);
            lblTicketTotal.Size = new Size(gbValueWidth, 20);
            lblTicketTotal.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblTicketTotal.ForeColor = textColor;
            lblTicketTotal.Text = "$0.00";
            ticketGroupBox.Controls.Add(lblTicketTotal);
            gbY += 25;

            lblDiscountTitle = new Label();
            lblDiscountTitle.Location = new Point(gbX, gbY);
            lblDiscountTitle.Size = new Size(gbLabelWidth, 20);
            lblDiscountTitle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblDiscountTitle.ForeColor = textColor;
            lblDiscountTitle.Text = "Discount:";
            ticketGroupBox.Controls.Add(lblDiscountTitle);

            cboDiscount = new ComboBox();
            cboDiscount.Location = new Point(gbX + gbLabelWidth, gbY);
            cboDiscount.Size = new Size(gbValueWidth, 25);
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
            ticketGroupBox.Controls.Add(cboDiscount);
            gbY += 30;

            Label lblFinalTotalTitle = new Label();
            lblFinalTotalTitle.Location = new Point(gbX, gbY);
            lblFinalTotalTitle.Size = new Size(gbLabelWidth, 25);
            lblFinalTotalTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFinalTotalTitle.ForeColor = textColor;
            lblFinalTotalTitle.Text = "Total:";
            ticketGroupBox.Controls.Add(lblFinalTotalTitle);

            lblFinalTotal = new Label();
            lblFinalTotal.Location = new Point(gbX + gbLabelWidth, gbY);
            lblFinalTotal.Size = new Size(gbValueWidth, 25);
            lblFinalTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFinalTotal.ForeColor = accentColor;
            lblFinalTotal.Text = "$0.00";
            ticketGroupBox.Controls.Add(lblFinalTotal);

            GroupBox paymentGroupBox = new GroupBox();
            paymentGroupBox.Location = new Point(15, 465);
            paymentGroupBox.Size = new Size(370, 80);
            paymentGroupBox.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            paymentGroupBox.ForeColor = accentColor;
            paymentGroupBox.Text = "Payment Method";
            paymentGroupBox.BackColor = Color.FromArgb(25, 35, 65);
            panelMovieInfo.Controls.Add(paymentGroupBox);

            cboPaymentMethod = new ComboBox();
            cboPaymentMethod.Location = new Point(15, 30);
            cboPaymentMethod.Size = new Size(340, 35);
            cboPaymentMethod.Font = new Font("Segoe UI", 10, FontStyle.Regular);
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
            paymentGroupBox.Controls.Add(cboPaymentMethod);

            panelActions.Visible = false;

            if (pictureBoxPoster != null)
            {
                pictureBoxPoster.BringToFront();
            }
        }

        private void UpdateSummary()
        {
            decimal subtotal = 0;
            lblTicketCount.Text = selectedSeats.Count.ToString();

            foreach (string seatId in selectedSeats)
            {
                if (seatButtons.ContainsKey(seatId))
                {
                    string categoryId = seatButtons[seatId].Tag.ToString();
                    decimal seatPrice = ticketPrice;

                    if (seatCategories.ContainsKey(categoryId))
                    {
                        seatPrice *= seatCategories[categoryId].PriceMultiplier;
                    }

                    subtotal += seatPrice;
                }
            }

            lblTicketPrice.Text = "$" + ticketPrice.ToString("0.00");
            lblTicketTotal.Text = "$" + subtotal.ToString("0.00");

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

            lblFinalTotal.Text = "$" + finalTotal.ToString("0.00");
            lblTotalPriceValue.Text = "$" + finalTotal.ToString("0.00");
        }

        private void CboDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        private async Task<bool> TryConnectToDatabaseAsync()
        {
            string[] possibleConnectionStrings = new string[]
            {
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=localhost;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=.\SQLEXPRESS;Initial Catalog=CinemaDB;Integrated Security=True",
                @"Data Source=127.0.0.1;Initial Catalog=CinemaDB;Integrated Security=True"
            };

            return await Task.Run(() =>
            {
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
            });
        }

        private async Task LoadSeatCategoriesAsync()
        {
            Dictionary<string, SeatCategoryInfo> tempCategories = new Dictionary<string, SeatCategoryInfo>();
            List<string> categoryItems = new List<string>();

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT CategoryID, CategoryName, PriceMultiplier FROM SeatCategory ORDER BY PriceMultiplier";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            categoryItems.Add("-- All Categories --");

                            while (reader.Read())
                            {
                                string categoryId = reader["CategoryID"].ToString();
                                string categoryName = reader["CategoryName"].ToString();
                                decimal priceMultiplier = Convert.ToDecimal(reader["PriceMultiplier"]);
                                Color categoryColor;

                                switch (categoryName)
                                {
                                    case "VIP":
                                        categoryColor = vipSeatColor;
                                        break;
                                    case "Premium":
                                        categoryColor = premiumSeatColor;
                                        break;
                                    default:
                                        categoryColor = standardSeatColor;
                                        break;
                                }

                                tempCategories.Add(categoryId, new SeatCategoryInfo(
                                    categoryId,
                                    categoryName,
                                    priceMultiplier,
                                    categoryColor));

                                categoryItems.Add($"{categoryName} (x{priceMultiplier:F2})");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        LoadSampleSeatCategories();
                        return;
                    }
                }
            });

            if (tempCategories.Count > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    seatCategories = tempCategories;
                    cboSeatCategory.Items.Clear();
                    cboSeatCategory.Items.AddRange(categoryItems.ToArray());
                    cboSeatCategory.SelectedIndex = 0;
                });
            }
        }

        private async Task LoadSeatCategoryMappingAsync()
        {
            string theaterId = selectedTheaterId;
            if (string.IsNullOrEmpty(theaterId))
                return;

            Dictionary<string, string> tempMapping = new Dictionary<string, string>();

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"
                            SELECT SeatID, CategoryID 
                            FROM Seat 
                            WHERE TheaterID = @TheaterID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@TheaterID", theaterId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string seatId = reader["SeatID"].ToString();
                                    string categoryId = reader["CategoryID"].ToString();
                                    tempMapping[seatId] = categoryId;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        CreateSampleSeatMapping(tempMapping);
                    }
                }
            });

            seatCategoryMapping = tempMapping;
        }

        private void CreateSampleSeatMapping(Dictionary<string, string> mapping)
        {
            mapping.Clear();
            for (int row = 0; row < ROWS; row++)
            {
                char rowLetter = ROW_LETTERS[row];

                string categoryId;
                if (row < 2)
                    categoryId = "SC003";
                else if (row < 4)
                    categoryId = "SC002";
                else
                    categoryId = "SC001";

                for (int col = 1; col <= SEATS_PER_ROW; col++)
                {
                    string seatId = rowLetter.ToString() + col.ToString("00");
                    mapping[seatId] = categoryId;
                }
            }
        }

        private void LoadSampleSeatCategories()
        {
            seatCategories.Clear();
            seatCategories.Add("SC001", new SeatCategoryInfo("SC001", "Standard", 1.0m, standardSeatColor));
            seatCategories.Add("SC002", new SeatCategoryInfo("SC002", "Premium", 1.2m, premiumSeatColor));
            seatCategories.Add("SC003", new SeatCategoryInfo("SC003", "VIP", 1.5m, vipSeatColor));

            this.Invoke((MethodInvoker)delegate
            {
                cboSeatCategory.Items.Clear();
                cboSeatCategory.Items.Add("-- All Categories --");
                cboSeatCategory.Items.Add("Standard (x1.00)");
                cboSeatCategory.Items.Add("Premium (x1.20)");
                cboSeatCategory.Items.Add("VIP (x1.50)");
                cboSeatCategory.SelectedIndex = 0;
            });

            Dictionary<string, string> mapping = new Dictionary<string, string>();
            CreateSampleSeatMapping(mapping);
            seatCategoryMapping = mapping;
        }

        private async Task LoadAllTheatersAsync()
        {
            DataTable theatersTable = new DataTable();
            theatersTable.Columns.Add("TheaterID", typeof(string));
            theatersTable.Columns.Add("TheaterName", typeof(string));
            theatersTable.Columns.Add("TheaterType", typeof(string));

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT TheaterID, TheaterName, TheaterType FROM Theater ORDER BY TheaterName";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(theatersTable);
                        }
                    }
                    catch (Exception)
                    {
                        LoadSampleTheatersData(theatersTable);
                    }
                }
            });

            this.Invoke((MethodInvoker)delegate
            {
                allTheatersTable = theatersTable;

                if (theatersTable.Rows.Count > 0)
                {
                    DataTable displayTable = theatersTable.Copy();
                    DataRow newRow = displayTable.NewRow();
                    newRow["TheaterID"] = DBNull.Value;
                    newRow["TheaterName"] = "-- Select a theater --";
                    newRow["TheaterType"] = "";
                    displayTable.Rows.InsertAt(newRow, 0);

                    cboTheaters.DataSource = displayTable;
                    cboTheaters.DisplayMember = "TheaterName";
                    cboTheaters.ValueMember = "TheaterID";
                    cboTheaters.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No theaters found in the database.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSampleTheaters();
                }
            });
        }

        private void LoadSampleTheatersData(DataTable theatersTable)
        {
            theatersTable.Rows.Add("T001", "Theater 1", "Standard");
            theatersTable.Rows.Add("T002", "Theater 2", "Standard");
            theatersTable.Rows.Add("T003", "Theater 3", "VIP");
            theatersTable.Rows.Add("T004", "Theater 4", "IMAX");
            theatersTable.Rows.Add("T005", "Theater 5", "Standard");
            theatersTable.Rows.Add("T006", "Theater 6", "4DX");
        }

        private void LoadSampleTheaters()
        {
            allTheatersTable = new DataTable();
            allTheatersTable.Columns.Add("TheaterID", typeof(string));
            allTheatersTable.Columns.Add("TheaterName", typeof(string));
            allTheatersTable.Columns.Add("TheaterType", typeof(string));

            LoadSampleTheatersData(allTheatersTable);

            DataTable displayTable = allTheatersTable.Copy();
            DataRow newRow = displayTable.NewRow();
            newRow["TheaterID"] = DBNull.Value;
            newRow["TheaterName"] = "-- Select a theater --";
            newRow["TheaterType"] = "";
            displayTable.Rows.InsertAt(newRow, 0);

            cboTheaters.DataSource = displayTable;
            cboTheaters.DisplayMember = "TheaterName";
            cboTheaters.ValueMember = "TheaterID";
            cboTheaters.SelectedIndex = 0;
        }

        private async Task LoadMoviesAsync()
        {
            DataTable moviesTable = new DataTable();
            moviesTable.Columns.Add("MovieID", typeof(string));
            moviesTable.Columns.Add("Title", typeof(string));

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT MovieID, Title FROM Movie WHERE Status = 'Active' ORDER BY Title";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(moviesTable);
                        }
                    }
                    catch (Exception)
                    {
                        LoadSampleMoviesData(moviesTable);
                    }
                }
            });

            this.Invoke((MethodInvoker)delegate
            {
                if (moviesTable.Rows.Count > 0)
                {
                    DataRow newRow = moviesTable.NewRow();
                    newRow["MovieID"] = DBNull.Value;
                    newRow["Title"] = "-- Select a movie --";
                    moviesTable.Rows.InsertAt(newRow, 0);

                    cboMovies.DataSource = moviesTable;
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
            });
        }

        private async Task<MovieInfo> GetMovieDetailsAsync(string movieID)
        {
            MovieInfo info = new MovieInfo();

            await Task.Run(() =>
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            SELECT Title, Genre, Duration, Description, PosterURL
                            FROM Movie
                            WHERE MovieID = @MovieID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MovieID", movieID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    info.Title = reader["Title"].ToString();
                                    info.Genre = reader["Genre"].ToString();
                                    info.Duration = Convert.ToInt32(reader["Duration"]);
                                    info.Description = reader["Description"].ToString();
                                    info.PosterURL = reader["PosterURL"] != DBNull.Value ? reader["PosterURL"].ToString() : "";
                                }
                                else
                                {
                                    info = GetSampleMovieDetails(movieID);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    info = GetSampleMovieDetails(movieID);
                }
            });

            return info;
        }

        private MovieInfo GetSampleMovieDetails(string movieID)
        {
            MovieInfo info = new MovieInfo();

            if (movieID == "M001")
            {
                info.Title = "Inception";
                info.Genre = "Science Fiction, Action";
                info.Duration = 148;
                info.Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg";
            }
            else if (movieID == "M002")
            {
                info.Title = "Avatar: The Way of Water";
                info.Genre = "Adventure, Sci-Fi";
                info.Duration = 192;
                info.Description = "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora. Once a familiar threat returns to finish what was previously started, Jake must work with Neytiri and the army of the Na'vi race to protect their home.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg";
            }
            else if (movieID == "M003")
            {
                info.Title = "The Batman";
                info.Genre = "Action, Crime, Drama";
                info.Duration = 176;
                info.Description = "When the Riddler, a sadistic serial killer, begins murdering key political figures in Gotham, Batman is forced to investigate the city's hidden corruption and question his family's involvement.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/74xTEgt7R36Fpooo50r9T25onhq.jpg";
            }
            else if (movieID == "M004")
            {
                info.Title = "Interstellar";
                info.Genre = "Adventure, Drama, Sci-Fi";
                info.Duration = 169;
                info.Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg";
            }
            else if (movieID == "M007")
            {
                info.Title = "Parasite";
                info.Genre = "Drama, Thriller";
                info.Duration = 132;
                info.Description = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg";
            }
            else if (movieID == "M006")
            {
                info.Title = "Everything Everywhere All at Once";
                info.Genre = "Adventure, Comedy, Sci-Fi";
                info.Duration = 139;
                info.Description = "An aging Chinese immigrant is swept up in an insane adventure, where she alone can save the world by exploring other universes connecting with the lives she could have led.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/w3LxiVYdWWRvEVdn5RYq6jIqkb1.jpg";
            }
            else
            {
                info.Title = "John Wick: Chapter 4";
                info.Genre = "Action, Crime, Thriller";
                info.Duration = 169;
                info.Description = "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, he must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.";
                info.PosterURL = "https://image.tmdb.org/t/p/w500/vZloFAK7NmvMGKE7VkF5UHaz0I.jpg";
            }

            return info;
        }

        private void LoadMoviePosterAsync(string posterURL)
        {
            if (string.IsNullOrEmpty(posterURL))
            {
                CreatePlaceholderImage();
                return;
            }

            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("User-Agent", "Mozilla/5.0");
                            byte[] data = client.DownloadData(posterURL);
                            using (MemoryStream ms = new MemoryStream(data))
                            {
                                Image posterImage = Image.FromStream(ms);
                                if (!IsDisposed && pictureBoxPoster != null)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        if (!IsDisposed && pictureBoxPoster != null)
                                        {
                                            pictureBoxPoster.Image?.Dispose();
                                            pictureBoxPoster.Image = new Bitmap(posterImage);
                                            pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch
                    {
                        if (!IsDisposed && pictureBoxPoster != null)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                if (!IsDisposed && pictureBoxPoster != null)
                                {
                                    CreatePlaceholderImage();
                                }
                            });
                        }
                    }
                });
            }
            catch
            {
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

                pictureBoxPoster.Image?.Dispose();
                pictureBoxPoster.Image = bmp;
                pictureBoxPoster.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating placeholder image: {ex.Message}");
                pictureBoxPoster.Image = null;
            }
        }

        private async Task<DataTable> GetAllShowTimesAsync(string movieID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ShowID", typeof(string));
            dataTable.Columns.Add("ShowTimeDisplay", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("TheaterID", typeof(string));
            dataTable.Columns.Add("TheaterName", typeof(string));

            if (string.IsNullOrEmpty(movieID) || movieID == DBNull.Value.ToString())
                return dataTable;

            await Task.Run(() =>
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            SELECT s.ShowID, 
                                   CONVERT(VARCHAR, s.ShowTime, 103) + ' ' + CONVERT(VARCHAR, s.ShowTime, 108) AS ShowTimeDisplay,
                                   s.Price,
                                   t.TheaterID,
                                   t.TheaterName
                            FROM Show s
                            JOIN Theater t ON s.TheaterID = t.TheaterID
                            WHERE s.MovieID = @MovieID AND s.Status = 'Active'
                            ORDER BY s.ShowTime";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MovieID", movieID);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting showtimes: {ex.Message}");
                    GetSampleAllShowtimes(dataTable, movieID);
                }
            });

            return dataTable;
        }

        private void GetSampleAllShowtimes(DataTable showtimesTable, string movieID)
        {
            DateTime today = DateTime.Today;

            if (movieID == "M001")
            {
                showtimesTable.Rows.Add("S001", today.AddDays(1).ToString("dd/MM/yyyy") + " 10:00", 12.50m, "T001", "Theater 1");
                showtimesTable.Rows.Add("S002", today.AddDays(1).ToString("dd/MM/yyyy") + " 14:30", 15.00m, "T001", "Theater 1");
                showtimesTable.Rows.Add("S003", today.AddDays(1).ToString("dd/MM/yyyy") + " 19:45", 18.00m, "T001", "Theater 1");
            }
            else if (movieID == "M002")
            {
                showtimesTable.Rows.Add("S004", today.AddDays(1).ToString("dd/MM/yyyy") + " 11:30", 12.50m, "T002", "Theater 2");
                showtimesTable.Rows.Add("S005", today.AddDays(1).ToString("dd/MM/yyyy") + " 15:00", 15.00m, "T002", "Theater 2");
                showtimesTable.Rows.Add("S006", today.AddDays(1).ToString("dd/MM/yyyy") + " 20:15", 18.00m, "T002", "Theater 2");
            }
            else if (movieID == "M003")
            {
                showtimesTable.Rows.Add("S007", today.AddDays(1).ToString("dd/MM/yyyy") + " 12:15", 12.50m, "T003", "Theater 3");
                showtimesTable.Rows.Add("S008", today.AddDays(1).ToString("dd/MM/yyyy") + " 16:30", 15.00m, "T003", "Theater 3");
                showtimesTable.Rows.Add("S009", today.AddDays(1).ToString("dd/MM/yyyy") + " 21:00", 18.00m, "T003", "Theater 3");
            }
            else if (movieID == "M004")
            {
                showtimesTable.Rows.Add("S010", today.AddDays(1).ToString("dd/MM/yyyy") + " 13:00", 12.50m, "T004", "Theater 4");
                showtimesTable.Rows.Add("S011", today.AddDays(1).ToString("dd/MM/yyyy") + " 17:30", 15.00m, "T004", "Theater 4");
                showtimesTable.Rows.Add("S012", today.AddDays(1).ToString("dd/MM/yyyy") + " 21:45", 18.00m, "T004", "Theater 4");
            }
            else if (movieID == "M006")
            {
                showtimesTable.Rows.Add("S013", today.AddDays(1).ToString("dd/MM/yyyy") + " 10:30", 12.50m, "T006", "Theater 6");
                showtimesTable.Rows.Add("S014", today.AddDays(1).ToString("dd/MM/yyyy") + " 14:45", 15.00m, "T006", "Theater 6");
                showtimesTable.Rows.Add("S015", today.AddDays(1).ToString("dd/MM/yyyy") + " 19:15", 18.00m, "T006", "Theater 6");
            }
            else if (movieID == "M007")
            {
                showtimesTable.Rows.Add("S016", today.AddDays(1).ToString("dd/MM/yyyy") + " 11:30", 12.50m, "T007", "Theater 7");
                showtimesTable.Rows.Add("S017", today.AddDays(1).ToString("dd/MM/yyyy") + " 16:00", 15.00m, "T007", "Theater 7");
                showtimesTable.Rows.Add("S018", today.AddDays(1).ToString("dd/MM/yyyy") + " 20:15", 17.50m, "T007", "Theater 7");
            }
            else
            {
                showtimesTable.Rows.Add("S019", today.AddDays(1).ToString("dd/MM/yyyy") + " 14:15", 12.50m, "T005", "Theater 5");
                showtimesTable.Rows.Add("S020", today.AddDays(1).ToString("dd/MM/yyyy") + " 18:00", 15.00m, "T005", "Theater 5");
                showtimesTable.Rows.Add("S021", today.AddDays(1).ToString("dd/MM/yyyy") + " 22:30", 18.00m, "T005", "Theater 5");
            }
        }

        private void LoadSampleMoviesData(DataTable moviesTable)
        {
            moviesTable.Rows.Add("M001", "Inception");
            moviesTable.Rows.Add("M002", "Avatar: The Way of Water");
            moviesTable.Rows.Add("M003", "The Batman");
            moviesTable.Rows.Add("M004", "Interstellar");
            moviesTable.Rows.Add("M005", "John Wick: Chapter 4");
            moviesTable.Rows.Add("M006", "Everything Everywhere All at Once");
            moviesTable.Rows.Add("M007", "Parasite");
        }

        private void LoadSampleData()
        {
            DataTable moviesTable = new DataTable();
            moviesTable.Columns.Add("MovieID", typeof(string));
            moviesTable.Columns.Add("Title", typeof(string));

            moviesTable.Rows.Add(DBNull.Value, "-- Select a movie --");
            LoadSampleMoviesData(moviesTable);

            cboMovies.DataSource = moviesTable;
            cboMovies.DisplayMember = "Title";
            cboMovies.ValueMember = "MovieID";

            LoadSampleTheaters();
            LoadSampleSeatCategories();
        }

        private void InitializeSeatLayout()
        {
            panelSeats.SuspendLayout();
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

                string categoryID;
                Color seatColor;

                if (row < 2)
                {
                    categoryID = "SC003";
                    seatColor = vipSeatColor;
                }
                else if (row < 4)
                {
                    categoryID = "SC002";
                    seatColor = premiumSeatColor;
                }
                else
                {
                    categoryID = "SC001";
                    seatColor = standardSeatColor;
                }

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

            panelSeats.ResumeLayout();
            UpdateSeatLegend();
            ApplySeatCategoryFilter();
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

            string[] legendTypes = new string[] { "Standard", "Premium", "VIP", "Selected", "Booked" };
            Color[] legendColors = new Color[] {
                standardSeatColor,
                premiumSeatColor,
                vipSeatColor,
                selectedSeatColor,
                bookedSeatColor
            };

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
                string categoryId = btnSeat.Tag.ToString();
                if (seatCategories.ContainsKey(categoryId))
                {
                    btnSeat.BackColor = seatCategories[categoryId].SeatColor;
                }
                else
                {
                    btnSeat.BackColor = availableSeatColor;
                }
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

        private void ApplySeatCategoryFilter()
        {
            if (seatButtons.Count == 0)
                return;

            panelSeats.SuspendLayout();

            if (cboSeatCategory.SelectedIndex == 0 || chkShowAllCategories.Checked)
            {
                foreach (Button btnSeat in seatButtons.Values)
                {
                    btnSeat.Visible = true;
                }
            }
            else
            {
                string selectedCategoryName = cboSeatCategory.SelectedItem.ToString().Split(' ')[0];
                string selectedCategoryId = "";

                foreach (var category in seatCategories.Values)
                {
                    if (category.CategoryName == selectedCategoryName)
                    {
                        selectedCategoryId = category.CategoryID;
                        break;
                    }
                }

                foreach (Button btnSeat in seatButtons.Values)
                {
                    string categoryId = btnSeat.Tag.ToString();
                    btnSeat.Visible = (categoryId == selectedCategoryId);
                }
            }

            panelSeats.ResumeLayout();
        }

        private void chkShowAllCategories_CheckedChanged(object sender, EventArgs e)
        {
            cboSeatCategory.Enabled = !chkShowAllCategories.Checked;
            ApplySeatCategoryFilter();
        }

        private void cboSeatCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySeatCategoryFilter();
        }

        private async Task LoadBookedSeatsAsync(string showID)
        {
            if (string.IsNullOrEmpty(showID) || showID == DBNull.Value.ToString())
                return;

            foreach (KeyValuePair<string, Button> seat in seatButtons)
            {
                string categoryId = seat.Value.Tag.ToString();
                if (seatCategories.ContainsKey(categoryId))
                {
                    seat.Value.BackColor = seatCategories[categoryId].SeatColor;
                }
                else
                {
                    seat.Value.BackColor = availableSeatColor;
                }
                seat.Value.Enabled = true;
                seat.Value.ForeColor = Color.Black;
            }

            bookedSeats.Clear();
            List<string> tempBookedSeats = new List<string>();

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"SELECT sb.SeatID 
                               FROM SeatBooking sb
                               JOIN Booking b ON sb.BookingID = b.BookingID
                               WHERE b.ShowID = @ShowID AND b.Status = 'Active'";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string seatID = reader["SeatID"].ToString();
                                    tempBookedSeats.Add(seatID);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Random rnd = new Random();
                        int numSeatsToMark = rnd.Next(10, 30);

                        for (int i = 0; i < numSeatsToMark; i++)
                        {
                            char row = ROW_LETTERS[rnd.Next(ROWS)];
                            int col = rnd.Next(1, SEATS_PER_ROW + 1);
                            string seatID = row.ToString() + col.ToString("00");

                            if (!tempBookedSeats.Contains(seatID))
                            {
                                tempBookedSeats.Add(seatID);
                            }
                        }
                    }
                }
            });

            bookedSeats = tempBookedSeats;

            foreach (string seatID in bookedSeats)
            {
                if (seatButtons.ContainsKey(seatID))
                {
                    seatButtons[seatID].BackColor = bookedSeatColor;
                    seatButtons[seatID].Enabled = false;
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
            btnBook.Enabled = selectedSeats.Count > 0;

            UpdateSummary();
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
                    string categoryId = seatButtons[seatID].Tag.ToString();
                    if (seatCategories.ContainsKey(categoryId))
                    {
                        seatButtons[seatID].BackColor = seatCategories[categoryId].SeatColor;
                    }
                    else
                    {
                        seatButtons[seatID].BackColor = availableSeatColor;
                    }
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
            selectedTheaterId = "";
            selectedMovieId = "";
            lblTheaterValue.Text = "";

            if (cboMovies.DataSource != null)
            {
                cboMovies.SelectedIndexChanged -= cboMovies_SelectedIndexChanged;
                ((DataTable)cboMovies.DataSource).Clear();
                cboMovies.DataSource = null;
                cboMovies.Items.Clear();
                cboMovies.SelectedIndexChanged += cboMovies_SelectedIndexChanged;
            }

            if (cboTheaters.DataSource != null)
            {
                cboTheaters.SelectedIndexChanged -= cboTheaters_SelectedIndexChanged;
                ((DataTable)cboTheaters.DataSource).Clear();
                cboTheaters.DataSource = null;
                cboTheaters.Items.Clear();
                cboTheaters.SelectedIndexChanged += cboTheaters_SelectedIndexChanged;
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
            txtDescription.Text = "";
            pictureBoxPoster.Image = null;

            lblTicketCount.Text = "Selected Seats: 0";
            lblTicketPrice.Text = "Price per Ticket: $0.00";
            lblTicketTotal.Text = "Subtotal: $0.00";
            lblFinalTotal.Text = "Total: $0.00";
            cboDiscount.SelectedIndex = 0;
            btnBook.Enabled = false;

            cboSeatCategory.SelectedIndex = 0;
            chkShowAllCategories.Checked = true;

            foreach (Button btn in seatButtons.Values)
            {
                btn.BackColor = availableSeatColor;
                btn.Enabled = true;
                btn.ForeColor = Color.Black;
            }

            bookedSeats.Clear();
            allShowtimesTable = null;
        }

        private async void UpdateTheatersForMovie(string movieId)
        {
            if (string.IsNullOrEmpty(movieId) || movieId == DBNull.Value.ToString())
            {
                if (allTheatersTable != null)
                {
                    DataTable displayTable = allTheatersTable.Copy();
                    DataRow newRow = displayTable.NewRow();
                    newRow["TheaterID"] = DBNull.Value;
                    newRow["TheaterName"] = "-- Select a theater --";
                    newRow["TheaterType"] = "";
                    displayTable.Rows.InsertAt(newRow, 0);

                    cboTheaters.DataSource = displayTable;
                    cboTheaters.DisplayMember = "TheaterName";
                    cboTheaters.ValueMember = "TheaterID";
                    cboTheaters.SelectedIndex = 0;
                }
                return;
            }

            try
            {
                allShowtimesTable = await GetAllShowTimesAsync(movieId);

                if (allShowtimesTable.Rows.Count > 0)
                {
                    DataTable theatersWithShowtimes = new DataTable();
                    theatersWithShowtimes.Columns.Add("TheaterID", typeof(string));
                    theatersWithShowtimes.Columns.Add("TheaterName", typeof(string));
                    theatersWithShowtimes.Columns.Add("TheaterType", typeof(string));

                    DataRow blankRow = theatersWithShowtimes.NewRow();
                    blankRow["TheaterID"] = DBNull.Value;
                    blankRow["TheaterName"] = "-- Select a theater --";
                    blankRow["TheaterType"] = "";
                    theatersWithShowtimes.Rows.Add(blankRow);

                    var uniqueTheaters = allShowtimesTable.AsEnumerable()
                        .Select(r => new
                        {
                            TheaterID = r.Field<string>("TheaterID"),
                            TheaterName = r.Field<string>("TheaterName")
                        })
                        .Distinct()
                        .ToList();

                    foreach (var theater in uniqueTheaters)
                    {
                        string theaterType = "";

                        if (allTheatersTable != null)
                        {
                            var theaterRow = allTheatersTable.AsEnumerable()
                                .FirstOrDefault(r => r.Field<string>("TheaterID") == theater.TheaterID);

                            if (theaterRow != null)
                            {
                                theaterType = theaterRow.Field<string>("TheaterType");
                            }
                        }

                        DataRow newRow = theatersWithShowtimes.NewRow();
                        newRow["TheaterID"] = theater.TheaterID;
                        newRow["TheaterName"] = theater.TheaterName;
                        newRow["TheaterType"] = theaterType;
                        theatersWithShowtimes.Rows.Add(newRow);
                    }

                    cboTheaters.SelectedIndexChanged -= cboTheaters_SelectedIndexChanged;
                    cboTheaters.DataSource = theatersWithShowtimes;
                    cboTheaters.DisplayMember = "TheaterName";
                    cboTheaters.ValueMember = "TheaterID";
                    cboTheaters.SelectedIndex = 0;
                    cboTheaters.SelectedIndexChanged += cboTheaters_SelectedIndexChanged;

                    UpdateShowtimes();
                }
                else
                {
                    MessageBox.Show("No showtimes available for this movie.",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (cboShowTimes.DataSource != null)
                    {
                        cboShowTimes.SelectedIndexChanged -= cboShowTimes_SelectedIndexChanged;
                        ((DataTable)cboShowTimes.DataSource).Clear();
                        DataTable emptyTable = new DataTable();
                        emptyTable.Columns.Add("ShowID", typeof(string));
                        emptyTable.Columns.Add("ShowTimeDisplay", typeof(string));
                        emptyTable.Columns.Add("Price", typeof(decimal));
                        emptyTable.Columns.Add("TheaterName", typeof(string));
                        emptyTable.Rows.Add(DBNull.Value, "-- Select a showtime --", 0, "");
                        cboShowTimes.DataSource = emptyTable;
                        cboShowTimes.DisplayMember = "ShowTimeDisplay";
                        cboShowTimes.ValueMember = "ShowID";
                        cboShowTimes.SelectedIndex = 0;
                        cboShowTimes.SelectedIndexChanged += cboShowTimes_SelectedIndexChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating theaters: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateShowtimes()
        {
            try
            {
                if (allShowtimesTable == null || allShowtimesTable.Rows.Count == 0)
                {
                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("ShowID", typeof(string));
                    emptyTable.Columns.Add("ShowTimeDisplay", typeof(string));
                    emptyTable.Columns.Add("Price", typeof(decimal));
                    emptyTable.Columns.Add("TheaterName", typeof(string));
                    emptyTable.Rows.Add(DBNull.Value, "-- Select a showtime --", 0, "");

                    cboShowTimes.SelectedIndexChanged -= cboShowTimes_SelectedIndexChanged;
                    cboShowTimes.DataSource = emptyTable;
                    cboShowTimes.DisplayMember = "ShowTimeDisplay";
                    cboShowTimes.ValueMember = "ShowID";
                    cboShowTimes.SelectedIndex = 0;
                    cboShowTimes.SelectedIndexChanged += cboShowTimes_SelectedIndexChanged;
                    return;
                }

                DataTable filteredShowtimes = new DataTable();
                filteredShowtimes.Columns.Add("ShowID", typeof(string));
                filteredShowtimes.Columns.Add("ShowTimeDisplay", typeof(string));
                filteredShowtimes.Columns.Add("Price", typeof(decimal));
                filteredShowtimes.Columns.Add("TheaterName", typeof(string));

                DataRow blankRow = filteredShowtimes.NewRow();
                blankRow["ShowID"] = DBNull.Value;
                blankRow["ShowTimeDisplay"] = "-- Select a showtime --";
                blankRow["Price"] = 0;
                blankRow["TheaterName"] = "";
                filteredShowtimes.Rows.Add(blankRow);

                string theaterId = cboTheaters.SelectedValue != null && cboTheaters.SelectedValue != DBNull.Value ?
                    cboTheaters.SelectedValue.ToString() : "";

                if (!string.IsNullOrEmpty(theaterId))
                {
                    var rows = allShowtimesTable.AsEnumerable()
                        .Where(r => r.Field<string>("TheaterID") == theaterId)
                        .ToList();

                    foreach (var row in rows)
                    {
                        filteredShowtimes.ImportRow(row);
                    }
                }
                else
                {
                    foreach (DataRow row in allShowtimesTable.Rows)
                    {
                        filteredShowtimes.ImportRow(row);
                    }
                }

                cboShowTimes.SelectedIndexChanged -= cboShowTimes_SelectedIndexChanged;
                cboShowTimes.DataSource = filteredShowtimes;
                cboShowTimes.DisplayMember = "ShowTimeDisplay";
                cboShowTimes.ValueMember = "ShowID";
                cboShowTimes.SelectedIndex = 0;
                cboShowTimes.SelectedIndexChanged += cboShowTimes_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating showtimes: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cboMovies_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetSelectedSeats();

                if (cboMovies.SelectedValue != null && cboMovies.SelectedValue != DBNull.Value && cboMovies.SelectedIndex > 0)
                {
                    string movieId = cboMovies.SelectedValue.ToString();
                    selectedMovieId = movieId;

                    var movieDetailsTask = GetMovieDetailsAsync(movieId);
                    await movieDetailsTask;

                    var movieDetails = movieDetailsTask.Result;

                    movieTitle = movieDetails.Title;
                    movieGenre = movieDetails.Genre;
                    movieDuration = movieDetails.Duration;
                    movieDescription = movieDetails.Description;
                    moviePosterURL = movieDetails.PosterURL;

                    lblMovieInfoTitle.Text = movieDetails.Title;
                    lblGenreValue.Text = movieDetails.Genre;
                    lblDurationValue.Text = movieDetails.Duration + " min";
                    txtDescription.Text = movieDetails.Description;

                    lblMovieInfoTitle.Visible = true;
                    lblGenreValue.Visible = true;
                    lblDurationValue.Visible = true;
                    txtDescription.Visible = true;

                    LoadMoviePosterAsync(movieDetails.PosterURL);

                    UpdateTheatersForMovie(movieId);
                }
                else
                {
                    selectedMovieId = "";

                    if (allTheatersTable != null)
                    {
                        DataTable displayTable = allTheatersTable.Copy();
                        DataRow newRow = displayTable.NewRow();
                        newRow["TheaterID"] = DBNull.Value;
                        newRow["TheaterName"] = "-- Select a theater --";
                        newRow["TheaterType"] = "";
                        displayTable.Rows.InsertAt(newRow, 0);

                        cboTheaters.SelectedIndexChanged -= cboTheaters_SelectedIndexChanged;
                        cboTheaters.DataSource = displayTable;
                        cboTheaters.DisplayMember = "TheaterName";
                        cboTheaters.ValueMember = "TheaterID";
                        cboTheaters.SelectedIndex = 0;
                        cboTheaters.SelectedIndexChanged += cboTheaters_SelectedIndexChanged;
                    }

                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("ShowID", typeof(string));
                    emptyTable.Columns.Add("ShowTimeDisplay", typeof(string));
                    emptyTable.Columns.Add("Price", typeof(decimal));
                    emptyTable.Columns.Add("TheaterName", typeof(string));
                    emptyTable.Rows.Add(DBNull.Value, "-- Select a showtime --", 0, "");

                    cboShowTimes.SelectedIndexChanged -= cboShowTimes_SelectedIndexChanged;
                    cboShowTimes.DataSource = emptyTable;
                    cboShowTimes.DisplayMember = "ShowTimeDisplay";
                    cboShowTimes.ValueMember = "ShowID";
                    cboShowTimes.SelectedIndex = 0;
                    cboShowTimes.SelectedIndexChanged += cboShowTimes_SelectedIndexChanged;

                    lblMovieInfoTitle.Text = "";
                    lblGenreValue.Text = "";
                    lblDurationValue.Text = "";
                    txtDescription.Text = "";
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

        private async void cboTheaters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetSelectedSeats();

                if (cboTheaters.SelectedValue != null && cboTheaters.SelectedValue != DBNull.Value && cboTheaters.SelectedIndex > 0)
                {
                    selectedTheaterId = cboTheaters.SelectedValue.ToString();
                    DataRowView selectedRow = (DataRowView)cboTheaters.SelectedItem;
                    string theaterName = selectedRow["TheaterName"].ToString();
                    string theaterType = selectedRow["TheaterType"].ToString();
                    lblTheaterValue.Text = $"{theaterName} ({theaterType})";

                    await LoadSeatCategoryMappingAsync();
                    InitializeSeatLayout();
                    UpdateShowtimes();
                }
                else
                {
                    selectedTheaterId = "";
                    lblTheaterValue.Text = "";
                    UpdateShowtimes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting theater: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cboShowTimes_SelectedIndexChanged(object sender, EventArgs e)
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
                        lblTicketPrice.Text = "$" + ticketPrice.ToString("0.00");

                        await LoadBookedSeatsAsync(selectedShowID);
                        lblScreen.Visible = true;
                    }
                }
                else
                {
                    selectedShowID = "";
                    ticketPrice = 0;
                    lblPriceValue.Text = "$0.00";
                    lblTicketPrice.Text = "$0.00";
                    lblScreen.Visible = false;

                    foreach (KeyValuePair<string, Button> seat in seatButtons)
                    {
                        string categoryId = seat.Value.Tag.ToString();
                        if (seatCategories.ContainsKey(categoryId))
                        {
                            seat.Value.BackColor = seatCategories[categoryId].SeatColor;
                        }
                        else
                        {
                            seat.Value.BackColor = availableSeatColor;
                        }
                        seat.Value.Enabled = true;
                        seat.Value.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting showtime: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ResetAllFormData();
                bool connected = await TryConnectToDatabaseAsync();

                if (connected)
                {
                    await LoadSeatCategoriesAsync();
                    await LoadAllTheatersAsync();
                    await LoadMoviesAsync();
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
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal totalPrice = CalculateTotalPrice();
                string paymentMethod = cboPaymentMethod.SelectedItem.ToString();
                string theaterName = "";
                if (cboTheaters.SelectedItem is DataRowView selectedRow)
                {
                    theaterName = selectedRow["TheaterName"].ToString();
                }

                Dictionary<string, int> seatTypeCounts = new Dictionary<string, int>();
                foreach (string seatId in selectedSeats)
                {
                    if (seatButtons.ContainsKey(seatId))
                    {
                        string categoryId = seatButtons[seatId].Tag.ToString();
                        string categoryName = "Standard";

                        if (seatCategories.ContainsKey(categoryId))
                        {
                            categoryName = seatCategories[categoryId].CategoryName;
                        }

                        if (!seatTypeCounts.ContainsKey(categoryName))
                        {
                            seatTypeCounts[categoryName] = 0;
                        }

                        seatTypeCounts[categoryName]++;
                    }
                }

                string seatTypeInfo = "";
                foreach (var kvp in seatTypeCounts)
                {
                    seatTypeInfo += $"{kvp.Value} {kvp.Key}, ";
                }

                if (seatTypeInfo.EndsWith(", "))
                {
                    seatTypeInfo = seatTypeInfo.Substring(0, seatTypeInfo.Length - 2);
                }

                ShowConfirmationDialog(theaterName, seatTypeInfo, totalPrice, paymentMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking tickets: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SaveBookingToDatabase(string bookingId, string showId, string customerId,
                                          string discountId, string paymentMethodId, decimal totalAmount)
        {
            List<string> seatsToBook = new List<string>(selectedSeats);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string insertBookingQuery = @"
                                INSERT INTO Booking (BookingID, ShowID, CustomerID, BookingTime, DiscountID, 
                                        PaymentMethodID, TotalAmount, Status)
                                VALUES (@BookingID, @ShowID, @CustomerID, @BookingTime, @DiscountID, 
                                        @PaymentMethodID, @TotalAmount, 'Active')";

                            using (SqlCommand cmd = new SqlCommand(insertBookingQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                                cmd.Parameters.AddWithValue("@ShowID", showId);

                                if (string.IsNullOrEmpty(customerId))
                                    cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                                else
                                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                                cmd.Parameters.AddWithValue("@BookingTime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@DiscountID", discountId);
                                cmd.Parameters.AddWithValue("@PaymentMethodID", paymentMethodId);
                                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                                cmd.ExecuteNonQuery();
                            }

                            foreach (string seatId in seatsToBook)
                            {
                                string categoryId = "SC001";
                                if (seatCategoryMapping.ContainsKey(seatId))
                                {
                                    categoryId = seatCategoryMapping[seatId];
                                }

                                decimal multiplier = 1.0m;
                                if (seatCategories.ContainsKey(categoryId))
                                {
                                    multiplier = seatCategories[categoryId].PriceMultiplier;
                                }

                                decimal seatPrice = ticketPrice * multiplier;

                                using (SqlCommand cmd = new SqlCommand(
                                    "INSERT INTO SeatBooking (BookingID, SeatID, Price) VALUES (@BookingID, @SeatID, @Price)",
                                    conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                                    cmd.Parameters.AddWithValue("@SeatID", seatId);
                                    cmd.Parameters.AddWithValue("@Price", seatPrice);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Transaction error: " + ex.Message);
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database connection error: " + ex.Message);
                    return false;
                }
            }
        }

        private void ShowConfirmationDialog(string theaterName, string seatTypeInfo, decimal totalPrice, string paymentMethod)
        {
            Form confirmDialog = new Form();
            confirmDialog.Text = "Confirm Booking";
            confirmDialog.Size = new Size(600, 650);
            confirmDialog.StartPosition = FormStartPosition.CenterScreen;
            confirmDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            confirmDialog.MaximizeBox = false;
            confirmDialog.MinimizeBox = false;
            confirmDialog.BackColor = Color.FromArgb(20, 30, 60);
            string movieName = "Unknown Movie";
            try
            {
                if (cboMovies.SelectedIndex > 0)
                {
                    if (cboMovies.SelectedItem is DataRowView selectedMovie)
                    {
                        movieName = selectedMovie["Title"].ToString();
                    }
                }
                if (string.IsNullOrEmpty(movieName) || movieName == "Unknown Movie")
                {
                    movieName = movieTitle;
                }
                if (string.IsNullOrEmpty(movieName) || movieName == "Unknown Movie")
                {
                    movieName = cboMovies.Text;
                }
            }
            catch
            {
                movieName = "Unknown Movie";
            }

            Label titleLabel = new Label();
            titleLabel.Text = "Confirm Booking Details";
            titleLabel.Size = new Size(600, 70);
            titleLabel.Location = new Point(0, 0);
            titleLabel.BackColor = Color.FromArgb(30, 40, 70);
            titleLabel.ForeColor = Color.FromArgb(240, 200, 70);
            titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            confirmDialog.Controls.Add(titleLabel);

            Label lblMovie = new Label();
            lblMovie.Text = "Movie:";
            lblMovie.Size = new Size(150, 40);
            lblMovie.Location = new Point(40, 100);
            lblMovie.ForeColor = Color.White;
            lblMovie.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblMovie.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblMovie);

            Label lblMovieValue = new Label();
            lblMovieValue.Text = movieName;
            lblMovieValue.Size = new Size(370, 40);
            lblMovieValue.Location = new Point(200, 100);
            lblMovieValue.ForeColor = Color.White;
            lblMovieValue.Font = new Font("Segoe UI", 14);
            lblMovieValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblMovieValue);

            Label lblTheater = new Label();
            lblTheater.Text = "Theater:";
            lblTheater.Size = new Size(150, 40);
            lblTheater.Location = new Point(40, 150);
            lblTheater.ForeColor = Color.White;
            lblTheater.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTheater.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblTheater);

            Label lblTheaterValue = new Label();
            lblTheaterValue.Text = theaterName;
            lblTheaterValue.Size = new Size(370, 40);
            lblTheaterValue.Location = new Point(200, 150);
            lblTheaterValue.ForeColor = Color.White;
            lblTheaterValue.Font = new Font("Segoe UI", 14);
            lblTheaterValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblTheaterValue);

            Label lblShowtime = new Label();
            lblShowtime.Text = "Showtime:";
            lblShowtime.Size = new Size(150, 40);
            lblShowtime.Location = new Point(40, 200);
            lblShowtime.ForeColor = Color.White;
            lblShowtime.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblShowtime.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblShowtime);

            Label lblShowtimeValue = new Label();
            lblShowtimeValue.Text = cboShowTimes.Text;
            lblShowtimeValue.Size = new Size(370, 40);
            lblShowtimeValue.Location = new Point(200, 200);
            lblShowtimeValue.ForeColor = Color.White;
            lblShowtimeValue.Font = new Font("Segoe UI", 14);
            lblShowtimeValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblShowtimeValue);

            Label lblSeats = new Label();
            lblSeats.Text = "Seats:";
            lblSeats.Size = new Size(150, 40);
            lblSeats.Location = new Point(40, 250);
            lblSeats.ForeColor = Color.White;
            lblSeats.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblSeats.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblSeats);

            Label lblSeatsValue = new Label();
            lblSeatsValue.Text = string.Join(", ", selectedSeats);
            lblSeatsValue.Size = new Size(370, 40);
            lblSeatsValue.Location = new Point(200, 250);
            lblSeatsValue.ForeColor = Color.White;
            lblSeatsValue.Font = new Font("Segoe UI", 14);
            lblSeatsValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblSeatsValue);

            Label lblSeatTypes = new Label();
            lblSeatTypes.Text = "Seat Types:";
            lblSeatTypes.Size = new Size(150, 40);
            lblSeatTypes.Location = new Point(40, 300);
            lblSeatTypes.ForeColor = Color.White;
            lblSeatTypes.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblSeatTypes.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblSeatTypes);

            Label lblSeatTypesValue = new Label();
            lblSeatTypesValue.Text = seatTypeInfo;
            lblSeatTypesValue.Size = new Size(370, 40);
            lblSeatTypesValue.Location = new Point(200, 300);
            lblSeatTypesValue.ForeColor = Color.White;
            lblSeatTypesValue.Font = new Font("Segoe UI", 14);
            lblSeatTypesValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblSeatTypesValue);

            Label lblDiscount = new Label();
            lblDiscount.Text = "Discount:";
            lblDiscount.Size = new Size(150, 40);
            lblDiscount.Location = new Point(40, 350);
            lblDiscount.ForeColor = Color.White;
            lblDiscount.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblDiscount.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblDiscount);

            Label lblDiscountValue = new Label();
            lblDiscountValue.Text = cboDiscount.Text;
            lblDiscountValue.Size = new Size(370, 40);
            lblDiscountValue.Location = new Point(200, 350);
            lblDiscountValue.ForeColor = Color.White;
            lblDiscountValue.Font = new Font("Segoe UI", 14);
            lblDiscountValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblDiscountValue);

            Label lblPayment = new Label();
            lblPayment.Text = "Payment:";
            lblPayment.Size = new Size(150, 40);
            lblPayment.Location = new Point(40, 400);
            lblPayment.ForeColor = Color.White;
            lblPayment.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblPayment.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblPayment);

            Label lblPaymentValue = new Label();
            lblPaymentValue.Text = paymentMethod;
            lblPaymentValue.Size = new Size(370, 40);
            lblPaymentValue.Location = new Point(200, 400);
            lblPaymentValue.ForeColor = Color.White;
            lblPaymentValue.Font = new Font("Segoe UI", 14);
            lblPaymentValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblPaymentValue);

            Label lblTotalPrice = new Label();
            lblTotalPrice.Text = "Total Price:";
            lblTotalPrice.Size = new Size(150, 40);
            lblTotalPrice.Location = new Point(40, 450);
            lblTotalPrice.ForeColor = Color.White;
            lblTotalPrice.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTotalPrice.TextAlign = ContentAlignment.MiddleRight;
            confirmDialog.Controls.Add(lblTotalPrice);

            Label lblTotalPriceValue = new Label();
            lblTotalPriceValue.Text = "$" + totalPrice.ToString("0.00");
            lblTotalPriceValue.Size = new Size(370, 40);
            lblTotalPriceValue.Location = new Point(200, 450);
            lblTotalPriceValue.ForeColor = Color.FromArgb(240, 200, 70);
            lblTotalPriceValue.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTotalPriceValue.TextAlign = ContentAlignment.MiddleLeft;
            confirmDialog.Controls.Add(lblTotalPriceValue);

            Panel buttonPanel = new Panel();
            buttonPanel.Size = new Size(600, 100);
            buttonPanel.Location = new Point(0, 520);
            buttonPanel.BackColor = Color.FromArgb(25, 35, 65);
            confirmDialog.Controls.Add(buttonPanel);

            Button btnYes = new Button();
            btnYes.Text = "Yes";
            btnYes.Size = new Size(150, 50);
            btnYes.Location = new Point(150, 25);
            btnYes.BackColor = Color.FromArgb(52, 152, 219);
            btnYes.ForeColor = Color.White;
            btnYes.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnYes.FlatStyle = FlatStyle.Flat;
            btnYes.FlatAppearance.BorderSize = 0;
            btnYes.DialogResult = DialogResult.Yes;
            buttonPanel.Controls.Add(btnYes);

            Button btnNo = new Button();
            btnNo.Text = "No";
            btnNo.Size = new Size(150, 50);
            btnNo.Location = new Point(320, 25);
            btnNo.BackColor = Color.FromArgb(175, 35, 35);
            btnNo.ForeColor = Color.White;
            btnNo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnNo.FlatStyle = FlatStyle.Flat;
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.DialogResult = DialogResult.No;
            buttonPanel.Controls.Add(btnNo);

            if (confirmDialog.ShowDialog() == DialogResult.Yes)
            {
                string customerId = "C001";
                string discountId = "D00" + (cboDiscount.SelectedIndex + 1);
                string paymentMethodId = "PM00" + (cboPaymentMethod.SelectedIndex + 1);
                string bookingId = "B" + DateTime.Now.ToString("yyyyMMddHHmmss");

                try
                {
                    SaveBookingToDatabase(bookingId, selectedShowID, customerId, discountId, paymentMethodId, totalPrice);
                    MessageBox.Show($"Booking successful! Your booking ID is {bookingId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (string seatId in selectedSeats)
                    {
                        if (seatButtons.ContainsKey(seatId))
                        {
                            seatButtons[seatId].BackColor = bookedSeatColor;
                            seatButtons[seatId].Enabled = false;
                            bookedSeats.Add(seatId);
                        }
                    }

                    ResetSelectedSeats();

                    TicketForm ticketForm = new TicketForm(bookingId);
                    ticketForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during booking: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private decimal CalculateTotalPrice()
        {
            decimal subtotal = 0;
            foreach (string seatId in selectedSeats)
            {
                string categoryId = "SC001";
                if (seatButtons.ContainsKey(seatId) && seatButtons[seatId].Tag != null)
                {
                    categoryId = seatButtons[seatId].Tag.ToString();
                }

                decimal seatPrice = ticketPrice;
                if (seatCategories.ContainsKey(categoryId))
                {
                    seatPrice *= seatCategories[categoryId].PriceMultiplier;
                }

                subtotal += seatPrice;
            }

            decimal discountPercent = 0;
            switch (cboDiscount.SelectedIndex)
            {
                case 1: discountPercent = 0.10m; break;
                case 2: discountPercent = 0.15m; break;
                case 3: discountPercent = 0.20m; break;
                default: discountPercent = 0; break;
            }

            decimal discountAmount = subtotal * discountPercent;
            return subtotal - discountAmount;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}