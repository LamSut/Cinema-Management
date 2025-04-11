using System;
using System.Windows.Forms;
using Cinema.Controllers.Admin;

namespace Cinema.Views
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
            lbCustomer.Text = controller.GetTotalCustomers().ToString();
            lbBooking.Text = controller.GetTotalBookings().ToString();
            lbMoney.Text = controller.GetTotalRevenue().ToString("N0");
        }
    }
}
