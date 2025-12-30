using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SCAIS
{
    public partial class frmEditStudent : Form
    {
        private int studentId;
        private int userId;
        private Administrator administrator;
        private bool isAddMode;

        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtStudentNumber;
        private TextBox txtEnrollmentYear;
        private ComboBox cmbSpecialization;
        private Button btnSave;
        private Button btnCancel;
        
        // Enrollment management controls
        private DataGridView dgvEnrollments;
        private Button btnSaveEnrollments;
        private Label lblEnrollmentsTitle;

        public frmEditStudent(int studentId, int userId, string email, string firstName, 
            string lastName, string studentNumber, int enrollmentYear, Administrator admin)
        {
            this.studentId = studentId;
            this.userId = userId;
            this.administrator = admin;
            this.isAddMode = (studentId == 0); // If studentId is 0, we're adding a new student

            InitializeComponent();
            
            if (!isAddMode)
            {
                LoadStudentData(email, firstName, lastName, studentNumber, enrollmentYear);
            }
        }

        private void InitializeComponent()
        {
            this.Text = isAddMode ? "Add New Student" : "Edit Student Information";
            this.Size = new Size(900, isAddMode ? 550 : 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title Label
            Label lblTitle = new Label
            {
                Text = isAddMode ? "Add New Student" : "Edit Student Information",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Email Label and TextBox
            Label lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 80),
                AutoSize = true
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 77),
                Size = new Size(350, 25)
            };
            this.Controls.Add(txtEmail);

            // Password Label and TextBox
            Label lblPassword = new Label
            {
                Text = "Password:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 120),
                AutoSize = true
            };
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 117),
                Size = new Size(350, 25),
                UseSystemPasswordChar = true
            };
            this.Controls.Add(txtPassword);

            // First Name Label and TextBox
            Label lblFirstName = new Label
            {
                Text = "First Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 160),
                AutoSize = true
            };
            this.Controls.Add(lblFirstName);

            txtFirstName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 157),
                Size = new Size(350, 25)
            };
            this.Controls.Add(txtFirstName);

            // Last Name Label and TextBox
            Label lblLastName = new Label
            {
                Text = "Last Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 200),
                AutoSize = true
            };
            this.Controls.Add(lblLastName);

            txtLastName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 197),
                Size = new Size(350, 25)
            };
            this.Controls.Add(txtLastName);

            // Student Number Label and TextBox
            Label lblStudentNumber = new Label
            {
                Text = "Student Number:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 240),
                AutoSize = true
            };
            this.Controls.Add(lblStudentNumber);

            txtStudentNumber = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 237),
                Size = new Size(350, 25)
            };
            this.Controls.Add(txtStudentNumber);

            // Enrollment Year Label and TextBox
            Label lblEnrollmentYear = new Label
            {
                Text = "Enrollment Year:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 280),
                AutoSize = true
            };
            this.Controls.Add(lblEnrollmentYear);

            txtEnrollmentYear = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 277),
                Size = new Size(350, 25)
            };
            this.Controls.Add(txtEnrollmentYear);

            // Specialization Label and ComboBox
            Label lblSpecialization = new Label
            {
                Text = "Specialization:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 320),
                AutoSize = true
            };
            this.Controls.Add(lblSpecialization);

            cmbSpecialization = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 317),
                Size = new Size(350, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbSpecialization);

            // Load specializations
            LoadSpecializations();

            // Save Button
            btnSave = new Button
            {
                Text = isAddMode ? "Add Student" : "Save Changes",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(200, 400),
                Size = new Size(170, 45),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(380, 400),
                Size = new Size(170, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
            
            // Only show enrollment management section if editing an existing student
            if (!isAddMode && studentId > 0)
            {
                // Enrollments Section Title
                lblEnrollmentsTitle = new Label
                {
                    Text = "Student Course Enrollments",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(44, 62, 80),
                    Location = new Point(30, 460),
                    AutoSize = true
                };
                this.Controls.Add(lblEnrollmentsTitle);
                
                // DataGridView for Enrollments
                dgvEnrollments = new DataGridView
                {
                    Location = new Point(30, 495),
                    Size = new Size(820, 150),
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    Font = new Font("Segoe UI", 9)
                };
                this.Controls.Add(dgvEnrollments);
                
                // Save Enrollments Button
                btnSaveEnrollments = new Button
                {
                    Text = "Save Enrollment Changes",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(30, 660),
                    Size = new Size(200, 40),
                    Cursor = Cursors.Hand
                };
                btnSaveEnrollments.FlatAppearance.BorderSize = 0;
                btnSaveEnrollments.Click += BtnSaveEnrollments_Click;
                this.Controls.Add(btnSaveEnrollments);
                
                // Load enrollments
                LoadEnrollments();
            }
        }

        private void LoadSpecializations()
        {
            try
            {
                DataTable dtSpecializations = administrator.GetSpecializations();
                
                // Add a default "Select Specialization" option
                DataRow defaultRow = dtSpecializations.NewRow();
                defaultRow["specialization_id"] = DBNull.Value;
                defaultRow["specialization_name"] = "-- Select Specialization --";
                dtSpecializations.Rows.InsertAt(defaultRow, 0);

                cmbSpecialization.DataSource = dtSpecializations;
                cmbSpecialization.DisplayMember = "specialization_name";
                cmbSpecialization.ValueMember = "specialization_id";
                cmbSpecialization.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading specializations: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentData(string email, string firstName, string lastName, 
            string studentNumber, int enrollmentYear)
        {
            try
            {
                if (isAddMode)
                {
                    // Clear all fields for add mode
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtStudentNumber.Text = "";
                    txtEnrollmentYear.Text = "";
                    cmbSpecialization.SelectedIndex = 0;
                }
                else
                {
                    // Load user data to get password
                    DataTable userTable = administrator.GetUserForDropdown();
                    DataRow[] userRows = userTable.Select($"user_id = {userId}");
                    
                    if (userRows.Length > 0)
                    {
                        txtEmail.Text = email;
                        txtPassword.Text = userRows[0]["password"].ToString();
                    }

                    txtFirstName.Text = firstName;
                    txtLastName.Text = lastName;
                    txtStudentNumber.Text = studentNumber;
                    txtEnrollmentYear.Text = enrollmentYear.ToString();

                    // Load student's specialization
                    DataTable studentTable = administrator.GetStudent();
                    DataRow[] studentRows = studentTable.Select($"user_id = {userId}");
                    
                    if (studentRows.Length > 0 && studentRows[0]["specialization_id"] != DBNull.Value)
                    {
                        int specializationId = Convert.ToInt32(studentRows[0]["specialization_id"]);
                        cmbSpecialization.SelectedValue = specializationId;
                    }
                    else
                    {
                        cmbSpecialization.SelectedIndex = 0; // Select default option
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStudentNumber.Text))
            {
                MessageBox.Show("Student number is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentNumber.Focus();
                return;
            }

            if (!int.TryParse(txtEnrollmentYear.Text, out int enrollmentYear) || enrollmentYear < 1900)
            {
                MessageBox.Show("Please enter a valid enrollment year.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEnrollmentYear.Focus();
                return;
            }

            // Get specialization ID (can be null)
            int? specializationId = null;
            if (cmbSpecialization.SelectedValue != null && cmbSpecialization.SelectedValue != DBNull.Value)
            {
                specializationId = Convert.ToInt32(cmbSpecialization.SelectedValue);
            }

            try
            {
                if (isAddMode)
                {
                    // Add new student
                    administrator.AddStudent(
                        txtEmail.Text.Trim(),
                        txtPassword.Text,
                        "Student",
                        txtFirstName.Text.Trim(),
                        txtLastName.Text.Trim(),
                        txtStudentNumber.Text.Trim(),
                        enrollmentYear,
                        specializationId
                    );
                }
                else
                {
                    // Update existing student
                    administrator.EditStudent(
                        userId,
                        txtEmail.Text.Trim(),
                        txtPassword.Text,
                        txtFirstName.Text.Trim(),
                        txtLastName.Text.Trim(),
                        txtStudentNumber.Text.Trim(),
                        enrollmentYear,
                        specializationId
                    );
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving student information: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void LoadEnrollments()
        {
            // Safety check - only load if the grid has been initialized
            if (dgvEnrollments == null)
                return;
                
            try
            {
                DataTable dtEnrollments = administrator.GetStudentEnrollments(studentId);
                
                // Clear existing columns and create them manually
                dgvEnrollments.AutoGenerateColumns = false;
                dgvEnrollments.Columns.Clear();
                
                // Hidden column for enrollment_id
                DataGridViewTextBoxColumn colEnrollmentId = new DataGridViewTextBoxColumn
                {
                    Name = "enrollment_id",
                    DataPropertyName = "enrollment_id",
                    Visible = false
                };
                dgvEnrollments.Columns.Add(colEnrollmentId);
                
                // Course Code column (read-only)
                DataGridViewTextBoxColumn colCourseCode = new DataGridViewTextBoxColumn
                {
                    Name = "course_code",
                    DataPropertyName = "course_code",
                    HeaderText = "Course Code",
                    ReadOnly = true,
                    Width = 100
                };
                dgvEnrollments.Columns.Add(colCourseCode);
                
                // Course Name column (read-only)
                DataGridViewTextBoxColumn colCourseName = new DataGridViewTextBoxColumn
                {
                    Name = "course_name",
                    DataPropertyName = "course_name",
                    HeaderText = "Course Name",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                dgvEnrollments.Columns.Add(colCourseName);
                
                // Credits column (read-only)
                DataGridViewTextBoxColumn colCredits = new DataGridViewTextBoxColumn
                {
                    Name = "credit_hours",
                    DataPropertyName = "credit_hours",
                    HeaderText = "Credits",
                    ReadOnly = true,
                    Width = 70
                };
                dgvEnrollments.Columns.Add(colCredits);
                
                // Semester column (read-only)
                DataGridViewTextBoxColumn colSemester = new DataGridViewTextBoxColumn
                {
                    Name = "semester",
                    DataPropertyName = "semester",
                    HeaderText = "Semester",
                    ReadOnly = true,
                    Width = 150
                };
                dgvEnrollments.Columns.Add(colSemester);
                
                // Status ComboBox column (editable)
                DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn
                {
                    Name = "status",
                    DataPropertyName = "status",
                    HeaderText = "Status",
                    Width = 120
                };
                colStatus.Items.AddRange(new string[] { "Pending", "InProgress", "Completed" });
                dgvEnrollments.Columns.Add(colStatus);
                
                // Grade ComboBox column (editable)
                DataGridViewComboBoxColumn colGrade = new DataGridViewComboBoxColumn
                {
                    Name = "grade",
                    DataPropertyName = "grade",
                    HeaderText = "Grade",
                    Width = 100
                };
                colGrade.Items.AddRange(new string[] { "", "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "F" });
                dgvEnrollments.Columns.Add(colGrade);
                
                // Hidden column for approval_status
                DataGridViewTextBoxColumn colApprovalStatus = new DataGridViewTextBoxColumn
                {
                    Name = "approval_status",
                    DataPropertyName = "approval_status",
                    Visible = false
                };
                dgvEnrollments.Columns.Add(colApprovalStatus);
                
                // Bind the data
                dgvEnrollments.DataSource = dtEnrollments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollments: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSaveEnrollments_Click(object sender, EventArgs e)
        {
            // Safety check
            if (dgvEnrollments == null)
                return;
                
            try
            {
                int updatedCount = 0;
                
                foreach (DataGridViewRow row in dgvEnrollments.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    int enrollmentId = Convert.ToInt32(row.Cells["enrollment_id"].Value);
                    string status = row.Cells["status"].Value?.ToString() ?? "Pending";
                    string grade = row.Cells["grade"].Value?.ToString() ?? "";
                    
                    if (administrator.UpdateEnrollmentStatusAndGrade(enrollmentId, status, grade))
                    {
                        updatedCount++;
                    }
                }
                
                if (updatedCount > 0)
                {
                    MessageBox.Show($"Successfully updated {updatedCount} enrollment(s)!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No enrollments were updated.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving enrollments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
