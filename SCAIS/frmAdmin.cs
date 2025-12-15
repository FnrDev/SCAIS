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
    public partial class frmAdmin : Form
    {
        private int adminId;
        private string adminEmail;

        public frmAdmin(int adminId, string email)
        {
            InitializeComponent();
            this.adminId = adminId;
            this.adminEmail = email;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {adminEmail}";
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
