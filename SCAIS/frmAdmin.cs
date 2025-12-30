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
                Text = "User Management - Students",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            Button btnAdd = new Button
            {
                Text = "Add New Student",
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Location = new Point(30, 80),
                Name = "btnAdd",
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            panelContent.Controls.Add(btnAdd);

            // Create DataGridView for students
            DataGridView dgvStudents = new DataGridView
            {
                Name = "dgvStudents",
                Location = new Point(30, 140),
                Size = new Size(panelContent.Width - 60, panelContent.Height - 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(dgvStudents);

            try
            {
                // Load students data
                DataTable dt = administrator.GetAllStudentsWithUsers();
                dgvStudents.DataSource = dt;

                // Hide IDs
                if (dgvStudents.Columns.Contains("student_id"))
                    dgvStudents.Columns["student_id"].Visible = false;
                if (dgvStudents.Columns.Contains("user_id"))
                    dgvStudents.Columns["user_id"].Visible = false;

                // Set friendly column headers
                if (dgvStudents.Columns.Contains("email"))
                    dgvStudents.Columns["email"].HeaderText = "Email";
                if (dgvStudents.Columns.Contains("first_name"))
                    dgvStudents.Columns["first_name"].HeaderText = "First Name";
                if (dgvStudents.Columns.Contains("last_name"))
                    dgvStudents.Columns["last_name"].HeaderText = "Last Name";
                if (dgvStudents.Columns.Contains("student_number"))
                    dgvStudents.Columns["student_number"].HeaderText = "Student Number";
                if (dgvStudents.Columns.Contains("enrollment_year"))
                    dgvStudents.Columns["enrollment_year"].HeaderText = "Enrollment Year";
                if (dgvStudents.Columns.Contains("current_semester"))
                    dgvStudents.Columns["current_semester"].HeaderText = "Current Semester";
                if (dgvStudents.Columns.Contains("gpa"))
                    dgvStudents.Columns["gpa"].HeaderText = "GPA";
                if (dgvStudents.Columns.Contains("completed_credit_hours"))
                    dgvStudents.Columns["completed_credit_hours"].HeaderText = "Completed Credits";
                if (dgvStudents.Columns.Contains("specialization_name"))
                    dgvStudents.Columns["specialization_name"].HeaderText = "Specialization";

                // Add Edit button column
                DataGridViewButtonColumn btnEditColumn = new DataGridViewButtonColumn
                {
                    Name = "btnEdit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    FlatStyle = FlatStyle.Flat
                };
                dgvStudents.Columns.Add(btnEditColumn);

                // Handle button clicks - use both events for better click detection
                dgvStudents.CellContentClick += DgvStudents_CellContentClick;
                dgvStudents.CellClick += DgvStudents_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Check if Edit button column was clicked
            if (dgv.Columns.Contains("btnEdit") && e.ColumnIndex == dgv.Columns["btnEdit"].Index)
            {
                // Get student data from the row
                int studentId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["student_id"].Value);
                int userId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["user_id"].Value);
                string email = dgv.Rows[e.RowIndex].Cells["email"].Value?.ToString();
                string firstName = dgv.Rows[e.RowIndex].Cells["first_name"].Value?.ToString();
                string lastName = dgv.Rows[e.RowIndex].Cells["last_name"].Value?.ToString();
                string studentNumber = dgv.Rows[e.RowIndex].Cells["student_number"].Value?.ToString();
                int enrollmentYear = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["enrollment_year"].Value);

                // Edit button clicked
                OpenEditStudentForm(studentId, userId, email, firstName, lastName, studentNumber, enrollmentYear);
            }
        }

        private void DgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void OpenEditStudentForm(int studentId, int userId, string email, string firstName, 
            string lastName, string studentNumber, int enrollmentYear)
        {
            try
            {
                frmEditStudent editForm = new frmEditStudent(studentId, userId, email, firstName, 
                    lastName, studentNumber, enrollmentYear, administrator);
                editForm.ShowDialog();

                // Refresh the grid after editing
                loadUserManagement();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearContentPanel()
        {
            panelContent.Controls.Clear();
          
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the edit form in "Add" mode (passing 0 for IDs and empty strings)
                frmEditStudent addForm = new frmEditStudent(0, 0, "", "", "", "", 0, administrator);
                addForm.ShowDialog();

                // Refresh the grid after adding
                loadUserManagement();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening add student form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Location = new Point(30, 80),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnAddCourse.FlatAppearance.BorderSize = 0;
            btnAddCourse.Click += (s, args) => OpenEditCourseForm(0, "", "", "", 0, "", true);
            panelContent.Controls.Add(btnAddCourse);

            // Create DataGridView for courses
            DataGridView dgvCourses = new DataGridView
            {
                Name = "dgvCourses",
                Location = new Point(30, 140),
                Size = new Size(panelContent.Width - 60, panelContent.Height - 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(dgvCourses);

            try
            {
                // Load courses data
                DataTable dt = administrator.GetAllCourses();
                dgvCourses.DataSource = dt;

                // Hide IDs
                if (dgvCourses.Columns.Contains("course_id"))
                    dgvCourses.Columns["course_id"].Visible = false;

                // Set friendly column headers
                if (dgvCourses.Columns.Contains("course_code"))
                    dgvCourses.Columns["course_code"].HeaderText = "Course Code";
                if (dgvCourses.Columns.Contains("course_name"))
                    dgvCourses.Columns["course_name"].HeaderText = "Course Name";
                if (dgvCourses.Columns.Contains("course_type"))
                    dgvCourses.Columns["course_type"].HeaderText = "Type";
                if (dgvCourses.Columns.Contains("credit_hours"))
                    dgvCourses.Columns["credit_hours"].HeaderText = "Credits";
                if (dgvCourses.Columns.Contains("description"))
                    dgvCourses.Columns["description"].HeaderText = "Description";
                if (dgvCourses.Columns.Contains("is_active"))
                {
                    dgvCourses.Columns["is_active"].HeaderText = "Active";
                    dgvCourses.Columns["is_active"].Width = 60;
                }

                // Add Edit button column
                DataGridViewButtonColumn btnEditColumn = new DataGridViewButtonColumn
                {
                    Name = "btnEditCourse",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    FlatStyle = FlatStyle.Flat
                };
                dgvCourses.Columns.Add(btnEditColumn);

                // Handle button clicks - use both events for better click detection
                dgvCourses.CellContentClick += DgvCourses_CellContentClick;
                dgvCourses.CellClick += DgvCourses_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Check if Edit button was clicked
            if (dgv.Columns.Contains("btnEditCourse") && e.ColumnIndex == dgv.Columns["btnEditCourse"].Index)
            {
                // Get course data from the row
                int courseId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["course_id"].Value);
                string courseCode = dgv.Rows[e.RowIndex].Cells["course_code"].Value?.ToString();
                string courseName = dgv.Rows[e.RowIndex].Cells["course_name"].Value?.ToString();
                string courseType = dgv.Rows[e.RowIndex].Cells["course_type"].Value?.ToString();
                int creditHours = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["credit_hours"].Value);
                string description = dgv.Rows[e.RowIndex].Cells["description"].Value?.ToString();
                bool isActive = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["is_active"].Value);

                // Edit button clicked
                OpenEditCourseForm(courseId, courseCode, courseName, courseType, creditHours, description, isActive);
            }
        }

        private void DgvCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void OpenEditCourseForm(int courseId, string courseCode, string courseName, 
            string courseType, int creditHours, string description, bool isActive)
        {
            try
            {
                frmEditCourse editForm = new frmEditCourse(courseId, courseCode, courseName, 
                    courseType, creditHours, description, isActive, administrator);
                editForm.ShowDialog();

                // Refresh the grid after editing
                LoadCourseCatalog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit course form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetActiveButton(button5);
            LoadSpecializationManagement();
        }

        private void LoadSpecializationManagement()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Specialization Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            // Add New Specialization Button
            Button btnAddSpecialization = new Button
            {
                Text = "Add New Specialization",
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Location = new Point(30, 80),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Cursor = Cursors.Hand
            };
            btnAddSpecialization.FlatAppearance.BorderSize = 0;
            btnAddSpecialization.Click += (s, args) => ShowAddSpecializationDialog();
            panelContent.Controls.Add(btnAddSpecialization);

            // Specializations DataGridView
            DataGridView dgvSpecializations = new DataGridView
            {
                Name = "dgvSpecializations",
                Location = new Point(30, 135),
                Size = new Size(panelContent.Width - 60, panelContent.Height - 175),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(dgvSpecializations);

            try
            {
                // Load specializations
                DataTable dtSpecializations = administrator.GetAllSpecializations();
                dgvSpecializations.DataSource = dtSpecializations;

                // Hide specialization_id
                if (dgvSpecializations.Columns.Contains("specialization_id"))
                    dgvSpecializations.Columns["specialization_id"].Visible = false;

                // Set headers
                if (dgvSpecializations.Columns.Contains("specialization_code"))
                {
                    dgvSpecializations.Columns["specialization_code"].HeaderText = "Code";
                    dgvSpecializations.Columns["specialization_code"].Width = 80;
                }
                if (dgvSpecializations.Columns.Contains("specialization_name"))
                    dgvSpecializations.Columns["specialization_name"].HeaderText = "Specialization Name";
                if (dgvSpecializations.Columns.Contains("description"))
                    dgvSpecializations.Columns["description"].HeaderText = "Description";
                if (dgvSpecializations.Columns.Contains("required_credit_hours"))
                {
                    dgvSpecializations.Columns["required_credit_hours"].HeaderText = "Required Credits";
                    dgvSpecializations.Columns["required_credit_hours"].Width = 120;
                }

                // Add Edit button column
                DataGridViewButtonColumn btnEditColumn = new DataGridViewButtonColumn
                {
                    Name = "btnEditSpecialization",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    FlatStyle = FlatStyle.Flat
                };
                dgvSpecializations.Columns.Add(btnEditColumn);

                // Handle button clicks
                dgvSpecializations.CellContentClick += DgvSpecializations_CellContentClick;
                dgvSpecializations.CellClick += DgvSpecializations_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading specializations: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSpecializations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Check if Edit button was clicked
            if (dgv.Columns.Contains("btnEditSpecialization") && e.ColumnIndex == dgv.Columns["btnEditSpecialization"].Index)
            {
                // Get specialization data from the row
                int specializationId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["specialization_id"].Value);
                string specializationCode = dgv.Rows[e.RowIndex].Cells["specialization_code"].Value?.ToString();
                string specializationName = dgv.Rows[e.RowIndex].Cells["specialization_name"].Value?.ToString();
                string description = dgv.Rows[e.RowIndex].Cells["description"].Value?.ToString();
                int requiredCreditHours = dgv.Rows[e.RowIndex].Cells["required_credit_hours"].Value != DBNull.Value 
                    ? Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["required_credit_hours"].Value) 
                    : 0;

                // Show edit specialization dialog
                ShowEditSpecializationDialog(specializationId, specializationCode, specializationName, description, requiredCreditHours);
            }
        }

        private void DgvSpecializations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void ShowAddSpecializationDialog()
        {
            Form specializationForm = new Form
            {
                Text = "Add New Specialization",
                Size = new Size(500, 410),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Create New Specialization",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblTitle);

            Label lblCode = new Label
            {
                Text = "Specialization Code:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblCode);

            TextBox txtCode = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 95),
                Size = new Size(420, 25),
                MaxLength = 10
            };
            specializationForm.Controls.Add(txtCode);

            Label lblName = new Label
            {
                Text = "Specialization Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 130),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblName);

            TextBox txtName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 155),
                Size = new Size(420, 25)
            };
            specializationForm.Controls.Add(txtName);

            Label lblDescription = new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 190),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblDescription);

            TextBox txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 215),
                Size = new Size(420, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            specializationForm.Controls.Add(txtDescription);

            Label lblCredits = new Label
            {
                Text = "Required Credit Hours:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 285),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblCredits);

            TextBox txtCredits = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(220, 282),
                Size = new Size(230, 25),
                Text = "120"
            };
            specializationForm.Controls.Add(txtCredits);

            Button btnCreate = new Button
            {
                Text = "Create Specialization",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 330),
                Size = new Size(180, 45),
                Cursor = Cursors.Hand
            };
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    MessageBox.Show("Specialization code is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Specialization name is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtCredits.Text, out int credits) || credits <= 0)
                {
                    MessageBox.Show("Please enter valid required credit hours.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = administrator.AddSpecialization(
                        txtCode.Text.Trim(),
                        txtName.Text.Trim(),
                        txtDescription.Text.Trim(),
                        credits
                    );

                    if (success)
                    {
                        specializationForm.DialogResult = DialogResult.OK;
                        specializationForm.Close();
                        LoadSpecializationManagement(); // Refresh
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating specialization: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            specializationForm.Controls.Add(btnCreate);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(230, 330),
                Size = new Size(140, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                specializationForm.DialogResult = DialogResult.Cancel;
                specializationForm.Close();
            };
            specializationForm.Controls.Add(btnCancel);

            specializationForm.ShowDialog();
        }

        private void ShowEditSpecializationDialog(int specializationId, string specializationCode, string specializationName, string description, int requiredCreditHours)
        {
            Form specializationForm = new Form
            {
                Text = "Edit Specialization",
                Size = new Size(500, 410),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Edit Specialization Information",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblTitle);

            Label lblCode = new Label
            {
                Text = "Specialization Code:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblCode);

            TextBox txtCode = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 95),
                Size = new Size(420, 25),
                Text = specializationCode,
                MaxLength = 10
            };
            specializationForm.Controls.Add(txtCode);

            Label lblName = new Label
            {
                Text = "Specialization Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 130),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblName);

            TextBox txtName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 155),
                Size = new Size(420, 25),
                Text = specializationName
            };
            specializationForm.Controls.Add(txtName);

            Label lblDescription = new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 190),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblDescription);

            TextBox txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 215),
                Size = new Size(420, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = description ?? ""
            };
            specializationForm.Controls.Add(txtDescription);

            Label lblCredits = new Label
            {
                Text = "Required Credit Hours:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 285),
                AutoSize = true
            };
            specializationForm.Controls.Add(lblCredits);

            TextBox txtCredits = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(220, 282),
                Size = new Size(230, 25),
                Text = requiredCreditHours.ToString()
            };
            specializationForm.Controls.Add(txtCredits);

            Button btnUpdate = new Button
            {
                Text = "Update Specialization",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 330),
                Size = new Size(180, 45),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    MessageBox.Show("Specialization code is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Specialization name is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtCredits.Text, out int credits) || credits <= 0)
                {
                    MessageBox.Show("Please enter valid required credit hours.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = administrator.UpdateSpecialization(
                        specializationId,
                        txtCode.Text.Trim(),
                        txtName.Text.Trim(),
                        txtDescription.Text.Trim(),
                        credits
                    );

                    if (success)
                    {
                        specializationForm.DialogResult = DialogResult.OK;
                        specializationForm.Close();
                        LoadSpecializationManagement(); // Refresh
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating specialization: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            specializationForm.Controls.Add(btnUpdate);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(230, 330),
                Size = new Size(140, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                specializationForm.DialogResult = DialogResult.Cancel;
                specializationForm.Close();
            };
            specializationForm.Controls.Add(btnCancel);

            specializationForm.ShowDialog();
        }

        private void btnAdvisorAssignment_Click(object sender, EventArgs e)
        {
            SetActiveButton(button3);
            LoadAdvisorAssignmentManagement();
        }

        private void btnSemesterOfferings_Click(object sender, EventArgs e)
        {
            SetActiveButton(button4);
            LoadSemesterOfferingsManagement();
        }

        private void LoadSemesterOfferingsManagement()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Semester Course Offerings Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            // Add New Semester Button
            Button btnAddSemester = new Button
            {
                Text = "Add New Semester",
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Location = new Point(30, 80),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnAddSemester.FlatAppearance.BorderSize = 0;
            btnAddSemester.Click += (s, args) => ShowAddSemesterDialog();
            panelContent.Controls.Add(btnAddSemester);

            // Semesters DataGridView
            DataGridView dgvSemesters = new DataGridView
            {
                Name = "dgvSemesters",
                Location = new Point(30, 135),
                Size = new Size(panelContent.Width - 60, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(dgvSemesters);

            // Course Offerings Section
            Label lblOfferings = new Label
            {
                Text = "Course Offerings for Selected Semester:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 350),
                AutoSize = true
            };
            panelContent.Controls.Add(lblOfferings);

            // Add Course Offering Button
            Button btnAddOffering = new Button
            {
                Text = "Add Course",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Location = new Point(30, 385),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 35),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnAddOffering.FlatAppearance.BorderSize = 0;
            panelContent.Controls.Add(btnAddOffering);

            // Course Offerings DataGridView
            DataGridView dgvOfferings = new DataGridView
            {
                Name = "dgvOfferings",
                Location = new Point(30, 430),
                Size = new Size(panelContent.Width - 60, panelContent.Height - 470),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 9)
            };
            panelContent.Controls.Add(dgvOfferings);

            try
            {
                // Load semesters
                DataTable dtSemesters = administrator.GetAllSemesters();
                dgvSemesters.DataSource = dtSemesters;

                // Hide semester_id
                if (dgvSemesters.Columns.Contains("semester_id"))
                    dgvSemesters.Columns["semester_id"].Visible = false;

                // Set headers
                if (dgvSemesters.Columns.Contains("semester_name"))
                    dgvSemesters.Columns["semester_name"].HeaderText = "Semester";
                if (dgvSemesters.Columns.Contains("academic_year"))
                    dgvSemesters.Columns["academic_year"].HeaderText = "Academic Year";
                if (dgvSemesters.Columns.Contains("start_date"))
                    dgvSemesters.Columns["start_date"].HeaderText = "Start Date";
                if (dgvSemesters.Columns.Contains("end_date"))
                    dgvSemesters.Columns["end_date"].HeaderText = "End Date";
                
                // Hide is_active column
                if (dgvSemesters.Columns.Contains("is_active"))
                    dgvSemesters.Columns["is_active"].Visible = false;

                // Add Edit button column
                DataGridViewButtonColumn btnEditSemesterColumn = new DataGridViewButtonColumn
                {
                    Name = "btnEditSemester",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    FlatStyle = FlatStyle.Flat
                };
                dgvSemesters.Columns.Add(btnEditSemesterColumn);

                // Handle button clicks for edit
                dgvSemesters.CellContentClick += DgvSemesters_CellContentClick;
                dgvSemesters.CellClick += DgvSemesters_CellClick;

                // Store the current semester ID in the button's Tag property
                btnAddOffering.Tag = 0; // Initialize with 0

                // Handle semester selection
                dgvSemesters.SelectionChanged += (s, e) =>
                {
                    if (dgvSemesters.CurrentRow != null && dgvSemesters.CurrentRow.Cells["semester_id"].Value != null)
                    {
                        int semesterId = Convert.ToInt32(dgvSemesters.CurrentRow.Cells["semester_id"].Value);
                        btnAddOffering.Enabled = true;
                        btnAddOffering.Tag = semesterId; // Store semester ID in Tag
                        
                        // Load offerings for this semester
                        LoadSemesterOfferingsGrid(dgvOfferings, semesterId);
                    }
                    else
                    {
                        btnAddOffering.Enabled = false;
                        btnAddOffering.Tag = 0;
                        dgvOfferings.DataSource = null;
                    }
                };

                // Set up Add Course button click ONCE, outside the SelectionChanged event
                btnAddOffering.Click += (s, e) =>
                {
                    if (btnAddOffering.Tag != null && (int)btnAddOffering.Tag > 0)
                    {
                        int semesterId = (int)btnAddOffering.Tag;
                        ShowAddOfferingDialog(semesterId, dgvOfferings);
                    }
                };

                // Select first semester if available
                if (dgvSemesters.Rows.Count > 0)
                {
                    dgvSemesters.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading semester offerings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSemesterOfferingsGrid(DataGridView dgvOfferings, int semesterId)
        {
            try
            {
                DataTable dtOfferings = administrator.GetSemesterOfferings(semesterId);
                dgvOfferings.DataSource = dtOfferings;

                // Hide IDs
                if (dgvOfferings.Columns.Contains("offering_id"))
                    dgvOfferings.Columns["offering_id"].Visible = false;
                if (dgvOfferings.Columns.Contains("course_id"))
                    dgvOfferings.Columns["course_id"].Visible = false;

                // Set headers
                if (dgvOfferings.Columns.Contains("course_code"))
                    dgvOfferings.Columns["course_code"].HeaderText = "Code";
                if (dgvOfferings.Columns.Contains("course_name"))
                    dgvOfferings.Columns["course_name"].HeaderText = "Course Name";
                if (dgvOfferings.Columns.Contains("credit_hours"))
                    dgvOfferings.Columns["credit_hours"].HeaderText = "Credits";
                if (dgvOfferings.Columns.Contains("max_capacity"))
                    dgvOfferings.Columns["max_capacity"].HeaderText = "Max Capacity";
                if (dgvOfferings.Columns.Contains("current_enrollment"))
                    dgvOfferings.Columns["current_enrollment"].HeaderText = "Enrolled";

                // Add Remove button column if not exists
                if (!dgvOfferings.Columns.Contains("btnRemove"))
                {
                    DataGridViewButtonColumn btnRemoveColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnRemove",
                        HeaderText = "Remove",
                        Text = "Remove",
                        UseColumnTextForButtonValue = true,
                        Width = 80,
                        FlatStyle = FlatStyle.Flat
                    };
                    dgvOfferings.Columns.Add(btnRemoveColumn);
                }

                // Handle button clicks - use both events for better click detection
                dgvOfferings.CellContentClick -= DgvOfferings_CellContentClick; // Remove previous handler
                dgvOfferings.CellContentClick += DgvOfferings_CellContentClick;
                dgvOfferings.CellClick -= DgvOfferings_CellClick; // Remove previous handler
                dgvOfferings.CellClick += DgvOfferings_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course offerings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvOfferings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            if (dgv.Columns.Contains("btnRemove") && e.ColumnIndex == dgv.Columns["btnRemove"].Index)
            {
                int offeringId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["offering_id"].Value);
                string courseName = dgv.Rows[e.RowIndex].Cells["course_name"].Value?.ToString();

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to remove '{courseName}' from this semester?",
                    "Confirm Removal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (administrator.RemoveSemesterOffering(offeringId))
                        {
                            MessageBox.Show("Course offering removed successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Refresh the grid
                            DataGridView dgvSemesters = panelContent.Controls["dgvSemesters"] as DataGridView;
                            if (dgvSemesters != null && dgvSemesters.CurrentRow != null)
                            {
                                int semesterId = Convert.ToInt32(dgvSemesters.CurrentRow.Cells["semester_id"].Value);
                                LoadSemesterOfferingsGrid(dgv, semesterId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error removing course offering: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DgvOfferings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void ShowAddSemesterDialog()
        {
            Form semesterForm = new Form
            {
                Text = "Add New Semester",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Create New Semester",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblTitle);

            Label lblSemesterName = new Label
            {
                Text = "Semester Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblSemesterName);

            ComboBox cmbSemesterName = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 67),
                Size = new Size(280, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSemesterName.Items.AddRange(new string[] { "Fall", "Spring", "Summer" });
            cmbSemesterName.SelectedIndex = 0;
            semesterForm.Controls.Add(cmbSemesterName);

            Label lblAcademicYear = new Label
            {
                Text = "Academic Year:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 110),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblAcademicYear);

            TextBox txtAcademicYear = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 107),
                Size = new Size(280, 25)
            };
            semesterForm.Controls.Add(txtAcademicYear);

            Label lblStartDate = new Label
            {
                Text = "Start Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 150),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblStartDate);

            DateTimePicker dtpStartDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 147),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Short
            };
            semesterForm.Controls.Add(dtpStartDate);

            Label lblEndDate = new Label
            {
                Text = "End Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 190),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblEndDate);

            DateTimePicker dtpEndDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 187),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Short
            };
            semesterForm.Controls.Add(dtpEndDate);

            CheckBox chkIsActive = new CheckBox
            {
                Text = "Set as Active Semester",
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 230),
                AutoSize = true
            };
            semesterForm.Controls.Add(chkIsActive);

            Button btnCreate = new Button
            {
                Text = "Create Semester",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(90, 290),
                Size = new Size(150, 45),
                Cursor = Cursors.Hand
            };
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtAcademicYear.Text))
                {
                    MessageBox.Show("Academic year is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpEndDate.Value <= dtpStartDate.Value)
                {
                    MessageBox.Show("End date must be after start date.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = administrator.AddSemester(
                        cmbSemesterName.SelectedItem.ToString(),
                        txtAcademicYear.Text.Trim(),
                        dtpStartDate.Value,
                        dtpEndDate.Value,
                        chkIsActive.Checked
                    );

                    if (success)
                    {
                        semesterForm.DialogResult = DialogResult.OK;
                        semesterForm.Close();
                        LoadSemesterOfferingsManagement(); // Refresh
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating semester: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            semesterForm.Controls.Add(btnCreate);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(260, 290),
                Size = new Size(150, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                semesterForm.DialogResult = DialogResult.Cancel;
                semesterForm.Close();
            };
            semesterForm.Controls.Add(btnCancel);

            semesterForm.ShowDialog();
        }

        private void DgvSemesters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Check if Edit button was clicked
            if (dgv.Columns.Contains("btnEditSemester") && e.ColumnIndex == dgv.Columns["btnEditSemester"].Index)
            {
                // Get semester data from the row
                int semesterId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["semester_id"].Value);
                string semesterName = dgv.Rows[e.RowIndex].Cells["semester_name"].Value?.ToString();
                string academicYear = dgv.Rows[e.RowIndex].Cells["academic_year"].Value?.ToString();
                DateTime startDate = Convert.ToDateTime(dgv.Rows[e.RowIndex].Cells["start_date"].Value);
                DateTime endDate = Convert.ToDateTime(dgv.Rows[e.RowIndex].Cells["end_date"].Value);
                bool isActive = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["is_active"].Value);

                // Show edit semester dialog
                ShowEditSemesterDialog(semesterId, semesterName, academicYear, startDate, endDate, isActive);
            }
        }

        private void DgvSemesters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void ShowEditSemesterDialog(int semesterId, string semesterName, string academicYear, DateTime startDate, DateTime endDate, bool isActive)
        {
            Form semesterForm = new Form
            {
                Text = "Edit Semester",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Edit Semester Information",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblTitle);

            Label lblSemesterName = new Label
            {
                Text = "Semester Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblSemesterName);

            ComboBox cmbSemesterName = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 67),
                Size = new Size(280, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSemesterName.Items.AddRange(new string[] { "Fall", "Spring", "Summer" });
            cmbSemesterName.SelectedItem = semesterName;
            semesterForm.Controls.Add(cmbSemesterName);

            Label lblAcademicYear = new Label
            {
                Text = "Academic Year:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 110),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblAcademicYear);

            TextBox txtAcademicYear = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 107),
                Size = new Size(280, 25),
                Text = academicYear
            };
            semesterForm.Controls.Add(txtAcademicYear);

            Label lblStartDate = new Label
            {
                Text = "Start Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 150),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblStartDate);

            DateTimePicker dtpStartDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 147),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Short,
                Value = startDate
            };
            semesterForm.Controls.Add(dtpStartDate);

            Label lblEndDate = new Label
            {
                Text = "End Date:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 190),
                AutoSize = true
            };
            semesterForm.Controls.Add(lblEndDate);

            DateTimePicker dtpEndDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 187),
                Size = new Size(280, 25),
                Format = DateTimePickerFormat.Short,
                Value = endDate
            };
            semesterForm.Controls.Add(dtpEndDate);

            CheckBox chkIsActive = new CheckBox
            {
                Text = "Set as Active Semester",
                Font = new Font("Segoe UI", 10),
                Location = new Point(180, 230),
                AutoSize = true,
                Checked = isActive
            };
            semesterForm.Controls.Add(chkIsActive);

            Button btnUpdate = new Button
            {
                Text = "Update Semester",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(90, 290),
                Size = new Size(150, 45),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtAcademicYear.Text))
                {
                    MessageBox.Show("Academic year is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpEndDate.Value <= dtpStartDate.Value)
                {
                    MessageBox.Show("End date must be after start date.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    bool success = administrator.UpdateSemester(
                        semesterId,
                        cmbSemesterName.SelectedItem.ToString(),
                        txtAcademicYear.Text.Trim(),
                        dtpStartDate.Value,
                        dtpEndDate.Value,
                        chkIsActive.Checked
                    );

                    if (success)
                    {
                        semesterForm.DialogResult = DialogResult.OK;
                        semesterForm.Close();
                        LoadSemesterOfferingsManagement(); // Refresh
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating semester: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            semesterForm.Controls.Add(btnUpdate);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(260, 290),
                Size = new Size(150, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                semesterForm.DialogResult = DialogResult.Cancel;
                semesterForm.Close();
            };
            semesterForm.Controls.Add(btnCancel);

            semesterForm.ShowDialog();
        }

        private void ShowAddOfferingDialog(int semesterId, DataGridView dgvOfferings)
        {
            Form offeringForm = new Form
            {
                Text = "Add Course Offering",
                Size = new Size(500, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = "Add Course to Semester",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            offeringForm.Controls.Add(lblTitle);

            Label lblCourse = new Label
            {
                Text = "Select Course:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            offeringForm.Controls.Add(lblCourse);

            ComboBox cmbCourses = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 95),
                Size = new Size(420, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            offeringForm.Controls.Add(cmbCourses);

            Label lblCapacity = new Label
            {
                Text = "Max Capacity:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 140),
                AutoSize = true
            };
            offeringForm.Controls.Add(lblCapacity);

            TextBox txtCapacity = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(160, 137),
                Size = new Size(290, 25),
                Text = "30"
            };
            offeringForm.Controls.Add(txtCapacity);

            // Load courses that are not already in this semester
            try
            {
                DataTable dtCourses = administrator.GetAvailableCoursesForSemester(semesterId);
                
                if (dtCourses.Rows.Count == 0)
                {
                    MessageBox.Show("All courses have already been added to this semester.", "No Available Courses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    offeringForm.Close();
                    return;
                }
                
                cmbCourses.DataSource = dtCourses;
                cmbCourses.DisplayMember = "course_display";
                cmbCourses.ValueMember = "course_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                offeringForm.Close();
                return;
            }

            Button btnAdd = new Button
            {
                Text = "Add Course",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(90, 200),
                Size = new Size(140, 45),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, ev) =>
            {
                if (cmbCourses.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtCapacity.Text, out int capacity) || capacity <= 0)
                {
                    MessageBox.Show("Please enter a valid capacity.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    int courseId = Convert.ToInt32(cmbCourses.SelectedValue);
                    
                    if (administrator.AddSemesterOffering(semesterId, courseId, capacity))
                    {
                        MessageBox.Show("Course offering added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        offeringForm.DialogResult = DialogResult.OK;
                        offeringForm.Close();
                        LoadSemesterOfferingsGrid(dgvOfferings, semesterId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding course offering: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            offeringForm.Controls.Add(btnAdd);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(250, 200),
                Size = new Size(140, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                offeringForm.DialogResult = DialogResult.Cancel;
                offeringForm.Close();
            };
            offeringForm.Controls.Add(btnCancel);

            offeringForm.ShowDialog();
        }

        private void LoadAdvisorAssignmentManagement()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Advisor Assignment Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            // Create DataGridView for students with advisers
            DataGridView dgvStudentAdvisers = new DataGridView
            {
                Name = "dgvStudentAdvisers",
                Location = new Point(30, 80),
                Size = new Size(panelContent.Width - 60, panelContent.Height - 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(dgvStudentAdvisers);

            try
            {
                // Load students with advisers data
                DataTable dt = administrator.GetStudentsWithAdvisers();
                dgvStudentAdvisers.DataSource = dt;

                // Hide IDs
                if (dgvStudentAdvisers.Columns.Contains("student_id"))
                    dgvStudentAdvisers.Columns["student_id"].Visible = false;
                if (dgvStudentAdvisers.Columns.Contains("adviser_id"))
                    dgvStudentAdvisers.Columns["adviser_id"].Visible = false;

                // Set friendly column headers
                if (dgvStudentAdvisers.Columns.Contains("student_number"))
                    dgvStudentAdvisers.Columns["student_number"].HeaderText = "Student Number";
                if (dgvStudentAdvisers.Columns.Contains("student_name"))
                    dgvStudentAdvisers.Columns["student_name"].HeaderText = "Student Name";
                if (dgvStudentAdvisers.Columns.Contains("student_email"))
                    dgvStudentAdvisers.Columns["student_email"].HeaderText = "Email";
                if (dgvStudentAdvisers.Columns.Contains("enrollment_year"))
                    dgvStudentAdvisers.Columns["enrollment_year"].HeaderText = "Enrollment Year";
                if (dgvStudentAdvisers.Columns.Contains("current_semester"))
                    dgvStudentAdvisers.Columns["current_semester"].HeaderText = "Semester";
                if (dgvStudentAdvisers.Columns.Contains("specialization_name"))
                    dgvStudentAdvisers.Columns["specialization_name"].HeaderText = "Specialization";
                if (dgvStudentAdvisers.Columns.Contains("adviser_faculty_id"))
                    dgvStudentAdvisers.Columns["adviser_faculty_id"].HeaderText = "Assigned Adviser";
                if (dgvStudentAdvisers.Columns.Contains("adviser_department"))
                    dgvStudentAdvisers.Columns["adviser_department"].HeaderText = "Department";

                // Add Assign Adviser button column
                DataGridViewButtonColumn btnAssignColumn = new DataGridViewButtonColumn
                {
                    Name = "btnAssign",
                    HeaderText = "Assign",
                    Text = "Assign Adviser",
                    UseColumnTextForButtonValue = true,
                    Width = 120,
                    FlatStyle = FlatStyle.Flat
                };
                dgvStudentAdvisers.Columns.Add(btnAssignColumn);

                // Handle button clicks - use both events for better click detection
                dgvStudentAdvisers.CellContentClick += DgvStudentAdvisers_CellContentClick;
                dgvStudentAdvisers.CellClick += DgvStudentAdvisers_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student-adviser assignments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvStudentAdvisers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Check if Assign button was clicked
            if (dgv.Columns.Contains("btnAssign") && e.ColumnIndex == dgv.Columns["btnAssign"].Index)
            {
                // Get student data from the row
                int studentId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["student_id"].Value);
                string studentName = dgv.Rows[e.RowIndex].Cells["student_name"].Value?.ToString();
                object currentAdviserId = dgv.Rows[e.RowIndex].Cells["adviser_id"].Value;

                // Show adviser selection dialog
                ShowAdviserSelectionDialog(studentId, studentName, currentAdviserId == DBNull.Value ? (int?)null : Convert.ToInt32(currentAdviserId));
            }
        }

        private void DgvStudentAdvisers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Intentionally left empty - CellClick handles everything
            // This prevents double-firing of the event
        }

        private void ShowAdviserSelectionDialog(int studentId, string studentName, int? currentAdviserId)
        {
            // Create a form for adviser selection
            Form adviserForm = new Form
            {
                Text = "Assign Adviser",
                Size = new Size(500, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = $"Assign Adviser for: {studentName}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            adviserForm.Controls.Add(lblTitle);

            Label lblSelectAdviser = new Label
            {
                Text = "Select Adviser:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 80),
                AutoSize = true
            };
            adviserForm.Controls.Add(lblSelectAdviser);

            ComboBox cmbAdvisers = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 110),
                Size = new Size(420, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            adviserForm.Controls.Add(cmbAdvisers);

            // Load advisers
            try
            {
                DataTable dtAdvisers = administrator.GetAdvisersForDropdown();
                cmbAdvisers.DataSource = dtAdvisers;
                cmbAdvisers.DisplayMember = "adviser_display";
                cmbAdvisers.ValueMember = "adviser_id";

                // Select current adviser if assigned
                if (currentAdviserId.HasValue && cmbAdvisers.Items.Count > 0)
                {
                    cmbAdvisers.SelectedValue = currentAdviserId.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading advisers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                adviserForm.Close();
                return;
            }

            Button btnAssign = new Button
            {
                Text = "Assign",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(30, 180),
                Size = new Size(200, 45),
                Cursor = Cursors.Hand
            };
            btnAssign.FlatAppearance.BorderSize = 0;
            btnAssign.Click += (s, ev) =>
            {
                if (cmbAdvisers.SelectedValue == null)
                {
                    MessageBox.Show("Please select an adviser.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    int selectedAdviserId = Convert.ToInt32(cmbAdvisers.SelectedValue);
                    bool success = administrator.AssignStudentToAdviser(studentId, selectedAdviserId);

                    if (success)
                    {
                        MessageBox.Show("Adviser assigned successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        adviserForm.DialogResult = DialogResult.OK;
                        adviserForm.Close();

                        // Refresh the grid
                        LoadAdvisorAssignmentManagement();
                    }
                    else
                    {
                        MessageBox.Show("Failed to assign adviser.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error assigning adviser: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            adviserForm.Controls.Add(btnAssign);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(250, 180),
                Size = new Size(200, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) =>
            {
                adviserForm.DialogResult = DialogResult.Cancel;
                adviserForm.Close();
            };
            adviserForm.Controls.Add(btnCancel);

            adviserForm.ShowDialog();
        }
    }
}