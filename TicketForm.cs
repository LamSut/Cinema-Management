using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;

namespace Cinema
{
    public partial class TicketForm : Form
    {
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        private string bookingID;

        public TicketForm(string bookingID)
        {
            InitializeComponent();
            this.bookingID = bookingID;
            LoadTicketData(bookingID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadTicketData(string bookingID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
    SELECT 
        sb.BookingID, 
        STRING_AGG(CONCAT(s.SeatRow, s.SeatNumber), ', ') AS Seats,
        sc.CategoryName AS SeatCategory,
        m.Title AS MovieTitle, -- Lấy tiêu đề phim thay vì MovieID
        sh.ShowTime, 
        t.TheaterName
    FROM SeatBooking sb
    JOIN Seat s ON sb.SeatID = s.SeatID
    JOIN SeatCategory sc ON s.CategoryID = sc.CategoryID
    JOIN Booking b ON sb.BookingID = b.BookingID
    JOIN Show sh ON b.ShowID = sh.ShowID
    JOIN Movie m ON sh.MovieID = m.MovieID
    JOIN Theater t ON sh.TheaterID = t.TheaterID
    WHERE sb.BookingID = @BookingID
    GROUP BY sb.BookingID, sc.CategoryName, m.Title, sh.ShowTime, t.TheaterName";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@BookingID", SqlDbType.NVarChar, 15).Value = bookingID;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblBookingID.Text = $"Booking ID: {reader["BookingID"]}";
                            lblShow.Text = $"Movie: {reader["MovieTitle"]}";
                            lblTheater.Text = $"Theater: {reader["TheaterName"]}";
                            lblTime.Text = $"Showtime: {Convert.ToDateTime(reader["ShowTime"]).ToString("dd/MM/yyyy HH:mm")}";
                            lblSeats.Text = $"Seats: {reader["Seats"]} ({reader["SeatCategory"]})";
                        }
                        else
                        {
                            MessageBox.Show("Not found tickets!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void TicketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }
    }
}

