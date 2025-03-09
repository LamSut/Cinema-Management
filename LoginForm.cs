using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Cinema
{
    public partial class LoginForm : Form
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

            int role = GetUserRole(empID, password);

            if (role == 0)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowsForm showForm = new ShowsForm();
                showForm.Show();
                this.Hide();
            }
            else if (role == 1)
            {
                MessageBox.Show("Admin login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminMainForm adminForm = new AdminMainForm();
                adminForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Employee ID or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetUserRole(string empID, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Employee WHERE EmployeeID = @EmployeeID AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 50).Value = empID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
