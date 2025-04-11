using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cinema.Controllers.Admin;
using Cinema.Models;

namespace Cinema.Views.Admin
{
    public partial class MoviesForm : Form
    {
        private Dictionary<string, object> originalCellValues = new Dictionary<string, object>();

        public MoviesForm()
        {
            InitializeComponent();
            this.Load += MoviesForm_Load;
        }

        private void MoviesForm_Load(object sender, EventArgs e)
        {
            dataGridViewMovies.CellBeginEdit += dataGridViewMovies_CellBeginEdit;
            dataGridViewMovies.CellEndEdit += dataGridViewMovies_CellEndEdit;

            LoadMovieData();
            GenerateNewMovieID();
        }

        private void LoadMovieData()
        {
            // Lấy danh sách Movie từ controller
            List<Movie> movies = MovieController.GetAllMoviesList();

            // Binding list vào DataGridView thông qua BindingSource
            BindingSource source = new BindingSource();
            source.DataSource = movies;

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

            dataGridViewMovies.DataSource = source;
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

        private void btnAddMovie_Click(object sender, EventArgs e)
        {
            Movie movie = new Movie
            {
                MovieID = txtMovieID.Text,
                Title = txtTitle.Text.Trim(),
                Genre = txtGenre.Text.Trim(),
                Duration = (int)numericDuration.Value,
                Description = txtDescription.Text.Trim(),
                Director = txtDirector.Text.Trim(),
                ReleaseDate = dateTimePickerRelease.Value,
                PosterURL = txtPosterURL.Text.Trim()
            };

            if (string.IsNullOrWhiteSpace(movie.Title) || string.IsNullOrWhiteSpace(movie.Genre))
            {
                MessageBox.Show("Please fill in all required fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MovieController.AddMovie(movie, out string err))
            {
                MessageBox.Show("Movie added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMovieData();
                GenerateNewMovieID();
            }
            else
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (MovieController.DeleteMovie(movieId, out string err))
                    {
                        MessageBox.Show("Movie deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMovieData();
                    }
                    else
                    {
                        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a movie to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (columnName == "MovieID")
                    return;
                string movieID = dataGridViewMovies.Rows[e.RowIndex].Cells["MovieID"].Value.ToString();
                object valueToUpdate = columnName == "ReleaseDate" ? (object)DateTime.Parse(newValue) : newValue;
                if (MovieController.UpdateMovieField(movieID, columnName, valueToUpdate, out string err))
                {
                    MessageBox.Show("Movie updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                originalCellValues.Remove(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateNewMovieID()
        {
            txtMovieID.Text = MovieController.GenerateNewMovieID();
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
