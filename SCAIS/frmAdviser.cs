using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAIS
{
    public partial class frmAdviser : Form
    {
        private int adviserId;
        private string adviserEmail;

        public frmAdviser(int adviserId, string email)
        {
            InitializeComponent();
            this.adviserId = adviserId;
            this.adviserEmail = email;
        }

        private void frmAdviser_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {adviserEmail}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
