using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
                    //Title,Genre,Duration,TheaterName, ShowTime,Price, 
                    string query = @"
                        SELECT s.ShowID, m.MovieId, m.Title,m.Genre, m.Duration, t.TheaterName, s.ShowTime, s.Price, s.Status 
                        FROM Show s
                        JOIN Movie m ON s.MovieID = m.MovieID
                        JOIN Theater t ON s.TheaterID = t.TheaterID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewShows.DataSource = dt;

                    // Tự động co giãn cột/hàng
                    dataGridViewShows.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewShows.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridViewShows.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn cả hàng

                    // Định dạng header
                    dataGridViewShows.EnableHeadersVisualStyles = false;
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy; // Màu nền tiêu đề
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Màu chữ trắng

                    // Chỉnh font chữ và căn chỉnh nội dung
                    dataGridViewShows.DefaultCellStyle.Font = new Font("Arial", 11);
                    dataGridViewShows.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridViewShows.DefaultCellStyle.ForeColor = Color.Black; // Màu chữ đen
                    dataGridViewShows.DefaultCellStyle.BackColor = Color.WhiteSmoke; // Màu nền sáng

                    //Tạo hiệu ứng dòng xen kẽ (giống Excel)
                    dataGridViewShows.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                    // Hiệu ứng hover (highlight khi rê chuột vào)
                    dataGridViewShows.CellMouseEnter += (s, e) => {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    };
                    dataGridViewShows.CellMouseLeave += (s, e) => {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.WhiteSmoke : Color.LightGray;
                        }
                    };

                    // Tạo hiệu ứng chọn dòng (màu xanh đậm)
                    dataGridViewShows.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                    dataGridViewShows.DefaultCellStyle.SelectionForeColor = Color.White;

                    // Chỉnh tên cột hiển thị
                    //dataGridViewShows.Columns["MovieID"].HeaderText = "Title";
                    dataGridViewShows.Columns["Title"].HeaderText = "Tên Phim";
                    dataGridViewShows.Columns["Genre"].HeaderText = "Genre";

                    // Ẩn cột ID không cần thiết
                    dataGridViewShows.Columns["MovieID"].Visible = false;
                    dataGridViewShows.Columns["ShowID"].Visible = false;

                    // Cho phép sắp xếp cột
                    foreach (DataGridViewColumn col in dataGridViewShows.Columns)
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

        private void AdminShowsForm_Load(object sender, EventArgs e)
        {
            dataGridViewShows.CellEndEdit += dataGridViewShows_CellEndEdit;
            LoadMovieIDs();
            GenerateNewShowID();
        }

        private void dataGridViewShows_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string columnName = dataGridViewShows.Columns[e.ColumnIndex].Name;
                if (columnName == "ShowID") return; // Không cho phép chỉnh sửa MovieID

                string newValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string showID = dataGridViewShows.Rows[e.RowIndex].Cells["ShowID"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"UPDATE Show SET {columnName} = @Value WHERE showID = @ShowID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Value", newValue);
                        cmd.Parameters.AddWithValue("@ShowID", showID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Show updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        LoadShowData(); 
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

        private void dataGridViewShows_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadMovieIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MovieID FROM Movie";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxMovieID.DataSource = dt;
                    comboBoxMovieID.DisplayMember = "MovieID";
                    comboBoxMovieID.ValueMember = "MovieID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerateNewShowID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MAX(CAST(SUBSTRING(ShowID, 2, LEN(ShowID)-1) AS INT)) FROM Show";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    int newID = (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 1;
                    txtShowID.Text = "S" + newID.ToString("000"); // Format M001, S002, ...
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnAddFilm_Click(object sender, EventArgs e)
        {
            {
                string showID = txtShowID.Text;
                string movieID = comboBoxMovieID.SelectedValue.ToString();
                string showTime = dateTimePickerShowTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string price = txtPrice.Text;

                if (string.IsNullOrWhiteSpace(price))
                {
                    MessageBox.Show("Please enter a price!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO Show (ShowID, MovieID, ShowTime, Price) VALUES (@ShowID, @MovieID, @ShowTime, @Price)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ShowID", showID);
                        cmd.Parameters.AddWithValue("@MovieID", movieID);
                        cmd.Parameters.AddWithValue("@ShowTime", showTime);
                        cmd.Parameters.AddWithValue("@Price", price);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Show added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadShowData(); // Refresh bảng DataGridView
                            GenerateNewShowID(); // Tạo ShowID mới
                        }
                        else
                        {
                            MessageBox.Show("Failed to add show.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dateTimePickerShowTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMovieID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
