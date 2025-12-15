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
        private int userId;
        private int adviserId;
        private string adviserEmail;
        private Button currentActiveButton;

        public frmAdviser(int userId, string email)
        {
            InitializeComponent();
            this.userId = userId;
            this.adviserEmail = email;
            LoadAdviserId();
        }

        private void LoadAdviserId()
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                string query = "SELECT adviser_id FROM advisers WHERE user_id = @userId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    dbConn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        adviserId = Convert.ToInt32(result);
                    }
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading adviser data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmAdviser_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {adviserEmail}";
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
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"SELECT s.student_id, s.student_number, 
                                s.first_name + ' ' + s.last_name AS student_name,
                                sp.specialization_name, s.current_semester, s.gpa,
                                s.completed_credit_hours
                                FROM students s
                                LEFT JOIN specializations sp ON s.specialization_id = sp.specialization_id
                                WHERE s.adviser_id = @adviserId
                                ORDER BY s.last_name, s.first_name";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvAdvisees.DataSource = dt;
                    
                    dgvAdvisees.Columns["student_id"].HeaderText = "ID";
                    dgvAdvisees.Columns["student_number"].HeaderText = "Student Number";
                    dgvAdvisees.Columns["student_name"].HeaderText = "Name";
                    dgvAdvisees.Columns["specialization_name"].HeaderText = "Specialization";
                    dgvAdvisees.Columns["current_semester"].HeaderText = "Semester";
                    dgvAdvisees.Columns["gpa"].HeaderText = "GPA";
                    dgvAdvisees.Columns["completed_credit_hours"].HeaderText = "Credits";
                }
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
                Text = "Select a student to view their academic history and specialization:",
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
            
            DataGridView dgvHistory = new DataGridView
            {
                Location = new Point(30, 150),
                Size = new Size(850, 380),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelContent.Controls.Add(dgvHistory);
            
            LoadStudentsComboBox(cboStudents);
            
            cboStudents.SelectedIndexChanged += (s, ev) =>
            {
                if (cboStudents.SelectedValue != null)
                {
                    int studentId = Convert.ToInt32(cboStudents.SelectedValue);
                    LoadStudentAcademicHistory(studentId, dgvHistory);
                }
            };
        }

        private void LoadStudentsComboBox(ComboBox combo)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"SELECT student_id, first_name + ' ' + last_name + ' (' + student_number + ')' AS display_name
                                FROM students WHERE adviser_id = @adviserId
                                ORDER BY last_name, first_name";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    combo.DisplayMember = "display_name";
                    combo.ValueMember = "student_id";
                    combo.DataSource = dt;
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
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"SELECT c.course_code, c.course_name, c.credit_hours,
                                e.grade, e.status, sem.semester_name + ' ' + sem.academic_year AS semester
                                FROM enrollments e
                                INNER JOIN courses c ON e.course_id = c.course_id
                                INNER JOIN semesters sem ON e.semester_id = sem.semester_id
                                WHERE e.student_id = @studentId
                                ORDER BY sem.start_date DESC";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                    
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                    dgv.Columns["grade"].HeaderText = "Grade";
                    dgv.Columns["status"].HeaderText = "Status";
                    dgv.Columns["semester"].HeaderText = "Semester";
                }
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
            
            LoadStudentsComboBox(cboStudents);
            
            cboStudents.SelectedIndexChanged += (s, ev) =>
            {
                if (cboStudents.SelectedValue != null)
                {
                    int studentId = Convert.ToInt32(cboStudents.SelectedValue);
                    LoadEligibleCourses(studentId, dgvRecommendations);
                }
            };
        }

        private void LoadEligibleCourses(int studentId, DataGridView dgv)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"SELECT c.course_code, c.course_name, c.credit_hours, c.course_type,
                                CASE WHEN EXISTS (
                                    SELECT 1 FROM prerequisites p
                                    WHERE p.course_id = c.course_id
                                    AND NOT EXISTS (
                                        SELECT 1 FROM enrollments e2
                                        WHERE e2.student_id = @studentId
                                        AND e2.course_id = p.prerequisite_course_id
                                        AND e2.status = 'Completed'
                                    )
                                ) THEN 'Prerequisites Not Met' ELSE 'Eligible' END AS eligibility_status
                                FROM courses c
                                WHERE c.is_active = 1
                                AND c.course_id NOT IN (
                                    SELECT course_id FROM enrollments
                                    WHERE student_id = @studentId
                                    AND status IN ('Completed', 'InProgress')
                                )
                                ORDER BY c.course_code";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                    
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                    dgv.Columns["course_type"].HeaderText = "Type";
                    dgv.Columns["eligibility_status"].HeaderText = "Status";
                }
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
                Size = new Size(850, 350),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelContent.Controls.Add(dgvPending);
            
            Button btnApprove = new Button
            {
                Text = "Approve Selected",
                Location = new Point(30, 380),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnApprove);
            
            Button btnReject = new Button
            {
                Text = "Reject Selected",
                Location = new Point(200, 380),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnReject);
            
            LoadPendingEnrollments(dgvPending);
            
            btnApprove.Click += (s, ev) => ProcessApproval(dgvPending, "Approved");
            btnReject.Click += (s, ev) => ProcessApproval(dgvPending, "Rejected");
        }

        private void LoadPendingEnrollments(DataGridView dgv)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"SELECT e.enrollment_id, s.first_name + ' ' + s.last_name AS student_name,
                                s.student_number, c.course_code, c.course_name, c.credit_hours,
                                sem.semester_name + ' ' + sem.academic_year AS semester,
                                e.enrollment_date
                                FROM enrollments e
                                INNER JOIN students s ON e.student_id = s.student_id
                                INNER JOIN courses c ON e.course_id = c.course_id
                                INNER JOIN semesters sem ON e.semester_id = sem.semester_id
                                WHERE s.adviser_id = @adviserId
                                AND e.approval_status = 'PendingApproval'
                                ORDER BY e.enrollment_date";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
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
                
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"UPDATE enrollments 
                                SET approval_status = @status, 
                                    adviser_id = @adviserId,
                                    approval_date = @approvalDate
                                WHERE enrollment_id = @enrollmentId";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    cmd.Parameters.AddWithValue("@approvalDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);
                    
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                    dbConn.Close();
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
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                StringBuilder report = new StringBuilder();
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine($"  {reportType.ToUpper()}");
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
                report.AppendLine();
                
                string studentQuery = @"SELECT s.first_name + ' ' + s.last_name AS name, 
                                       s.student_number, sp.specialization_name, 
                                       s.current_semester, s.gpa, s.completed_credit_hours
                                       FROM students s
                                       LEFT JOIN specializations sp ON s.specialization_id = sp.specialization_id
                                       WHERE s.student_id = @studentId";
                
                using (SqlCommand cmd = new SqlCommand(studentQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    dbConn.Open();
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            report.AppendLine($"Student: {reader["name"]}");
                            report.AppendLine($"Student Number: {reader["student_number"]}");
                            report.AppendLine($"Specialization: {reader["specialization_name"]}");
                            report.AppendLine($"Current Semester: {reader["current_semester"]}");
                            report.AppendLine($"GPA: {reader["gpa"]}");
                            report.AppendLine($"Completed Credits: {reader["completed_credit_hours"]}");
                        }
                    }
                    
                    dbConn.Close();
                }
                
                report.AppendLine();
                report.AppendLine("-".PadRight(60, '-'));
                report.AppendLine("COURSE HISTORY");
                report.AppendLine("-".PadRight(60, '-'));
                
                string coursesQuery = @"SELECT c.course_code, c.course_name, e.grade, e.status
                                       FROM enrollments e
                                       INNER JOIN courses c ON e.course_id = c.course_id
                                       WHERE e.student_id = @studentId
                                       ORDER BY e.enrollment_date";
                
                using (SqlCommand cmd = new SqlCommand(coursesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        report.AppendLine($"{row["course_code"],-10} {row["course_name"],-30} {row["grade"],-5} {row["status"]}");
                    }
                }
                
                report.AppendLine();
                report.AppendLine("=".PadRight(60, '='));
                report.AppendLine("End of Report");
                report.AppendLine("=".PadRight(60, '='));
                
                output.Text = report.ToString();
                
                SaveReportToDatabase(studentId, reportType, report.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveReportToDatabase(int studentId, string reportType, string content)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();
                
                string query = @"INSERT INTO reports (generated_by, student_id, report_type, content, generated_date)
                                VALUES (@adviserId, @studentId, @reportType, @content, @generatedDate)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    cmd.Parameters.AddWithValue("@reportType", reportType.Replace(" ", ""));
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@generatedDate", DateTime.Now);
                    
                    dbConn.Open();
                    cmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving report: {ex.Message}");
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
