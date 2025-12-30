using System;
using System.Data;
using System.Data.SqlClient;

namespace SCAIS
{
    /// <summary>
    /// Service class for handling enrollment operations using the builder pattern
    /// </summary>
    public class EnrollmentService
    {
        private EnrollmentDirector director;

        public EnrollmentService()
        {
            IEnrollmentBuilder builder = new EnrollmentBuilder();
            director = new EnrollmentDirector(builder);
        }

        /// <summary>
        /// Creates a new enrollment and saves it to the database
        /// </summary>
        public Enrollment CreateEnrollment(int studentId, int courseId, int semesterId)
        {
            Enrollment enrollment = director.BuildPendingEnrollment(studentId, courseId, semesterId);
            
            // Save to database
            DatabaseConnection dbConn = DatabaseConnection.Instance;
            SqlConnection conn = dbConn.GetConnection();

            string query = @"INSERT INTO enrollments (student_id, course_id, semester_id, status, approval_status, enrollment_date)
                            OUTPUT INSERTED.enrollment_id
                            VALUES (@studentId, @courseId, @semesterId, @status, @approvalStatus, @enrollmentDate)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@studentId", enrollment.StudentId);
                cmd.Parameters.AddWithValue("@courseId", enrollment.CourseId);
                cmd.Parameters.AddWithValue("@semesterId", enrollment.SemesterId);
                cmd.Parameters.AddWithValue("@status", enrollment.Status);
                cmd.Parameters.AddWithValue("@approvalStatus", enrollment.ApprovalStatus);
                cmd.Parameters.AddWithValue("@enrollmentDate", enrollment.EnrollmentDate);

                dbConn.Open();
                int enrollmentId = (int)cmd.ExecuteScalar();
                dbConn.Close();

                enrollment.EnrollmentId = enrollmentId;
            }

            return enrollment;
        }

        /// <summary>
        /// Approves an enrollment
        /// </summary>
        public bool ApproveEnrollment(int enrollmentId, int adviserId, string remarks = "")
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();

                string query = @"UPDATE enrollments 
                                SET approval_status = @status, 
                                    adviser_id = @adviserId,
                                    approval_date = @approvalDate,
                                    adviser_remarks = @remarks
                                WHERE enrollment_id = @enrollmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", "Approved");
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    cmd.Parameters.AddWithValue("@approvalDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks);
                    cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);

                    dbConn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    dbConn.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving enrollment: {ex.Message}");
            }
        }

        /// <summary>
        /// Rejects an enrollment
        /// </summary>
        public bool RejectEnrollment(int enrollmentId, int adviserId, string remarks)
        {
            try
            {
                DatabaseConnection dbConn = DatabaseConnection.Instance;
                SqlConnection conn = dbConn.GetConnection();

                string query = @"UPDATE enrollments 
                                SET approval_status = @status, 
                                    adviser_id = @adviserId,
                                    approval_date = @approvalDate,
                                    adviser_remarks = @remarks
                                WHERE enrollment_id = @enrollmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", "Rejected");
                    cmd.Parameters.AddWithValue("@adviserId", adviserId);
                    cmd.Parameters.AddWithValue("@approvalDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks);
                    cmd.Parameters.AddWithValue("@enrollmentId", enrollmentId);

                    dbConn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    dbConn.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error rejecting enrollment: {ex.Message}");
            }
        }
    }
}
