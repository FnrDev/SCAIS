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
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                Location = new Point(30, 100),
                Name = "btnAdd",
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30),

            };
            btnAdd.FlatAppearance.BorderSize = 1;
            btnAdd.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            panelContent.Controls.Add(btnAdd);

            Button btnEdit = new Button
            {
                Text = "Edit User",
                Location = new Point(180, 100),
                Font = new Font("Segoe UI", 12),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30)
            };
            btnEdit.FlatAppearance.BorderSize = 1;
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);

            btnEdit.Click += (s, args) =>
            {
                LoadEditUser();
            };
            panelContent.Controls.Add(btnEdit);
            Button btnDelete = new Button
            {
                Text = "Delete User",
                Font = new Font("Segoe UI", 12),
                Location = new Point(330, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30)
            };
            btnDelete.FlatAppearance.BorderSize = 1;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);
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
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 150),
                Size = new Size(subPanel.Width - 40, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a label for the initial password input
            Label lblPassword = new Label
            {
                Text = "Enter Initial Password:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblPassword);

            // Add a secure textbox for the initial password input
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 200),
                Size = new Size(subPanel.Width - 40, 20),
                UseSystemPasswordChar = true
            };
            subPanel.Controls.Add(txtPassword);

            // Add a label for the role selection
            Label lblRole = new Label
            {
                Text = "Select Role:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblRole);

            // Add a dropdown list for role selection
            ComboBox cmbRole = new ComboBox
            {
                Name = "cmbRole",
                Location = new Point(200, 250),
                Size = new Size(subPanel.Width - 40, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.Add("Select User Role");
            cmbRole.Items.Add("Adviser");
            cmbRole.Items.Add("Student");
            cmbRole.SelectedIndex = 0; // Set default selection
            subPanel.Controls.Add(cmbRole);
            Label lblSFirstName = new Label
            {
                Text = "Student First Name:",
                Location = new Point(30, 300),

                AutoSize = true
            };
            Label lbldepartment = new Label
            {
                Text = "Department:",
                Location = new Point(30, 300),

                AutoSize = true
            };
            TextBox txtSFirstName = new TextBox
            {
                Name = "txtSFirstName",
                Location = new Point(200, 300),
                Size = new Size(subPanel.Width - 40, 20)
            };
            TextBox txtdepartment = new TextBox
            {
                Name = "txtdepartment",
                Location = new Point(200, 300),
                Size = new Size(subPanel.Width - 40, 20)
            };
            Label lblLastName = new Label
            {
                Text = "Student Last Name:",
                Location = new Point(30, 350),
                AutoSize = true
            };
            TextBox txtSLastName = new TextBox
            {
                Name = "txtSLastName",
                Location = new Point(200, 350),
                Size = new Size(subPanel.Width - 40, 20)
            };
            Label lblStudentNumber = new Label
            {
                Text = "Student Number:",
                Location = new Point(30, 400),
                AutoSize = true
            };
            TextBox txtStudentNumber = new TextBox
            {
                Name = "txtStudentNumber",
                Location = new Point(200, 400),
                Size = new Size(subPanel.Width - 40, 20)
            };
            Label lblYear = new Label
            {
                Text = "Enrolment Year :",
                Location = new Point(30, 450),
                AutoSize = true
            };
            TextBox txtYear = new TextBox
            {
                Name = "txtYear",
                Location = new Point(200, 450),
                Size = new Size(subPanel.Width - 40, 20)
            };
            Label lblFacultyid = new Label
            {

                Text = "Faculty  ID:",
                Location = new Point(30, 350),
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            }; 
            TextBox txtfid = new TextBox {
                Name = "txtFid",
                Location = new Point(200, 350),
                Size = new Size(subPanel.Width - 40, 20)
            };
            subPanel.Controls.Add(lblFacultyid);
            subPanel.Controls.Add(txtfid);
            subPanel.Controls.Add(txtSFirstName);
            subPanel.Controls.Add(txtdepartment);
            subPanel.Controls.Add(lblYear);
            subPanel.Controls.Add(txtYear);
            subPanel.Controls.Add(lblStudentNumber);
            subPanel.Controls.Add(txtStudentNumber);
            subPanel.Controls.Add(lblLastName);
            subPanel.Controls.Add(txtSLastName);
            lblLastName.Visible = false;
            txtSLastName.Visible = false;
            lblFacultyid.Visible = false;
            txtfid.Visible = false;
            lblStudentNumber.Visible = false;
            txtStudentNumber.Visible = false;
            lblYear.Visible = false;
            txtYear.Visible = false;
            txtdepartment.Visible = false;
            txtSFirstName.Visible = false;
            subPanel.Controls.Add(lblSFirstName);
            subPanel.Controls.Add(lbldepartment);
            lblSFirstName.Visible = false;
            lbldepartment.Visible = false;
            //  String selectedRole = cmbRole.SelectedItem?.ToString().Trim();
            cmbRole.SelectedIndexChanged += (s, ev) =>
            {

                if (cmbRole.SelectedIndex == 2)
                {
                    lbldepartment.Visible = false;
                    lblSFirstName.Visible = true;
                    txtdepartment.Visible = false;
                    txtSFirstName.Visible = true;
                    lblLastName.Visible = true;
                    lblStudentNumber.Visible = true;
                    txtStudentNumber.Visible = true;
                    lblFacultyid.Visible = false;
                    txtfid.Visible = false;
                    lblYear.Visible = true;
                    txtYear.Visible = true;
                }
                else if (cmbRole.SelectedIndex == 1)
                {
                    lblSFirstName.Visible = false;
                    lbldepartment.Visible = true;
                    txtSFirstName.Visible = false;
                    txtdepartment.Visible = true;
                    lblLastName.Visible = false;
                    txtSLastName.Visible = false;
                    lblStudentNumber.Visible = false;
                    lblYear.Visible = false;
                    txtYear.Visible = false;
                    lblFacultyid.Visible = true;
                    txtfid.Visible = true;
                }
                else
                {
                    // Hide all additional fields
                    lblYear.Visible = false;
                    txtYear.Visible = false;
                    lblSFirstName.Visible = false;
                    lblFacultyid.Visible = false;
                    txtfid.Visible = false;
                    lbldepartment.Visible = false;
                    txtSFirstName.Visible = false;
                    txtdepartment.Visible = false;
                    lblLastName.Visible = false;
                    txtSLastName.Visible = false;
                    lblStudentNumber.Visible = false;
                    txtStudentNumber.Visible = false;
                }
            };



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
                String sFirstName = txtSFirstName.Text;
                String sLastName = txtSLastName.Text;
                String studentNumber = txtStudentNumber.Text;
                int enrolYear = int.TryParse( txtYear.Text, out int year) ? year : 0;
                string password = txtPassword.Text;
                string role = cmbRole.SelectedItem?.ToString();
                String department = txtdepartment.Text;
                String facultyId = txtfid.Text;
                if (string.IsNullOrWhiteSpace(userEmail) ||
                    string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (role == "Student")
                {
                    if (string.IsNullOrWhiteSpace(sFirstName) ||
                    string.IsNullOrWhiteSpace(sLastName) || string.IsNullOrWhiteSpace(studentNumber) || enrolYear == 0)
                    {
                        MessageBox.Show("All student fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    administrator.AddStudent(userEmail, password, role, sFirstName, sLastName, studentNumber, year);
                }
                else if (role == "Adviser")
                {
                    if (string.IsNullOrWhiteSpace(department) || String.IsNullOrEmpty(facultyId))
                    {
                        MessageBox.Show("All advisor fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    administrator.AddAdvisor( userEmail, password, role, facultyId, department);
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
   
        private void LoadEditUser()
        {
            panelContent.Controls.Add(subPanel);
            subPanel.Controls.Clear();

            // Add a label for the user ID selection
            Label lblUserID = new Label
            {
                Text = "Select User ID:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserID);

            // Add a dropdown list for user ID selection
            ComboBox cmbUserID = new ComboBox
            {
                Name = "cmbUserID",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 150),
                Size = new Size(subPanel.Width - 40, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Populate the dropdown with user IDs
            subPanel.Controls.Add(cmbUserID);
            LoadUser(cmbUserID);
            
            // Add a label for the user email input
           Label lblUserEmail = new Label
            {
                Text = "User Email:",
                Font = new Font("Segoe UI", 10),
               Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserEmail);

            // Add a textbox for the user Email input
            TextBox txtUserEmail = new TextBox
            {
                Name = "txtUserEmail",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 200),
                Size = new Size(subPanel.Width -40, 20)
            };
            subPanel.Controls.Add(txtUserEmail);

            // Add a label for the user Password input
            Label lblUserPassword = new Label
            {
                Text = "User Password:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserPassword);

            // Add a textbox for the user password input
            TextBox txtUserPassword = new TextBox
            {
                Name = "txtUserEmail",
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 250),
                Size = new Size(subPanel.Width - 40, 20)

            };
            subPanel.Controls.Add(txtUserPassword);
            // Add panel fpr student info 
          
            Panel pnlStudent = new Panel
            {
                Name = "pnlStudent",
                Location = new Point(30, 300),
                Size = new Size(subPanel.Width - 0, 200),
                
                Visible = false
            };
            subPanel.Controls.Add(pnlStudent);
            Label lblStudentFirstName = new Label
            {
                Text = "Student First Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 0),
                AutoSize = true
            };
            pnlStudent.Controls.Add(lblStudentFirstName);
            TextBox txtStudentFirstName = new TextBox
            {
                Name = "txtStudentFirstName",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 0),
                Size = new Size(subPanel.Width - 40, 20)
            };
            pnlStudent.Controls.Add(txtStudentFirstName);
            Label lblStudentLastName = new Label
            {
                Text = "Student Last Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 50),
                AutoSize = true
            };
            pnlStudent.Controls.Add(lblStudentLastName);
            TextBox txtStudentLastName = new TextBox
            {
                Name = "txtStudentLastName",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 50),
                Size = new Size(subPanel.Width - 40, 20)

            };
            pnlStudent.Controls.Add(txtStudentLastName);
            Label lblStudentNumber = new Label
            {
                Text = "Student Number:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 100),
                AutoSize = true
            };
            pnlStudent.Controls.Add(lblStudentNumber);
            TextBox txtStudentNumber = new TextBox
            {
                Name = "txtStudentNumber",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 100),
                Size = new Size(subPanel.Width - 40, 20)

            };
            pnlStudent.Controls.Add(txtStudentNumber);
            Label lblEnrollmentYear = new Label
            {
                Text = "Enrollment Year:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 150),
                AutoSize = true
            };
            pnlStudent.Controls.Add(lblEnrollmentYear);
            TextBox txtEnrollmentYear = new TextBox
            {
                Name = "txtEnrollmentYear",
                Location = new Point(170, 150),
                Size = new Size(subPanel.Width - 40, 20)

            };
            pnlStudent.Controls.Add(txtEnrollmentYear);
            
            // Add panel for advisor info
            Panel pnlAdvisor = new Panel
            {
                Name = "pnlAdvisor",
                Location = new Point(30, 300),
                Size = new Size(subPanel.Width - 0, 150),
                Visible = false
            };

            subPanel.Controls.Add(pnlAdvisor);
            Label lblDepartment = new Label
            {
                Text = "Department:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 0),
                AutoSize = true
            };
            pnlAdvisor.Controls.Add(lblDepartment);
            TextBox txtDepartment = new TextBox
            {
                Name = "txtDepartment",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 0),
                Size = new Size(subPanel.Width - 40, 20)

            };
            pnlAdvisor.Controls.Add(txtDepartment);
            Label lblFacultyID = new Label
            {
                Text = "Faculty ID:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(0, 50),
                AutoSize = true
            };
            pnlAdvisor.Controls.Add(lblFacultyID);
            TextBox txtFacultyID = new TextBox
            {
                Name = "txtFacultyID",
                Font = new Font("Segoe UI", 10),
                Location = new Point(170, 50),
                Size = new Size(subPanel.Width - 40, 20)

            };
            pnlAdvisor.Controls.Add(txtFacultyID);
            DataTable df = administrator.GetUserForDropdown();
            DataTable ds = administrator.GetStudent();
            DataTable da = administrator.GetAdviser();
            cmbUserID.SelectedIndexChanged += (s, ev) =>
            {
                if (cmbUserID.SelectedValue != null)
                {
                    int selectedUserId = Convert.ToInt32(cmbUserID.SelectedValue);
                    DataRow[] selectedUserRows = df.Select("user_id = " + selectedUserId);
                    DataRow[] selectedStudentRows = ds.Select("user_id = " + selectedUserId);
                    DataRow[] selectedAdvisorRows = da.Select("user_id = " + selectedUserId);
                    if (selectedUserRows.Length > 0)
                    {
                        DataRow selectedUser = selectedUserRows[0];
                        txtUserEmail.Text = selectedUser["email"].ToString();
                        txtUserPassword.Text = selectedUser["password"].ToString();
                        string role = selectedUser["role"].ToString();
                        if (role == "Student")
                        {
                            DataRow selectedStudent = selectedStudentRows[0];
                            pnlAdvisor.Visible = false;
                            pnlStudent.Visible = true;
                            // Load student-specific details
                            txtStudentFirstName.Text = selectedStudent["first_name"].ToString();
                            txtStudentNumber.Text = selectedStudent["student_number"].ToString();
                            txtStudentLastName.Text = selectedStudent["last_name"].ToString();
                            txtEnrollmentYear.Text = selectedStudent["enrollment_year"].ToString();
                        }
                        else if (role == "Adviser")
                        {
                            DataRow selectedAdvisor = selectedAdvisorRows[0];
                            pnlStudent.Visible = false;
                            pnlAdvisor.Visible = true;
                            // Load advisor-specific details
                           txtDepartment.Text = selectedAdvisor["department"].ToString();
                            txtFacultyID.Text = selectedAdvisor["faculty_id"].ToString();
                        }
                        else
                        {
                            pnlAdvisor.Visible = false;
                            pnlStudent.Visible = false;
                        }
                    }
                }
            };
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
               int selectedUserID = Convert.ToInt32(cmbUserID.SelectedValue);
                string userEmail = txtUserEmail.Text;
                string userPassword = txtUserPassword.Text;
                string studentFirstName = txtStudentFirstName.Text;
                string studentLastName = txtStudentLastName.Text;
                string studentNumber = txtStudentNumber.Text;
                string department = txtDepartment.Text;
                string facultyID = txtFacultyID.Text;
                int enrollmentYear = int.TryParse(txtEnrollmentYear.Text, out int year) ? year : 0;
                
                
                if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrEmpty(userPassword))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (pnlStudent.Visible == true)
                {
                    if (string.IsNullOrWhiteSpace(studentFirstName) || string.IsNullOrWhiteSpace(studentLastName) || string.IsNullOrWhiteSpace(studentNumber) || enrollmentYear == 0)
                    {
                        MessageBox.Show("All student fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    administrator.EditStudent(selectedUserID, userEmail, userPassword, studentFirstName, studentLastName, studentNumber, year);
                }
                else if (pnlAdvisor.Visible == true)
                { 
                if (string.IsNullOrWhiteSpace(department) || string.IsNullOrWhiteSpace(facultyID))
                    {
                        MessageBox.Show("All advisor fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    administrator.EditAdviser(selectedUserID, userEmail, userPassword, facultyID, department);
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
                Font = new Font("Segoe UI",10),
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblUserID);

            // Add a dropdown list for user ID selection
            ComboBox cmbUserID = new ComboBox
            {
                Name = "cmbUserID",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 150),
                Size = new Size(subPanel.Width - 40, 20),
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
                    DialogResult dg = MessageBox.Show($"Are you sure you want to delete user with id: {selectedUserID}?", "Conform Delete", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    // Delete user
                    if (dg == DialogResult.Yes)
                    {
                        try
                        {
                            bool isDelete = administrator.DeleteUser(selectedUserID);
                            if (isDelete)
                            {
                                MessageBox.Show("User Deleued Successfully!  ","Successful",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                LoadUser(cmbUserID);
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
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                Location = new Point(30, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 30)
            };
            btnAddCourse.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);
            btnAddCourse.Click += new System.EventHandler(this.btnAddCourse_Click);
            panelContent.Controls.Add(btnAddCourse);
            Button btnUpdatePre = new Button
            {
                Text = "Update Prerequisites",
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                Location = new Point(230,100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200,30)
            };
            btnUpdatePre.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);
            btnAddCourse.FlatAppearance.BorderSize = 1;
            btnUpdatePre.Click += (s, args) =>
            {
                panelContent.Controls.Add(subPanel);
                subPanel.Controls.Clear();
                // Implementation for updating prerequisites
                Label lblCourseId = new Label
                {
                    Text = "Required Course ID for Update: ",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(30, 150),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCourseId);
                ComboBox cmbCourseId = new ComboBox
                {
                    Name = "cmbCourseId",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(250, 150),
                    Size = new Size(subPanel.Width - 40, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // Populate cmbCourseId with course IDs from the database
                subPanel.Controls.Add(cmbCourseId);
                LoadCourse(cmbCourseId);
                Label lblPrereqIds = new Label
                {
                    Text = "New Prerequisite Course IDs : ",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(30, 200),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblPrereqIds);
                DataGridView txtPrereqIds = new DataGridView
                {
                    Name = "txtPrereqIds",
                    Location = new Point(30, 250),
                    Size = new Size(subPanel.Width - 40, subPanel.Height - 350),
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


                DataTable dvg = administrator.GetCourseForDropdown();
                txtPrereqIds.DataSource = dvg;

                subPanel.Controls.Add(txtPrereqIds);
                if (txtPrereqIds.Columns.Contains("course_id"))
                    txtPrereqIds.Columns["course_id"].HeaderText = "Course Id";
                if (txtPrereqIds.Columns.Contains("course_code"))
                    txtPrereqIds.Columns["course_code"].HeaderText = "Course Code";
                if (txtPrereqIds.Columns.Contains("course_name"))
                    txtPrereqIds.Columns["course_name"].HeaderText = "Course Name";
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
                    int selectedCourseId = Convert.ToInt32(cmbCourseId.SelectedValue);
                    List<int> selectedPreid = new List<int>();
                    foreach (DataGridViewRow row in txtPrereqIds.Rows)
                    {
                        bool isChecked = Convert.ToBoolean(row.Cells[0].Value);

                        if (!isChecked) continue;

                        if (!txtPrereqIds.Columns.Contains("course_id") || row.Cells["course_id"].Value == null)
                            continue;

                        selectedPreid.Add(Convert.ToInt32(row.Cells["course_id"].Value));
                    }

                    if (selectedPreid.Count == 0)
                    {
                        MessageBox.Show("Please select at least one course.", "No Selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool isUpdated = administrator.UpdatePrereq(selectedCourseId, selectedPreid);
                    if (isUpdated)
                    {
                        MessageBox.Show("Prerequisites updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        subPanel.Controls.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update prerequisites.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                BackColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Location = new Point(430, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200 ,30)
            };
            btnUpdateCore.FlatAppearance.BorderColor = Color.FromArgb(44, 52, 80);
            btnAddCourse.FlatAppearance.BorderSize = 1;
            btnUpdateCore.Click += (s, args) =>
            {
                panelContent.Controls.Add(subPanel);
                subPanel.Controls.Clear();
                // Implementation for updating corequisites
                Label lblCourseId = new Label
                {
                    Text = "Required Course ID for Update: ",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(30, 150),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCourseId);
               ComboBox cmbCourseIds = new ComboBox
                {
                    Name = "cmbCourseId",
                    Font = new Font("Segoe UI", 10),
                   Location = new Point(250, 150),
                    Size = new Size(subPanel.Width - 40, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                // Populate cmbCourseId with course IDs from the database
                subPanel.Controls.Add(cmbCourseIds);
                LoadCourse(cmbCourseIds);
                Label lblCoreqIds = new Label
                {
                    Text = "New Corequisite Course IDs : ",
                    Font = new Font("Segoe UI", 10),
                    Location = new Point(30, 200),
                    AutoSize = true
                };
                subPanel.Controls.Add(lblCoreqIds);
                DataGridView CorerereqIds = new DataGridView
                {
                    Name = "txtPrereqIds",
                    Location = new Point(30, 250),
                    Size = new Size(subPanel.Width - 40, subPanel.Height - 350),
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
                DataGridViewCheckBoxColumn Chk = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Select",
                    Width = 50,
                    ReadOnly = false,
                    Name = "checkBoxColumn"
                };
                CorerereqIds.Columns.Insert(0, Chk);


                DataTable dvg = administrator.GetCourseForDropdown();
                CorerereqIds.DataSource = dvg;

                subPanel.Controls.Add(CorerereqIds);
                if (CorerereqIds.Columns.Contains("course_id"))
                    CorerereqIds.Columns["course_id"].HeaderText = "Course Id";
                if (CorerereqIds.Columns.Contains("course_code"))
                    CorerereqIds.Columns["course_code"].HeaderText = "Course Code";
                if (CorerereqIds.Columns.Contains("course_name"))
                    CorerereqIds.Columns["course_name"].HeaderText = "Course Name";
                
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

                    int selectedCourseId = Convert.ToInt32(cmbCourseIds.SelectedValue);
                    List<int> selectedCoreid = new List<int>();
                    foreach (DataGridViewRow row in CorerereqIds.Rows)
                    {
                        bool isChecked = Convert.ToBoolean(row.Cells[0].Value);

                        if (!isChecked) continue;

                        if (!CorerereqIds.Columns.Contains("course_id") || row.Cells["course_id"].Value == null)
                            continue;

                        selectedCoreid.Add(Convert.ToInt32(row.Cells["course_id"].Value));
                    }

                    if (selectedCoreid.Count == 0)
                    {
                        MessageBox.Show("Please select at least one course.", "No Selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool isUpdated = administrator.UpdateCorereq(selectedCourseId, selectedCoreid);
                    if (isUpdated)
                    {
                        MessageBox.Show("Corequisites updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        subPanel.Controls.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update prerequisites.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 150),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseCode);
            TextBox txtCourceCode = new TextBox
            {
                Name = "txtCourceCode",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 150),
                Size = new Size(subPanel.Width - 40, 20)
            };
            subPanel.Controls.Add(txtCourceCode);
            Label lblCourseName = new Label
            {
                Text = "Course Name: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 200),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseName);
            TextBox txtCourceName = new TextBox
            {
                Name = "txtCourceName",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 200),
                Size = new Size(subPanel.Width - 40, 20)
            };
            subPanel.Controls.Add(txtCourceName);
            Label lblCourseType = new Label
            {
                Text = "Course Type: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 250),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCourseType);
            ComboBox cmbCourseType = new ComboBox
            {
                Name = "cmbCourseType",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 250),
                Size = new Size(subPanel.Width - 40, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCourseType.Items.AddRange(new string[] {"Select Type", "Core", "Elective" ,"Specialized  "});
            cmbCourseType.SelectedIndex = 0;
            subPanel.Controls.Add(cmbCourseType);
            Label lblCredits = new Label
            {
                Text = "Credit Hours: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 300),
                AutoSize = true
            };
            subPanel.Controls.Add(lblCredits);
            TextBox txtCredits = new TextBox
            {
                Name = "txtCredits",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 300),
                Size = new Size(subPanel.Width - 40, 20)
            };
            subPanel.Controls.Add(txtCredits);
            Label lblDescripitioh = new Label
            {
                Text = "Course Description: ",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 350),
                AutoSize = true
            };
            subPanel.Controls.Add(lblDescripitioh);
            TextBox txtDescription = new TextBox
            {
                Name = "txtDescription",
                Location = new Point(200, 350),
                Font = new Font("Segoe UI", 10),
                Size = new Size(subPanel.Width - 40, 60),
                Multiline = true
            };
            subPanel.Controls.Add(txtDescription);
        
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
                int credits = int.TryParse(txtCredits.Text, out int credit) ? credit : 0;
                string description = txtDescription.Text;
                if (string.IsNullOrWhiteSpace(courseCode) || string.IsNullOrWhiteSpace(courseName) ||
                    cmbCourseType.SelectedIndex == 0 || credit == 0 ||
                    string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("All fields are required. Please fill in all the details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                }
                else
                {
                    // Insert to database
                    administrator.AddCourse(courseCode, courseName, courseType, credit, description);
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