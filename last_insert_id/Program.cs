using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

class Course
{
    public string Id { set; get; }
    public string Name { set; get; }
}

class Program
{
    static void Main()
    {
        // skapa en connectionstring mot min databas:
        string constring = File.ReadLines("connectionstring.txt").First();

        // skapa ett IDBConnection-objekt (dbcon) av MySQL-typ:
        using IDbConnection dbcon = new MySqlConnection(constring);

        // Samla data för en ny student:
        Console.Write("Ange studentens namn: ");
        string studentName = Console.ReadLine();
        Console.Write("Ange studentens email: ");
        string studentEmail = Console.ReadLine();
        Console.Write("Ange studentens födelsedatum (åååå-mm-dd): ");
        string studentBirthdate = Console.ReadLine();
        // Lägg till studenten i databasen:
        string insertStudentQuery = $"insert intoo Student (Name, Email, DateOfBirth) values ('{studentName}', '{studentEmail}', '{studentBirthdate}'); SELECT LAST_INSERT_ID();";
        // Använd ExecuteScalar för att få tillbaka den nya studentens id:
        int studentId = dbcon.ExecuteScalar<int>(insertStudentQuery);
        Console.WriteLine($"Student med id {studentId} har lagts till.");

        string query = "select * from Course";
        IEnumerable<Course> courses = dbcon.Query<Course>(query);

        // Låt användaren välja kurser att lägga till för studenten:
        while(true)
        {
            // Skriv ut alla kurser och deras Id:
            foreach (Course c in courses)
                Console.WriteLine($"Kurs: {c.Name}, Id: {c.Id}");

            // läs in vilken kurs som ska läggas till (till studenten):
            Console.Write("Ange kursens id (avsluta med enter): ");
            string courseId = Console.ReadLine();
            if (courseId == "")
                break;
                
            // Lägg till kursen för studenten i tabellen Enrollment:
            string insertQuery = $"insert into Enrollment (StudentId, CourseId, EnrollmentDate) values ({studentId}, {courseId}, now())";
            int rowsAffected = dbcon.Execute(insertQuery);

            Console.WriteLine($"{rowsAffected} rad(er) påverkade.");
        }

    }
}