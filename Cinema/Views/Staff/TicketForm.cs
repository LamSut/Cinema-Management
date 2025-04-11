using System;
using System.Windows.Forms;
using Cinema.Controllers.Staff;
using Cinema.Models;

namespace Cinema.Views.Staff
{
    public partial class TicketForm : Form
    {
        private readonly TicketController _controller;

        public TicketForm(string bookingID)
        {
            InitializeComponent();
            _controller = new TicketController();
            Load += (s, e) => LoadTicketData(bookingID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadTicketData(string bookingID)
        {
            Ticket info = _controller.GetTicketInfo(bookingID);

            if (info == null)
            {
                MessageBox.Show("Not found tickets!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblBookingID.Text = $"Booking ID: {info.BookingID}";
            lblShow.Text = $"Movie: {info.MovieTitle}";
            lblTheater.Text = $"Theater: {info.TheaterName}";
            lblTime.Text = $"Showtime: {info.ShowTime:dd/MM/yyyy HH:mm}";
            lblSeats.Text = $"Seats: {info.Seats} ({info.SeatCategory})";
            lblTotal.Text = $"Total Price: {info.TotalPrice:C}";
        }

        private void TicketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }
    }
}
