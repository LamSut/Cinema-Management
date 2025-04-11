using System;
using System.Windows.Forms;
using Cinema.Controllers.Admin;

namespace Cinema.Views
{
    public partial class MainForm : Form
    {
        private readonly MainController controller;

        public MainForm()
        {
            InitializeComponent();
            IsMdiContainer = true;
            controller = new MainController(this);

            Load += MainForm_Load;
            btnDashboard.Click += (s, e) => controller.ShowDashboard();
            btnMovie.Click += (s, e) => controller.ShowMovies();
            btnShow.Click += (s, e) => controller.ShowShows();
            btnExit.Click += (s, e) => controller.ExitApplication();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            controller.ShowDashboard();
        }
    }
}
