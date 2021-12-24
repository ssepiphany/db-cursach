using static System.Console;
using System.Collections.Generic;

static class View
{
    public static void DisplayStudentSearch(List<List<string>> table)
    {
        List<string> attrs = new List<string> {"Student id", "Student fullname", "Student age", "Student specialty", "Tutor id", "Tutor fullname"};
        int counter = 0; 
        for (int i = 0; i <= table.Count; i++)
        {
            if (counter == 0) 
            {
                WriteLine($"{attrs[0], 10} | {attrs[1], 20} | {attrs[2], 12} | {attrs[3], 20} | {attrs[4], 10} | {attrs[5], 20} |");
                counter++;
            }
            else
            {
                WriteLine($"{table[i-1][0], 10} | {table[i-1][1], 20} | {table[i-1][2], 12} | {table[i-1][3], 20} | {table[i-1][4], 10} | {table[i-1][5], 20} |");
            }
            WriteLine("-------------------------------------------------------------------------------------------------------------");
        }
    }

    public static void DisplayTeacherSearch(List<List<string>> table, bool extended)
    {
        List<string> attrs = new List<string> {"Teacher fullname", "Experince", "Subject title", "Exam date"};
        int counter = 0; 
        for (int i = 0; i <= table.Count; i++)
        {
            if (counter == 0) 
            {
                if (!extended) 
                {
                    WriteLine($"{attrs[0], 20} | {attrs[1], 14} | {attrs[2], 16} |");
                    WriteLine("----------------------------------------------------------");
                }
                else 
                {
                    WriteLine($"{attrs[0], 20} | {attrs[1], 14} | {attrs[2], 16} | {attrs[3], 20} |");
                    WriteLine("---------------------------------------------------------------------------------");
                }
                    
                counter++;
            }
            else
            {
                if (!extended)
                {
                    WriteLine($"{table[i-1][0], 20} | {table[i-1][1] + " months", 14} | {table[i-1][2], 16} |");
                    WriteLine("----------------------------------------------------------");
                }
                else
                {
                    WriteLine($"{table[i-1][0], 20} | {table[i-1][1] + " months", 14} | {table[i-1][2], 16} | {table[i-1][3], 20} |");
                    WriteLine("---------------------------------------------------------------------------------");
                }
            }
        }
    }


    public static void DisplaySubjectSearch(List<List<string>> table)
    {
        List<string> attrs = new List<string> {"Subject id", "Subject title", "Exam date"};
        int counter = 0; 
        for (int i = 0; i <= table.Count; i++)
        {
            if (counter == 0) 
            {
                WriteLine($"{attrs[0], 10} | {attrs[1], 20} | {attrs[2], 20} |");
                counter++;
            }
            else
            {
                WriteLine($"{table[i-1][0], 10} | {table[i-1][1], 20} | {table[i-1][2], 20} |");
            }
            WriteLine("----------------------------------------------------------");
        }
    }

    public static void DisplayPerformanceSearch(List<List<string>> table)
    {
        List<string> attrs = new List<string> {"Student fullname", "Subject title", "Mark", "Date"};
        int counter = 0; 
        for (int i = 0; i <= table.Count; i++)
        {
            if (counter == 0) 
            {
                WriteLine($"{attrs[0], 20} | {attrs[1], 20} | {attrs[2], 10} | {attrs[3], 20} |");
                counter++;
            }
            else
            {
                WriteLine($"{table[i-1][0], 20} | {table[i-1][1], 20} | {table[i-1][2], 10} | {table[i-1][3], 20} |");
            }
            WriteLine("---------------------------------------------------------------------------------");
        }
    }

    public static void DisplayTeacherStudentSearch(List<List<string>> table)
    {
        List<string> attrs = new List<string> {"Student fullname", "Teacher fullname", "Subject title"};
        int counter = 0; 
        for (int i = 0; i <= table.Count; i++)
        {
            if (counter == 0) 
            {
                WriteLine($"{attrs[0], 20} | {attrs[1], 20} | {attrs[2], 20} |");
                counter++;
            }
            else
            {
                WriteLine($"{table[i-1][0], 20} | {table[i-1][1], 20} | {table[i-1][2], 20} |");
            }
            WriteLine("--------------------------------------------------------------------");
        }
    }

    public static void DisplayMessage(string message)
    {
        WriteLine(message);
    }
}