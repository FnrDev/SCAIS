using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SCAIS
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter your email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Authenticate user
            AuthenticateUser(txtEmail.Text.Trim(), txtPassword.Text);
        }

        private void AuthenticateUser(string email, string password)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();

                string query = "SELECT user_id, email, password, role, is_active FROM users WHERE email = @email";
                
                int userId = 0;
                string role = "";
                bool loginSuccessful = false;
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    
                    dbConn.Open();
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = reader.GetBoolean(reader.GetOrdinal("is_active"));
                            
                            if (!isActive)
                            {
                                MessageBox.Show("Your account is inactive. Please contact the administrator.", 
                                    "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                reader.Close();
                                dbConn.Close();
                                return;
                            }

                            string storedPassword = reader.GetString(reader.GetOrdinal("password"));
                            
                            if (VerifyPassword(password, storedPassword))
                            {
                                userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                                role = reader.GetString(reader.GetOrdinal("role"));
                                loginSuccessful = true;
                            }
                            else
                            {
                                MessageBox.Show("Invalid email or password.", "Login Failed", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtPassword.Clear();
                                txtPassword.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.", "Login Failed", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
                
                if (loginSuccessful)
                {
                    UpdateLastLogin(userId, conn);
                    dbConn.Close();
                    RedirectToRoleBasedForm(userId, email, role);
                }
                else
                {
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DatabaseConnection.Instance.Close();
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }

        private void UpdateLastLogin(int userId, SqlConnection conn)
        {
            try
            {
                string updateQuery = "UPDATE users SET last_login = @lastLogin WHERE user_id = @userId";
                
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@lastLogin", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@userId", userId);
                    
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    
                    System.Diagnostics.Debug.WriteLine($"UpdateLastLogin: {rowsAffected} row(s) affected for user_id {userId}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to update last login: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void RedirectToRoleBasedForm(int userId, string email, string role)
        {
            Form targetForm = null;
            
            switch (role)
            {
                case "Student":
                    targetForm = new frmStudent(userId, email);
                    break;
                    
                case "Adviser":
                    targetForm = new frmAdviser(userId, email);
                    break;
                    
                case "Administrator":
                    targetForm = new frmAdmin(userId, email);
                    break;
                    
                default:
                    MessageBox.Show("Invalid user role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
            
            if (targetForm != null)
            {
                this.Hide();
                targetForm.ShowDialog();
                
                txtEmail.Clear();
                txtPassword.Clear();
                txtEmail.Focus();
                this.Show();
            }
        }

        private void btnLoginStudent_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "student@polytechnic.bh";
            txtPassword.Text = "student123";
            txtEmail.Focus();
        }

        private void btnLoginAdviser_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "adviser@polytechnic.bh";
            txtPassword.Text = "adviser123";
            txtEmail.Focus();
        }

        private void btnLoginAdmin_Click(object sender, EventArgs e)
        {
            txtEmail.Text = "admin@polytechnic.bh";
            txtPassword.Text = "admin123";
            txtEmail.Focus();
        }
    }
}
