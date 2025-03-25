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
                    dataGridViewShows.Columns["Title"].HeaderText = "Movie";
                    dataGridViewShows.Columns["Genre"].HeaderText = "Genre";
                    dataGridViewShows.Columns["ShowTime"].HeaderText = "Time";
                    dataGridViewShows.Columns["TheaterName"].HeaderText = "Theater";

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
            LoadTheaterIDs();
            GenerateNewShowID();
        }

        private void dataGridViewShows_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string columnName = dataGridViewShows.Columns[e.ColumnIndex].Name;
                if (columnName == "ShowID") return; 

                string newValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                string showID = dataGridViewShows.Rows[e.RowIndex].Cells["ShowID"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(newValue) || string.IsNullOrWhiteSpace(showID))
                {
                    MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"UPDATE Show SET {columnName} = @Value WHERE ShowID = @ShowID", conn);

                    // Kiểm tra kiểu dữ liệu cho Price
                    if (columnName == "Price")
                    {
                        if (decimal.TryParse(newValue, out decimal price))
                        {
                            cmd.Parameters.AddWithValue("@Value", price);
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid decimal number for Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Value", newValue);
                    }

                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    cmd.ExecuteNonQuery();
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
        private void LoadTheaterIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TheaterID FROM Theater";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBoxTheaterID.DataSource = dt;
                    comboBoxTheaterID.DisplayMember = "TheaterID";
                    comboBoxTheaterID.ValueMember = "TheaterID";
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
            string showID = txtShowID.Text.Trim();
            string movieID = comboBoxMovieID.SelectedValue?.ToString();
            string theaterID = comboBoxTheaterID.SelectedValue?.ToString();
            DateTime showTime = dateTimePickerShowTime.Value;
            string priceText = txtPrice.Text.Trim();
            decimal price;

            // Kiểm tra dữ liệu hợp lệ
            //if (string.IsNullOrWhiteSpace(showID) || string.IsNullOrWhiteSpace(movieID) ||
            //    string.IsNullOrWhiteSpace(theaterID) || string.IsNullOrWhiteSpace(priceText))
            //{
            //    MessageBox.Show("Please fill in all required fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            if (!decimal.TryParse(priceText, out price))
            {
                MessageBox.Show("Please enter a valid price!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Show (ShowID, MovieID, TheaterID, ShowTime, Price, Status) 
                             VALUES (@ShowID, @MovieID, @TheaterID, @ShowTime, @Price, @Status)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                    cmd.Parameters.AddWithValue("@ShowTime", showTime);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Status", "Active");  

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Show added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadShowData();  
                        GenerateNewShowID();  
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


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dateTimePickerShowTime_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerShowTime.Format = DateTimePickerFormat.Custom;
            dateTimePickerShowTime.CustomFormat = "dd/MM/yyyy";
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxMovieID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
