using static System.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;

static class Controller
{
    public static StudentModel studentRepo;
    public static TeacherModel teacherRepo;
    public static SubjectModel subjectRepo;
    public static PerformanceModel performanceRepo;
    public static TeacherStudentModel teacherStudentRepo;

    //DELETE

    public static void ProcessStudentDelete()
    {
        int id = GetRowIdForRemoval();

        if (studentRepo.GetById(id) == null)
        {
            View.DisplayMessage($"Record with id {id} in students table doesn't exist.");
            return;
        }

        int res = studentRepo.DeleteById(id);
        if (res != -1)
        {
            View.DisplayMessage($"Record with id {id} in students table was successfully deleted.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }

    public static void ProcessTeacherDelete()
    {
        int id = GetRowIdForRemoval();

        if (teacherRepo.GetById(id) == null)
        {
            View.DisplayMessage($"Record with id {id} in teachers table doesn't exist.");
            return;
        }

        int res = teacherRepo.DeleteById(id);
        if (res != -1)
        {
            View.DisplayMessage($"Record with id {id} in teachers table was successfully deleted.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }

    public static void ProcessSubjectDelete()
    {
        int id = GetRowIdForRemoval();

        if (subjectRepo.GetById(id) == null)
        {
            View.DisplayMessage($"Record with id {id} in subjects table doesn't exist.");
            return;
        }

        int res = subjectRepo.DeleteById(id);
        if (res != -1)
        {
            View.DisplayMessage($"Record with id {id} in subjects table was successfully deleted.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }

    public static void ProcessPerformanceDelete()
    {
        int id = GetRowIdForRemoval();

        if (performanceRepo.GetById(id) == null)
        {
            View.DisplayMessage($"Record with id {id} in performances table doesn't exist.");
            return;
        }

        int res = performanceRepo.DeleteById(id);
        if (res != -1)
        {
            View.DisplayMessage($"Record with id {id} in performances table was successfully deleted.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }

    public static void ProcessTeacherStudentDelete()
    {
        int id = GetRowIdForRemoval();

        if (teacherStudentRepo.GetById(id) == null)
        {
            View.DisplayMessage($"Record with id {id} in teacherStudents table doesn't exist.");
            return;
        }

        int res = teacherStudentRepo.DeleteById(id);
        if (res != -1)
        {
            View.DisplayMessage($"Record with id {id} in teacherStudents table was successfully deleted.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }
    private static int GetRowIdForRemoval()
    {
        Write("Please, enter row id for removal: ");
        int id;
        while (!int.TryParse(ReadLine(), out id))
        {
            WriteLine("Invalid id.");
            Write("Please, enter row id for removal: ");
        }

        return id;
    }


    //INSERT

    public static void ProcessStudentInsert()
    {
        Student student = Filler.FillStudentFields();

        int newId = studentRepo.Insert(student);
        if (newId > 0) View.DisplayMessage($"New record with id {newId} was successfully inserted into students table.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessTeacherInsert()
    {
        Teacher teacher = Filler.FillTeacherFields();

        int newId = teacherRepo.Insert(teacher);
        if (newId > 0) View.DisplayMessage($"New record with id {newId} was successfully inserted into teachers table.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessSubjectInsert()
    {
        Subject subject = Filler.FillSubjectFields();
        int newId = subjectRepo.Insert(subject);
        if (newId > 0) View.DisplayMessage($"New record with id {newId} was successfully inserted into subjects table.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessPerformanceInsert()
    {
        Performance performance = Filler.FillPerformanceFields();
        int newId = performanceRepo.Insert(performance);
        if (newId > 0) View.DisplayMessage($"New record with id {newId} was successfully inserted into performances table.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessTeacherStudentInsert()
    {
        TeacherStudent ts = Filler.FillTeacherStudentFields();

        int newId = teacherStudentRepo.Insert(ts);
        if (newId > 0) View.DisplayMessage($"New record with id {newId} was successfully inserted into teacherStudents table.");
        else View.DisplayMessage("Something went wrong.");
    }


    //UPDATE


    public static void ProcessStudentUpdate()
    {
        Write("Please, enter row id for update:  ");

        int id;
        while (!int.TryParse(ReadLine(), out id) || studentRepo.GetById(id) == null)
        {
            WriteLine("Invalid id.");
            Write("Id: ");
        }

        Student student = Filler.FillStudentFields();
        bool updated = studentRepo.Update(id, student);
        if (updated) View.DisplayMessage($"Record in students table with id {id} was successfully updated.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessTeacherUpdate()
    {
        Write("Please, enter row id for update:  ");

        int id;
        while (!int.TryParse(ReadLine(), out id) || teacherRepo.GetById(id) == null)
        {
            WriteLine("Invalid id.");
            Write("Id: ");
        }
        
        Teacher teacher = Filler.FillTeacherFields();
        bool updated = teacherRepo.Update(id, teacher);

        if (updated) View.DisplayMessage($"Record in teachers table with id {id} was successfully updated.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessSubjectUpdate()
    {
        Write("Please, enter row id for update:  ");

        int id;
        while (!int.TryParse(ReadLine(), out id) || subjectRepo.GetById(id) == null)
        {
            WriteLine("Invalid id.");
            Write("Id: ");
        }

        Subject subject = Filler.FillSubjectFields();

        bool updated = subjectRepo.Update(id, subject);

        if (updated) View.DisplayMessage($"Record in subjects table with id {id} was successfully updated.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessPerformanceUpdate()
    {
        Write("Please, enter row id for update:  ");

        int id;
        while (!int.TryParse(ReadLine(), out id) || performanceRepo.GetById(id) == null)
        {
            WriteLine("Invalid id.");
            Write("Id: ");
        }

        Performance performance = Filler.FillPerformanceFields();
        bool updated = performanceRepo.Update(id, performance);

        if (updated) View.DisplayMessage($"Record in performances table with id {id} was successfully updated.");
        else View.DisplayMessage("Something went wrong.");
    }

    public static void ProcessTeacherSubjectUpdate()
    {
        Write("Please, enter row id for update:  ");

        int id;
        while (!int.TryParse(ReadLine(), out id) || teacherRepo.GetById(id) == null)
        {
            WriteLine("Invalid id.");
            Write("Id: ");
        }
        TeacherStudent ts = Filler.FillTeacherStudentFields();

        bool updated = teacherStudentRepo.Update(id, ts);

        if (updated) View.DisplayMessage($"Record in teacherStudents table with id {id} was successfully updated.");
        else View.DisplayMessage("Something went wrong.");
    }


    //OTHER


    public static void ProcessGenerate(string table, int rows)
    {
        int res = 0; 
        switch (table)
        {
            case "student": 
                res = studentRepo.GenerateData(rows);
                break;
            case "teacher":
                res = teacherRepo.GenerateData(rows);
                break;
            case "subject":
                res = subjectRepo.GenerateData(rows);
                break;
            case "performance":
                res = performanceRepo.GenerateData(rows);
                break;
            case "teacherStudent":
                res = teacherStudentRepo.GenerateData(rows);
                break; 
        }

        if (res != -1)
        {
            View.DisplayMessage($"Done. {res} rows were inserted into the {table}s table.");
        }
        else
        {
            View.DisplayMessage("Something went wrong.");
        }
    }

    public static void CreateData()
    {
        studentRepo.ImportCsvFullnames();
    }


    //SEARCH


    public static void ProcessStudentSearch()
    {
        List<string> options = new List<string> {"f", "a", "s"};
        Write("Choose student field for search(f = fullname; a = age; s = specialty): ");
        string searchField = ReadLine().Trim();
        while (!options.Contains(searchField))
        {
            WriteLine("Invalid field.");
            Write("Field: ");
            searchField = ReadLine();
        }

        List<List<string>> res = new List<List<string>>();

        switch (searchField)
        {
            case "f": 
                Write("Student fullname filter: ");
                string fullnameFilter = ReadLine();

                res = studentRepo.SearchStudentsByFullname(fullnameFilter);
                break;
            case "a":
                Write("Student min age: ");
                int minAge;
                while(!int.TryParse(ReadLine(), out minAge))
                {
                    Write("Invalid value. Min age: ");
                }

                Write("Student max age: ");
                int maxAge;
                while(!int.TryParse(ReadLine(), out maxAge))
                {
                    Write("Invalid value. Max age: ");
                }

                res = studentRepo.SearchStudentsByAge(minAge, maxAge);
                break;
            case "s":
                Write("Student specialty: ");
                string specialtyFilter = ReadLine();
                
                res = studentRepo.SearchStudentsBySpecialty(specialtyFilter);
                break;
        }

        if (res.Count == 0) 
        {
            View.DisplayMessage("There are no results.");
            return;
        } 
        View.DisplayStudentSearch(res);
    }

    public static void ProcessTeacherSearch()
    {
        List<string> options = new List<string> {"f", "s", "se"};
        Write("Choose teacher field for search(f = fullname; s = subject; se = subject + experience): ");
        string searchField = ReadLine().Trim();
        while (!options.Contains(searchField))
        {
            WriteLine("Invalid field.");
            Write("Field: ");
            searchField = ReadLine();
        }

        List<List<string>> res = new List<List<string>>();
        string subjectFilter = "";
        bool extended = false;

        switch (searchField)
        {
            case "f": 
                Write("Teacher fullname filter: ");
                string fullnameFilter = ReadLine().Trim();

                res = teacherRepo.SearchTeachersByFullanme(fullnameFilter);
                break;
            case "s":
                Write("Subject title filter: ");
                subjectFilter = ReadLine().Trim();

                res = teacherRepo.SearchTeachersBySubject(subjectFilter);
                break;
            case "se":
                Write("Subject title filter: ");
                subjectFilter = ReadLine().Trim();

                Write("Teacher experience (in months): ");
                int experience;
                while(!int.TryParse(ReadLine(), out experience))
                {
                    Write("Invalid value. Experience: ");
                }
                
                res = teacherRepo.SearchTeachersBySubjectAndExperience(subjectFilter, experience);
                extended = true;
                break;
        }

        if (res.Count == 0) 
        {
            View.DisplayMessage("There are no results.");
            return;
        } 
        View.DisplayTeacherSearch(res, extended);
    }

    public static void ProcessSubjectSearch()
    {
        List<List<string>> res = new List<List<string>>();

        Write("Subject title filter: ");
        string titleFilter = ReadLine().Trim();

        res = subjectRepo.SearchSubjectByTitle(titleFilter);

        if (res.Count == 0) 
        {
            View.DisplayMessage("There are no results.");
            return;
        } 
        View.DisplaySubjectSearch(res);
    }

    public static void ProcessPerformanceSearch()
    {
        List<string> options = new List<string> {"f", "fs", "fd"};
        Write("Choose teacher field for search(f = fullname; fs = fullname + subject; fd = fullname + date): ");
        string searchField = ReadLine().Trim();
        while (!options.Contains(searchField))
        {
            WriteLine("Invalid field.");
            Write("Field: ");
            searchField = ReadLine();
        }

        List<List<string>> res = new List<List<string>>();
        string fullnameFilter = "";

        switch (searchField)
        {
            case "f": 
                Write("Student fullname filter: ");
                fullnameFilter = ReadLine().Trim();

                res = performanceRepo.SearchPerformancesByStudent(fullnameFilter);
                break;
            case "fs":
                Write("Student fullname filter: ");
                fullnameFilter = ReadLine().Trim();

                Write("Subject title filter: ");
                string subjectFilter = ReadLine().Trim();

                res = performanceRepo.SearchPerformancesByStudentAndSubject(fullnameFilter, subjectFilter);
                break;
            case "fd":
                Write("Student fullname filter: ");
                fullnameFilter = ReadLine().Trim();

                Write("Mark date: ");
                DateTime date;
                while(!DateTime.TryParse(ReadLine(), out date))
                {
                    Write("Invalid value. Date: ");
                }
                
                res = performanceRepo.SearchPerformancesByStudentAndDate(fullnameFilter, date);
                break;
        }

        if (res.Count == 0) 
        {
            View.DisplayMessage("There are no results.");
            return;
        } 
        View.DisplayPerformanceSearch(res);
    }

    public static void ProcessTeacherStudentSearch()
    {
        List<string> options = new List<string> {"sf", "fs", "tf"};
        Write("Choose teacher field for search(sf = student fullname; fs = student fullname + subject; tf = teacher fullname): ");
        string searchField = ReadLine().Trim();
        while (!options.Contains(searchField))
        {
            WriteLine("Invalid field.");
            Write("Field: ");
            searchField = ReadLine();
        }

        List<List<string>> res = new List<List<string>>();
        string studentFilter = "";

        switch (searchField)
        {
            case "sf": 
                Write("Student fullname filter: ");
                studentFilter = ReadLine().Trim();

                res = teacherStudentRepo.SearchByStudentFullname(studentFilter);
                break;
            case "fs":
                Write("Student fullname filter: ");
                studentFilter = ReadLine().Trim();

                Write("Subject title filter: ");
                string subjectFilter = ReadLine().Trim();

                res = teacherStudentRepo.SearchByStudentAndSubject(studentFilter, subjectFilter);
                break;
            case "tf":
                Write("Teacher fullname filter: ");
                string teacherFilter = ReadLine().Trim();
                
                res = teacherStudentRepo.SearchByTeacher(teacherFilter);
                break;
        }

        if (res.Count == 0) 
        {
            View.DisplayMessage("There are no results.");
            return;
        } 
        View.DisplayTeacherStudentSearch(res);
    }

    //STATISTICS

    public static void ProcessStudentStatistics()
    {
        Console.WriteLine(@"Please, choose option number from this list:
1) Average students' mark of every specialty.
2) Student average mark statistics (all subjects).
3) Specialty average mark statistics (all subjects).
4) Change of student's marks.");
        int option;
        while (!int.TryParse(ReadLine(), out option) || option <= 0 || option >= 5)
        {
            Console.Write("Wrong option.\r\nOption: ");
        }

        if (option == 1)
        {
            BuildAverageMarkStatistics();
        }
        else if (option == 2)
        {
            BuildStudentStatistics();
        }
        else if (option == 3)
        {
            BuilSpecialtyStatistics();
        }
        else if (option == 4)
        {
            BuildMarkChangeStatistics();
        }
    }

    public static void ProcessTeacherStatistics()
    {
        Console.WriteLine("Teacher subject distribution.");

        Dictionary<string, long> res = teacherRepo.GetTeacherSubjectDistribution();
        string graphic = GraphicsBuilder.BuildTeacherSubjectDistribution(res);
        Console.WriteLine($"Diagram was saved in this directory ({graphic}).");
    }

    private static void BuildAverageMarkStatistics()
    {
        Dictionary<string, decimal> res = studentRepo.GetSpecialtyPerformance();
        string graphic = GraphicsBuilder.BuildSpeciltyGeneralPerformance(res);
        Console.WriteLine($"Diagram was saved in this directory ({graphic}).");
    }

    private static void BuildStudentStatistics()
    {
        Write("Student fullname: ");
        string fullname = ReadLine().Trim();
        while (Controller.studentRepo.GetByFullname(fullname) == null)
        {
            Write("Invalid student fullname.\r\nFullname: ");
            fullname = ReadLine().Trim();
        }

        Dictionary<string, decimal> res = studentRepo.GetStudentStatistics(fullname);
        string graphic = GraphicsBuilder.BuildStudentPerformance(res, fullname);
        Console.WriteLine($"Diagram was saved in this directory ({graphic}).");
    }

    private static void BuilSpecialtyStatistics()
    {
        List<string> specialties = new List<string> {"Maths", "Languages", "Biology", "PE", "Science", "History", "Literature"};
        Write("Specialty: ");
        string specialty = ReadLine().Trim();
        while (!specialties.Contains(specialty))
        {
            Write("Invalid specialty.\r\nSpecialty: ");
            specialty = ReadLine().Trim();
        }

        Dictionary<string, decimal> res = studentRepo.GetSpecialtyStatistics(specialty);
        string graphic = GraphicsBuilder.BuildSpecialtyPerformance(res, specialty);
        Console.WriteLine($"Diagram was saved in this directory ({graphic}).");
    }

    private static void BuildMarkChangeStatistics()
    {
        Write("Student fullname: ");
        string fullname = ReadLine().Trim();
        while (Controller.studentRepo.GetByFullname(fullname) == null)
        {
            Write("Invalid student fullname.\r\nFullname: ");
            fullname = ReadLine().Trim();
        }

        Dictionary<DateTime, decimal> res = studentRepo.GetStudentProgressReport(fullname);
        string graphic = GraphicsBuilder.BuildStudentProgress(res, fullname);
        Console.WriteLine($"Graphic was saved in this directory ({graphic}).");
    }
}