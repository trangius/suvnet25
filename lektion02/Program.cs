using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

class Teacher
{
    public string Name { set; get; }
    public string Email { set; get; }
}

class Course
{
    public string Name { set; get; }
    public float Credits { set; get; }
}

class Program
{
    static void Main()
    {
        string constring = File.ReadLines("connectionstring.txt").First();
        using IDbConnection dbcon = new MySqlConnection(constring);

        // Skapa en query med en JOIN och hämta ut data ifrån två olika tabeller.
        // Eftersom vi gör det så kan vi inte mappa mot ena (Course) eller andra (Teachers)
        // tabellen direkt...
        string query = @"select Course.Name as CourseName, Course.Credits, Teacher.Name as TeacherName
                         from Course
                         join Teacher on Course.TeacherId=Teacher.Id";

        // ...därför skapar vi en IENumerable med tuples som vardera kan innehålla tre värden:
        var coursesInfo = dbcon.Query<(string CourseName, float Credits, string TeacherName)>(query);

        // loopa igenom ienumerablen. För varje element, skapa en ny tuple och skriv ut :
        //foreach ((string CourseName, float Credits, string TeacherName) ci in coursesInfo)
        foreach (var ci in coursesInfo)
        {
            Console.WriteLine($"Kursnamn: {ci.CourseName}, Poäng {ci.Credits}, Lärare:{ci.TeacherName}");
        }
    }
}
