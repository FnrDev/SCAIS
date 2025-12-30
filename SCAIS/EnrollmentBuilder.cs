using System;
using System.Data;
using System.Data.SqlClient;

namespace SCAIS
{
    /// <summary>
    /// Concrete builder for constructing Enrollment objects
    /// </summary>
    public class EnrollmentBuilder : IEnrollmentBuilder
    {
        private Enrollment enrollment;

        public EnrollmentBuilder()
        {
            Reset();
        }

        /// <summary>
        /// Resets the builder to create a new Enrollment
        /// </summary>
        public void Reset()
        {
            enrollment = new Enrollment();
        }

        /// <summary>
        /// Builds and returns the final Enrollment object
        /// </summary>
        public Enrollment Build()
        {
            Enrollment result = enrollment;
            Reset(); // Reset for next build
            return result;
        }

        public IEnrollmentBuilder SetStudentId(int studentId)
        {
            enrollment.StudentId = studentId;
            return this;
        }

        public IEnrollmentBuilder SetCourseId(int courseId)
        {
            enrollment.CourseId = courseId;
            return this;
        }

        public IEnrollmentBuilder SetSemesterId(int semesterId)
        {
            enrollment.SemesterId = semesterId;
            return this;
        }

        public IEnrollmentBuilder SetStatus(string status)
        {
            enrollment.Status = status;
            return this;
        }

        public IEnrollmentBuilder SetApprovalStatus(string approvalStatus)
        {
            enrollment.ApprovalStatus = approvalStatus;
            return this;
        }

        public IEnrollmentBuilder SetEnrollmentDate(DateTime enrollmentDate)
        {
            enrollment.EnrollmentDate = enrollmentDate;
            return this;
        }

        public IEnrollmentBuilder SetAdviserId(int? adviserId)
        {
            enrollment.AdviserId = adviserId;
            return this;
        }

        public IEnrollmentBuilder SetApprovalDate(DateTime? approvalDate)
        {
            enrollment.ApprovalDate = approvalDate;
            return this;
        }

        public IEnrollmentBuilder SetAdviserRemarks(string remarks)
        {
            enrollment.AdviserRemarks = remarks;
            return this;
        }

        public IEnrollmentBuilder SetGrade(string grade)
        {
            enrollment.Grade = grade;
            return this;
        }

        public IEnrollmentBuilder SetEnrollmentId(int enrollmentId)
        {
            enrollment.EnrollmentId = enrollmentId;
            return this;
        }
    }
}
