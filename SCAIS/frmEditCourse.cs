using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SCAIS
{
    public partial class frmEditCourse : Form
    {
        private int courseId;
        private Administrator administrator;
        private bool isAddMode;

        private TextBox txtCourseCode;
        private TextBox txtCourseName;
        private ComboBox cmbCourseType;
        private TextBox txtCreditHours;
        private TextBox txtDescription;
        private CheckBox chkIsActive;
        private DataGridView dgvPrerequisites;
        private DataGridView dgvCorequisites;
        private Button btnSave;
        private Button btnCancel;

        public frmEditCourse(int courseId, string courseCode, string courseName, 
            string courseType, int creditHours, string description, bool isActive, Administrator admin)
        {
            this.courseId = courseId;
            this.administrator = admin;
            this.isAddMode = (courseId == 0);

            InitializeComponent();
            LoadAvailableCourses();
            
            if (!isAddMode)
            {
                LoadCourseData(courseCode, courseName, courseType, creditHours, description, isActive);
            }
        }

        private void InitializeComponent()
        {
            this.Text = isAddMode ? "Add New Course" : "Edit Course Information";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title Label
            Label lblTitle = new Label
            {
                Text = isAddMode ? "Add New Course" : "Edit Course Information",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Course Code Label and TextBox
            Label lblCourseCode = new Label
            {
                Text = "Course Code:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 80),
                AutoSize = true
            };
            this.Controls.Add(lblCourseCode);

            txtCourseCode = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 77),
                Size = new Size(250, 25)
            };
            this.Controls.Add(txtCourseCode);

            // Course Name Label and TextBox
            Label lblCourseName = new Label
            {
                Text = "Course Name:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 120),
                AutoSize = true
            };
            this.Controls.Add(lblCourseName);

            txtCourseName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 117),
                Size = new Size(250, 25)
            };
            this.Controls.Add(txtCourseName);

            // Course Type Label and ComboBox
            Label lblCourseType = new Label
            {
                Text = "Course Type:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 160),
                AutoSize = true
            };
            this.Controls.Add(lblCourseType);

            cmbCourseType = new ComboBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 157),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCourseType.Items.AddRange(new string[] { "Core", "Elective", "Specialized" });
            this.Controls.Add(cmbCourseType);

            // Credit Hours Label and TextBox
            Label lblCreditHours = new Label
            {
                Text = "Credit Hours:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 200),
                AutoSize = true
            };
            this.Controls.Add(lblCreditHours);

            txtCreditHours = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 197),
                Size = new Size(250, 25)
            };
            this.Controls.Add(txtCreditHours);

            // Description Label and TextBox
            Label lblDescription = new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(30, 240),
                AutoSize = true
            };
            this.Controls.Add(lblDescription);

            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 237),
                Size = new Size(250, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtDescription);

            // Is Active CheckBox
            chkIsActive = new CheckBox
            {
                Text = "Active",
                Font = new Font("Segoe UI", 10),
                Location = new Point(200, 330),
                AutoSize = true,
                Checked = true
            };
            this.Controls.Add(chkIsActive);

            // Prerequisites Section
            Label lblPrerequisites = new Label
            {
                Text = "Prerequisites:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(480, 80),
                AutoSize = true
            };
            this.Controls.Add(lblPrerequisites);

            Label lblPrereqInfo = new Label
            {
                Text = "Select courses that are required before this course:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(480, 105),
                AutoSize = true
            };
            this.Controls.Add(lblPrereqInfo);

            dgvPrerequisites = new DataGridView
            {
                Location = new Point(480, 130),
                Size = new Size(380, 180),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(dgvPrerequisites);

            // Corequisites Section
            Label lblCorequisites = new Label
            {
                Text = "Corequisites:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(480, 330),
                AutoSize = true
            };
            this.Controls.Add(lblCorequisites);

            Label lblCoreqInfo = new Label
            {
                Text = "Select courses that must be taken together with this course:",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(90, 90, 90),
                Location = new Point(480, 355),
                AutoSize = true
            };
            this.Controls.Add(lblCoreqInfo);

            dgvCorequisites = new DataGridView
            {
                Location = new Point(480, 380),
                Size = new Size(380, 180),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(dgvCorequisites);

            // Save Button
            btnSave = new Button
            {
                Text = isAddMode ? "Add Course" : "Save Changes",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(480, 590),
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
                Location = new Point(660, 590),
                Size = new Size(170, 45),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void LoadAvailableCourses()
        {
            try
            {
                // Load all courses
                DataTable dtCourses = administrator.GetCourseForDropdown();

                // Setup Prerequisites DataGridView
                DataTable dtPrereq = dtCourses.Copy();
                
                // Add a boolean column for the checkbox BEFORE setting the DataSource
                if (!dtPrereq.Columns.Contains("Selected"))
                {
                    dtPrereq.Columns.Add("Selected", typeof(bool));
                    dtPrereq.Columns["Selected"].DefaultValue = false;
                    
                    // Set all rows to unchecked initially
                    foreach (DataRow row in dtPrereq.Rows)
                    {
                        row["Selected"] = false;
                    }
                }
                
                dgvPrerequisites.DataSource = dtPrereq;

                // Bind the checkbox column to the "Selected" column in the DataTable
                if (dgvPrerequisites.Columns.Contains("chkPrereq"))
                {
                    dgvPrerequisites.Columns.Remove("chkPrereq");
                }
                
                DataGridViewCheckBoxColumn chkPrereq = new DataGridViewCheckBoxColumn
                {
                    Name = "chkPrereq",
                    HeaderText = "Select",
                    DataPropertyName = "Selected", // Bind to the Selected column
                    Width = 60,
                    ReadOnly = false,
                    TrueValue = true,
                    FalseValue = false
                };
                dgvPrerequisites.Columns.Insert(0, chkPrereq);

                // Hide course_id column
                if (dgvPrerequisites.Columns.Contains("course_id"))
                    dgvPrerequisites.Columns["course_id"].Visible = false;
                    
                // Hide the Selected column (we're using it for binding only)
                if (dgvPrerequisites.Columns.Contains("Selected"))
                    dgvPrerequisites.Columns["Selected"].Visible = false;

                // Set headers
                if (dgvPrerequisites.Columns.Contains("course_code"))
                    dgvPrerequisites.Columns["course_code"].HeaderText = "Code";
                if (dgvPrerequisites.Columns.Contains("course_name"))
                    dgvPrerequisites.Columns["course_name"].HeaderText = "Course Name";

                // Setup Corequisites DataGridView
                DataTable dtCoreq = dtCourses.Copy();
                
                // Add a boolean column for the checkbox BEFORE setting the DataSource
                if (!dtCoreq.Columns.Contains("Selected"))
                {
                    dtCoreq.Columns.Add("Selected", typeof(bool));
                    dtCoreq.Columns["Selected"].DefaultValue = false;
                    
                    // Set all rows to unchecked initially
                    foreach (DataRow row in dtCoreq.Rows)
                    {
                        row["Selected"] = false;
                    }
                }
                
                dgvCorequisites.DataSource = dtCoreq;

                // Bind the checkbox column to the "Selected" column in the DataTable
                if (dgvCorequisites.Columns.Contains("chkCoreq"))
                {
                    dgvCorequisites.Columns.Remove("chkCoreq");
                }
                
                DataGridViewCheckBoxColumn chkCoreq = new DataGridViewCheckBoxColumn
                {
                    Name = "chkCoreq",
                    HeaderText = "Select",
                    DataPropertyName = "Selected", // Bind to the Selected column
                    Width = 60,
                    ReadOnly = false,
                    TrueValue = true,
                    FalseValue = false
                };
                dgvCorequisites.Columns.Insert(0, chkCoreq);

                // Hide course_id column
                if (dgvCorequisites.Columns.Contains("course_id"))
                    dgvCorequisites.Columns["course_id"].Visible = false;
                    
                // Hide the Selected column (we're using it for binding only)
                if (dgvCorequisites.Columns.Contains("Selected"))
                    dgvCorequisites.Columns["Selected"].Visible = false;

                // Set headers
                if (dgvCorequisites.Columns.Contains("course_code"))
                    dgvCorequisites.Columns["course_code"].HeaderText = "Code";
                if (dgvCorequisites.Columns.Contains("course_name"))
                    dgvCorequisites.Columns["course_name"].HeaderText = "Course Name";

                // Enable immediate checkbox updates
                dgvPrerequisites.CurrentCellDirtyStateChanged += (s, e) =>
                {
                    if (dgvPrerequisites.IsCurrentCellDirty)
                        dgvPrerequisites.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };

                dgvCorequisites.CurrentCellDirtyStateChanged += (s, e) =>
                {
                    if (dgvCorequisites.IsCurrentCellDirty)
                        dgvCorequisites.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCourseData(string courseCode, string courseName, string courseType, 
            int creditHours, string description, bool isActive)
        {
            try
            {
                if (isAddMode)
                {
                    txtCourseCode.Text = "";
                    txtCourseName.Text = "";
                    cmbCourseType.SelectedIndex = 0;
                    txtCreditHours.Text = "";
                    txtDescription.Text = "";
                    chkIsActive.Checked = true;
                }
                else
                {
                    txtCourseCode.Text = courseCode;
                    txtCourseName.Text = courseName;
                    
                    // Set course type in combobox
                    int index = cmbCourseType.FindStringExact(courseType);
                    if (index >= 0)
                        cmbCourseType.SelectedIndex = index;
                    else
                        cmbCourseType.SelectedIndex = 0;

                    txtCreditHours.Text = creditHours.ToString();
                    txtDescription.Text = description;
                    chkIsActive.Checked = isActive;

                    // Load existing prerequisites and corequisites
                    LoadExistingPrerequisites();
                    LoadExistingCorequisites();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingPrerequisites()
        {
            try
            {
                DataTable dtExisting = administrator.GetPrerequisites(courseId);
                
                System.Diagnostics.Debug.WriteLine($"===== LoadExistingPrerequisites =====");
                System.Diagnostics.Debug.WriteLine($"Course ID: {courseId}");
                System.Diagnostics.Debug.WriteLine($"Found {dtExisting.Rows.Count} prerequisites in database");
                
                if (dtExisting.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"===== End LoadExistingPrerequisites (no data) =====\n");
                    return;
                }
                
                // Get the underlying DataTable
                DataTable dt = dgvPrerequisites.DataSource as DataTable;
                if (dt == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: DataSource is not a DataTable!");
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"DataTable has {dt.Rows.Count} rows");
                
                foreach (DataRow existingRow in dtExisting.Rows)
                {
                    int prereqId = Convert.ToInt32(existingRow["prerequisite_course_id"]);
                    System.Diagnostics.Debug.WriteLine($"Looking for prerequisite ID: {prereqId}");
                    
                    // Find the row in the DataTable and set Selected = true
                    DataRow[] matchingRows = dt.Select($"course_id = {prereqId}");
                    if (matchingRows.Length > 0)
                    {
                        matchingRows[0]["Selected"] = true;
                        System.Diagnostics.Debug.WriteLine($"  ? Set prerequisite {prereqId} Selected=true in DataTable");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"  ? Could not find course {prereqId} in DataTable!");
                    }
                }
                
                // Refresh the grid to show changes
                dgvPrerequisites.Refresh();
                
                System.Diagnostics.Debug.WriteLine($"===== End LoadExistingPrerequisites =====\n");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in LoadExistingPrerequisites: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error loading prerequisites: {ex.Message}\n\nPlease check the debug output for details.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExistingCorequisites()
        {
            try
            {
                DataTable dtExisting = administrator.GetCorequisites(courseId);
                
                System.Diagnostics.Debug.WriteLine($"===== LoadExistingCorequisites =====");
                System.Diagnostics.Debug.WriteLine($"Course ID: {courseId}");
                System.Diagnostics.Debug.WriteLine($"Found {dtExisting.Rows.Count} corequisites in database");
                
                if (dtExisting.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"===== End LoadExistingCorequisites (no data) =====\n");
                    return;
                }
                
                // Get the underlying DataTable
                DataTable dt = dgvCorequisites.DataSource as DataTable;
                if (dt == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: DataSource is not a DataTable!");
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"DataTable has {dt.Rows.Count} rows");
                
                foreach (DataRow existingRow in dtExisting.Rows)
                {
                    int coreqId = Convert.ToInt32(existingRow["corequisite_course_id"]);
                    System.Diagnostics.Debug.WriteLine($"Looking for corequisite ID: {coreqId}");
                    
                    // Find the row in the DataTable and set Selected = true
                    DataRow[] matchingRows = dt.Select($"course_id = {coreqId}");
                    if (matchingRows.Length > 0)
                    {
                        matchingRows[0]["Selected"] = true;
                        System.Diagnostics.Debug.WriteLine($"  ? Set corequisite {coreqId} Selected=true in DataTable");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"  ? Could not find course {coreqId} in DataTable!");
                    }
                }
                
                // Refresh the grid to show changes
                dgvCorequisites.Refresh();
                
                System.Diagnostics.Debug.WriteLine($"===== End LoadExistingCorequisites =====\n");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in LoadExistingCorequisites: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error loading corequisites: {ex.Message}\n\nPlease check the debug output for details.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtCourseCode.Text))
            {
                MessageBox.Show("Course code is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCourseCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("Course name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCourseName.Focus();
                return;
            }

            if (cmbCourseType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a course type.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCourseType.Focus();
                return;
            }

            if (!int.TryParse(txtCreditHours.Text, out int creditHours) || creditHours <= 0)
            {
                MessageBox.Show("Please enter valid credit hours.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCreditHours.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Description is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return;
            }

            try
            {
                int savedCourseId = courseId;

                if (isAddMode)
                {
                    // Add new course and get the new course ID
                    savedCourseId = administrator.AddCourseAndGetId(
                        txtCourseCode.Text.Trim(),
                        txtCourseName.Text.Trim(),
                        cmbCourseType.SelectedItem.ToString(),
                        creditHours,
                        txtDescription.Text.Trim()
                    );
                }
                else
                {
                    // Update existing course
                    administrator.EditCourse(
                        courseId,
                        txtCourseCode.Text.Trim(),
                        txtCourseName.Text.Trim(),
                        cmbCourseType.SelectedItem.ToString(),
                        creditHours,
                        txtDescription.Text.Trim(),
                        chkIsActive.Checked
                    );
                }

                // Get selected prerequisites from the DataTable
                List<int> selectedPrerequisites = new List<int>();
                if (dgvPrerequisites.DataSource is DataTable dtPrereq)
                {
                    DataRow[] selectedRows = dtPrereq.Select("Selected = true");
                    foreach (DataRow row in selectedRows)
                    {
                        int prereqCourseId = Convert.ToInt32(row["course_id"]);
                        // Don't allow a course to be its own prerequisite
                        if (prereqCourseId != savedCourseId)
                        {
                            selectedPrerequisites.Add(prereqCourseId);
                        }
                    }
                }

                // Get selected corequisites from the DataTable
                List<int> selectedCorequisites = new List<int>();
                if (dgvCorequisites.DataSource is DataTable dtCoreq)
                {
                    DataRow[] selectedRows = dtCoreq.Select("Selected = true");
                    foreach (DataRow row in selectedRows)
                    {
                        int coreqCourseId = Convert.ToInt32(row["course_id"]);
                        // Don't allow a course to be its own corequisite
                        if (coreqCourseId != savedCourseId)
                        {
                            selectedCorequisites.Add(coreqCourseId);
                        }
                    }
                }

                // Update prerequisites
                if (selectedPrerequisites.Count > 0 || !isAddMode)
                {
                    administrator.UpdatePrereq(savedCourseId, selectedPrerequisites);
                }

                // Update corequisites
                if (selectedCorequisites.Count > 0 || !isAddMode)
                {
                    administrator.UpdateCorereq(savedCourseId, selectedCorequisites);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving course information: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
