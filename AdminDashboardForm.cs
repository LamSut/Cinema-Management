using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class AdminDashboardForm : Form
    {
        public AdminDashboardForm()
        {
            InitializeComponent();
            getTotalCustomer();
            getTotalBookings();
            getTotalMoney();
        }
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        public void getTotalCustomer()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(CustomerID) FROM Customer";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int totalCustomers = (int)cmd.ExecuteScalar();
                    Console.WriteLine("Tổng khách hàng là: " + totalCustomers);
                    lbCustomer.Text = totalCustomers.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
        public void getTotalBookings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(BookingID) FROM Booking;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int totalBookings = (int)cmd.ExecuteScalar();
                    lbBooking.Text = totalBookings.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
        public void getTotalMoney()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT SUM(TotalAmount) FROM Booking WHERE Status != 'Cancelled';";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    decimal totalMoney = (decimal)cmd.ExecuteScalar();
                    lbMoney.Text = totalMoney.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
    }
}
