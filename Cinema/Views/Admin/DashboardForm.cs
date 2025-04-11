using System;
using System.Windows.Forms;
using Cinema.Controllers.Admin;
using Cinema.Models;

namespace Cinema.Views.Admin
{
    public partial class DashboardForm : Form
    {
        private readonly DashboardController controller;

        public DashboardForm()
        {
            InitializeComponent();
            controller = new DashboardController();
            Load += DashboardForm_Load;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            Dashboard data = controller.GetDashboardData();
            lbCustomer.Text = data.TotalCustomers.ToString();
            lbBooking.Text = data.TotalBookings.ToString();
            lbMoney.Text = data.TotalRevenue.ToString("N0");
        }
    }
}
