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
        private DataTable dtMovies;
        private DataTable dtTheaters;
        private DataTable dtShows;

        public AdminShowsForm()
        {
            InitializeComponent();
            LoadShowData();
        }

        private void LoadShowData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                dtMovies = new DataTable();
                new SqlDataAdapter("SELECT MovieID, Title FROM Movie", conn).Fill(dtMovies);
                dtTheaters = new DataTable();
                new SqlDataAdapter("SELECT TheaterID, TheaterName FROM Theater", conn).Fill(dtTheaters);
                dtShows = new DataTable();
                string query = "SELECT s.ShowID, s.MovieID, s.TheaterID, s.ShowTime, s.Price, s.Status FROM Show s";
                new SqlDataAdapter(query, conn).Fill(dtShows);
                dataGridViewShows.AutoGenerateColumns = false;
                dataGridViewShows.Columns.Clear();
                DataGridViewTextBoxColumn colShowID = new DataGridViewTextBoxColumn();
                colShowID.Name = "ShowID";
                colShowID.DataPropertyName = "ShowID";
                colShowID.Visible = false;
                dataGridViewShows.Columns.Add(colShowID);
                DataGridViewComboBoxColumn colMovie = new DataGridViewComboBoxColumn();
                colMovie.Name = "MovieID";
                colMovie.HeaderText = "Movie";
                colMovie.DataPropertyName = "MovieID";
                colMovie.DataSource = dtMovies;
                colMovie.DisplayMember = "Title";
                colMovie.ValueMember = "MovieID";
                colMovie.FlatStyle = FlatStyle.Flat;
                dataGridViewShows.Columns.Add(colMovie);
                DataGridViewComboBoxColumn colTheater = new DataGridViewComboBoxColumn();
                colTheater.Name = "TheaterID";
                colTheater.HeaderText = "Theater";
                colTheater.DataPropertyName = "TheaterID";
                colTheater.DataSource = dtTheaters;
                colTheater.DisplayMember = "TheaterName";
                colTheater.ValueMember = "TheaterID";
                colTheater.FlatStyle = FlatStyle.Flat;
                dataGridViewShows.Columns.Add(colTheater);
                DateTimeColumn colShowTime = new DateTimeColumn();
                colShowTime.Name = "ShowTime";
                colShowTime.HeaderText = "Time";
                colShowTime.DataPropertyName = "ShowTime";
                dataGridViewShows.Columns.Add(colShowTime);
                DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn();
                colPrice.Name = "Price";
                colPrice.HeaderText = "Price";
                colPrice.DataPropertyName = "Price";
                dataGridViewShows.Columns.Add(colPrice);
                DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                colStatus.Name = "Status";
                colStatus.HeaderText = "Status";
                colStatus.DataPropertyName = "Status";
                dataGridViewShows.Columns.Add(colStatus);
                dataGridViewShows.DataSource = dtShows;
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
                dataGridViewShows.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                dataGridViewShows.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewShows.AllowUserToAddRows = false;
                foreach (DataGridViewColumn col in dataGridViewShows.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }
                dataGridViewShows.CellMouseEnter += (s, e) =>
                {
                    if (e.RowIndex >= 0)
                        dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                };
                dataGridViewShows.CellMouseLeave += (s, e) =>
                {
                    if (e.RowIndex >= 0)
                        dataGridViewShows.Rows[e.RowIndex].DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.WhiteSmoke : Color.LightGray;
                };
            }
        }

        private void AdminShowsForm_Load(object sender, EventArgs e)
        {
            dateTimePickerShowTime.Format = DateTimePickerFormat.Custom;
            dateTimePickerShowTime.CustomFormat = "MM/dd/yyyy HH:mm";
            dateTimePickerShowTime.ShowUpDown = true;
            dataGridViewShows.CellBeginEdit += dataGridViewShows_CellBeginEdit;
            dataGridViewShows.CellEndEdit += dataGridViewShows_CellEndEdit;
            dataGridViewShows.CurrentCellDirtyStateChanged += dataGridViewShows_CurrentCellDirtyStateChanged;
            LoadMovieTitles();
            LoadTheaterNames();
            GenerateNewShowID();
        }

        private void dataGridViewShows_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewShows.IsCurrentCellDirty)
                dataGridViewShows.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewShows_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
        }

        private void dataGridViewShows_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridViewShows.Columns[e.ColumnIndex].Name;
            if (columnName == "ShowID") return;
            string newValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            string originalValue = dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag?.ToString();
            string showID = dataGridViewShows.Rows[e.RowIndex].Cells["ShowID"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(newValue) || string.IsNullOrWhiteSpace(showID))
            {
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                return;
            }
            if (newValue == originalValue) return;
            bool updateSuccess = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (columnName == "MovieID")
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE Show SET MovieID=@Value WHERE ShowID=@ShowID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Value", newValue);
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            updateSuccess = cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else if (columnName == "TheaterID")
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE Show SET TheaterID=@Value WHERE ShowID=@ShowID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Value", newValue);
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            updateSuccess = cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else if (columnName == "ShowTime")
                    {
                        if (!DateTime.TryParseExact(newValue, "MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                            return;
                        }
                        using (SqlCommand cmd = new SqlCommand("UPDATE Show SET ShowTime=@Value WHERE ShowID=@ShowID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Value", parsedDate);
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            updateSuccess = cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else if (columnName == "Price")
                    {
                        if (!decimal.TryParse(newValue, out decimal price))
                        {
                            dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                            return;
                        }
                        using (SqlCommand cmd = new SqlCommand("UPDATE Show SET Price=@Value WHERE ShowID=@ShowID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Value", price);
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            updateSuccess = cmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE Show SET " + columnName + "=@Value WHERE ShowID=@ShowID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Value", newValue);
                            cmd.Parameters.AddWithValue("@ShowID", showID);
                            updateSuccess = cmd.ExecuteNonQuery() > 0;
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
            catch
            {
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
            }
        }

        private void btnDeleteFilm_Click(object sender, EventArgs e)
        {
            if (dataGridViewShows.SelectedRows.Count > 0)
            {
                string showId = dataGridViewShows.SelectedRows[0].Cells["ShowID"].Value.ToString();
                DialogResult result = MessageBox.Show("Are you sure you want to delete: " + showId + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                    DeleteShow(showId);
            }
        }

        private void DeleteShow(string showId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Show WHERE ShowID = @ShowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShowID", showId);
                cmd.ExecuteNonQuery();
                LoadShowData();
            }
        }

        private void LoadMovieTitles()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DataTable dt = new DataTable();
                new SqlDataAdapter("SELECT Title FROM Movie", conn).Fill(dt);
                comboBoxMovieTitle.DataSource = dt;
                comboBoxMovieTitle.DisplayMember = "Title";
                comboBoxMovieTitle.ValueMember = "Title";
            }
        }

        private void LoadTheaterNames()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DataTable dt = new DataTable();
                new SqlDataAdapter("SELECT TheaterName FROM Theater", conn).Fill(dt);
                comboBoxTheaterName.DataSource = dt;
                comboBoxTheaterName.DisplayMember = "TheaterName";
                comboBoxTheaterName.ValueMember = "TheaterName";
            }
        }

        private void GenerateNewShowID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MAX(CAST(SUBSTRING(ShowID, 2, LEN(ShowID)-1) AS INT)) FROM Show";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                int newID = (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 1;
                txtShowID.Text = "S" + newID.ToString("000");
            }
        }

        private void btnAddFilm_Click(object sender, EventArgs e)
        {
            string showID = txtShowID.Text.Trim();
            string movieTitle = comboBoxMovieTitle.Text.Trim();
            string theaterName = comboBoxTheaterName.Text.Trim();
            DateTime showTime = dateTimePickerShowTime.Value;
            string priceText = txtPrice.Text.Trim();
            decimal price;
            if (!decimal.TryParse(priceText, out price)) return;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string getMovieIDQuery = "SELECT MovieID FROM Movie WHERE Title = @Title";
                string movieID = null;
                using (SqlCommand getMovieIDCmd = new SqlCommand(getMovieIDQuery, conn))
                {
                    getMovieIDCmd.Parameters.AddWithValue("@Title", movieTitle);
                    object movieIDObj = getMovieIDCmd.ExecuteScalar();
                    if (movieIDObj == null) return;
                    movieID = movieIDObj.ToString();
                }
                string getTheaterIDQuery = "SELECT TheaterID FROM Theater WHERE TheaterName = @Name";
                string theaterID = null;
                using (SqlCommand getTheaterIDCmd = new SqlCommand(getTheaterIDQuery, conn))
                {
                    getTheaterIDCmd.Parameters.AddWithValue("@Name", theaterName);
                    object theaterIDObj = getTheaterIDCmd.ExecuteScalar();
                    if (theaterIDObj == null) return;
                    theaterID = theaterIDObj.ToString();
                }
                string insertQuery = "INSERT INTO Show (ShowID, MovieID, TheaterID, ShowTime, Price, Status) VALUES (@ShowID, @MovieID, @TheaterID, @ShowTime, @Price, @Status)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                    cmd.Parameters.AddWithValue("@ShowTime", showTime);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.ExecuteNonQuery();
                    LoadShowData();
                    GenerateNewShowID();
                }
            }
        }

    }

    public class DateTimeColumn : DataGridViewColumn
    {
        public DateTimeColumn() : base(new DateTimeCell()) { }
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DateTimeCell)))
                    throw new InvalidCastException("Must be a DateTimeCell");
                base.CellTemplate = value;
            }
        }
    }

    public class DateTimeCell : DataGridViewTextBoxCell
    {
        public DateTimeCell() : base() { }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DateTimeEditingControl ctl = DataGridView.EditingControl as DateTimeEditingControl;
            if (Value == null || Value == DBNull.Value) ctl.Value = DateTime.Now;
            else ctl.Value = (DateTime)Value;
        }
        public override Type EditType => typeof(DateTimeEditingControl);
        public override Type ValueType => typeof(DateTime);
        public override object DefaultNewRowValue => DateTime.Now;
    }

    public class DateTimeEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        bool valueChanged;
        int rowIndex;
        public object EditingControlFormattedValue
        {
            get { return Value.ToString("MM/dd/yyyy HH:mm"); }
            set
            {
                if (value is string s)
                {
                    if (!DateTime.TryParseExact(s, "MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                    {
                        dt = DateTime.Now;
                    }
                    Value = dt;
                }
            }
        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;
            CalendarForeColor = dataGridViewCellStyle.ForeColor;
            CalendarMonthBackground = dataGridViewCellStyle.BackColor;
            Format = DateTimePickerFormat.Custom;
            CustomFormat = "MM/dd/yyyy HH:mm";
            ShowUpDown = true;
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
            EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}
