using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cinema
{
    public partial class AdminMainForm : Form
    {
        AdminMoviesForm filmsForm;
        AdminShowsForm showsForm;
        AdminDashboardForm dashboardForm;

        public AdminMainForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.Load += AdminMainForm_Load; 
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            ShowDashboardForm();
        }

        private void ShowDashboardForm()
        {
            if (dashboardForm == null || dashboardForm.IsDisposed)
            {
                dashboardForm = new AdminDashboardForm();
                dashboardForm.FormClosed += DashboardForm_FormClosed;
                dashboardForm.MdiParent = this;
                dashboardForm.Dock = DockStyle.Fill;
                dashboardForm.Show();
            }
            else
            {
                dashboardForm.Activate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (filmsForm == null || filmsForm.IsDisposed)
            {
                filmsForm = new AdminMoviesForm();
                filmsForm.FormClosed += FilmsForm_FormClosed;
                filmsForm.MdiParent = this;
                filmsForm.Dock = DockStyle.Fill;
                filmsForm.Show();
            }
            else
            {
                filmsForm.Activate();
            }
        }

        private void FilmsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            filmsForm = null;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (showsForm == null || showsForm.IsDisposed)
            {
                showsForm = new AdminShowsForm();
                showsForm.FormClosed += ShowsForm_FormClosed;
                showsForm.MdiParent = this;
                showsForm.Dock = DockStyle.Fill;
                showsForm.Show();
            }
            else
            {
                showsForm.Activate();
            }
        }

        private void ShowsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            showsForm = null;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboardForm();
        }

        private void DashboardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboardForm = null;
        }
    }
}
