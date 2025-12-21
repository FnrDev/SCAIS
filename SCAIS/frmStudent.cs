using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCAIS
{
    public partial class frmStudent : Form
    {
        private int studentId;
        private string studentEmail;
        private Student studentModel;
        private Button currentActiveButton;
        private Panel panelSidebar;
        private Panel panelMain;
        private Panel panelContent;

        public frmStudent(int studentId, string email)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.studentEmail = email;
            this.studentModel = new Student();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {studentEmail}";
            BuildLayout();
            SetActiveButton(CreateSidebarButton(">  Academic Record", 20, AcademicRecord_Click));
            LoadAcademicRecord();
        }

        private void BuildLayout()
        {
            // Create sidebar (if not already in designer)
            if (panelSidebar == null)
            {
                panelSidebar = new Panel
                {
                    BackColor = Color.FromArgb(44, 62, 80),
                    Size = new Size(250, this.ClientSize.Height - 100),
                    Location = new Point(0, 100),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
                };
                this.Controls.Add(panelSidebar);
            }

            // Create main panel container
            if (panelMain == null)
            {
                panelMain = new Panel
                {
                    BackColor = Color.FromArgb(236, 240, 241),
                    Location = new Point(250, 100),
                    Size = new Size(this.ClientSize.Width - 250, this.ClientSize.Height - 100),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    Padding = new Padding(20)
                };
                this.Controls.Add(panelMain);
            }

            // Create content panel inside main
            if (panelContent == null)
            {
                panelContent = new Panel
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(30),
                    AutoScroll = true
                };
                panelMain.Controls.Add(panelContent);
            }

            // Sidebar buttons
            // Clear existing sidebar children to avoid duplicates on resize
            panelSidebar.Controls.Clear();

            // Buttons
            Button btnAcademicRecord = CreateSidebarButton(">  Academic Record", 20, AcademicRecord_Click);
            Button btnEligibleCourses = CreateSidebarButton(">  Eligible Courses", 80, EligibleCourses_Click);
            Button btnSubmitPreferences = CreateSidebarButton(">  Submit Preferences", 140, SubmitPreferences_Click);
            Button btnApprovedCourses = CreateSidebarButton(">  Approved Courses & Remarks", 200, ApprovedCourses_Click);
            Button btnLogoutSide = CreateSidebarButton(">  Logout", 260, Logout_Click, backColor: Color.FromArgb(231, 76, 60));

            panelSidebar.Controls.Add(btnAcademicRecord);
            panelSidebar.Controls.Add(btnEligibleCourses);
            panelSidebar.Controls.Add(btnSubmitPreferences);
            panelSidebar.Controls.Add(btnApprovedCourses);
            panelSidebar.Controls.Add(btnLogoutSide);
        }

        private Button CreateSidebarButton(string text, int top, EventHandler onClick, Color? backColor = null)
        {
            Button btn = new Button
            {
                BackColor = backColor ?? Color.FromArgb(52, 73, 94),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                Location = new Point(0, top),
                Padding = new Padding(20, 0, 0, 0),
                Size = new Size(250, 60),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void SetActiveButton(Button button)
        {
            if (currentActiveButton != null)
                currentActiveButton.BackColor = Color.FromArgb(52, 73, 94);

            button.BackColor = Color.FromArgb(41, 128, 185);
            currentActiveButton = button;
        }

        private void ClearContentPanel()
        {
            panelContent.Controls.Clear();
        }

        // Page: Academic Record
        private void AcademicRecord_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            LoadAcademicRecord();
        }

        private void LoadAcademicRecord()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "My Academic Record",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            // Summary area (GPA, Credits)
            Label lblSummary = new Label
            {
                Text = "Summary",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(30, 70),
                AutoSize = true
            };
            panelContent.Controls.Add(lblSummary);

            // Attempt to load student summary
            try
            {
                // Load basic student summary 
                var db = new Student();
                DataTable summaryTable = GetStudentSummary(studentId);
                int top = 100;

                if (summaryTable.Rows.Count > 0)
                {
                    DataRow r = summaryTable.Rows[0];

                    Label lblName = new Label { Text = $"Name: {r["name"]}", Location = new Point(30, top), AutoSize = true, Font = new Font("Segoe UI", 10) };
                    Label lblNumber = new Label { Text = $"Student Number: {r["student_number"]}", Location = new Point(30, top + 25), AutoSize = true, Font = new Font("Segoe UI", 10) };
                    Label lblSpec = new Label { Text = $"Specialization: {r["specialization_name"]}", Location = new Point(30, top + 50), AutoSize = true, Font = new Font("Segoe UI", 10) };
                    Label lblGPA = new Label { Text = $"GPA: {r["gpa"]}", Location = new Point(350, top), AutoSize = true, Font = new Font("Segoe UI", 10) };
                    Label lblCredits = new Label { Text = $"Completed Credits: {r["completed_credit_hours"]}", Location = new Point(350, top + 25), AutoSize = true, Font = new Font("Segoe UI", 10) };

                    panelContent.Controls.Add(lblName);
                    panelContent.Controls.Add(lblNumber);
                    panelContent.Controls.Add(lblSpec);
                    panelContent.Controls.Add(lblGPA);
                    panelContent.Controls.Add(lblCredits);
                }

                // Course history grid
                DataGridView dgvHistory = new DataGridView
                {
                    Location = new Point(30, 180),
                    Size = new Size(850, 380),
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None
                };
                panelContent.Controls.Add(dgvHistory);

                DataTable dtHistory = studentModel.GetAcademicHistory(studentId);
                dgvHistory.DataSource = dtHistory;

                if (dtHistory.Columns.Contains("course_code"))
                {
                    dgvHistory.Columns["course_code"].HeaderText = "Course Code";
                    dgvHistory.Columns["course_name"].HeaderText = "Course Name";
                    dgvHistory.Columns["credit_hours"].HeaderText = "Credits";
                    dgvHistory.Columns["grade"].HeaderText = "Grade";
                    dgvHistory.Columns["status"].HeaderText = "Status";
                    dgvHistory.Columns["semester"].HeaderText = "Semester";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading academic record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper to get student summary (Name, number, specialization, gpa, credits)
        private DataTable GetStudentSummary(int studentId)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                var conn = dbConn.GetConnection();
                string studentQuery = @"SELECT s.first_name + ' ' + s.last_name AS name, 
                                   s.student_number, sp.specialization_name, 
                                   s.current_semester, s.gpa, s.completed_credit_hours
                                   FROM students s
                                   LEFT JOIN specializations sp ON s.specialization_id = sp.specialization_id
                                   WHERE s.student_id = @studentId";

                using (var cmd = new System.Data.SqlClient.SqlCommand(studentQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading student summary: {ex.Message}");
            }
        }

        // Page: Eligible Courses
        private void EligibleCourses_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            LoadEligibleCourses();
        }

        private void LoadEligibleCourses()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Eligible Courses",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            DataGridView dgv = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(850, 430),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelContent.Controls.Add(dgv);

            try
            {
                DataTable dt = studentModel.GetEligibleCourses(studentId);
                dgv.DataSource = dt;

                if (dt.Columns.Contains("course_code"))
                {
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                    dgv.Columns["course_type"].HeaderText = "Type";
                    dgv.Columns["eligibility_status"].HeaderText = "Status";
                    // course_id hidden for actions
                    if (dgv.Columns.Contains("course_id"))
                        dgv.Columns["course_id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading eligible courses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page: Submit Preferences
        private void SubmitPreferences_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            LoadSubmitPreferences();
        }

        private void LoadSubmitPreferences()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Submit Preferred Courses",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            DataGridView dgv = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(850, 360),
                AllowUserToAddRows = false,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelContent.Controls.Add(dgv);

            Button btnSubmit = new Button
            {
                Text = "Submit Selected",
                Location = new Point(30, 460),
                Size = new Size(160, 45),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            panelContent.Controls.Add(btnSubmit);

            try
            {
                DataTable dt = studentModel.GetEligibleCourses(studentId);
                dgv.DataSource = dt;

                // add checkbox column
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Select",
                    Width = 50,
                    Name = "chkSelect"
                };
                dgv.Columns.Insert(0, chk);

                if (dt.Columns.Contains("course_id"))
                    dgv.Columns["course_id"].Visible = false;

                btnSubmit.Click += (s, ev) =>
                {
                    List<int> selectedCourseIds = new List<int>();
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        bool isChecked = false;
                        if (row.Cells["chkSelect"].Value != null)
                            isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);

                        if (isChecked)
                        {
                            if (dt.Columns.Contains("course_id"))
                            {
                                int cid = Convert.ToInt32(row.Cells["course_id"].Value);
                                selectedCourseIds.Add(cid);
                            }
                        }
                    }

                    if (selectedCourseIds.Count == 0)
                    {
                        MessageBox.Show("Please select at least one course to submit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        var result = studentModel.SubmitCoursePreferences(studentId, selectedCourseIds);
                        MessageBox.Show($"Submitted: {result.inserted} course(s). Skipped: {result.skipped} (duplicates/constraints).", "Submission Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSubmitPreferences(); // refresh grid
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting preferences: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading eligible courses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page: Approved Courses + Adviser Remarks
        private void ApprovedCourses_Click(object sender, EventArgs e)
        {
            SetActiveButton((Button)sender);
            LoadApprovedCourses();
        }

        private void LoadApprovedCourses()
        {
            ClearContentPanel();

            Label lblTitle = new Label
            {
                Text = "Approved Courses & Adviser Remarks",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 30),
                AutoSize = true
            };
            panelContent.Controls.Add(lblTitle);

            DataGridView dgv = new DataGridView
            {
                Location = new Point(30, 80),
                Size = new Size(850, 430),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelContent.Controls.Add(dgv);

            try
            {
                DataTable dt = studentModel.GetApprovedCoursesAndRemarks(studentId);
                dgv.DataSource = dt;

                if (dt.Columns.Contains("course_code"))
                {
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                    dgv.Columns["semester"].HeaderText = "Semester";
                    dgv.Columns["approval_status"].HeaderText = "Status";
                    dgv.Columns["adviser_remarks"].HeaderText = "Adviser Remarks";
                }

                // Auto-resize remarks column
                if (dgv.Columns.Contains("adviser_remarks"))
                    dgv.Columns["adviser_remarks"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading approved courses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        // Designer-generated controls may define lblWelcome and btnLogout; btnLogout's click still handled in designer.
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout_Click(sender, e);
        }
    }
}