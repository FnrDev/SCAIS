namespace SCAIS
{
    partial class frmStudent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnApprovedCourses = new System.Windows.Forms.Button();
            this.btnSubmitPreferences = new System.Windows.Forms.Button();
            this.btnEligibleCourses = new System.Windows.Forms.Button();
            this.btnAcademicRecord = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 100);
            this.panelHeader.TabIndex = 0;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 70);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(64, 19);
            this.lblWelcome.TabIndex = 2;
            this.lblWelcome.Text = "Welcome";
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(880, 30);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 40);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Student Dashboard - SCAIS";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelMain.Controls.Add(this.pnlContent);
            this.panelMain.Controls.Add(this.pnlSidebar);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 100);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1000, 500);
            this.panelMain.TabIndex = 1;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(250, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(750, 500);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlSidebar.Controls.Add(this.btnApprovedCourses);
            this.pnlSidebar.Controls.Add(this.btnSubmitPreferences);
            this.pnlSidebar.Controls.Add(this.btnEligibleCourses);
            this.pnlSidebar.Controls.Add(this.btnAcademicRecord);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(250, 500);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnApprovedCourses
            // 
            this.btnApprovedCourses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnApprovedCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprovedCourses.FlatAppearance.BorderSize = 0;
            this.btnApprovedCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprovedCourses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApprovedCourses.ForeColor = System.Drawing.Color.White;
            this.btnApprovedCourses.Location = new System.Drawing.Point(0, 180);
            this.btnApprovedCourses.Name = "btnApprovedCourses";
            this.btnApprovedCourses.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnApprovedCourses.Size = new System.Drawing.Size(250, 60);
            this.btnApprovedCourses.TabIndex = 3;
            this.btnApprovedCourses.Text = ">  Approved Courses";
            this.btnApprovedCourses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApprovedCourses.UseVisualStyleBackColor = false;
            this.btnApprovedCourses.Click += new System.EventHandler(this.btnApprovedCourses_Click_1);
            // 
            // btnSubmitPreferences
            // 
            this.btnSubmitPreferences.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSubmitPreferences.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmitPreferences.FlatAppearance.BorderSize = 0;
            this.btnSubmitPreferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitPreferences.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitPreferences.ForeColor = System.Drawing.Color.White;
            this.btnSubmitPreferences.Location = new System.Drawing.Point(0, 120);
            this.btnSubmitPreferences.Name = "btnSubmitPreferences";
            this.btnSubmitPreferences.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSubmitPreferences.Size = new System.Drawing.Size(250, 60);
            this.btnSubmitPreferences.TabIndex = 2;
            this.btnSubmitPreferences.Text = ">  Submit Preferences";
            this.btnSubmitPreferences.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmitPreferences.UseVisualStyleBackColor = false;
            this.btnSubmitPreferences.Click += new System.EventHandler(this.btnSubmitPreferences_Click_1);
            // 
            // btnEligibleCourses
            // 
            this.btnEligibleCourses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnEligibleCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEligibleCourses.FlatAppearance.BorderSize = 0;
            this.btnEligibleCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEligibleCourses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEligibleCourses.ForeColor = System.Drawing.Color.White;
            this.btnEligibleCourses.Location = new System.Drawing.Point(0, 60);
            this.btnEligibleCourses.Name = "btnEligibleCourses";
            this.btnEligibleCourses.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnEligibleCourses.Size = new System.Drawing.Size(250, 60);
            this.btnEligibleCourses.TabIndex = 1;
            this.btnEligibleCourses.Text = ">  Eligible Courses";
            this.btnEligibleCourses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEligibleCourses.UseVisualStyleBackColor = false;
            this.btnEligibleCourses.Click += new System.EventHandler(this.btnEligibleCourses_Click_1);
            // 
            // btnAcademicRecord
            // 
            this.btnAcademicRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAcademicRecord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAcademicRecord.FlatAppearance.BorderSize = 0;
            this.btnAcademicRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcademicRecord.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcademicRecord.ForeColor = System.Drawing.Color.White;
            this.btnAcademicRecord.Location = new System.Drawing.Point(0, 0);
            this.btnAcademicRecord.Name = "btnAcademicRecord";
            this.btnAcademicRecord.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAcademicRecord.Size = new System.Drawing.Size(250, 60);
            this.btnAcademicRecord.TabIndex = 0;
            this.btnAcademicRecord.TabStop = false;
            this.btnAcademicRecord.Text = ">  Academic Record";
            this.btnAcademicRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAcademicRecord.UseVisualStyleBackColor = false;
            this.btnAcademicRecord.Click += new System.EventHandler(this.btnAcademicRecord_Click_1);
            // 
            // frmStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "frmStudent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCAIS - Student Dashboard";
            this.Load += new System.EventHandler(this.frmStudent_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnApprovedCourses;
        private System.Windows.Forms.Button btnSubmitPreferences;
        private System.Windows.Forms.Button btnEligibleCourses;
        private System.Windows.Forms.Button btnAcademicRecord;
    }
}
