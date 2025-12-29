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
    public partial class frmAdviser : Form
    {
        private Adviser adviser;
        private Button currentActiveButton;

        public frmAdviser(int userId, string email)
        {
            InitializeComponent();
            try
            {
                adviser = new Adviser(userId, email);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing adviser: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void frmAdviser_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {adviser.Email}";
            SetActiveButton(btnMyAdvisees);
            LoadMyAdvisees();
        }

        private void SetActiveButton(Button button)
        {
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.FromArgb(52, 73, 94);
            }
            
            button.BackColor = Color.FromArgb(41, 128, 185);
            currentActiveButton = button;
        }

        private void ClearContentPanel()
        {
            panelContent.Controls.Clear();
        }

        private void btnMyAdvisees_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnMyAdvisees);
            LoadMyAdvisees();
        }

        private void LoadMyAdvisees()
        {
            ClearContentPanel();
            
            Label lblTitle = new Label
            {
                Text = "My Advisees",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            DataGridView dgvAdvisees = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(850, 450),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelContent.Controls.Add(dgvAdvisees);
            
            try
            {
                DataTable dt = adviser.ViewAssignedAdvisees();
                dgvAdvisees.DataSource = dt;
                
                dgvAdvisees.Columns["student_id"].HeaderText = "ID";
                dgvAdvisees.Columns["student_number"].HeaderText = "Student Number";
                dgvAdvisees.Columns["student_name"].HeaderText = "Name";
                dgvAdvisees.Columns["specialization_name"].HeaderText = "Specialization";
                dgvAdvisees.Columns["current_semester"].HeaderText = "Semester";
                dgvAdvisees.Columns["gpa"].HeaderText = "GPA";
                dgvAdvisees.Columns["completed_credit_hours"].HeaderText = "Credits";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading advisees: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAcademicHistory_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAcademicHistory);
            LoadAcademicHistory();
        }

        private void LoadAcademicHistory()
        {
            ClearContentPanel();
            
            Label lblTitle = new Label
            {
                Text = "Academic History",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            Label lblInstruction = new Label
            {
                Text = "Select a student to view their academic history:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            panelContent.Controls.Add(lblInstruction);
            
            ComboBox cboStudents = new ComboBox
            {
                Location = new Point(30, 100),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(cboStudents);
            
            // Student Info Panel
            Panel pnlStudentInfo = new Panel
            {
                Location = new Point(30, 140),
                Size = new Size(850, 120),
                BackColor = Color.FromArgb(236, 240, 241),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelContent.Controls.Add(pnlStudentInfo);
            
            DataGridView dgvHistory = new DataGridView
            {
                Location = new Point(30, 270),
                Size = new Size(850, 260),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            panelContent.Controls.Add(dgvHistory);
            
            // Attach event handler FIRST
            cboStudents.SelectedIndexChanged += (s, ev) =>
            {
                if (cboStudents.SelectedValue != null && cboStudents.SelectedIndex >= 0)
                {
                    try
                    {
                        int studentId = Convert.ToInt32(cboStudents.SelectedValue);
                        LoadStudentInfo(studentId, pnlStudentInfo);
                        LoadStudentAcademicHistory(studentId, dgvHistory);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading student data: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            
            // Load students into dropdown
            LoadStudentsComboBox(cboStudents);
            
            // Auto-select first student AFTER everything is set up
            if (cboStudents.Items.Count > 0)
            {
                cboStudents.SelectedIndex = 0;
            }
            else
            {
                Label lblNoStudents = new Label
                {
                    Text = "No students assigned to you yet.",
                    Location = new Point(30, 150),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Gray
                };
                panelContent.Controls.Add(lblNoStudents);
            }
        }

        private void LoadStudentInfo(int studentId, Panel panel)
        {
            panel.Controls.Clear();
            
            try
            {
                DataTable dt = adviser.GetStudentInfo(studentId);
                
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    
                    // Column 1
                    panel.Controls.Add(new Label
                    {
                        Text = $"Student: {row["student_name"]}",
                        Location = new Point(15, 10),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"Student Number: {row["student_number"]}",
                        Location = new Point(15, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"Email: {row["email"]}",
                        Location = new Point(15, 55),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"Specialization: {row["specialization_name"]}",
                        Location = new Point(15, 75),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    // Column 2
                    panel.Controls.Add(new Label
                    {
                        Text = $"Current Semester: {row["current_semester"]}",
                        Location = new Point(350, 10),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"GPA: {row["gpa"]}",
                        Location = new Point(350, 35),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"Completed Credits: {row["completed_credit_hours"]}",
                        Location = new Point(350, 60),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = $"Enrollment Year: {row["enrollment_year"]}",
                        Location = new Point(350, 85),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 9)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student info: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentsComboBox(ComboBox combo)
        {
            try
            {
                DataTable dt = adviser.GetAdviseesForDropdown();
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    combo.DisplayMember = "display_name";
                    combo.ValueMember = "student_id";
                    combo.DataSource = dt;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No students found for this adviser");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentAcademicHistory(int studentId, DataGridView dgv)
        {
            try
            {
                DataTable dt = adviser.GetAcademicHistory(studentId);
                dgv.DataSource = dt;
                
                dgv.Columns["course_code"].HeaderText = "Course Code";
                dgv.Columns["course_name"].HeaderText = "Course Name";
                dgv.Columns["credit_hours"].HeaderText = "Credits";
                dgv.Columns["grade"].HeaderText = "Grade";
                dgv.Columns["status"].HeaderText = "Status";
                dgv.Columns["semester"].HeaderText = "Semester";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading academic history: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCourseRecommendations_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCourseRecommendations);
            LoadCourseRecommendations();
        }

        private void LoadCourseRecommendations()
        {
            ClearContentPanel();
            
            Label lblTitle = new Label
            {
                Text = "Course Recommendations",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            Label lblInstruction = new Label
            {
                Text = "Select a student to view eligible courses:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            panelContent.Controls.Add(lblInstruction);
            
            ComboBox cboStudents = new ComboBox
            {
                Location = new Point(30, 100),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(cboStudents);
            
            DataGridView dgvRecommendations = new DataGridView
            {
                Location = new Point(30, 150),
                Size = new Size(850, 380),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelContent.Controls.Add(dgvRecommendations);
            
            // Attach event handler FIRST
            cboStudents.SelectedIndexChanged += (s, ev) =>
            {
                if (cboStudents.SelectedValue != null && cboStudents.SelectedIndex >= 0)
                {
                    try
                    {
                        int studentId = Convert.ToInt32(cboStudents.SelectedValue);
                        LoadEligibleCourses(studentId, dgvRecommendations);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
            
            // Load students into dropdown
            LoadStudentsComboBox(cboStudents);
            
            // Auto-select first student AFTER everything is set up
            if (cboStudents.Items.Count > 0)
            {
                cboStudents.SelectedIndex = 0;
            }
            else
            {
                Label lblNoStudents = new Label
                {
                    Text = "No students assigned to you yet.",
                    Location = new Point(30, 150),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Gray
                };
                panelContent.Controls.Add(lblNoStudents);
            }
        }

        private void LoadEligibleCourses(int studentId, DataGridView dgv)
        {
            try
            {
                DataTable dt = adviser.RecommendCourses(studentId);
                dgv.DataSource = dt;
                
                dgv.Columns["course_code"].HeaderText = "Course Code";
                dgv.Columns["course_name"].HeaderText = "Course Name";
                dgv.Columns["credit_hours"].HeaderText = "Credits";
                dgv.Columns["course_type"].HeaderText = "Type";
                dgv.Columns["eligibility_status"].HeaderText = "Status";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course recommendations: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApprovePlans_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnApprovePlans);
            LoadPendingApprovals();
        }

        private void LoadPendingApprovals()
        {
            ClearContentPanel();
            
            Label lblTitle = new Label
            {
                Text = "Approve Course Plans",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            Label lblInstruction = new Label
            {
                Text = "Pending course enrollments awaiting your approval:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 70),
                AutoSize = true
            };
            panelContent.Controls.Add(lblInstruction);
            
            DataGridView dgvPending = new DataGridView
            {
                Location = new Point(30, 110),
                Size = new Size(850, 320),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            panelContent.Controls.Add(dgvPending);
            
            Button btnApprove = new Button
            {
                Text = "Approve Selected",
                Location = new Point(30, 450),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnApprove.FlatAppearance.BorderSize = 0;
            panelContent.Controls.Add(btnApprove);
            
            Button btnReject = new Button
            {
                Text = "Reject Selected",
                Location = new Point(200, 450),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReject.FlatAppearance.BorderSize = 0;
            panelContent.Controls.Add(btnReject);
            
            LoadPendingEnrollments(dgvPending);
            
            btnApprove.Click += (s, ev) => ProcessApproval(dgvPending, "Approved");
            btnReject.Click += (s, ev) => ProcessApproval(dgvPending, "Rejected");
        }

        private void LoadPendingEnrollments(DataGridView dgv)
        {
            try
            {
                DataTable dt = adviser.GetPendingApprovals();
                dgv.DataSource = dt;
                
                dgv.Columns["enrollment_id"].HeaderText = "ID";
                dgv.Columns["student_name"].HeaderText = "Student";
                dgv.Columns["student_number"].HeaderText = "Student #";
                dgv.Columns["course_code"].HeaderText = "Course";
                dgv.Columns["course_name"].HeaderText = "Course Name";
                dgv.Columns["credit_hours"].HeaderText = "Credits";
                dgv.Columns["semester"].HeaderText = "Semester";
                dgv.Columns["enrollment_date"].HeaderText = "Date";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pending enrollments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessApproval(DataGridView dgv, string status)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an enrollment to process.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                int enrollmentId = Convert.ToInt32(dgv.SelectedRows[0].Cells["enrollment_id"].Value);
                string studentName = dgv.SelectedRows[0].Cells["student_name"].Value.ToString();
                string courseName = dgv.SelectedRows[0].Cells["course_name"].Value.ToString();
                
                string remarks = "";
                
                // Prompt for remarks
                using (Form remarkForm = new Form())
                {
                    remarkForm.Text = $"{status} Enrollment";
                    remarkForm.Size = new Size(500, 300);
                    remarkForm.StartPosition = FormStartPosition.CenterParent;
                    remarkForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    remarkForm.MaximizeBox = false;
                    remarkForm.MinimizeBox = false;
                    
                    Label lblInfo = new Label
                    {
                        Text = $"Student: {studentName}\nCourse: {courseName}\n\n" +
                               (status == "Rejected" ? "Please provide a reason for rejection:" : "Optional remarks:"),
                        Location = new Point(20, 20),
                        Size = new Size(440, 80),
                        Font = new Font("Segoe UI", 10)
                    };
                    remarkForm.Controls.Add(lblInfo);
                    
                    TextBox txtRemarks = new TextBox
                    {
                        Location = new Point(20, 110),
                        Size = new Size(440, 80),
                        Multiline = true,
                        ScrollBars = ScrollBars.Vertical,
                        Font = new Font("Segoe UI", 10)
                    };
                    remarkForm.Controls.Add(txtRemarks);
                    
                    Button btnOK = new Button
                    {
                        Text = "OK",
                        DialogResult = DialogResult.OK,
                        Location = new Point(280, 210),
                        Size = new Size(90, 35),
                        BackColor = Color.FromArgb(52, 152, 219),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };
                    btnOK.FlatAppearance.BorderSize = 0;
                    remarkForm.Controls.Add(btnOK);
                    
                    Button btnCancel = new Button
                    {
                        Text = "Cancel",
                        DialogResult = DialogResult.Cancel,
                        Location = new Point(380, 210),
                        Size = new Size(90, 35),
                        BackColor = Color.FromArgb(189, 195, 199),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };
                    btnCancel.FlatAppearance.BorderSize = 0;
                    remarkForm.Controls.Add(btnCancel);
                    
                    remarkForm.AcceptButton = btnOK;
                    remarkForm.CancelButton = btnCancel;
                    
                    if (remarkForm.ShowDialog() == DialogResult.OK)
                    {
                        remarks = txtRemarks.Text.Trim();
                        
                        // For rejection, require remarks
                        if (status == "Rejected" && string.IsNullOrWhiteSpace(remarks))
                        {
                            MessageBox.Show("Please provide a reason for rejection.", "Remarks Required", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        return; // User cancelled
                    }
                }
                
                if (status == "Approved")
                {
                    adviser.ApproveCoursePlan(enrollmentId, remarks);
                }
                else if (status == "Rejected")
                {
                    adviser.RejectCoursePlan(enrollmentId, remarks);
                }
                
                MessageBox.Show($"Enrollment {status.ToLower()} successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadPendingEnrollments(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing approval: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnGenerateReports);
            LoadGenerateReports();
        }

        private void LoadGenerateReports()
        {
            ClearContentPanel();
            
            Label lblTitle = new Label
            {
                Text = "Generate Reports",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);
            
            Label lblStudent = new Label
            {
                Text = "Select Student:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 80),
                AutoSize = true
            };
            panelContent.Controls.Add(lblStudent);
            
            ComboBox cboStudents = new ComboBox
            {
                Location = new Point(30, 110),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panelContent.Controls.Add(cboStudents);
            
            Label lblReportType = new Label
            {
                Text = "Report Type:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 160),
                AutoSize = true
            };
            panelContent.Controls.Add(lblReportType);
            
            ComboBox cboReportType = new ComboBox
            {
                Location = new Point(30, 190),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboReportType.Items.AddRange(new object[] { "Progress Report", "Transcript", "Advising Report", "Course History" });
            cboReportType.SelectedIndex = 0;
            panelContent.Controls.Add(cboReportType);
            
            Button btnGenerate = new Button
            {
                Text = "Generate Report",
                Location = new Point(30, 240),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnGenerate);
            
            TextBox txtReportOutput = new TextBox
            {
                Location = new Point(30, 300),
                Size = new Size(850, 230),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Courier New", 9),
                ReadOnly = true
            };
            panelContent.Controls.Add(txtReportOutput);
            
            LoadStudentsComboBox(cboStudents);
            
            btnGenerate.Click += (s, ev) =>
            {
                if (cboStudents.SelectedValue != null)
                {
                    int studentId = Convert.ToInt32(cboStudents.SelectedValue);
                    string reportType = cboReportType.SelectedItem.ToString();
                    GenerateReport(studentId, reportType, txtReportOutput);
                }
                else
                {
                    MessageBox.Show("Please select a student.", "No Selection", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        private void GenerateReport(int studentId, string reportType, TextBox output)
        {
            try
            {
                string reportContent = adviser.GenerateReportContent(studentId, reportType);
                output.Text = reportContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
