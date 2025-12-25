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
        private Button currentActiveButton;
        
        private void SetActiveButton(Button btn)
        {
            if (currentActiveButton != null && currentActiveButton != btn)
                currentActiveButton.BackColor = Color.FromArgb(88, 92, 100); // normal sidebar

            btn.BackColor = Color.FromArgb(120, 130, 145); // active highlight
            currentActiveButton = btn;
        }
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
                this.Close();
            }
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnUserManagement);
            loadUserManagement();
        }
        private void loadUserManagement()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "User Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            Button btnAdd = new Button
            {
                Text = "Add New User",
                Location = new Point(30, 100),
                Name = "btnAdd",
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30),

            };
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            panelContent.Controls.Add(btnAdd);

            Button btnEdit = new Button
            {
                Text = "Edit User",
                Location = new Point(160, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            panelContent.Controls.Add(btnEdit);
            Button btnDelete = new Button
            {
                Text = "Delete User",
                Location = new Point(290, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            panelContent.Controls.Add(btnDelete);
        }
        private void ClearContentPanel()
        {
            panelContent.Controls.Clear();
          
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            LoadAddUser();


        }
        private void LoadAddUser()
        {
            panelContent.Controls.Add(subPanel);
            subPanel.Controls.Clear();

            // Add a label for the user name input
            Label lblUserName = new Label
            {
                Text = "User Name: ",
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserName);

            // Add a textbox for the user name input
            TextBox txtUserName = new TextBox
            {
                Name = "txtUserEmail",
                Location = new Point(200, 150),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserName);

            // Add a label for the user email input
            Label lblUserEmail = new Label
            {
                Text = "User Email: ",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserEmail);

            // Add a textbox for the user email input
            TextBox txtUserEmail = new TextBox
            {
                Name = "txtUserName",
                Location = new Point(200, 200),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a label for the initial password input
            Label lblPassword = new Label
            {
                Text = "Enter Initial Password:",
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblPassword);

            // Add a secure textbox for the initial password input
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(200, 250),
                Size = new Size(200, 20),
                UseSystemPasswordChar = true
            };
            subPanel.Controls.Add(txtPassword);

            // Add a label for the role selection
            Label lblRole = new Label
            {
                Text = "Select Role:",
                Location = new Point(30, 300),
                AutoSize = true
            };
            subPanel.Controls.Add(lblRole);

            // Add a dropdown list for role selection
            ComboBox cmbRole = new ComboBox
            {
                Name = "cmbRole",
                Location = new Point(200, 300),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new string[] { "Advisor", "Student" });
            subPanel.Controls.Add(cmbRole);

            // Add a button to confirm the add action
            Button btnConfirmAdd = new Button
            {
                Text = "Confirm Add",
                Location = new Point(30, 350),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)

            };
            btnConfirmAdd.Click += (s, args) =>
            {
                string userEmail = txtUserEmail.Text;
                string userName = txtUserName.Text;
                string password = txtPassword.Text;
                string role = cmbRole.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userName) ||
                    string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"User added successfully!\nEmail: {userEmail}\nName: {userName}\nRole: {role}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add logic to save the user details
                }
            };
            subPanel.Controls.Add(btnConfirmAdd);

            // Add a button to cancel the add action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(160, 350),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnCancel.Click += (s, args) =>
            {
                subPanel.Controls.Clear();
            };
            subPanel.Controls.Add(btnCancel);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            LoadEditUser();
        }
        private void LoadEditUser()
        {
            panelContent.Controls.Add(subPanel);
            subPanel.Controls.Clear();

            // Add a label for the user ID selection
            Label lblUserID = new Label
            {
                Text = "Select User ID:",
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserID);

            // Add a dropdown list for user ID selection
            ComboBox cmbUserID = new ComboBox
            {
                Name = "cmbUserID",
                Location = new Point(200, 150),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Populate the dropdown with user IDs
            subPanel.Controls.Add(cmbUserID);

            // Add a label for the user name input
            Label lblUserName = new Label
            {
                Text = "User Name:",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserName);

            // Add a textbox for the user name input
            TextBox txtUserName = new TextBox
            {
                Name = "txtUserName",
                Location = new Point(200, 200),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserName);

            // Add a label for the user email input
            Label lblUserEmail = new Label
            {
                Text = "User Email:",
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserEmail);

            // Add a textbox for the user email input
            TextBox txtUserEmail = new TextBox
            {
                Name = "txtUserEmail",
                Location = new Point(200, 250),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a button to confirm the edit action
            Button btnConfirmEdit = new Button
            {
                Text = "Confirm Edit",
                Location = new Point(30, 300),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnConfirmEdit.Click += (s, args) =>
            {
                string selectedUserID = cmbUserID.SelectedItem?.ToString();
                string userName = txtUserName.Text;
                string userEmail = txtUserEmail.Text;

                if (string.IsNullOrWhiteSpace(selectedUserID) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userEmail))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"User with ID {selectedUserID} updated successfully!\nName: {userName}\nEmail: {userEmail}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add logic to update the user details
                }
            };
            subPanel.Controls.Add(btnConfirmEdit);

            // Add a button to cancel the edit action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(160, 300),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnCancel.Click += (s, args) =>
            {
                subPanel.Controls.Clear();
            };
            subPanel.Controls.Add(btnCancel);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            LoadDeleteUser();
        }
        private void LoadDeleteUser()
        {
            panelContent.Controls.Add(subPanel);
            subPanel.Controls.Clear();

            // Add a label for the user ID selection
            Label lblUserID = new Label
            {
                Text = "Select User ID to Delete:",
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserID);

            // Add a dropdown list for user ID selection
            ComboBox cmbUserID = new ComboBox
            {
                Name = "cmbUserID",
                Location = new Point(200, 150),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Populate the dropdown with user IDs
            
            subPanel.Controls.Add(cmbUserID);

            // Add a button to confirm the delete action
            Button btnConfirmDelete = new Button
            {
                Text = "Confirm Delete",
                Location = new Point(30, 200),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnConfirmDelete.Click += (s, args) =>
            {
                string selectedUserID = cmbUserID.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(selectedUserID))
                {
                    MessageBox.Show("Please select a user ID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"User with ID {selectedUserID} deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add logic to delete the user
                }
            };
            subPanel.Controls.Add(btnConfirmDelete);

            // Add a button to cancel the delete action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(160, 200),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnCancel.Click += (s, args) =>
            {
                subPanel.Controls.Clear();
            };
            subPanel.Controls.Add(btnCancel);
        }
        private void btnCourceCatalog_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCourceCatalog);
            LoadCourseCatalog();

        }
        private void LoadCourseCatalog()
        {
            ClearContentPanel();
            Label lblTitle = new Label
            {
                Text = "Course Catalog Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            Button btnAddCourse = new Button
            {
                Text = "Add Course",
                Location = new Point(30, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            btnAddCourse.Click += new System.EventHandler(this.btnAddCourse_Click);
            panelContent.Controls.Add(btnAddCourse);
            Button btnUpdateCatalog = new Button
            {
                Text = "Update Catalog",
                Location = new Point(160, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 30)
            };
            panelContent.Controls.Add(btnUpdateCatalog);
        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            LoadAddCourse();
        }
        private void LoadAddCourse()
        {
            panelContent.Controls.Add(subPanel);
            subPanel.Controls.Clear();
            // Implementation for adding course 
            Label lblCourceName = new Label
            {
                Text = "Course Name: ",
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourceName);
            TextBox txtCourceName = new TextBox
            {
                Name = "txtCourceName",
                Location = new Point(200, 150),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtCourceName);
            Label lblCourseType = new Label
            {
                Text = "Course Type: ",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseType);
        }
    }
}