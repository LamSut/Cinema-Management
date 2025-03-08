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
    public partial class AdminShowsForm: Form
    {
        public AdminShowsForm()
        {
            InitializeComponent();
            LoadShowData();
        }

        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        private void LoadShowData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Show";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewShows.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void AdminShowsForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteFilm_Click(object sender, EventArgs e)
        {
            if (dataGridViewShows.SelectedRows.Count > 0) // Check if a row is selected
            {
                string showId = dataGridViewShows.SelectedRows[0].Cells["ShowID"].Value.ToString();

                // Show confirmation message
                DialogResult result = MessageBox.Show($"Are you sure you want to delete: {showId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteShow(showId);
                }
            }
            else
            {
                MessageBox.Show("Please select a show to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }  

        private void DeleteShow(String showId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Show WHERE ShowID = @ShowID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShowID", showId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Show deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadShowData(); // Refresh DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete show.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
