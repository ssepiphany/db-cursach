using System;
using MySql.Data.MySqlClient;
using System.IO;
using MySql.Data;
using System.Collections.Generic;
using System.Linq;

// interface IModel
// {
    
// }

class StudentModel
{
    private MySqlConnection connection;

    public StudentModel(MySqlConnection connection)
    {
        this.connection = connection;
    }

    public int GenerateData(int rows)
    {
        Random random = new Random();
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO Student(fullname, age, specialty, tutorId)
            WITH RECURSIVE nums AS (
            SELECT 1 AS value
            UNION ALL
            SELECT value + 1 AS value
            FROM nums
            WHERE nums.value <= {rows-1}
            )

            SELECT 
                (SELECT title AS fullname FROM studentsdb.Fullname ORDER BY RAND() LIMIT 1) AS res1,
                (SELECT FLOOR(RAND()*(19-6+1)+6) AS age) AS res2,
                (SELECT ELT(0.5 + RAND() * 7, 'Maths', 'Biology', 'PE', 'Languages', 'Science', 'History', 'Literature')) AS res3,
                (SELECT id AS tutorId FROM studentsdb.Teacher ORDER BY RAND() LIMIT 1) AS res4
                FROM nums";
            // CROSS JOIN
            //     ( SELECT 0 AS l UNION SELECT 1  UNION SELECT 2  UNION SELECT 3  UNION SELECT 4  UNION SELECT 5  UNION SELECT 6  UNION SELECT 7  UNION SELECT 8  UNION SELECT 9 ) l";
        // command.Parameters.AddWithValue("fullname", File.ReadLines("./fullnames.txt").Skip(random.Next(1, 501)).Take(1).First());
        // command.Parameters.AddWithValue("specialty", specialties[random.Next(0, specialties.Length-1)]);
        command.Parameters.AddWithValue("rows", rows);
        int res = command.ExecuteNonQuery();
        return res;
    }

    public Student GetById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Student WHERE id = @id";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Student student = ReadStudent(reader); 
            reader.Close(); 
            return student;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public Student GetByFullname(string fullname)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Student WHERE fullname = @fullname";
        command.Parameters.AddWithValue("fullname", fullname); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Student student = ReadStudent(reader); 
            reader.Close(); 
            return student;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public Dictionary<string, decimal> GetSpecialtyPerformance()
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = @"SELECT specialty, avg(mark) 
            FROM Student LEFT JOIN Performance
            ON Student.id = Performance.studentId
            GROUP BY specialty";
        MySqlDataReader reader = command.ExecuteReader(); 
        Dictionary<string, decimal> res = new Dictionary<string, decimal>();
        while (reader.Read())
        {
            res.Add(reader.GetString(0), reader.GetFieldValue<decimal>(1));
        } 

        reader.Close(); 
        return res;
    }

    public Dictionary<string, decimal> GetStudentStatistics(string fullname)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = @"SELECT title, avg(mark)
            FROM Student, Performance, Subject 
            WHERE Student.id = Performance.studentId
            AND Performance.subjectId = Subject.id
            AND Student.fullname = @fullname
            GROUP BY Subject.title";
        command.Parameters.AddWithValue("fullname", fullname); 
        MySqlDataReader reader = command.ExecuteReader(); 
        Dictionary<string, decimal> res = new Dictionary<string, decimal>();
        while (reader.Read())
        {
            res.Add(reader.GetString(0), reader.GetFieldValue<decimal>(1));
        } 

        reader.Close(); 
        return res;
    }

    public Dictionary<string, decimal> GetSpecialtyStatistics(string specialty)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = @"SELECT subject.title, avg(mark)
            FROM Student, Performance, Subject 
            WHERE Student.id = Performance.studentId
            AND Performance.subjectId = Subject.id
            AND specialty = @specialty
            GROUP BY Subject.title";
        command.Parameters.AddWithValue("specialty", specialty); 
        MySqlDataReader reader = command.ExecuteReader(); 
        Dictionary<string, decimal> res = new Dictionary<string, decimal>();
        while (reader.Read())
        {
            res.Add(reader.GetString(0), reader.GetFieldValue<decimal>(1));
        } 

        reader.Close(); 
        return res;
    }

    public Dictionary<DateTime, decimal> GetStudentProgressReport(string fullname)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = @"SELECT date, avg(mark)
            FROM Student CROSS JOIN Performance
            WHERE Student.id = Performance.studentId
            AND fullname = @fullname
            GROUP BY date
            ORDER BY date ASC;";
        command.Parameters.AddWithValue("fullname", fullname); 
        MySqlDataReader reader = command.ExecuteReader(); 
        Dictionary<DateTime, decimal> res = new Dictionary<DateTime, decimal>();
        while (reader.Read())
        {
            res.Add(reader.GetFieldValue<DateTime>(0), reader.GetFieldValue<decimal>(1));
        } 

        reader.Close(); 
        return res;
    }

    public int Insert(Student student) 
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "INSERT INTO Student (fullname, age, specialty, tutorId) VALUES(@fullname, @age, @specialty, @tutorId); SELECT LAST_INSERT_ID()";
        command.Parameters.AddWithValue("fullname", student.fullname); 
        command.Parameters.AddWithValue("age", student.age); 
        command.Parameters.AddWithValue("specialty", student.specialty); 
        command.Parameters.AddWithValue("tutorId", student.tutorId); 
        ulong newId = (ulong)command.ExecuteScalar();
        return (int)newId; 
    }

    public int DeleteById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "DELETE FROM Student WHERE id = @id"; 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res ; 
    }

    public bool Update(int id, Student student)
    {
        MySqlCommand command = connection.CreateCommand() ; 
        command.CommandText = "UPDATE Student SET fullname = @fullname, age = @age, specialty = @specialty, tutorId = @tutorId WHERE id = @id"; 
        command.Parameters.AddWithValue("fullname", student.fullname); 
        command.Parameters.AddWithValue("age", student.age); 
        command.Parameters.AddWithValue("specialty", student.specialty); 
        command.Parameters.AddWithValue("tutorId", student.tutorId); 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res == 1;
    }

    public List<List<string>> SearchStudentsByFullname(string fullnameFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.id, Student.fullname, age, specialty, tutorId, Teacher.fullname FROM Student CROSS JOIN Teacher
            WHERE Student.tutorId = teacher.id
            AND student.fullname Like '%{fullnameFilter}%'";
        command.Parameters.AddWithValue("fullnameFilter", fullnameFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 6));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchStudentsBySpecialty(string specialtyFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.id, Student.fullname, age, specialty, tutorId, Teacher.fullname FROM Student CROSS JOIN Teacher
            WHERE Student.tutorId = teacher.id
            AND student.specialty Like '%{specialtyFilter}%'";
        command.Parameters.AddWithValue("specialtyFilter", specialtyFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 6));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchStudentsByAge(int minAge, int maxAge)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.id, Student.fullname, age, specialty, tutorId, Teacher.fullname FROM Student CROSS JOIN Teacher
            WHERE Student.tutorId = teacher.id
            AND student.age BETWEEN {minAge} AND {maxAge}";
        command.Parameters.AddWithValue("minAge", minAge); 
        command.Parameters.AddWithValue("maxAge", maxAge); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 6));
        } 

        reader.Close(); 
        return res;
    }

    public void ImportCsvFullnames()
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"CREATE TABLE Fullname(
            title VARCHAR(255) NOT NULL);";
        
        int res = command.ExecuteNonQuery();

        command.CommandText = $@"
        LOAD DATA INFILE 'C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/fullnames.csv'
        INTO TABLE Fullname
        FIELDS TERMINATED BY ','
        LINES TERMINATED BY '\r\n';";

        res = command.ExecuteNonQuery();

    }

    private Student ReadStudent(MySqlDataReader reader)
    {
        Student student =  new Student(); 
        student.id = reader.GetFieldValue<int>(0); 
        student.fullname = reader.GetString(1);
        student.age = reader.GetFieldValue<int>(2); 
        student.specialty = reader.GetString(3);   
        student.tutorId = reader.GetFieldValue<int>(4);
        return student;
    }

    private List<string> ReadSearchQuery(MySqlDataReader reader, int queryLength)
    {
        List<string> res = new List<string>();
        for (int i = 0; i < queryLength; i++)
        {
            res.Add(reader.GetString(i).Trim());
        }
        
        return res;
    }
}

class TeacherModel
{
    private MySqlConnection connection;

    public TeacherModel(MySqlConnection connection)
    {
        this.connection = connection;
    }

    public int GenerateData(int rows)
    {
        Random random = new Random();
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO Teacher(fullname, experience, subjectId)
            WITH RECURSIVE nums AS (
            SELECT 1 AS value
            UNION ALL
            SELECT value + 1 AS value
            FROM nums
            WHERE nums.value <= {rows-1})

            SELECT 
                (SELECT title AS fullname FROM studentsdb.Fullname ORDER BY RAND() LIMIT 1) AS res1,
                (SELECT FLOOR(RAND()*(240-1+1)+1)) AS res2,
                (SELECT id AS subjectId FROM studentsdb.Subject ORDER BY RAND() LIMIT 1) AS res3
                FROM nums";
        command.Parameters.AddWithValue("rows", rows);
        int res = command.ExecuteNonQuery();
        return res;
    }

    public Teacher GetById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Teacher WHERE id = @id";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Teacher teacher = ReadTeacher(reader); 
            reader.Close(); 
            return teacher;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    // public List<Teacher> GetTopTeachersForStatistics()
    // {
    //     MySqlCommand command = connection.CreateCommand(); 
    //     command.CommandText = @"SELECT * from Teacher 
    //         ORDER BY experience DESC
    //         limit 6";
    //     MySqlDataReader reader = command.ExecuteReader();

    //     List<Teacher> res = new List<Teacher>();
    //     while (reader.Read())
    //     {
    //         Teacher teacher = ReadTeacher(reader); 
    //         res.Add(teacher);
    //     } 

    //     return res;
    // }

    public List<Teacher> GetGroupAbsentTeachers()
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT * FROM Teacher 
            LEFT JOIN Student ON Teacher.id = Student.tutorId
            WHERE NOT EXISTS (
            SELECT * FROM Student
            WHERE Student.tutorId = Teacher.id);";
        MySqlDataReader reader = command.ExecuteReader(); 
        List<Teacher> res = new List<Teacher>();
        while (reader.Read())
        {
            Teacher teacher = ReadTeacher(reader); 
            res.Add(teacher);
        } 

        reader.Close(); 
        return res;
    }

    public Dictionary<string, long> GetTeacherSubjectDistribution()
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = @"SELECT title, COUNT(*) FROM Teacher, Subject
            WHERE Teacher.subjectId = Subject.id 
            GROUP BY subjectId";
        MySqlDataReader reader = command.ExecuteReader(); 
        Dictionary<string, long> res = new Dictionary<string, long>();
        while (reader.Read())
        {
            res.Add(reader.GetString(0), reader.GetFieldValue<long>(1));
        } 

        reader.Close(); 
        return res;
    }

    public int Insert(Teacher teacher) 
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "INSERT INTO Teacher (fullname, experience, subjectId) VALUES(@fullname, @experience, @subjectId); SELECT LAST_INSERT_ID()";
        command.Parameters.AddWithValue("fullname", teacher.fullname); 
        command.Parameters.AddWithValue("experience", teacher.experience); 
        command.Parameters.AddWithValue("subjectId", teacher.subjectId); 
        ulong newId = (ulong)command.ExecuteScalar();
        return (int)newId; 
    }

    public int DeleteById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "DELETE FROM Teacher WHERE id = @id"; 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res ; 
    }

    public bool Update(int id, Teacher teacher)
    {
        MySqlCommand command = connection.CreateCommand() ; 
        command.CommandText = "UPDATE Teacher SET fullname = @fullname, experience = @experience, subjectId = @subjectId WHERE id = @id"; 
        command.Parameters.AddWithValue("fullname", teacher.fullname); 
        command.Parameters.AddWithValue("experience", teacher.experience); 
        command.Parameters.AddWithValue("subjectId", teacher.subjectId); 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res == 1;
    }

    public List<List<string>> SearchTeachersByFullanme(string fullnameFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT fullname, experience, title FROM Teacher CROSS JOIN Subject
            WHERE Teacher.subjectId = Subject.id
            AND fullname Like '%{fullnameFilter}%'";
        command.Parameters.AddWithValue("fullnameFilter", fullnameFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchTeachersBySubject(string subjectFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT fullname, experience, title FROM Teacher CROSS JOIN Subject
            WHERE Teacher.subjectId = Subject.id
            AND title Like '%{subjectFilter}%'";
        command.Parameters.AddWithValue("subjectFilter", subjectFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchTeachersBySubjectAndExperience(string subjectFilter, int minExperience)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT fullname, experience, title, examDate FROM Teacher CROSS JOIN Subject
            WHERE Teacher.subjectId = Subject.id
            AND title Like '%{subjectFilter}%'
            AND experience >= {minExperience}";
        command.Parameters.AddWithValue("subjectFilter", subjectFilter); 
        command.Parameters.AddWithValue("minExperience", minExperience); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 4));
        } 

        reader.Close(); 
        return res;
    }

    private List<string> ReadSearchQuery(MySqlDataReader reader, int queryLength)
    {
        List<string> res = new List<string>();
        for (int i = 0; i < queryLength; i++)
        {
            res.Add(reader.GetString(i).Trim());
        }
        
        return res;
    }

    private Teacher ReadTeacher(MySqlDataReader reader)
    {
        Teacher teacher =  new Teacher(); 
        teacher.id = reader.GetFieldValue<int>(0); 
        teacher.fullname = reader.GetString(1);
        teacher.experience = reader.GetFieldValue<int>(2); 
        teacher.subjectId = reader.GetFieldValue<int>(3);
        return teacher;
    }
}


class SubjectModel
{
    private MySqlConnection connection;

    public SubjectModel(MySqlConnection connection)
    {
        this.connection = connection;
    }

    public int GenerateData(int rows)
    {
        // Random random = new Random();
        // string[] titles = new string[] {"Algebra", "Geometry", "Biology", "English", "Spanish", "PE", "Crafts", "Geography", "Science", "Physics", "History", "IT", "English Literature", "Foreign literature"};
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO Subject(title, examDate)
            WITH RECURSIVE nums AS (
            SELECT 1 AS value
            UNION ALL
            SELECT value + 1 AS value
            FROM nums
            WHERE nums.value <= {rows-1})

            SELECT 
                ELT(0.5 + RAND() * 11, 'Algebra', 'Geometry', 'Biology', 'English', 'Spanish', 'PE', 'Crafts', 'Geography', 'Science', 'Physics', 'History', 'IT', 'English Literature', 'Foreign literature') AS res3,
                CURRENT_DATE + INTERVAL FLOOR(RAND() * 120) DAY
                FROM nums";
        // command.Parameters.AddWithValue("title", titles[random.Next(0, titles.Length-1)]);
        command.Parameters.AddWithValue("rows", rows);
        int res = command.ExecuteNonQuery();
        return res;
    }

    public Subject GetById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Subject WHERE id = @id";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Subject subject = ReadSubject(reader); 
            reader.Close(); 
            return subject;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public int Insert(Subject subject) 
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "INSERT INTO Subject (title, examDate) VALUES(@title, @examDate); SELECT LAST_INSERT_ID()";
        command.Parameters.AddWithValue("title", subject.title); 
        command.Parameters.AddWithValue("examDate", subject.examDate); 
        ulong newId = (ulong)command.ExecuteScalar();
        return (int)newId; 
    }

    public int DeleteById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "DELETE FROM Subject WHERE id = @id"; 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res ; 
    }

    public bool Update(int id, Subject subject)
    {
        MySqlCommand command = connection.CreateCommand() ; 
        command.CommandText = "UPDATE Subject SET title = @title, examDate = @examDate WHERE id = @id"; 
        command.Parameters.AddWithValue("title", subject.title); 
        command.Parameters.AddWithValue("examDate", subject.examDate); 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res == 1;
    }

    public List<List<string>> SearchSubjectByTitle(string titleFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT id, title, examDate FROM Subject
            WHERE title Like '%{titleFilter}%'";
        command.Parameters.AddWithValue("titleFilter", titleFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    private List<string> ReadSearchQuery(MySqlDataReader reader, int queryLength)
    {
        List<string> res = new List<string>();
        for (int i = 0; i < queryLength; i++)
        {
            res.Add(reader.GetString(i).Trim());
        }
        
        return res;
    }

    private Subject ReadSubject(MySqlDataReader reader)
    {
        Subject subject =  new Subject(); 
        subject.id = reader.GetFieldValue<int>(0); 
        subject.title = reader.GetString(1);
        subject.examDate = reader.GetFieldValue<DateTime>(2); 
        return subject;
    }
}


class PerformanceModel
{
    private MySqlConnection connection;

    public PerformanceModel(MySqlConnection connection)
    {
        this.connection = connection;
    }

    public int GenerateData(int rows)
    {
        Random random = new Random();
        
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO Performance(mark, subjectId, studentId, date)
            WITH RECURSIVE nums AS (
            SELECT 1 AS value
            UNION ALL
            SELECT value + 1 AS value
            FROM nums
            WHERE nums.value <= {rows-1})

            SELECT
                (select FLOOR(RAND()*(12-1+1)+1)) AS result1,
                (select id AS subjectId FROM studentsdb.Subject ORDER BY RAND() LIMIT 1) AS result2,
                (select id AS studentId FROM studentsdb.Student ORDER BY RAND() LIMIT 1)AS result3, 
                (select CURRENT_DATE - INTERVAL FLOOR(RAND() * 100) DAY) AS result4
                FROM nums";
        command.Parameters.AddWithValue("rows", rows);
        int res = command.ExecuteNonQuery();
        return res;
    }

    public Performance GetById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM Performance WHERE id = @id";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Performance performance = ReadPerformance(reader); 
            reader.Close(); 
            return performance;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public Performance GetByStudentId(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Subject.title, Performance.mark, Performance.date 
            FROM Performance LEFT JOIN Student 
            ON Performance.studentId = Student.id
            LEFT JOIN Subject 
            ON Performance.subjectId = Subject.id 
            WHERE studentId = {id};";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            Performance performance = ReadPerformance(reader); 
            reader.Close(); 
            return performance;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public int Insert(Performance performance) 
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "INSERT INTO Performance (mark, subjectId, studentId, date) VALUES(@mark, @subjectId, @studentId, @date); SELECT LAST_INSERT_ID()";
        command.Parameters.AddWithValue("mark", performance.mark); 
        command.Parameters.AddWithValue("subjectId", performance.subjectId); 
        command.Parameters.AddWithValue("studentId", performance.studentId); 
        command.Parameters.AddWithValue("date", performance.date); 
        ulong newId = (ulong)command.ExecuteScalar();
        return (int)newId; 
    }

    public int DeleteById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "DELETE FROM Performance WHERE id = @id"; 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res ; 
    }

    public bool Update(int id, Performance performance)
    {
        MySqlCommand command = connection.CreateCommand() ; 
        command.CommandText = "UPDATE Performance SET mark = @mark, subjectId = @subjectId, studentId = @studentId, date = @date WHERE id = @id"; 
        command.Parameters.AddWithValue("mark", performance.mark); 
        command.Parameters.AddWithValue("subjectId", performance.subjectId); 
        command.Parameters.AddWithValue("studentId", performance.studentId);
        command.Parameters.AddWithValue("date", performance.date);
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res == 1;
    }

    public List<List<string>> SearchPerformancesByStudent(string studentFullnameFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Subject.title, mark, date FROM Performance LEFT JOIN Student 
            ON Performance.studentId = Student.id 
            LEFT JOIN Subject ON Performance.subjectId = Subject.id
            WHERE fullname Like '%{studentFullnameFilter}%'";
        command.Parameters.AddWithValue("studentFullnameFilter", studentFullnameFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 4));
        } 

        reader.Close(); 
        return res;
    }


    public List<List<string>> SearchPerformancesByStudentAndSubject(string studentFullnameFilter, string subjectFilter)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Subject.title, mark, date FROM Performance LEFT JOIN Student 
            ON Performance.studentId = Student.id 
            LEFT JOIN Subject ON Performance.subjectId = Subject.id
            WHERE fullname Like '%{studentFullnameFilter}%'
            AND title Like '%{subjectFilter}%'";
        command.Parameters.AddWithValue("studentFullnameFilter", studentFullnameFilter); 
        command.Parameters.AddWithValue("subjectFilter", subjectFilter); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 4));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchPerformancesByStudentAndDate(string studentFullnameFilter, DateTime dateMinValue)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Subject.title, mark, date FROM Performance LEFT JOIN Student 
            ON Performance.studentId = Student.id 
            LEFT JOIN Subject ON Performance.subjectId = Subject.id
            WHERE fullname Like '%{studentFullnameFilter}%'
            AND date >= '{dateMinValue.ToString("yyyy-M-dd hh:mm:ss")}'";
        command.Parameters.AddWithValue("studentFullnameFilter", studentFullnameFilter); 
        command.Parameters.AddWithValue("dateMinValue", dateMinValue.ToString("yyyy-M-dd hh:mm")); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 4));
        } 

        reader.Close(); 
        return res;
    }


    private List<string> ReadSearchQuery(MySqlDataReader reader, int queryLength)
    {
        List<string> res = new List<string>();
        for (int i = 0; i < queryLength; i++)
        {
            res.Add(reader.GetString(i).Trim());
        }
        
        return res;
    }

    private Performance ReadPerformance(MySqlDataReader reader)
    {
        Performance performance =  new Performance(); 
        performance.id = reader.GetFieldValue<int>(0); 
        performance.mark = reader.GetFieldValue<int>(1);
        performance.subjectId = reader.GetFieldValue<int>(2);
        performance.studentId = reader.GetFieldValue<int>(3);
        performance.date = reader.GetFieldValue<DateTime>(4); 
        return performance;
    }
}

class TeacherStudentModel
{
    private MySqlConnection connection;

    public TeacherStudentModel(MySqlConnection connection)
    {
        this.connection = connection;
    }

    public int GenerateData(int rows)
    {
        Random random = new Random();
        
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = $@"INSERT INTO TeacherStudent(teacherId, studentId)
            WITH RECURSIVE nums AS (
            SELECT 1 AS value
            UNION ALL
            SELECT value + 1 AS value
            FROM nums
            WHERE nums.value <= {rows-1})

            SELECT 
                (SELECT id AS teacherId FROM studentsdb.Teacher ORDER BY RAND() LIMIT 1) AS res1,
                (SELECT id AS studentId FROM studentsdb.Student ORDER BY RAND() LIMIT 1) AS res2
                FROM nums";
        command.Parameters.AddWithValue("rows", rows);
        int res = command.ExecuteNonQuery();
        return res;
    }

    public TeacherStudent GetById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "SELECT * FROM TeacherStudent WHERE id = @id";
        command.Parameters.AddWithValue("id", id); 
        MySqlDataReader reader = command.ExecuteReader(); 
        if (reader.Read())
        {
            TeacherStudent ts = ReadTeacherStudent(reader); 
            reader.Close(); 
            return ts;
        } 
        else
        {
            reader.Close(); 
            return null; 
        } 
    }

    public int Insert(TeacherStudent teacherStudent) 
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "INSERT INTO TeacherStudent (teacherId, studentId) VALUES(@teacherId, @studentId); SELECT LAST_INSERT_ID()";
        command.Parameters.AddWithValue("teacherId", teacherStudent.teacherId); 
        command.Parameters.AddWithValue("studentId", teacherStudent.studentId); 
        ulong newId = (ulong)command.ExecuteScalar();
        return (int)newId; 
    }

    public int DeleteById(int id)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = "DELETE FROM TeacherStudent WHERE id = @id"; 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res ; 
    }

    public bool Update(int id, TeacherStudent teacherStudent)
    {
        MySqlCommand command = connection.CreateCommand() ; 
        command.CommandText = "UPDATE TeacherStudent SET teacherId = @teacherId, studentId = @studentId WHERE id = @id"; 
        command.Parameters.AddWithValue("teacherId", teacherStudent.teacherId); 
        command.Parameters.AddWithValue("studentId", teacherStudent.studentId); 
        command.Parameters.AddWithValue("id", id); 
        int res = command.ExecuteNonQuery(); 
        return res == 1;
    }

    public List<List<string>> SearchByStudentFullname(string fullname)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Teacher.fullname, Subject.title FROM TeacherStudent
            LEFT JOIN Student ON TeacherStudent.studentId = Student.id
            LEFT JOIN Teacher ON TeacherStudent.teacherId = Teacher.id
            LEFT JOIN Subject ON Teacher.subjectId = Subject.id
            WHERE Student.fullname LIKE '%{fullname}%'";
        command.Parameters.AddWithValue("fullname", fullname); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchByTeacher(string fullname)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Teacher.fullname, Subject.title FROM TeacherStudent
            LEFT JOIN Student ON TeacherStudent.studentId = Student.id
            LEFT JOIN Teacher ON TeacherStudent.teacherId = Teacher.id
            LEFT JOIN Subject ON Teacher.subjectId = Subject.id
            WHERE Teacher.fullname LIKE '%{fullname}%'";
        command.Parameters.AddWithValue("fullname", fullname); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    public List<List<string>> SearchByStudentAndSubject(string fullname, string title)
    {
        MySqlCommand command = connection.CreateCommand(); 
        command.CommandText = $@"SELECT Student.fullname, Teacher.fullname, Subject.title FROM TeacherStudent
            LEFT JOIN Student ON TeacherStudent.studentId = Student.id
            LEFT JOIN Teacher ON TeacherStudent.teacherId = Teacher.id
            LEFT JOIN Subject ON Teacher.subjectId = Subject.id
            WHERE Student.fullname LIKE '%{fullname}%'
            AND Subject.title LIKE '%{title}%'";
        command.Parameters.AddWithValue("fullname", fullname); 
        command.Parameters.AddWithValue("title", title); 
        MySqlDataReader reader = command.ExecuteReader(); 
        List<List<string>> res = new List<List<string>>();
        while (reader.Read())
        {
            res.Add(ReadSearchQuery(reader, 3));
        } 

        reader.Close(); 
        return res;
    }

    private List<string> ReadSearchQuery(MySqlDataReader reader, int queryLength)
    {
        List<string> res = new List<string>();
        for (int i = 0; i < queryLength; i++)
        {
            res.Add(reader.GetString(i).Trim());
        }
        
        return res;
    }

    private TeacherStudent ReadTeacherStudent(MySqlDataReader reader)
    {
        TeacherStudent ts =  new TeacherStudent(); 
        ts.id = reader.GetFieldValue<int>(0); 
        ts.teacherId = reader.GetFieldValue<int>(1);
        ts.studentId = reader.GetFieldValue<int>(2);
        return ts;
    }
}