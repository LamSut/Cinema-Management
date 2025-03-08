using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class AdminMainForm: Form
    {
        AdminFilmsForm filmsForm;
        AdminShowsForm showsForm;
        AdminMainForm homeForm;

        public AdminMainForm()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (filmsForm == null)
            {
                filmsForm = new AdminFilmsForm();
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
            if (showsForm == null)
            {
              
                //filmsForm.Close();s
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
            if (homeForm == null)
            {
                homeForm = new AdminMainForm();
                homeForm.FormClosed += HomeForm_FormClosed;
                homeForm.MdiParent = this;
                homeForm.Show();
            } 
            else
            {
                homeForm.Activate();
            }
        }

        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            homeForm = null;
        }
    }
}
