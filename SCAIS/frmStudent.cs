using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace SCAIS
{
    public partial class frmStudent : Form
    {
        private readonly int studentId;
        private readonly string studentEmail;
        private readonly Student studentModel;
        private Button currentActiveButton;
        private void SetActiveButton(Button btn)
        {
            if (currentActiveButton != null && currentActiveButton != btn)
                currentActiveButton.BackColor = Color.FromArgb(52, 73, 94); // normal sidebar

            btn.BackColor = Color.FromArgb(41, 128, 185); // active highlight
            currentActiveButton = btn;
        }

        private DataTable GetStudentSummary(int studentId)
        {
            DatabaseConnection dbConn = DatabaseConnection.Instance;
            var conn = dbConn.GetConnection();

            string studentQuery = @"
        SELECT 
            s.first_name + ' ' + s.last_name AS name, 
            s.student_number, 
            sp.specialization_name, 
            s.current_semester, 
            s.gpa, 
            s.completed_credit_hours
        FROM students s
        LEFT JOIN specializations sp ON s.specialization_id = sp.specialization_id
        WHERE s.student_id = @studentId";

            using (var cmd = new SqlCommand(studentQuery, conn))
            {
                cmd.Parameters.AddWithValue("@studentId", studentId);
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        public frmStudent(int studentId, string email)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.studentEmail = email;
            this.studentModel = new Student();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            SetActiveButton(btnAcademicRecord);

            lblWelcome.Text = $"Welcome, {studentEmail}";
            LoadAcademicRecord(); // default page

        }

        private void ClearContent()
        {
            pnlContent.Controls.Clear();
        }
   
        // Sidebar button events (connect in Designer by double-clicking each button)
        private void btnAcademicRecord_Click(object sender, EventArgs e) => LoadAcademicRecord();
        private void btnEligibleCourses_Click(object sender, EventArgs e) => LoadEligibleCourses();
        private void btnSubmitPreferences_Click(object sender, EventArgs e) => LoadSubmitPreferences();
        private void btnApprovedCourses_Click(object sender, EventArgs e) => LoadApprovedCourses();

        private void btnLogout_Click(object sender, EventArgs e) => Logout();

        private void Logout()
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void LoadAcademicRecord()
        {
            ClearContent();

            // Title
            var lblTitle = new Label
            {
                Text = "My Academic Record",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlContent.Controls.Add(lblTitle);

            // Summary heading
            var lblSummary = new Label
            {
                Text = "Summary",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 60)
            };
            pnlContent.Controls.Add(lblSummary);

            try
            {
                // Summary info
                DataTable summaryTable = GetStudentSummary(studentId);
                if (summaryTable.Rows.Count > 0)
                {
                    DataRow r = summaryTable.Rows[0];
                    int top = 90;

                    pnlContent.Controls.Add(new Label
                    {
                        Text = $"Name: {r["name"]}",
                        Location = new Point(20, top),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    });

                    pnlContent.Controls.Add(new Label
                    {
                        Text = $"Student Number: {r["student_number"]}",
                        Location = new Point(20, top + 25),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    });

                    pnlContent.Controls.Add(new Label
                    {
                        Text = $"Specialization: {r["specialization_name"]}",
                        Location = new Point(20, top + 50),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    });

                    pnlContent.Controls.Add(new Label
                    {
                        Text = $"GPA: {r["gpa"]}",
                        Location = new Point(350, top),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    });

                    pnlContent.Controls.Add(new Label
                    {
                        Text = $"Completed Credits: {r["completed_credit_hours"]}",
                        Location = new Point(350, top + 25),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    });
                }
                
                // Course History heading
                var lblCourseHistory = new Label
                {
                    Text = "Course History",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(20, 215)
                };
                pnlContent.Controls.Add(lblCourseHistory);
                
                // DataGridView for course enrollments
                var dgvEnrollments = new DataGridView
                {
                    Location = new Point(20, 245),
                    Size = new Size(pnlContent.Width - 40, pnlContent.Height - 265),
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
                    Font = new Font("Segoe UI", 9)
                };
                pnlContent.Controls.Add(dgvEnrollments);
                
                // Load academic history (enrollments with status and grades)
                DataTable dtHistory = studentModel.GetAcademicHistory(studentId);
                dgvEnrollments.DataSource = dtHistory;
                
                // Set column headers
                if (dgvEnrollments.Columns.Contains("course_code"))
                    dgvEnrollments.Columns["course_code"].HeaderText = "Course Code";
                if (dgvEnrollments.Columns.Contains("course_name"))
                    dgvEnrollments.Columns["course_name"].HeaderText = "Course Name";
                if (dgvEnrollments.Columns.Contains("credit_hours"))
                {
                    dgvEnrollments.Columns["credit_hours"].HeaderText = "Credits";
                    dgvEnrollments.Columns["credit_hours"].Width = 70;
                }
                if (dgvEnrollments.Columns.Contains("grade"))
                {
                    dgvEnrollments.Columns["grade"].HeaderText = "Grade";
                    dgvEnrollments.Columns["grade"].Width = 80;
                }
                if (dgvEnrollments.Columns.Contains("status"))
                {
                    dgvEnrollments.Columns["status"].HeaderText = "Status";
                    dgvEnrollments.Columns["status"].Width = 120;
                }
                if (dgvEnrollments.Columns.Contains("semester"))
                {
                    dgvEnrollments.Columns["semester"].HeaderText = "Semester";
                    dgvEnrollments.Columns["semester"].Width = 150;
                }
                
                // Show message if no courses found
                if (dtHistory.Rows.Count == 0)
                {
                    var lblNoCourses = new Label
                    {
                        Text = "No course enrollments found yet.",
                        Font = new Font("Segoe UI", 10, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(20, 250)
                    };
                    pnlContent.Controls.Add(lblNoCourses);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading academic record: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEligibleCourses()
        {
            ClearContent();

            // Title
            var lblTitle = new Label
            {
                Text = "Eligible Courses",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlContent.Controls.Add(lblTitle);
            
            // Info label
            var lblInfo = new Label
            {
                Text = "Courses with unmet prerequisites/corequisites are shown in gray.",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 150, 150),
                AutoSize = true,
                Location = new Point(20, 50)
            };
            pnlContent.Controls.Add(lblInfo);

            // Grid
            var dgv = new DataGridView
            {
                Location = new Point(20, 75),
                Size = new Size(pnlContent.Width - 40, pnlContent.Height - 95),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            pnlContent.Controls.Add(dgv);

            try
            {
                DataTable dt = studentModel.GetEligibleCourses(studentId);
                dgv.DataSource = dt;

                // Friendly headers + hide IDs
                if (dt.Columns.Contains("course_code"))
                {
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                    if (dgv.Columns.Contains("course_name"))
                        dgv.Columns["course_name"].HeaderText = "Course Name";
                    if (dgv.Columns.Contains("credit_hours"))
                        dgv.Columns["credit_hours"].HeaderText = "Credits";
                    if (dgv.Columns.Contains("course_type"))
                        dgv.Columns["course_type"].HeaderText = "Type";
                    if (dgv.Columns.Contains("eligibility_status"))
                        dgv.Columns["eligibility_status"].HeaderText = "Status";
                }

                if (dgv.Columns.Contains("course_id"))
                    dgv.Columns["course_id"].Visible = false;
                
                // Style rows based on eligibility
                dgv.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex < 0 || e.RowIndex >= dgv.Rows.Count)
                        return;
                        
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    if (row.Cells["eligibility_status"].Value != null)
                    {
                        string status = row.Cells["eligibility_status"].Value.ToString();
                        if (status != "Eligible")
                        {
                            // Gray out ineligible courses
                            e.CellStyle.ForeColor = Color.Gray;
                            e.CellStyle.BackColor = Color.FromArgb(245, 245, 245);
                            e.CellStyle.SelectionForeColor = Color.DarkGray;
                            e.CellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
                        }
                    }
                };

                // If nothing returned
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No eligible courses found for this student (or all already taken).",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading eligible courses: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSubmitPreferences()
        {
            ClearContent();

            // Title
            var lblTitle = new Label
            {
                Text = "Submit Preferred Courses",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlContent.Controls.Add(lblTitle);

            // Info label (optional)
            var lblInfo = new Label
            {
                Text = "Select eligible courses (checkboxes disabled for courses with unmet prerequisites/corequisites).",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = true,
                Location = new Point(20, 55)
            };
            pnlContent.Controls.Add(lblInfo);

            // Grid
            var dgv = new DataGridView
            {
                Location = new Point(20, 85),
                Size = new Size(pnlContent.Width - 40, pnlContent.Height - 160),
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
            pnlContent.Controls.Add(dgv);

            // Submit button
            var btnSubmit = new Button
            {
                Text = "Submit Selected",
                Size = new Size(170, 45),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Location = new Point(20, pnlContent.Height - 60),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            pnlContent.Controls.Add(btnSubmit);

            try
            {
                // Load eligible courses as preference list
                DataTable dt = studentModel.GetEligibleCourses(studentId);
                dgv.DataSource = dt;

                // Add checkbox column ONCE
                if (!dgv.Columns.Contains("chkSelect"))
                {
                    var chk = new DataGridViewCheckBoxColumn
                    {
                        Name = "chkSelect",
                        HeaderText = "Select",
                        Width = 60,
                        ReadOnly = false
                    };
                    dgv.Columns.Insert(0, chk);
                }

                // Hide course_id but keep it for submission
                if (dgv.Columns.Contains("course_id"))
                    dgv.Columns["course_id"].Visible = false;

                // Friendly column headers
                if (dgv.Columns.Contains("course_code"))
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                if (dgv.Columns.Contains("course_name"))
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                if (dgv.Columns.Contains("credit_hours"))
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                if (dgv.Columns.Contains("course_type"))
                    dgv.Columns["course_type"].HeaderText = "Type";
                if (dgv.Columns.Contains("eligibility_status"))
                    dgv.Columns["eligibility_status"].HeaderText = "Status";

                // Style rows based on eligibility and disable checkbox for ineligible courses
                dgv.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex < 0 || e.RowIndex >= dgv.Rows.Count)
                        return;
                        
                    DataGridViewRow row = dgv.Rows[e.RowIndex];
                    if (row.Cells["eligibility_status"].Value != null)
                    {
                        string status = row.Cells["eligibility_status"].Value.ToString();
                        if (status != "Eligible")
                        {
                            // Gray out ineligible courses
                            e.CellStyle.ForeColor = Color.Gray;
                            e.CellStyle.BackColor = Color.FromArgb(245, 245, 245);
                            e.CellStyle.SelectionForeColor = Color.DarkGray;
                            e.CellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
                        }
                    }
                };

                // Prevent editing checkbox for ineligible courses
                dgv.CellBeginEdit += (s, e) =>
                {
                    if (e.RowIndex < 0 || e.ColumnIndex < 0)
                        return;
                        
                    // Check if it's the checkbox column
                    if (dgv.Columns[e.ColumnIndex].Name == "chkSelect")
                    {
                        DataGridViewRow row = dgv.Rows[e.RowIndex];
                        if (row.Cells["eligibility_status"].Value != null)
                        {
                            string status = row.Cells["eligibility_status"].Value.ToString();
                            if (status != "Eligible")
                            {
                                // Cancel the edit before it happens
                                DataGridViewCellCancelEventArgs cancelArgs = e as DataGridViewCellCancelEventArgs;
                                if (cancelArgs != null)
                                {
                                    cancelArgs.Cancel = true;
                                }
                                MessageBox.Show($"Cannot select this course. {status}.", "Prerequisites/Corequisites Not Met",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                };

                // Make checkbox editable on single click, but validate changes
                dgv.CurrentCellDirtyStateChanged += (s, e) =>
                {
                    if (dgv.IsCurrentCellDirty && dgv.CurrentCell is DataGridViewCheckBoxCell)
                    {
                        // Check if trying to check an ineligible course
                        int rowIndex = dgv.CurrentCell.RowIndex;
                        if (rowIndex >= 0 && dgv.Columns[dgv.CurrentCell.ColumnIndex].Name == "chkSelect")
                        {
                            DataGridViewRow row = dgv.Rows[rowIndex];
                            if (row.Cells["eligibility_status"].Value != null)
                            {
                                string status = row.Cells["eligibility_status"].Value.ToString();
                                if (status != "Eligible")
                                {
                                    // Don't commit changes for ineligible courses
                                    dgv.CancelEdit();
                                    return;
                                }
                            }
                        }
                        dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                };

                btnSubmit.Click += (s, e) =>
                {
                    // Collect selected IDs (only eligible ones)
                    var selectedCourseIds = new List<int>();

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        bool isChecked = row.Cells["chkSelect"].Value != null &&
                                         Convert.ToBoolean(row.Cells["chkSelect"].Value);

                        if (!isChecked) continue;
                        
                        // Double-check eligibility
                        string status = row.Cells["eligibility_status"].Value?.ToString() ?? "";
                        if (status != "Eligible")
                        {
                            // Skip ineligible courses
                            continue;
                        }

                        if (!dgv.Columns.Contains("course_id") || row.Cells["course_id"].Value == null)
                            continue;

                        selectedCourseIds.Add(Convert.ToInt32(row.Cells["course_id"].Value));
                    }

                    if (selectedCourseIds.Count == 0)
                    {
                        MessageBox.Show("Please select at least one eligible course.", "No Selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        var result = studentModel.SubmitCoursePreferences(studentId, selectedCourseIds);

                        MessageBox.Show(
                            $"Submitted: {result.inserted} course(s).\nSkipped: {result.skipped} (duplicates/constraints).",
                            "Submission Result",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Refresh (clears selections + updates if needed)
                        LoadSubmitPreferences();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting preferences: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading eligible courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadApprovedCourses()
        {
            ClearContent();

            // Title
            var lblTitle = new Label
            {
                Text = "Approved Courses & Adviser Remarks",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlContent.Controls.Add(lblTitle);

            // Grid (top)
            var dgv = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(pnlContent.Width - 40, pnlContent.Height - 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            pnlContent.Controls.Add(dgv);

            // Remarks label
            var lblRemarks = new Label
            {
                Text = "Adviser Remarks",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(20, pnlContent.Height - 140)
            };
            pnlContent.Controls.Add(lblRemarks);

            // Remarks box (bottom)
            var txtRemarks = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(20, pnlContent.Height - 110),
                Size = new Size(pnlContent.Width - 40, 90),
                Font = new Font("Segoe UI", 10)
            };
            pnlContent.Controls.Add(txtRemarks);

            try
            {
                DataTable dt = studentModel.GetApprovedCoursesAndRemarks(studentId);
                dgv.DataSource = dt;

                // Friendly headers
                if (dgv.Columns.Contains("course_code"))
                    dgv.Columns["course_code"].HeaderText = "Course Code";
                if (dgv.Columns.Contains("course_name"))
                    dgv.Columns["course_name"].HeaderText = "Course Name";
                if (dgv.Columns.Contains("credit_hours"))
                    dgv.Columns["credit_hours"].HeaderText = "Credits";
                if (dgv.Columns.Contains("semester"))
                    dgv.Columns["semester"].HeaderText = "Semester";
                if (dgv.Columns.Contains("approval_status"))
                    dgv.Columns["approval_status"].HeaderText = "Status";

                // Hide IDs if exist
                if (dgv.Columns.Contains("course_id"))
                    dgv.Columns["course_id"].Visible = false;

                // Hide remarks column in grid (we show it below)
                if (dgv.Columns.Contains("adviser_remarks"))
                    dgv.Columns["adviser_remarks"].Visible = false;

                // Show remarks for selected row
                void UpdateRemarksFromSelectedRow()
                {
                    if (dgv.CurrentRow == null)
                    {
                        txtRemarks.Text = "";
                        return;
                    }

                    if (dgv.Columns.Contains("adviser_remarks"))
                    {
                        var val = dgv.CurrentRow.Cells["adviser_remarks"].Value;
                        txtRemarks.Text = val == null ? "" : val.ToString();
                    }
                    else
                    {
                        txtRemarks.Text = "";
                    }
                }

                dgv.SelectionChanged += (s, e) => UpdateRemarksFromSelectedRow();

                // Select first row automatically
                if (dgv.Rows.Count > 0)
                {
                    dgv.Rows[0].Selected = true;
                    UpdateRemarksFromSelectedRow();
                }
                else
                {
                    txtRemarks.Text = "No approved courses found yet.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading approved courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAcademicRecord_Click_1(object sender, EventArgs e)
        {
            SetActiveButton(btnAcademicRecord);
            LoadAcademicRecord();
        }

        private void btnEligibleCourses_Click_1(object sender, EventArgs e)
        {
            SetActiveButton(btnEligibleCourses);

            LoadEligibleCourses();
        }

        private void btnSubmitPreferences_Click_1(object sender, EventArgs e)
        {
            SetActiveButton(btnSubmitPreferences);

            LoadSubmitPreferences();
        }

        private void btnApprovedCourses_Click_1(object sender, EventArgs e)
        {
            SetActiveButton(btnApprovedCourses);

            LoadApprovedCourses();
        }
    }
}
