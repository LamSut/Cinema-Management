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
    public partial class AdminFilmsForm : Form
    {
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        private Dictionary<string, object> originalCellValues = new Dictionary<string, object>();

        public AdminFilmsForm()
        {
            InitializeComponent();
            LoadMovieData();
        }

        private void LoadMovieData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MovieID, Title, Genre, Duration, Director, ReleaseDate, PosterURL, Status FROM Movie";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewMovies.DataSource = dt;

                    dataGridViewMovies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewMovies.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridViewMovies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    dataGridViewMovies.EnableHeadersVisualStyles = false;
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    dataGridViewMovies.DefaultCellStyle.Font = new Font("Arial", 11);
                    dataGridViewMovies.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridViewMovies.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridViewMovies.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGridViewMovies.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                    dataGridViewMovies.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                    dataGridViewMovies.DefaultCellStyle.SelectionForeColor = Color.White;

                    dataGridViewMovies.Columns["ReleaseDate"].HeaderText = "Date";
                    dataGridViewMovies.Columns["MovieID"].Visible = false;

                    foreach (DataGridViewColumn col in dataGridViewMovies.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnAddFilm_Click(object sender, EventArgs e)
        {
            string movieID = txtMovieID.Text;
            string title = txtTitle.Text.Trim();
            string genre = txtGenre.Text.Trim();
            int duration = (int)numericDuration.Value;
            string description = txtDescription.Text.Trim();
            string director = txtDirector.Text.Trim();
            DateTime releaseDate = dateTimePickerRelease.Value;
            string posterURL = txtPosterURL.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(genre))
            {
                MessageBox.Show("Please fill in all required fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Movie (MovieID, Title, Genre, Duration, Description, Director, ReleaseDate, PosterURL) " +
                                   "VALUES (@MovieID, @Title, @Genre, @Duration, @Description, @Director, @ReleaseDate, @PosterURL)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Genre", genre);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Director", director);
                    cmd.Parameters.AddWithValue("@ReleaseDate", releaseDate);
                    cmd.Parameters.AddWithValue("@PosterURL", posterURL);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Movie added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMovieData();
                        GenerateNewMovieID();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add movie.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteFilm_Click(object sender, EventArgs e)
        {
            if (dataGridViewMovies.SelectedRows.Count > 0)
            {
                string movieId = dataGridViewMovies.SelectedRows[0].Cells["MovieID"].Value.ToString();
                DialogResult result = MessageBox.Show($"Are you sure you want to delete: {movieId}?",
                                                      "Confirm Deletion",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteMovie(movieId);
                }
            }
            else
            {
                MessageBox.Show("Please select a movie to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteMovie(string movieId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Movie WHERE MovieID = @MovieID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", movieId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Movie deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMovieData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete movie.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewMovies_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string key = $"{e.RowIndex}-{e.ColumnIndex}";
            if (!originalCellValues.ContainsKey(key))
            {
                originalCellValues[key] = dataGridViewMovies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void dataGridViewMovies_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string key = $"{e.RowIndex}-{e.ColumnIndex}";
                if (!originalCellValues.ContainsKey(key))
                    return;

                object originalValue = originalCellValues[key];
                object newValueObj = dataGridViewMovies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                string newValue = newValueObj?.ToString() ?? "";
                string originalValueStr = originalValue?.ToString() ?? "";


                if (newValue == originalValueStr)
                {
                    originalCellValues.Remove(key);
                    return;
                }

                string columnName = dataGridViewMovies.Columns[e.ColumnIndex].Name;
                if (columnName == "MovieID") return; 

                string movieID = dataGridViewMovies.Rows[e.RowIndex].Cells["MovieID"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"UPDATE Movie SET {columnName} = @Value WHERE MovieID = @MovieID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Value", newValue);
                        cmd.Parameters.AddWithValue("@MovieID", movieID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Movie updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                originalCellValues.Remove(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminFilmsForm_Load(object sender, EventArgs e)
        {
            dataGridViewMovies.CellBeginEdit += dataGridViewMovies_CellBeginEdit;
            dataGridViewMovies.CellEndEdit += dataGridViewMovies_CellEndEdit;
            GenerateNewMovieID();
        }

        private void GenerateNewMovieID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MAX(CAST(SUBSTRING(MovieID, 2, LEN(MovieID)-1) AS INT)) FROM Movie";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    int newID = (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 1;
                    txtMovieID.Text = "M" + newID.ToString("000"); // Format M001, M002, ...
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditFilm_Click(object sender, EventArgs e)
        {
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void txtTitle_TextChanged(object sender, EventArgs e) { }
        private void numericDuration_ValueChanged(object sender, EventArgs e) { }
        private void txtPosterURL_TextChanged(object sender, EventArgs e) { }
        private void txtDirector_TextChanged(object sender, EventArgs e) { }

        private void dateTimePickerRelease_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerRelease.Format = DateTimePickerFormat.Custom;
            dateTimePickerRelease.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePickerRelease.ShowUpDown = true;
        }

        private void txtGenre_TextChanged(object sender, EventArgs e) { }
        private void txtDescription_TextChanged(object sender, EventArgs e) { }
        private void txtMovieID_TextChanged(object sender, EventArgs e) { }
    }
}
