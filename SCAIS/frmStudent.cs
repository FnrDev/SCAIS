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
    public partial class frmStudent : Form
    {
        private int studentId;
        private string studentEmail;

        public frmStudent(int studentId, string email)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.studentEmail = email;
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {studentEmail}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
