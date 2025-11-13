using System.Data;
using MySql.Data.MySqlClient;

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
        
        // skapa ett connection-objekt av MySQL-typ:
        using MySqlConnection dbcon = new MySqlConnection(constring);
        
        // öppna anslutningen:
        dbcon.Open();
        
        // skapa ett command-objekt:
        string query = "select Name, Email from Teacher;";
        Console.WriteLine(query);
        MySqlCommand cmd = new MySqlCommand(query, dbcon);
        
        // skapa en reader för att läsa resultatet:
        MySqlDataReader reader = cmd.ExecuteReader();
        
        // skapa en lista för teachers:
        List<Teacher> teachers = new List<Teacher>();
        
        // läs rad för rad:
        while (reader.Read())
        {
            Teacher t = new Teacher();
            t.Name = reader["Name"].ToString();
            t.Email = reader["Email"].ToString();
            teachers.Add(t);
        }
        
        // stäng reader:
        reader.Close();
        
        // loopa igenom teachers-listan och skriv ut:
        foreach (Teacher t in teachers)
        {
            Console.WriteLine($"Hello from {t.Name}, you can reach me at {t.Email}");
        }
    }
}