-- ============================================
-- SCAIS DATABASE SCHEMA
-- Smart Course Advising Information System
-- Bahrain Institute of Technology (BIT)
-- Microsoft SQL Server Database
-- ============================================

-- Drop existing tables if they exist (in reverse order of dependencies)
IF OBJECT_ID('reports', 'U') IS NOT NULL DROP TABLE reports;
IF OBJECT_ID('academic_records', 'U') IS NOT NULL DROP TABLE academic_records;
IF OBJECT_ID('enrollments', 'U') IS NOT NULL DROP TABLE enrollments;
IF OBJECT_ID('semester_offerings', 'U') IS NOT NULL DROP TABLE semester_offerings;
IF OBJECT_ID('semesters', 'U') IS NOT NULL DROP TABLE semesters;
IF OBJECT_ID('corequisites', 'U') IS NOT NULL DROP TABLE corequisites;
IF OBJECT_ID('prerequisites', 'U') IS NOT NULL DROP TABLE prerequisites;
IF OBJECT_ID('courses', 'U') IS NOT NULL DROP TABLE courses;
IF OBJECT_ID('students', 'U') IS NOT NULL DROP TABLE students;
IF OBJECT_ID('specializations', 'U') IS NOT NULL DROP TABLE specializations;
IF OBJECT_ID('advisers', 'U') IS NOT NULL DROP TABLE advisers;
IF OBJECT_ID('administrators', 'U') IS NOT NULL DROP TABLE administrators;
IF OBJECT_ID('users', 'U') IS NOT NULL DROP TABLE users;

-- ============================================
-- USER MANAGEMENT LAYER
-- ============================================

-- Users Table
CREATE TABLE users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL, -- Encrypted password
    role VARCHAR(20) NOT NULL CHECK (role IN ('Administrator', 'Adviser', 'Student')),
    created_date DATETIME DEFAULT GETDATE(),
    last_login DATETIME NULL,
    is_active BIT DEFAULT 1,
    INDEX idx_email (email),
    INDEX idx_role (role)
);

-- Administrators Table
CREATE TABLE administrators (
    admin_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    department VARCHAR(100) NULL,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Advisers Table
CREATE TABLE advisers (
    adviser_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    faculty_id VARCHAR(50) NOT NULL UNIQUE, -- Unique faculty ID
    department VARCHAR(100) NULL,
    office_location VARCHAR(100) NULL,
    max_advisees INT DEFAULT 30,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idx_faculty_id (faculty_id),
    INDEX idx_department (department)
);

-- ============================================
-- ACADEMIC STRUCTURE LAYER
-- ============================================

-- Specializations Table
CREATE TABLE specializations (
    specialization_id INT IDENTITY(1,1) PRIMARY KEY,
    specialization_name VARCHAR(100) NOT NULL, -- Programming | Networking | Cybersecurity | Database
    specialization_code VARCHAR(10) NOT NULL UNIQUE, -- PROG | NET | SEC | DB
    description VARCHAR(MAX) NULL,
    required_credit_hours INT DEFAULT 120,
    is_active BIT DEFAULT 1,
    INDEX idx_code (specialization_code)
);

-- Students Table
CREATE TABLE students (
    student_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    adviser_id INT NULL,
    specialization_id INT NULL,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    student_number VARCHAR(50) NOT NULL UNIQUE, -- Unique student ID
    current_semester INT DEFAULT 1,
    gpa DECIMAL(3,2) DEFAULT 0.00 CHECK (gpa >= 0.00 AND gpa <= 4.00),
    enrollment_year INT NOT NULL,
    total_credit_hours INT DEFAULT 0,
    completed_credit_hours INT DEFAULT 0,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (adviser_id) REFERENCES advisers(adviser_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (specialization_id) REFERENCES specializations(specialization_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    INDEX idx_student_number (student_number),
    INDEX idx_adviser (adviser_id),
    INDEX idx_specialization (specialization_id)
);

-- Courses Table
CREATE TABLE courses (
    course_id INT IDENTITY(1,1) PRIMARY KEY,
    specialization_id INT NULL, -- NULL for core courses
    course_code VARCHAR(20) NOT NULL UNIQUE, -- CS101, DB201, etc.
    course_name VARCHAR(200) NOT NULL,
    credit_hours INT NOT NULL CHECK (credit_hours > 0),
    course_type VARCHAR(20) NOT NULL CHECK (course_type IN ('Core', 'Specialized', 'Elective')),
    description VARCHAR(MAX) NULL,
    is_active BIT DEFAULT 1,
    FOREIGN KEY (specialization_id) REFERENCES specializations(specialization_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    INDEX idx_course_code (course_code),
    INDEX idx_course_type (course_type),
    INDEX idx_specialization (specialization_id)
);

-- Prerequisites Table
CREATE TABLE prerequisites (
    prerequisite_id INT IDENTITY(1,1) PRIMARY KEY,
    course_id INT NOT NULL, -- The course that has prerequisite
    prerequisite_course_id INT NOT NULL, -- The required course
    is_strict BIT DEFAULT 1, -- Can be waived by adviser
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (prerequisite_course_id) REFERENCES courses(course_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT unique_prerequisite UNIQUE (course_id, prerequisite_course_id),
    CHECK (course_id != prerequisite_course_id),
    INDEX idx_course (course_id),
    INDEX idx_prereq_course (prerequisite_course_id)
);

-- Corequisites Table
CREATE TABLE corequisites (
    corequisite_id INT IDENTITY(1,1) PRIMARY KEY,
    course_id INT NOT NULL, -- Primary course
    corequisite_course_id INT NOT NULL, -- Must take together
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (corequisite_course_id) REFERENCES courses(course_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT unique_corequisite UNIQUE (course_id, corequisite_course_id),
    CHECK (course_id != corequisite_course_id),
    INDEX idx_course (course_id),
    INDEX idx_coreq_course (corequisite_course_id)
);

-- ============================================
-- ENROLLMENT & SCHEDULING LAYER
-- ============================================

-- Semesters Table
CREATE TABLE semesters (
    semester_id INT IDENTITY(1,1) PRIMARY KEY,
    semester_name VARCHAR(20) NOT NULL CHECK (semester_name IN ('Fall', 'Spring', 'Summer')),
    academic_year VARCHAR(20) NOT NULL, -- 2024-2025
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    is_active BIT DEFAULT 0, -- Only one active at a time
    registration_start_date DATE NOT NULL,
    registration_end_date DATE NOT NULL,
    CONSTRAINT unique_semester UNIQUE (semester_name, academic_year),
    CHECK (end_date > start_date),
    CHECK (registration_end_date > registration_start_date),
    INDEX idx_active (is_active),
    INDEX idx_academic_year (academic_year)
);

-- Semester Offerings Table
CREATE TABLE semester_offerings (
    offering_id INT IDENTITY(1,1) PRIMARY KEY,
    semester_id INT NOT NULL,
    course_id INT NOT NULL,
    max_capacity INT DEFAULT 30,
    current_enrollment INT DEFAULT 0,
    instructor_name VARCHAR(200) NULL,
    schedule VARCHAR(100) NULL, -- Sun/Tue 10:00-11:30
    room_number VARCHAR(50) NULL,
    FOREIGN KEY (semester_id) REFERENCES semesters(semester_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT unique_offering UNIQUE (semester_id, course_id, schedule),
    CHECK (current_enrollment <= max_capacity),
    INDEX idx_semester (semester_id),
    INDEX idx_course (course_id)
);

-- Enrollments Table
CREATE TABLE enrollments (
    enrollment_id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    semester_id INT NOT NULL,
    adviser_id INT NULL, -- Approving adviser
    status VARCHAR(20) DEFAULT 'Pending' CHECK (status IN ('Completed', 'InProgress', 'Pending', 'Dropped')),
    grade VARCHAR(5) NULL, -- A, A-, B+, B, etc.
    enrollment_date DATETIME DEFAULT GETDATE(),
    approval_status VARCHAR(20) DEFAULT 'PendingApproval' CHECK (approval_status IN ('PendingApproval', 'Approved', 'Rejected', 'Modified')),
    adviser_remarks VARCHAR(MAX) NULL,
    approval_date DATETIME NULL,
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (course_id) REFERENCES courses(course_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (semester_id) REFERENCES semesters(semester_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (adviser_id) REFERENCES advisers(adviser_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT unique_enrollment UNIQUE (student_id, course_id, semester_id),
    INDEX idx_student (student_id),
    INDEX idx_course (course_id),
    INDEX idx_semester (semester_id),
    INDEX idx_status (status),
    INDEX idx_approval (approval_status)
);

-- ============================================
-- TRACKING & REPORTING LAYER
-- ============================================

-- Academic Records Table
CREATE TABLE academic_records (
    record_id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL UNIQUE, -- One record per student
    total_credit_hours INT DEFAULT 0,
    completed_credit_hours INT DEFAULT 0,
    gpa DECIMAL(3,2) DEFAULT 0.00, -- Auto-calculated
    last_updated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE ON UPDATE CASCADE,
    INDEX idx_student (student_id)
);

-- Reports Table
CREATE TABLE reports (
    report_id INT IDENTITY(1,1) PRIMARY KEY,
    generated_by INT NOT NULL, -- AdviserID
    student_id INT NULL,
    report_type VARCHAR(20) NOT NULL CHECK (report_type IN ('ProgressReport', 'Transcript', 'AdvisingReport', 'CourseHistory')),
    generated_date DATETIME DEFAULT GETDATE(),
    content VARCHAR(MAX) NULL,
    file_path VARCHAR(500) NULL,
    FOREIGN KEY (generated_by) REFERENCES advisers(adviser_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    INDEX idx_generated_by (generated_by),
    INDEX idx_student (student_id),
    INDEX idx_report_type (report_type),
    INDEX idx_generated_date (generated_date)
);

-- ============================================
-- INITIAL DATA (Sample)
-- ============================================

-- Insert sample specializations
INSERT INTO specializations (specialization_name, specialization_code, description, required_credit_hours) VALUES
('Programming', 'PROG', 'Software development and programming specialization', 120),
('Networking', 'NET', 'Computer networking and infrastructure specialization', 120),
('Cybersecurity', 'SEC', 'Information security and cybersecurity specialization', 120),
('Database', 'DB', 'Database management and data science specialization', 120);

-- ============================================
-- USER 1: ADMINISTRATOR
-- ============================================
INSERT INTO users (email, password, role) VALUES
('admin@polytechnic.bh', 'admin123', 'Administrator');

DECLARE @admin_user_id INT = SCOPE_IDENTITY();

INSERT INTO administrators (user_id, department) VALUES
(@admin_user_id, 'Information Technology');

-- ============================================
-- USER 2: ADVISER
-- ============================================
INSERT INTO users (email, password, role) VALUES
('adviser@polytechnic.bh', 'adviser123', 'Adviser');

DECLARE @adviser_user_id INT = SCOPE_IDENTITY();

INSERT INTO advisers (user_id, faculty_id, department, office_location) VALUES
(@adviser_user_id, 'FAC001', 'Computer Science', 'Building A, Room 201');

DECLARE @adviser_id INT = SCOPE_IDENTITY();

-- ============================================
-- USER 3: STUDENT
-- ============================================
INSERT INTO users (email, password, role) VALUES
('student@polytechnic.bh', 'student123', 'Student');

DECLARE @student_user_id INT = SCOPE_IDENTITY();
DECLARE @prog_spec_id INT = (SELECT specialization_id FROM specializations WHERE specialization_code = 'PROG');

INSERT INTO students (user_id, adviser_id, specialization_id, first_name, last_name, student_number, current_semester, enrollment_year) VALUES
(@student_user_id, @adviser_id, @prog_spec_id, 'Ahmed', 'Ali', 'S2024001', 1, 2024);

-- Insert academic record for the student
DECLARE @new_student_id INT = SCOPE_IDENTITY();

INSERT INTO academic_records (student_id, total_credit_hours, completed_credit_hours, gpa) VALUES
(@new_student_id, 0, 0, 0.00);

-- Insert sample courses
INSERT INTO courses (course_code, course_name, credit_hours, course_type, description) VALUES
('CS101', 'Introduction to Programming', 3, 'Core', 'Fundamentals of programming using Python'),
('CS102', 'Data Structures', 3, 'Core', 'Introduction to data structures and algorithms'),
('CS201', 'Object-Oriented Programming', 3, 'Core', 'OOP concepts using Java'),
('DB101', 'Database Fundamentals', 3, 'Specialized', 'Introduction to database concepts'),
('NET101', 'Computer Networks', 3, 'Specialized', 'Fundamentals of computer networking'),
('SEC101', 'Introduction to Cybersecurity', 3, 'Specialized', 'Basic concepts of information security');

-- Insert sample prerequisites
DECLARE @cs101_id INT = (SELECT course_id FROM courses WHERE course_code = 'CS101');
DECLARE @cs102_id INT = (SELECT course_id FROM courses WHERE course_code = 'CS102');
DECLARE @cs201_id INT = (SELECT course_id FROM courses WHERE course_code = 'CS201');

INSERT INTO prerequisites (course_id, prerequisite_course_id, is_strict) VALUES
(@cs102_id, @cs101_id, 1),
(@cs201_id, @cs102_id, 1);

-- Insert sample semester
INSERT INTO semesters (semester_name, academic_year, start_date, end_date, registration_start_date, registration_end_date, is_active) VALUES
('Fall', '2024-2025', '2024-09-01', '2024-12-31', '2024-08-01', '2024-08-31', 1);

-- ============================================
-- STORED PROCEDURES
-- ============================================

-- Drop procedure if it exists
IF OBJECT_ID('sp_calculate_student_gpa', 'P') IS NOT NULL
    DROP PROCEDURE sp_calculate_student_gpa;
GO

-- Procedure: Calculate Student GPA
CREATE PROCEDURE sp_calculate_student_gpa
    @p_student_id INT
AS
BEGIN
    DECLARE @v_total_points DECIMAL(10,2);
    DECLARE @v_total_hours INT;
    DECLARE @v_calculated_gpa DECIMAL(3,2);
    
    -- Calculate GPA based on completed courses
    SELECT 
        @v_total_points = SUM(c.credit_hours * 
            CASE e.grade
                WHEN 'A' THEN 4.00
                WHEN 'A-' THEN 3.70
                WHEN 'B+' THEN 3.30
                WHEN 'B' THEN 3.00
                WHEN 'B-' THEN 2.70
                WHEN 'C+' THEN 2.30
                WHEN 'C' THEN 2.00
                WHEN 'C-' THEN 1.70
                WHEN 'D+' THEN 1.30
                WHEN 'D' THEN 1.00
                WHEN 'F' THEN 0.00
                ELSE 0.00
            END
        ),
        @v_total_hours = SUM(c.credit_hours)
    FROM enrollments e
    JOIN courses c ON e.course_id = c.course_id
    WHERE e.student_id = @p_student_id 
      AND e.status = 'Completed'
      AND e.grade IS NOT NULL;
    
    -- Calculate GPA
    IF @v_total_hours > 0
        SET @v_calculated_gpa = @v_total_points / @v_total_hours;
    ELSE
        SET @v_calculated_gpa = 0.00;
    
    -- Update student record
    UPDATE students 
    SET gpa = @v_calculated_gpa,
        completed_credit_hours = ISNULL(@v_total_hours, 0)
    WHERE student_id = @p_student_id;
    
    -- Update academic record
    UPDATE academic_records 
    SET gpa = @v_calculated_gpa,
        completed_credit_hours = ISNULL(@v_total_hours, 0),
        last_updated = GETDATE()
    WHERE student_id = @p_student_id;
    
END;
GO

-- ============================================
-- END OF SCHEMA
-- ============================================