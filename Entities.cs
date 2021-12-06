using System;

class Student
{
    public int id;
    public string fullname;
    public int age;
    public string specialty;
    public int tutorId;

    public Student() { }

    public Student(int id, string fullname, int age, string speciality, int tutorId)
    {
        this.id = id;
        this.fullname = fullname;
        this.age = age;
        this.specialty = speciality;
        this.tutorId = tutorId;
    }
}

class Teacher
{
    public int id;
    public string fullname;
    public int experience;
    public int subjectId;

    public Teacher() {}

    public Teacher(int id, string fullname, int experience, int subjectId)
    {
        this.id = id;
        this.fullname = fullname;
        this.experience = experience;
        this.subjectId = subjectId;
    }
}

class Subject
{
    public int id;
    public string title;
    public DateTime examDate;

    public Subject() {}

    public Subject(int id, string title)
    {
        this.id = id;
        this.title = title;
    }
}

class Performance
{
    public int id;
    public int mark;
    public int subjectId;
    public int studentId;
    public DateTime date;

    public Performance() {}

    public Performance(int id, int score, int subjectId, int studentId, DateTime date)
    {
        this.id = id;
        this.mark = score;
        this.subjectId = subjectId;
        this.studentId = studentId;
        this.date = date;

    }
}

class TeacherStudent
{
    public int id;
    public int teacherId;
    public int studentId;

    public TeacherStudent() {}

    public TeacherStudent(int id, int teacherId, int studentId)
    {
        this.id = id;
        this.teacherId = teacherId;
        this.studentId = studentId;
    }
}