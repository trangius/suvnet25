using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

class Teacher
{
    public string Name { set; get; }
    public string Email { set; get; }
}

class Program
{
    static void Main()
    {
        // skapa en connectionstring mot min databas:
        string constring = File.ReadLines("connectionstring.txt").First();

        // skapa ett IDBConnection-objekt (dbcon) av MySQL-typ:
        using IDbConnection dbcon = new MySqlConnection(constring);

        // lägg in en lärare:
        //dbcon.Execute("INSERT INTO Teacher (Name, Email) VALUES ('Janne Långben', 'janne@suvnet.se');");

        // skapa en lista med teachers, fyll på den med data från ett result set
        // genom att anropa dapper-metoden Query():
        string query = "select Name, Email from Teacher;";
        Console.WriteLine(query);
        IEnumerable<Teacher> teachers = dbcon.Query<Teacher>(query);

        // loopa igenom teachers-ienumerablen (typ lista) och skriv ut:
        foreach (Teacher t in teachers)
        {
            Console.WriteLine($"Hello from {t.Name}, you can reach me at {t.Email}");
        }
    }
}