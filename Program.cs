using static System.Console;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using ScottPlot;

namespace курсова
{
    class Program
    {
        static void Main(string[] args)
        {
            // string connStr = "server=localhost;user=root;database=studentsdb;password=210403;";
            
            // MySqlConnection conn = new MySqlConnection(connStr);
      
            // conn.Open();
            // if (conn.State == ConnectionState.Open)
            // {
            //     Controller.teacherRepo = new TeacherModel(conn);
            //     Controller.studentRepo = new StudentModel(conn);
            //     Controller.subjectRepo = new SubjectModel(conn);
            //     Controller.performanceRepo = new PerformanceModel(conn);
            //     Controller.teacherStudentRepo = new TeacherStudentModel(conn);


            //     if (!File.Exists("C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/fullnames.csv"))
            //     {
            //         GenerateFullnames();
            //         Controller.CreateData();
            //     }

                // Stopwatch watch = new Stopwatch();
                // StudentModel studentRepo = new StudentModel(conn);
                // TeacherModel teacherRepo = new TeacherModel(conn);
                // SubjectModel subjectRepo = new SubjectModel(conn);
                // PerformanceModel performanceRepo = new PerformanceModel(conn);
                // TeacherStudentModel teacherStudentRepo = new TeacherStudentModel(conn);

                // watch.Start();
                // teacherStudentRepo.SearchByStudentAndSubject("a", "a");
                // watch.Stop();
                // Console.WriteLine(watch.ElapsedMilliseconds);


                
            //     List<string> actionList = new List<string>() {"generate", "insert", "delete", "update", "search", "statistics"};
            //     List<string> tableNames = new List<string>() {"student", "teacher", "subject", "performance", "teacherStudent", }; 
            //     string[] command = new string[2];
            //     int counter = 0; 
            //     do
            //     {
            //         if (counter != 0) WriteLine("Invalid input.");
            //         if (counter == 0) 
            //         {
            //             WriteLine($"List of actions: {string.Join(", ", actionList)};");
            //             // Write("Do you want to see search options(y/n): ");
            //             // if (ReadLine() == "y") WriteLine("Search options:\n1) actors (movies, movieActors)\n2) users (reviews, movies)\n3) reviews (movies)");
            //         }


            //         WriteLine("Please, enter your command in this format: {table name} {action}");
            //         command = ReadLine().Trim().Split();
            //         counter ++; 
            //     }
            //     while(command.Length <= 1 || !actionList.Contains(command[1]) || !tableNames.Contains(command[0]));

            //     ProcessCommand(command);

            //     conn.Close();
            // }
            // else
            // {
            //     WriteLine("Error occured while connecting to database !") ; 
            // }
        }

        static void GenerateFullnames()
        {
            HtmlAgilityPack.HtmlWeb web1 = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc1 = web1.Load("https://www.thebump.com/b/most-popular-baby-names");

            HtmlAgilityPack.HtmlWeb web2 = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc2 = web1.Load("https://www.scb.se/en/finding-statistics/statistics-by-subject-area/population/general-statistics/name-statistics/pong/tables-and-graphs/all-registered-persons-in-sweden---last-names-top-100-list/last-names-top-100/");

            StreamWriter writer = new StreamWriter("C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/fullnames.csv");


            for (int i = 0; i < 500; i++)
            {
                string line = "";
                line += doc1.DocumentNode.SelectNodes("//div[@class='jsx-4181252039 name']")[i].InnerHtml.Trim();
                line = line + " " + doc2.DocumentNode.SelectNodes("//td[@class='Left']")[i % 100].InnerHtml.Trim();
                if (i == 499) writer.Write(line.Trim());
                else writer.WriteLine(line.Trim());
            }

            writer.Close();
        }

        static void ProcessCommand(string[] command)
        {
            if (command[1] == "generate")
            {
                Write("Please, input number of rows you want to generate: ");
                int rows;
                while(!int.TryParse(ReadLine(), out rows))
                {
                    WriteLine("Invalid number of rows.");
                    Write("Number of rows: ");
                }
                Controller.ProcessGenerate(command[0], rows);
            }

            else if (command[1] == "delete")
            {
                switch (command[0])
                {
                    case "student":
                        Controller.ProcessStudentDelete();
                        break;
                    case "teacher":
                        Controller.ProcessTeacherDelete();
                        break;
                    case "subject":
                        Controller.ProcessSubjectDelete();
                        break;
                    case "performance":
                        Controller.ProcessPerformanceDelete();
                        break;
                    case "teacherStudent":
                        Controller.ProcessTeacherStudentDelete();
                        break; 
                }
            }
            
            else if (command[1] == "insert")
            {
                switch (command[0])
                {
                    case "student": 
                        Controller.ProcessStudentInsert();
                        break;
                    case "teacher":
                        Controller.ProcessTeacherInsert();
                        break;
                    case "subject":
                        Controller.ProcessSubjectInsert();
                        break;
                    case "performance":
                        Controller.ProcessPerformanceInsert();
                        break;
                    case "teacherStudent":
                        Controller.ProcessTeacherStudentInsert();
                        break; 
                }
            }

            else if (command[1] == "update")
            {
                switch (command[0])
                {
                    case "student": 
                        Controller.ProcessStudentUpdate();
                        break;
                    case "teacher":
                        Controller.ProcessTeacherUpdate();
                        break;
                    case "subject":
                        Controller.ProcessSubjectUpdate();
                        break;
                    case "performance":
                        Controller.ProcessPerformanceUpdate();
                        break;
                    case "teacherStudent":
                        Controller.ProcessTeacherSubjectUpdate();
                        break;
                }
            }

            else if (command[1] == "search")
            {
                switch (command[0])
                {
                    case "student": 
                        Controller.ProcessStudentSearch();
                        break;
                    case "teacher":
                        Controller.ProcessTeacherSearch();
                        break;
                    case "subject":
                        Controller.ProcessSubjectSearch();
                        break;
                    case "performance":
                        Controller.ProcessPerformanceSearch();
                        break;
                    case "teacherStudent":
                        Controller.ProcessTeacherStudentSearch();
                        break;
                }
            }

            else if (command[1] == "statistics")
            {
                switch (command[0])
                {
                    case "student": 
                        Controller.ProcessStudentStatistics();
                        break;
                    case "teacher":
                        Controller.ProcessTeacherStatistics();
                        break;
                }
            }
        }
    }
}
