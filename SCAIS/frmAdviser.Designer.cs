namespace SCAIS
{
    partial class frmAdviser
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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnMyAdvisees = new System.Windows.Forms.Button();
            this.btnAcademicHistory = new System.Windows.Forms.Button();
            this.btnCourseRecommendations = new System.Windows.Forms.Button();
            this.btnApprovePlans = new System.Windows.Forms.Button();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblContentTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Controls.Add(this.btnLogout);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1200, 100);
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
            this.btnLogout.Location = new System.Drawing.Point(1080, 30);
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
            this.lblTitle.Text = "Adviser Dashboard - SCAIS";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.panelSidebar.Controls.Add(this.btnMyAdvisees);
            this.panelSidebar.Controls.Add(this.btnAcademicHistory);
            this.panelSidebar.Controls.Add(this.btnCourseRecommendations);
            this.panelSidebar.Controls.Add(this.btnApprovePlans);
            this.panelSidebar.Controls.Add(this.btnGenerateReports);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 100);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(250, 600);
            this.panelSidebar.TabIndex = 1;
            // 
            // btnMyAdvisees
            // 
            this.btnMyAdvisees.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnMyAdvisees.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMyAdvisees.FlatAppearance.BorderSize = 0;
            this.btnMyAdvisees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyAdvisees.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyAdvisees.ForeColor = System.Drawing.Color.White;
            this.btnMyAdvisees.Location = new System.Drawing.Point(0, 20);
            this.btnMyAdvisees.Name = "btnMyAdvisees";
            this.btnMyAdvisees.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMyAdvisees.Size = new System.Drawing.Size(250, 60);
            this.btnMyAdvisees.TabIndex = 0;
            this.btnMyAdvisees.Text = ">  My Advisees";
            this.btnMyAdvisees.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyAdvisees.UseVisualStyleBackColor = false;
            this.btnMyAdvisees.Click += new System.EventHandler(this.btnMyAdvisees_Click);
            // 
            // btnAcademicHistory
            // 
            this.btnAcademicHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAcademicHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAcademicHistory.FlatAppearance.BorderSize = 0;
            this.btnAcademicHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcademicHistory.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcademicHistory.ForeColor = System.Drawing.Color.White;
            this.btnAcademicHistory.Location = new System.Drawing.Point(0, 80);
            this.btnAcademicHistory.Name = "btnAcademicHistory";
            this.btnAcademicHistory.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAcademicHistory.Size = new System.Drawing.Size(250, 60);
            this.btnAcademicHistory.TabIndex = 1;
            this.btnAcademicHistory.Text = ">  Academic History";
            this.btnAcademicHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAcademicHistory.UseVisualStyleBackColor = false;
            this.btnAcademicHistory.Click += new System.EventHandler(this.btnAcademicHistory_Click);
            // 
            // btnCourseRecommendations
            // 
            this.btnCourseRecommendations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnCourseRecommendations.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCourseRecommendations.FlatAppearance.BorderSize = 0;
            this.btnCourseRecommendations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourseRecommendations.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCourseRecommendations.ForeColor = System.Drawing.Color.White;
            this.btnCourseRecommendations.Location = new System.Drawing.Point(0, 140);
            this.btnCourseRecommendations.Name = "btnCourseRecommendations";
            this.btnCourseRecommendations.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCourseRecommendations.Size = new System.Drawing.Size(250, 60);
            this.btnCourseRecommendations.TabIndex = 2;
            this.btnCourseRecommendations.Text = ">  Course Recommendations";
            this.btnCourseRecommendations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCourseRecommendations.UseVisualStyleBackColor = false;
            this.btnCourseRecommendations.Click += new System.EventHandler(this.btnCourseRecommendations_Click);
            // 
            // btnApprovePlans
            // 
            this.btnApprovePlans.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnApprovePlans.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApprovePlans.FlatAppearance.BorderSize = 0;
            this.btnApprovePlans.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApprovePlans.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApprovePlans.ForeColor = System.Drawing.Color.White;
            this.btnApprovePlans.Location = new System.Drawing.Point(0, 200);
            this.btnApprovePlans.Name = "btnApprovePlans";
            this.btnApprovePlans.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnApprovePlans.Size = new System.Drawing.Size(250, 60);
            this.btnApprovePlans.TabIndex = 3;
            this.btnApprovePlans.Text = ">  Approve Course Plans";
            this.btnApprovePlans.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApprovePlans.UseVisualStyleBackColor = false;
            this.btnApprovePlans.Click += new System.EventHandler(this.btnApprovePlans_Click);
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnGenerateReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateReports.FlatAppearance.BorderSize = 0;
            this.btnGenerateReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReports.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReports.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReports.Location = new System.Drawing.Point(0, 260);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnGenerateReports.Size = new System.Drawing.Size(250, 60);
            this.btnGenerateReports.TabIndex = 4;
            this.btnGenerateReports.Text = ">  Generate Reports";
            this.btnGenerateReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerateReports.UseVisualStyleBackColor = false;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(250, 100);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(950, 600);
            this.panelMain.TabIndex = 2;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.lblContentTitle);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(20, 20);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(30);
            this.panelContent.Size = new System.Drawing.Size(910, 560);
            this.panelContent.TabIndex = 0;
            // 
            // lblContentTitle
            // 
            this.lblContentTitle.AutoSize = true;
            this.lblContentTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblContentTitle.Location = new System.Drawing.Point(30, 30);
            this.lblContentTitle.Name = "lblContentTitle";
            this.lblContentTitle.Size = new System.Drawing.Size(457, 30);
            this.lblContentTitle.TabIndex = 0;
            this.lblContentTitle.Text = "Welcome to Adviser Dashboard";
            this.lblContentTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmAdviser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "frmAdviser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCAIS - Adviser Dashboard";
            this.Load += new System.EventHandler(this.frmAdviser_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSidebar.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnMyAdvisees;
        private System.Windows.Forms.Button btnAcademicHistory;
        private System.Windows.Forms.Button btnCourseRecommendations;
        private System.Windows.Forms.Button btnApprovePlans;
        private System.Windows.Forms.Button btnGenerateReports;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblContentTitle;
    }
}
