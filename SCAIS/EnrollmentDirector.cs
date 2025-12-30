
using System;

namespace SCAIS
{
    /// <summary>
    /// Director class that defines the order of building steps for common Enrollment scenarios
    /// </summary>
    public class EnrollmentDirector
    {
        private IEnrollmentBuilder builder;

        public EnrollmentDirector(IEnrollmentBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Builds a pending enrollment waiting for approval
        /// </summary>
        public Enrollment BuildPendingEnrollment(int studentId, int courseId, int semesterId)
        {
            return builder
                .SetStudentId(studentId)
                .SetCourseId(courseId)
                .SetSemesterId(semesterId)
                .SetStatus("Pending")
                .SetApprovalStatus("PendingApproval")
                .SetEnrollmentDate(DateTime.Now)
                .Build();
        }

        /// <summary>
        /// Builds an approved enrollment
        /// </summary>
        public Enrollment BuildApprovedEnrollment(int studentId, int courseId, int semesterId, int adviserId, string remarks = "")
        {
            return builder
                .SetStudentId(studentId)
                .SetCourseId(courseId)
                .SetSemesterId(semesterId)
                .SetStatus("Pending")
                .SetApprovalStatus("Approved")
                .SetEnrollmentDate(DateTime.Now)
                .SetAdviserId(adviserId)
                .SetApprovalDate(DateTime.Now)
                .SetAdviserRemarks(remarks)
                .Build();
        }

        /// <summary>
        /// Builds a rejected enrollment
        /// </summary>
        public Enrollment BuildRejectedEnrollment(int studentId, int courseId, int semesterId, int adviserId, string remarks)
        {
            return builder
                .SetStudentId(studentId)
                .SetCourseId(courseId)
                .SetSemesterId(semesterId)
                .SetStatus("Pending")
                .SetApprovalStatus("Rejected")
                .SetEnrollmentDate(DateTime.Now)
                .SetAdviserId(adviserId)
                .SetApprovalDate(DateTime.Now)
                .SetAdviserRemarks(remarks)
                .Build();
        }

        /// <summary>
        /// Builds a completed enrollment with grade
        /// </summary>
        public Enrollment BuildCompletedEnrollment(int enrollmentId, string grade)
        {
            return builder
                .SetEnrollmentId(enrollmentId)
                .SetStatus("Completed")
                .SetGrade(grade)
                .Build();
        }
    }
}
