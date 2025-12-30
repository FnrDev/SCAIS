///////////////////////////////////////////////////////////
//  IEnrollmentBuilder.cs
//  Interface for building Enrollment objects
//  Created on:      22-Dec-2024
///////////////////////////////////////////////////////////

using System;

namespace SCAIS
{
    /// <summary>
    /// Builder interface for constructing Enrollment objects step by step
    /// </summary>
    public interface IEnrollmentBuilder
    {
        /// <summary>
        /// Builds and returns the final Enrollment object
        /// </summary>
        Enrollment Build();

        /// <summary>
        /// Resets the builder to start building a new Enrollment
        /// </summary>
        void Reset();

        /// <summary>
        /// Sets the student ID for the enrollment
        /// </summary>
        IEnrollmentBuilder SetStudentId(int studentId);

        /// <summary>
        /// Sets the course ID for the enrollment
        /// </summary>
        IEnrollmentBuilder SetCourseId(int courseId);

        /// <summary>
        /// Sets the semester ID for the enrollment
        /// </summary>
        IEnrollmentBuilder SetSemesterId(int semesterId);

        /// <summary>
        /// Sets the enrollment status (Pending, InProgress, Completed)
        /// </summary>
        IEnrollmentBuilder SetStatus(string status);

        /// <summary>
        /// Sets the approval status (PendingApproval, Approved, Rejected)
        /// </summary>
        IEnrollmentBuilder SetApprovalStatus(string approvalStatus);

        /// <summary>
        /// Sets the enrollment date
        /// </summary>
        IEnrollmentBuilder SetEnrollmentDate(DateTime enrollmentDate);

        /// <summary>
        /// Sets the adviser ID who approved/rejected
        /// </summary>
        IEnrollmentBuilder SetAdviserId(int? adviserId);

        /// <summary>
        /// Sets the approval date
        /// </summary>
        IEnrollmentBuilder SetApprovalDate(DateTime? approvalDate);

        /// <summary>
        /// Sets the adviser remarks
        /// </summary>
        IEnrollmentBuilder SetAdviserRemarks(string remarks);

        /// <summary>
        /// Sets the grade for the course
        /// </summary>
        IEnrollmentBuilder SetGrade(string grade);

        /// <summary>
        /// Sets the enrollment ID (for updates)
        /// </summary>
        IEnrollmentBuilder SetEnrollmentId(int enrollmentId);
    }
}
