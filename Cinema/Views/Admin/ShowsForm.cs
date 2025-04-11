using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Cinema.Controllers.Admin;

namespace Cinema.Views
{
    public partial class ShowsForm : Form
    {
        public ShowsForm()
        {
            InitializeComponent();
            LoadShowData();
        }

        private void LoadShowData()
        {
            DataTable dtMovies = ShowController.GetMovies();
            DataTable dtTheaters = ShowController.GetTheaters();
            DataTable dtShows = ShowController.GetShows();
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
            object valueToUpdate = newValue;
            if (columnName == "ShowTime")
            {
                if (!DateTime.TryParseExact(newValue, "MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                    return;
                }
                valueToUpdate = parsedDate;
            }
            else if (columnName == "Price")
            {
                if (!decimal.TryParse(newValue, out decimal price))
                {
                    dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = originalValue;
                    return;
                }
                valueToUpdate = price;
            }
            if (ShowController.UpdateShowField(showID, columnName, valueToUpdate, out string err))
            {
                dataGridViewShows.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = newValue;
                MessageBox.Show("Show updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (ShowController.DeleteShow(showId, out string err))
                LoadShowData();
        }

        private void LoadMovieTitles()
        {
            DataTable dt = ShowController.GetMovies();
            comboBoxMovieTitle.DataSource = dt;
            comboBoxMovieTitle.DisplayMember = "Title";
            comboBoxMovieTitle.ValueMember = "Title";
        }

        private void LoadTheaterNames()
        {
            DataTable dt = ShowController.GetTheaters();
            comboBoxTheaterName.DataSource = dt;
            comboBoxTheaterName.DisplayMember = "TheaterName";
            comboBoxTheaterName.ValueMember = "TheaterName";
        }

        private void GenerateNewShowID()
        {
            txtShowID.Text = ShowController.GenerateNewShowID();
        }

        private void btnAddFilm_Click(object sender, EventArgs e)
        {
            string showID = txtShowID.Text.Trim();
            string movieTitle = comboBoxMovieTitle.Text.Trim();
            string theaterName = comboBoxTheaterName.Text.Trim();
            DateTime showTime = dateTimePickerShowTime.Value;
            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                return;
            if (ShowController.InsertShow(showID, movieTitle, theaterName, showTime, price, out string err))
            {
                LoadShowData();
                GenerateNewShowID();
            }
            else
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
