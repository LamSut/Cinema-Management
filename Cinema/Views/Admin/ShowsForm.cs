using Cinema.Controllers.Admin;
using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cinema.Views.Admin
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
            List<Movie> movies = ShowController.GetMovies();
            List<Theater> theaters = ShowController.GetTheaters();
            List<Show> shows = ShowController.GetShows();

            dataGridViewShows.AutoGenerateColumns = false;
            dataGridViewShows.Columns.Clear();

            DataGridViewTextBoxColumn colShowID = new DataGridViewTextBoxColumn()
            {
                Name = "ShowID",
                DataPropertyName = "ShowID",
                Visible = false
            };
            dataGridViewShows.Columns.Add(colShowID);

            DataGridViewComboBoxColumn colMovie = new DataGridViewComboBoxColumn()
            {
                Name = "MovieID",
                HeaderText = "Movie",
                DataPropertyName = "MovieID",
                DataSource = movies,
                DisplayMember = "Title",
                ValueMember = "MovieID",
                FlatStyle = FlatStyle.Flat
            };
            dataGridViewShows.Columns.Add(colMovie);

            DataGridViewComboBoxColumn colTheater = new DataGridViewComboBoxColumn()
            {
                Name = "TheaterID",
                HeaderText = "Theater",
                DataPropertyName = "TheaterID",
                DataSource = theaters,
                DisplayMember = "TheaterName",
                ValueMember = "TheaterID",
                FlatStyle = FlatStyle.Flat
            };
            dataGridViewShows.Columns.Add(colTheater);

            DateTimeColumn colShowTime = new DateTimeColumn()
            {
                Name = "ShowTime",
                HeaderText = "Time",
                DataPropertyName = "ShowTime"
            };
            dataGridViewShows.Columns.Add(colShowTime);

            DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = "Price",
                DataPropertyName = "Price"
            };
            dataGridViewShows.Columns.Add(colPrice);

            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn()
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status"
            };
            dataGridViewShows.Columns.Add(colStatus);

            dataGridViewShows.DataSource = shows;

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