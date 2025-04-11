using System.Windows.Forms;
using Cinema.Views;

namespace Cinema.Controllers.Admin
{
    public class MainController
    {
        private readonly Form mdiParent;
        private MoviesForm filmsForm;
        private ShowsForm showsForm;
        private DashboardForm dashboardForm;

        public MainController(Form mdiParent)
        {
            this.mdiParent = mdiParent;
        }

        public void ShowDashboard()
        {
            if (dashboardForm == null || dashboardForm.IsDisposed)
            {
                dashboardForm = new DashboardForm();
                dashboardForm.FormClosed += (s, e) => dashboardForm = null;
                SetupChildForm(dashboardForm);
            }
            else
            {
                dashboardForm.Activate();
            }
        }

        public void ShowMovies()
        {
            if (filmsForm == null || filmsForm.IsDisposed)
            {
                filmsForm = new MoviesForm();
                filmsForm.FormClosed += (s, e) => filmsForm = null;
                SetupChildForm(filmsForm);
            }
            else
            {
                filmsForm.Activate();
            }
        }

        public void ShowShows()
        {
            if (showsForm == null || showsForm.IsDisposed)
            {
                showsForm = new ShowsForm();
                showsForm.FormClosed += (s, e) => showsForm = null;
                SetupChildForm(showsForm);
            }
            else
            {
                showsForm.Activate();
            }
        }

        public void ExitApplication()
        {
            Application.Exit();
        }

        private void SetupChildForm(Form child)
        {
            child.MdiParent = mdiParent;
            child.Dock = DockStyle.Fill;
            child.Show();
        }
    }
}
