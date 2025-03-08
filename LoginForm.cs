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
    public partial class LoginForm: Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string empID = txtEmpID.Text.Trim();
            string password = txtPass.Text.Trim();

            if (AuthenticateUser(empID, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open Show form
                ShowsForm showForm = new ShowsForm();
                showForm.Show();

                // Hide the login window
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Employee ID or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool AuthenticateUser(string empID, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employee WHERE EmployeeID = @EmployeeID AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 50).Value = empID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
