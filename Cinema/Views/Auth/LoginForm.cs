using System;
using System.Windows.Forms;
using Cinema.Controllers.Auth;
using Cinema.Models;
using Cinema.Views.Admin;
using Cinema.Views.Staff;

namespace Cinema.Views.Auth
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string empID = txtEmpID.Text.Trim();
            string password = txtPass.Text.Trim();

            Employee employee = AuthController.Authenticate(empID, password);

            if (employee != null)
            {
                if (employee.Role == 0)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BookingForm bookingForm = new BookingForm();
                    bookingForm.Show();
                    this.Hide();
                }
                else if (employee.Role == 1)
                {
                    MessageBox.Show("Admin login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm adminForm = new MainForm();
                    adminForm.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Invalid Employee ID or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
