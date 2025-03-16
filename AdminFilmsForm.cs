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
    public partial class AdminFilmsForm: Form
    {
        public AdminFilmsForm()
        {
            InitializeComponent();
            LoadMovieData();
        }

        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";

        private void LoadMovieData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Movie";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewMovies.DataSource = dt;

                    // Tự động co giãn cột/hàng
                    dataGridViewMovies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewMovies.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridViewMovies.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn cả hàng

                    // Định dạng header
                    dataGridViewMovies.EnableHeadersVisualStyles = false;
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy; // Màu nền tiêu đề
                    dataGridViewMovies.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Màu chữ trắng

                    // Chỉnh font chữ và căn chỉnh nội dung
                    dataGridViewMovies.DefaultCellStyle.Font = new Font("Arial", 11);
                    dataGridViewMovies.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridViewMovies.DefaultCellStyle.ForeColor = Color.Black; // Màu chữ đen
                    dataGridViewMovies.DefaultCellStyle.BackColor = Color.WhiteSmoke; // Màu nền sáng

                    //Tạo hiệu ứng dòng xen kẽ (giống Excel)
                    dataGridViewMovies.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                    // Hiệu ứng hover (highlight khi rê chuột vào)
                    dataGridViewMovies.CellMouseEnter += (s, e) => {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewMovies.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    };
                    dataGridViewMovies.CellMouseLeave += (s, e) => {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewMovies.Rows[e.RowIndex].DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.WhiteSmoke : Color.LightGray;
                        }
                    };

                    // Tạo hiệu ứng chọn dòng (màu xanh đậm)
                    dataGridViewMovies.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                    dataGridViewMovies.DefaultCellStyle.SelectionForeColor = Color.White;

                    // Chỉnh tên cột hiển thị
                    dataGridViewMovies.Columns["MovieID"].HeaderText = "Title";
                    dataGridViewMovies.Columns["Title"].HeaderText = "Tên Phim";
                    dataGridViewMovies.Columns["Genre"].HeaderText = "Genre";

                    // Ẩn cột ID không cần thiết
                    dataGridViewMovies.Columns["MovieID"].Visible = false;

                    // Cho phép sắp xếp cột
                    foreach (DataGridViewColumn col in dataGridViewMovies.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }


        private void btnDeleteFilm_Click(object sender, EventArgs e)
        {
            if (dataGridViewMovies.SelectedRows.Count > 0) // Check if a row is selected
            {
                string movieId = dataGridViewMovies.SelectedRows[0].Cells["MovieID"].Value.ToString();

                // Show confirmation message
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridViewMovies_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string columnName = dataGridViewMovies.Columns[e.ColumnIndex].Name;
                if (columnName == "MovieID") return; // Không cho phép chỉnh sửa MovieID

                string newValue = dataGridViewMovies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string movieID = dataGridViewMovies.Rows[e.RowIndex].Cells["MovieID"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"UPDATE Movie SET {columnName} = @Value WHERE MovieID = @MovieID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Value", newValue);
                        cmd.Parameters.AddWithValue("@MovieID", movieID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Movie updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void AdminFilmsForm_Load(object sender, EventArgs e)
        {
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
                    txtMovieID.Text = "M" + newID.ToString("000"); // Format M001, S002, ...
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnAddFilm_Click(object sender, EventArgs e)
        {
            string movieID = txtMovieID.Text;
            string title = txtTitle.Text.Trim();
            string genre = txtGenre.Text.Trim();
            int duration = (int)numericDuration.Value;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(genre))
            {
                MessageBox.Show("Please fill in all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Movie (MovieID, Title, Duration, Genre) VALUES (@MovieID, @Title, @Duration, @Genre)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration;
                    cmd.Parameters.AddWithValue("@Genre", genre);

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

        private void btnEditFilm_Click(object sender, EventArgs e)
        {
        }
    }
}
