using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Cinema
{
    public partial class AdminShowsForm : Form
    {
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";

        public AdminShowsForm()
        {
            InitializeComponent();
            LoadShowData();
        }

        private void LoadShowData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT s.ShowID, m.MovieID, m.Title, t.TheaterName, s.ShowTime, s.Price, s.Status
                        FROM Show s
                        JOIN Movie m ON s.MovieID = m.MovieID
                        JOIN Theater t ON s.TheaterID = t.TheaterID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewShows.DataSource = dt;

                    dataGridViewShows.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewShows.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridViewShows.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewShows.EnableHeadersVisualStyles = false;
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    dataGridViewShows.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridViewShows.DefaultCellStyle.Font = new Font("Arial", 11);
                    dataGridViewShows.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridViewShows.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridViewShows.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGridViewShows.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                    dataGridViewShows.CellMouseEnter += (s, e) =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    };

                    dataGridViewShows.CellMouseLeave += (s, e) =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                                e.RowIndex % 2 == 0 ? Color.WhiteSmoke : Color.LightGray;
                        }
                    };

                    dataGridViewShows.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                    dataGridViewShows.DefaultCellStyle.SelectionForeColor = Color.White;
                    dataGridViewShows.Columns["Title"].HeaderText = "Movie";
                    // Set Title column as read-only
                    dataGridViewShows.Columns["Title"].ReadOnly = true;
                    dataGridViewShows.Columns["ShowTime"].HeaderText = "Time";
                    dataGridViewShows.Columns["TheaterName"].HeaderText = "Theater";
                    dataGridViewShows.Columns["MovieID"].Visible = false;
                    dataGridViewShows.Columns["ShowID"].Visible = false;

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
            dataGridViewShows.CellBeginEdit += dataGridViewShows_CellBeginEdit;
            dataGridViewShows.CellEndEdit += dataGridViewShows_CellEndEdit;
            LoadMovieIDs();
            LoadTheaterIDs();
            GenerateNewShowID();
        }

        // Store the original cell value before editing begins.
        private void dataGridViewShows_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag =
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
        }

        // Compare new value with the original. If changed, attempt update.
        // If update fails, revert UI to the original value.
        private void dataGridViewShows_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridViewShows.Columns[e.ColumnIndex].Name;
            if (columnName == "ShowID") return;

            string newValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            string originalValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag?.ToString();
            string showID = dataGridViewShows.Rows[e.RowIndex].Cells["ShowID"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(newValue) || string.IsNullOrWhiteSpace(showID))
            {
                MessageBox.Show("Invalid data!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Revert UI to original value
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                return;
            }

            // If value has not changed, do nothing.
            if (newValue == originalValue) return;

            bool updateSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (columnName == "TheaterName")
                    {
                        string theaterID = null;
                        using (SqlCommand cmdLookup = new SqlCommand("SELECT TheaterID FROM Theater WHERE TheaterName = @Name", conn))
                        {
                            cmdLookup.Parameters.AddWithValue("@Name", newValue);
                            object result = cmdLookup.ExecuteScalar();
                            if (result == null)
                            {
                                MessageBox.Show("Theater name not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                                return;
                            }
                            theaterID = result.ToString();
                        }

                        using (SqlCommand cmdUpdate = new SqlCommand("UPDATE Show SET TheaterID = @Value WHERE ShowID = @ShowID", conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Value", theaterID);
                            cmdUpdate.Parameters.AddWithValue("@ShowID", showID);
                            int rowsAffected = cmdUpdate.ExecuteNonQuery();
                            updateSuccess = rowsAffected > 0;
                        }
                    }
                    else if (columnName == "Price")
                    {
                        if (decimal.TryParse(newValue, out decimal price))
                        {
                            using (SqlCommand cmdUpdate = new SqlCommand("UPDATE Show SET Price = @Value WHERE ShowID = @ShowID", conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("@Value", price);
                                cmdUpdate.Parameters.AddWithValue("@ShowID", showID);
                                int rowsAffected = cmdUpdate.ExecuteNonQuery();
                                updateSuccess = rowsAffected > 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid decimal number for Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                            return;
                        }
                    }
                    else if (columnName == "Title")
                    {
                        MessageBox.Show("Cannot edit this column directly. Please update Movie info.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                        return;
                    }
                    else if (columnName == "ShowTime")
                    {
                        // Attempt to parse user input in "dd/MM/yyyy HH:mm" format
                        if (!DateTime.TryParseExact(
                                newValue,
                                "dd/MM/yyyy HH:mm",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out DateTime parsedDate))
                        {
                            // Revert to old value if parsing fails
                            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                            MessageBox.Show("Invalid date/time format. Please use dd/MM/yyyy HH:mm.",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }

                        using (SqlCommand cmdUpdate = new SqlCommand("UPDATE Show SET ShowTime = @Value WHERE ShowID = @ShowID", conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Value", parsedDate);
                            cmdUpdate.Parameters.AddWithValue("@ShowID", showID);
                            int rowsAffected = cmdUpdate.ExecuteNonQuery();
                            updateSuccess = rowsAffected > 0;
                        }
                    }

                    else
                    {
                        using (SqlCommand cmdUpdate = new SqlCommand($"UPDATE Show SET {columnName} = @Value WHERE ShowID = @ShowID", conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Value", newValue);
                            cmdUpdate.Parameters.AddWithValue("@ShowID", showID);
                            int rowsAffected = cmdUpdate.ExecuteNonQuery();
                            updateSuccess = rowsAffected > 0;
                        }
                    }
                }

                if (updateSuccess)
                {
                    dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = newValue;
                    MessageBox.Show("Show updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Update failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
            }
        }

        private void btnDeleteFilm_Click(object sender, EventArgs e)
        {
            if (dataGridViewShows.SelectedRows.Count > 0)
            {
                string showId = dataGridViewShows.SelectedRows[0].Cells["ShowID"].Value.ToString();
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

        private void DeleteShow(string showId)
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

        private void LoadMovieIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Title FROM Movie";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBoxMovieTitle.DataSource = dt;
                    comboBoxMovieTitle.DisplayMember = "Title";
                    comboBoxMovieTitle.ValueMember = "Title";
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
                    txtShowID.Text = "S" + newID.ToString("000");
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
            string movieTitle = comboBoxMovieTitle.Text.Trim();
            string theaterID = comboBoxTheaterID.SelectedValue?.ToString();
            DateTime showTime = dateTimePickerShowTime.Value;
            string priceText = txtPrice.Text.Trim();
            decimal price;

            if (string.IsNullOrWhiteSpace(showID) || string.IsNullOrWhiteSpace(movieTitle) ||
                string.IsNullOrWhiteSpace(theaterID) || string.IsNullOrWhiteSpace(priceText))
            {
                MessageBox.Show("Please fill in all required fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                    string getMovieIDQuery = "SELECT MovieID FROM Movie WHERE Title = @Title";
                    SqlCommand getMovieIDCmd = new SqlCommand(getMovieIDQuery, conn);
                    getMovieIDCmd.Parameters.AddWithValue("@Title", movieTitle);
                    object movieIDObj = getMovieIDCmd.ExecuteScalar();
                    if (movieIDObj == null)
                    {
                        MessageBox.Show("Movie title not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string movieID = movieIDObj.ToString();
                    string insertQuery = "INSERT INTO Show (ShowID, MovieID, TheaterID, ShowTime, Price, Status) VALUES (@ShowID, @MovieID, @TheaterID, @ShowTime, @Price, @Status)";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
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

        private void dataGridViewShows_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void dateTimePickerShowTime_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerShowTime.Format = DateTimePickerFormat.Custom;
            dateTimePickerShowTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePickerShowTime.ShowUpDown = true;
        }
        private void txtPrice_TextChanged(object sender, EventArgs e) { }
        private void comboBoxMovieID_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
