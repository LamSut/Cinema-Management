using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Cinema
{
    public partial class AdminMoviesForm : Form
    {
        private string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";
        private Dictionary<string, object> originalCellValues = new Dictionary<string, object>();

        public AdminMoviesForm()
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
                    dataGridViewMovies.AutoGenerateColumns = false;
                    dataGridViewMovies.Columns.Clear();

                    DataGridViewTextBoxColumn colMovieID = new DataGridViewTextBoxColumn();
                    colMovieID.DataPropertyName = "MovieID";
                    colMovieID.Name = "MovieID";
                    colMovieID.Visible = false;
                    dataGridViewMovies.Columns.Add(colMovieID);

                    DataGridViewTextBoxColumn colTitle = new DataGridViewTextBoxColumn();
                    colTitle.DataPropertyName = "Title";
                    colTitle.Name = "Title";
                    dataGridViewMovies.Columns.Add(colTitle);

                    DataGridViewTextBoxColumn colGenre = new DataGridViewTextBoxColumn();
                    colGenre.DataPropertyName = "Genre";
                    colGenre.Name = "Genre";
                    dataGridViewMovies.Columns.Add(colGenre);

                    DataGridViewTextBoxColumn colDuration = new DataGridViewTextBoxColumn();
                    colDuration.DataPropertyName = "Duration";
                    colDuration.Name = "Duration";
                    dataGridViewMovies.Columns.Add(colDuration);

                    DataGridViewTextBoxColumn colDirector = new DataGridViewTextBoxColumn();
                    colDirector.DataPropertyName = "Director";
                    colDirector.Name = "Director";
                    dataGridViewMovies.Columns.Add(colDirector);

                    CalendarColumn colReleaseDate = new CalendarColumn();
                    colReleaseDate.DataPropertyName = "ReleaseDate";
                    colReleaseDate.Name = "ReleaseDate";
                    colReleaseDate.HeaderText = "Date";
                    dataGridViewMovies.Columns.Add(colReleaseDate);

                    DataGridViewTextBoxColumn colPosterURL = new DataGridViewTextBoxColumn();
                    colPosterURL.DataPropertyName = "PosterURL";
                    colPosterURL.Name = "PosterURL";
                    dataGridViewMovies.Columns.Add(colPosterURL);

                    DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                    colStatus.DataPropertyName = "Status";
                    colStatus.Name = "Status";
                    dataGridViewMovies.Columns.Add(colStatus);

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
                    dataGridViewMovies.AllowUserToAddRows = false;

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

        private void btnAddMovie_Click(object sender, EventArgs e)
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
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)  // ERROR CODE OF UNIQUE
                    {
                        MessageBox.Show("A movie with this title already exists. Please choose a different title.",
                                        "Duplicate Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("SQL Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnDeleteMovie_Click(object sender, EventArgs e)
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
                        if (columnName == "ReleaseDate")
                            cmd.Parameters.AddWithValue("@Value", DateTime.Parse(newValue));
                        else
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

        private void AdminMoviesForm_Load(object sender, EventArgs e)
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
                    txtMovieID.Text = "M" + newID.ToString("000");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dateTimePickerRelease_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerRelease.Format = DateTimePickerFormat.Custom;
            dateTimePickerRelease.CustomFormat = "MM/dd/yyyy";
        }
    }

    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn() : base(new CalendarCell()) { }
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                    throw new InvalidCastException("Must be a CalendarCell");
                base.CellTemplate = value;
            }
        }
    }

    public class CalendarCell : DataGridViewTextBoxCell
    {
        public CalendarCell() : base() { }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
            if (this.Value == null || this.Value == DBNull.Value)
                ctl.Value = (DateTime)this.DefaultNewRowValue;
            else
                ctl.Value = (DateTime)this.Value;
        }
        public override Type EditType => typeof(CalendarEditingControl);
        public override Type ValueType => typeof(DateTime);
        public override object DefaultNewRowValue => DateTime.Now;
    }

    public class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        bool valueChanged = false;
        int rowIndex;
        public object EditingControlFormattedValue
        {
            get { return this.Value.ToShortDateString(); }
            set
            {
                if (value is string)
                {
                    try { this.Value = DateTime.Parse((string)value); }
                    catch { this.Value = DateTime.Now; }
                }
            }
        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }
        public int EditingControlRowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }
        public void PrepareEditingControlForEdit(bool selectAll) { }
        public bool RepositionEditingControlOnValueChange => false;
        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }
        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { valueChanged = value; }
        }
        public Cursor EditingPanelCursor => base.Cursor;
        protected override void OnValueChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}
