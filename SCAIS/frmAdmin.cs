using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAIS
{
    public partial class frmAdmin : Form
    {
        private Administrator administrator;
        private int adminId;
        private string adminEmail;
        private Button currentActiveButton;
        
        private void SetActiveButton(Button btn)
        {
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.FromArgb(60, 63, 65);
            }

            btn.BackColor = Color.FromArgb(41, 128, 185);
            currentActiveButton = btn;
        }
        public frmAdmin(int adminId, string email)
        {
            InitializeComponent();
            try
            {
                administrator = new Administrator(adminId, email);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing admin: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            //   this.adminId = adminId;
             //this.adminEmail = email;

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

            // Add a label for the user email input
            Label lblUserEmail = new Label
            {
                Text = "User Email: ",
                Location = new Point(30, 150),
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserEmail);

            // Add a textbox for the user email input
            TextBox txtUserEmail = new TextBox
            {
                Name = "txtUserEmail",
                Location = new Point(200, 150),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a label for the initial password input
            Label lblPassword = new Label
            {
                Text = "Enter Initial Password:",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblPassword);

            // Add a secure textbox for the initial password input
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(200, 200),
                Size = new Size(200, 20),
                UseSystemPasswordChar = true
            };
            subPanel.Controls.Add(txtPassword);

            // Add a label for the role selection
            Label lblRole = new Label
            {
                Text = "Select Role:",
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblRole);

            // Add a dropdown list for role selection
            ComboBox cmbRole = new ComboBox
            {
                Name = "cmbRole",
                Location = new Point(200, 250),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new string[] { "Advisor", "Student" });
            subPanel.Controls.Add(cmbRole);
            Label lblSFirstName = new Label
            {
                Text = "Student First Name:",
                Location = new Point(30, 300),

                AutoSize = true
            };
            subPanel.Controls.Add(lblSFirstName);
            
           
         
            // Add a button to confirm the add action
            Button btnConfirmAdd = new Button
            {
                Text = "Confirm Add",
                Location = new Point(30, subPanel.Height - 60),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)
            };
            btnConfirmAdd.Click += (s, args) =>
            {
                string userEmail = txtUserEmail.Text;
                
                string password = txtPassword.Text;
                string role = cmbRole.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(userEmail) ||
                    string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"User added successfully!\nEmail: {userEmail}\nRole: {role}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add logic to save the user details
                }
            };
            subPanel.Controls.Add(btnConfirmAdd);

            // Add a button to cancel the add action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, subPanel.Height - 60),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)

            };
            btnCancel.Click += (s, args) =>
            {
                subPanel.Controls.Clear();
            };
            subPanel.Controls.Add(btnCancel);
        }
        private void LoadUser(ComboBox cm)
        {
            try {
                DataTable df = administrator.GetUserForDropdown();
                cm.ValueMember = "user_id";
                cm.DataSource = df;
            } catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// Load courses into a ComboBox
        private void LoadCourse(ComboBox cm)
        {
            try
            {
                DataTable df = administrator.GetCourseForDropdown();
                cm.ValueMember = "course_id";
                cm.DataSource = df;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Load Courses for select Prerequisites and Corequisites
        private void LoadCourseInfo(DataGridView cm)
        {
            try
            {
                DataTable df = administrator.GetCourseForDropdown();
                
                cm.DataSource = df;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading course: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            LoadUser(cmbUserID);
            
            // Add a label for the user email input
           Label lblUserEmail = new Label
            {
                Text = "User Email:",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserEmail);

            // Add a textbox for the user Email input
            TextBox txtUserEmail = new TextBox
            {
                Name = "txtUserEmail",
                Location = new Point(200, 200),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a label for the user Password input
            Label lblUserPassword = new Label
            {
                Text = "User Password:",
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserPassword);

            // Add a textbox for the user password input
            TextBox txtUserPassword = new TextBox
            {
                Name = "txtUserEmail",
                Location = new Point(200, 250),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtUserPassword);

            // Add a button to confirm the edit action
            Button btnConfirmEdit = new Button
            {
                Text = "Confirm Edit",
                Location = new Point(30, subPanel.Height - 60),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)


            };
            btnConfirmEdit.Click += (s, args) =>
            {
                string selectedUserID = cmbUserID.SelectedItem?.ToString();
                string userEmail = txtUserEmail.Text;

                if (string.IsNullOrWhiteSpace(selectedUserID)  || string.IsNullOrWhiteSpace(userEmail))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"User with ID {selectedUserID} updated successfully!\nEmail: {userEmail}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add logic to update the user details
                }
            };
            subPanel.Controls.Add(btnConfirmEdit);

            // Add a button to cancel the edit action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, subPanel.Height - 60),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)

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
            LoadUser(cmbUserID);
            
            // Add a button to confirm the delete action
            Button btnConfirmDelete = new Button
            {
                Text = "Confirm Delete",
                Location = new Point(30, subPanel.Height - 60),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)


            };
            btnConfirmDelete.Click += (s, args) =>
            {

                int selectedUserID = Convert.ToInt32(cmbUserID.SelectedValue);

                if (selectedUserID == -1)
                {
                    MessageBox.Show("Please select a user ID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dg = MessageBox.Show($"Are you sure you want to delete user with id: {selectedUserID}?", "Conform Delete", MessageBoxButtons.YesNo);
                    // Delete user
                    if (dg == DialogResult.Yes)
                    {
                        try
                        {
                            bool isDelete = administrator.DeleteUser(selectedUserID);
                            if (isDelete)
                            {
                                MessageBox.Show("User Deleued Successfully!  ");
                            }
                            else
                            {
                                MessageBox.Show("Error");
                            }    
                        } catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        } catch (Exception e)
                        {

                            MessageBox.Show(e.Message);
                        }
                        }
                }
            };
            subPanel.Controls.Add(btnConfirmDelete);

            // Add a button to cancel the delete action
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, subPanel.Height - 60),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)


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
                Text = "Add New Course",
                Location = new Point(30, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135, 30)
            };
            btnAddCourse.Click += new System.EventHandler(this.btnAddCourse_Click);
            panelContent.Controls.Add(btnAddCourse);
            Button btnUpdatePre = new Button
            {
                Text = "Update Prerequisites",
                Location = new Point(185,100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135,30)
            };
            btnUpdatePre.Click += (s, args) =>
            {
                panelContent.Controls.Add(subPanel);
                subPanel.Controls.Clear();
                // Implementation for updating prerequisites
                Label lblCourseId = new Label
                {
                    Text = "Required Course ID for Update: ",
                    Location = new Point(30, 150),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCourseId);
                ComboBox cmbCourseId = new ComboBox
                {
                    Name = "cmbCourseId",
                    Location = new Point(250, 150),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // Populate cmbCourseId with course IDs from the database
                subPanel.Controls.Add(cmbCourseId);
                LoadCourse(cmbCourseId);
                Label lblPrereqIds = new Label
                {
                    Text = "New Prerequisite Course IDs : ",
                    Location = new Point(30, 200),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblPrereqIds);
                DataGridView txtPrereqIds = new DataGridView
                {
                    Name = "txtPrereqIds",
                    Location = new Point(30, 250),
                    Size = new Size(subPanel.Width - 100, subPanel.Height - 400),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    AllowUserToAddRows = false,
                    ReadOnly = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false

                };
                DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Select",
                    Width = 50,
                    ReadOnly = false,
                    Name = "checkBoxColumn"
                };
                txtPrereqIds.Columns.Insert(0, CheckBoxColumn);
                LoadCourseInfo(txtPrereqIds);
                subPanel.Controls.Add(txtPrereqIds);
                Button btnConfirmUpdatePrereq = new Button
                {
                    Text = "Confirm Update",
                    Location = new Point(30, subPanel.Height - 60),
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Size = new Size(200, 45)
                };
                btnConfirmUpdatePrereq.Click += (sender2, args2) =>
                {
                    string selectedCourseId = cmbCourseId.SelectedItem?.ToString();
                    string prereqIdsInput = txtPrereqIds.Text;
                    if (string.IsNullOrWhiteSpace(selectedCourseId) || string.IsNullOrWhiteSpace(prereqIdsInput))
                    {
                        MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Prerequisites for Course ID {selectedCourseId} updated successfully to: {prereqIdsInput}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Add logic to update the prerequisites in the database
                 
                    }
                };
                subPanel.Controls.Add(btnConfirmUpdatePrereq);
                Button btnCancel = new Button
                    {
                        Text = "Cancel",
                        Location = new Point(250, subPanel.Height - 60),
                        BackColor = Color.FromArgb(231, 76, 60),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        Size = new Size(200, 45)
                    };
                    btnCancel.Click += (s3, args3) =>
                    {
                        subPanel.Controls.Clear();
                    };
                    subPanel.Controls.Add(btnCancel);
              
              
            };  
            panelContent.Controls.Add(btnUpdatePre);
            Button btnUpdateCore = new Button
            {
                Text = "Update Corerequisites:",
                Location = new Point(340, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(135, 30)
            };
            btnUpdateCore.Click += (s, args) =>
            {
                panelContent.Controls.Add(subPanel);
                subPanel.Controls.Clear();
                // Implementation for updating corequisites
                Label lblCourseId = new Label
                {
                    Text = "Required Course ID for Update: ",
                    Location = new Point(30, 150),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCourseId);
               ComboBox cmbCourseIds = new ComboBox
                {
                    Name = "cmbCourseId",
                    Location = new Point(250, 150),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // Populate cmbCourseId with course IDs from the database
                subPanel.Controls.Add(cmbCourseIds);
                LoadCourse(cmbCourseIds);
                Label lblCoreqIds = new Label
                {
                    Text = "New Corequisite Course IDs : ",
                    Location = new Point(30, 200),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCoreqIds);
               CheckedListBox txtCoreqIds = new CheckedListBox
               {
                    Name = "txtCoreqIds",
                    Location = new Point(250, 200),
                    Size = new Size(200, 20),
                };
                subPanel.Controls.Add(txtCoreqIds);
                Button btnConfirmUpdateCoreq = new Button
                {
                    Text = "Confirm Update",
                    Location = new Point(30, subPanel.Height - 60),
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Size = new Size(200, 45)
                };
                btnConfirmUpdateCoreq.Click += (sender2, args2) =>
                {
                    string selectedCourseId = cmbCourseIds.SelectedItem?.ToString();
                    string coreqIdsInput = txtCoreqIds.Text;
                    if (string.IsNullOrWhiteSpace(selectedCourseId) || string.IsNullOrWhiteSpace(coreqIdsInput))
                    {
                        MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Corequisites for Course ID {selectedCourseId} updated successfully to: {coreqIdsInput}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Add logic to update the corequisites in the database
                    }
                };
                subPanel.Controls.Add(btnConfirmUpdateCoreq);
                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(250, subPanel.Height - 60),
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Size = new Size(200, 45)
                };
                btnCancel.Click += (s3, args3) =>
                {
                    subPanel.Controls.Clear();
                };
                subPanel.Controls.Add(btnCancel);
            };
            panelContent.Controls.Add(btnUpdateCore);
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
            Label lblCourseCode = new Label
            {
                Text = "Course Code: ",
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseCode);
            TextBox txtCourceCode = new TextBox
            {
                Name = "txtCourceCode",
                Location = new Point(200, 150),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtCourceCode);
            Label lblCourseName = new Label
            {
                Text = "Course Name: ",
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseName);
            TextBox txtCourceName = new TextBox
            {
                Name = "txtCourceName",
                Location = new Point(200, 200),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtCourceName);
            Label lblCourseType = new Label
            {
                Text = "Course Type: ",
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseType);
            ComboBox cmbCourseType = new ComboBox
            {
                Name = "cmbCourseType",
                Location = new Point(200, 250),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCourseType.Items.AddRange(new string[] { "Core", "Elective" ,"NRQ"});
            subPanel.Controls.Add(cmbCourseType);
            Label lblCredits = new Label
            {
                Text = "Credit Hours: ",
                Location = new Point(30, 300),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCredits);
            TextBox txtCredits = new TextBox
            {
                Name = "txtCredits",
                Location = new Point(200, 300),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtCredits);
            Label lblDescripitioh = new Label
            {
                Text = "Course Description: ",
                Location = new Point(30, 350),
                AutoSize = true
            };
            subPanel.Controls.Add(lblDescripitioh);
            TextBox txtDescription = new TextBox
            {
                Name = "txtDescription",
                Location = new Point(200, 350),
                Size = new Size(200, 60),
                Multiline = true
            };
            subPanel.Controls.Add(txtDescription);
            Label lblSpID = new Label
            {
                Text = "Specialization ID: ",
                Location = new Point(30, 430),
                AutoSize = true
            };
            subPanel.Controls.Add(lblSpID);
            TextBox txtSpID = new TextBox
            {
                Name = "txtSpID",
                Location = new Point(200, 430),
                Size = new Size(200, 20)
            };
            subPanel.Controls.Add(txtSpID);
            Button btnConfirmAddCourse = new Button
            {
                Text = "Confirm Add Course",
                Location = new Point(30, subPanel.Height - 60),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)
            };
            btnConfirmAddCourse.Click += (s, args) =>
            {
                string courseCode = txtCourceCode.Text;
                string courseName = txtCourceName.Text;
                string courseType = cmbCourseType.SelectedItem?.ToString();
                int credits;
                string description = txtDescription.Text;
                if (string.IsNullOrWhiteSpace(courseCode) || string.IsNullOrWhiteSpace(courseName) ||
                    string.IsNullOrWhiteSpace(courseType) || string.IsNullOrWhiteSpace(txtCredits.Text) ||
                    string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (!int.TryParse(txtCredits.Text, out credits))
                {
                    MessageBox.Show("Credits must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    credits = int.Parse(txtCredits.Text);
                    MessageBox.Show($"Course added successfully!\nCourse Code: {courseCode}\nCourse Name: {courseName}\nCourse Type: {courseType}\nCredits: {credits}\nDescription: {description}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Insert to database
                    administrator.AddCourse(courseCode, courseName, courseType, credits, description);
                }
            };
            subPanel.Controls.Add(btnConfirmAddCourse);
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, subPanel.Height - 60),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Size = new Size(200, 45)


            };
            btnCancel.Click += (s, args) =>
            {
                subPanel.Controls.Clear();
            };
            subPanel.Controls.Add(btnCancel);
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}