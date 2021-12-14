using static System.Console;
using System;
using System.Collections.Generic;

static class Filler
{
    public static Student FillStudentFields()
    {
        Student student = new Student();
        WriteLine("Please, fill student fields.");

        Write("Fullname: ");
        student.fullname = ReadLine();
        while (String.IsNullOrEmpty(student.fullname))
        {
            WriteLine("Fullname is mandatory field.");
            Write("Fullname: ");
            student.fullname = ReadLine();
        }

        Write("age: ");
        while (!int.TryParse(ReadLine(), out student.age))
        {
            WriteLine("Invalid age.");
            Write("Age: ");
        }

        List<string> specialties = new List<string> {"Maths", "Languages", "Biology", "PE", "Science", "History", "Literature"};
        Write("Specialty: ");
        student.specialty = ReadLine();
        while (!specialties.Contains(student.specialty))
        {
            WriteLine("Invalid specialty.");
            Write("Specialty: ");
            student.specialty = ReadLine();
        }

        Write("Tutor id: ");
        while (!int.TryParse(ReadLine(), out student.tutorId) || Controller.teacherRepo.GetById(student.tutorId) == null)
        {
            WriteLine("Invalid tutor id.");
            Write("Tutor id: ");
        }

        return student;
    }

    public static Teacher FillTeacherFields()
    {
        Teacher teacher = new Teacher();
        WriteLine("Please, fill teacher fields.");

        Write("Fullname: ");
        teacher.fullname = ReadLine();
        while (String.IsNullOrEmpty(teacher.fullname))
        {
            WriteLine("Fullname is mandatory field.");
            Write("Fullname: ");
            teacher.fullname = ReadLine();
        }

        Write("Experience (in months): ");
        while (!int.TryParse(ReadLine(), out teacher.experience))
        {
            WriteLine("Invalid experience.");
            Write("Experience: ");
        }

        Write("Subject id: ");
        while (!int.TryParse(ReadLine(), out teacher.subjectId) || Controller.subjectRepo.GetById(teacher.subjectId) == null)
        {
            WriteLine("Invalid subject id.");
            Write("Subject id: ");
        }

        return teacher;
    }

    public static Subject FillSubjectFields()
    {
        Subject subject = new Subject();
        WriteLine("Please, fill subject fields.");

        List<string> titles = new List<string> {"Algebra", "Geometry", "Biology", "English", "Spanish", "PE", "Crafts", "Geography", "Science", "Physics", "History", "IT", "English literature", "Foreign literature", "Astronomy"};
        Write("Title: ");
        subject.title = ReadLine();
        while (!titles.Contains(subject.title))
        {
            WriteLine("Invalid title.");
            Write("Title: ");
            subject.title = ReadLine();
        }

        Write("Exam date: ");
        while (!DateTime.TryParse(ReadLine(), out subject.examDate))
        {
            WriteLine("Invalid exam date.");
            Write("Exam date: ");
        }

        return subject; 
    }

    public static Performance FillPerformanceFields()
    {
        Performance performance = new Performance();
        WriteLine("Please, fill performance fields.");

        Write("Mark: ");
        while (!int.TryParse(ReadLine(), out performance.mark) || performance.mark > 12 || performance.mark < 1)
        {
            WriteLine("Invalid mark.");
            Write("Mark: ");
        }

        Write("subject id: ");
        while (!int.TryParse(ReadLine(), out performance.subjectId) || Controller.subjectRepo.GetById(performance.subjectId) == null)
        {
            WriteLine("Invalid subject id.");
            Write("Subject id: ");
        }

        Write("Student id: ");
        while (!int.TryParse(ReadLine(), out performance.studentId) || Controller.studentRepo.GetById(performance.studentId) == null)
        {
            WriteLine("Invalid student id.");
            Write("Student id: ");
        }

        Write("Date: ");
        while (!DateTime.TryParse(ReadLine(), out performance.date))
        {
            WriteLine("Invalid date.");
            Write("Date: ");
        }

        return performance;
    }

    public static TeacherStudent FillTeacherStudentFields()
    {
        TeacherStudent ts = new TeacherStudent();
        WriteLine("Please, fill teacherStudent fields.");

        Write("Teacher id: ");
        while (!int.TryParse(ReadLine(), out ts.teacherId) || Controller.teacherRepo.GetById(ts.teacherId) == null)
        {
            WriteLine("Invalid teacher id.");
            Write("Teacher id: ");
        }

        Write("Student id: ");
        while (!int.TryParse(ReadLine(), out ts.studentId) || Controller.studentRepo.GetById(ts.studentId) == null)
        {
            WriteLine("Invalid student id.");
            Write("Student id: ");
        }

        return ts; 
    }
}