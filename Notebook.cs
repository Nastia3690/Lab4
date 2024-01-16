using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Text.Json;

public class Contact
{
    [JsonInclude]
    public string Name { get; set; }
    [JsonInclude]
    public string Surname { get; set; }
    [JsonInclude]
    public string Phone { get; set; }
    [JsonInclude]
    public string Email { get; set; }

    public Contact(string Name, string Surname, string Phone, string Email)
    {
        this.Name = Name;
        this.Surname = Surname;
        this.Phone = Phone;
        this.Email = Email;
    }
    public Contact() { }

    public override string ToString()
    {
        return this.Name + " " + this.Surname + " " + this.Phone + " " + this.Email;
    }
}
public class Notebook
{
    private static List<Contact> Contacts = new List<Contact>();
    private static SQLiteConnection m_dbConn;
    private static SQLiteCommand m_sqlCmd;

    public bool SaveToXML()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Contact>));
            StreamWriter writer = new StreamWriter("notebook.xml");
            serializer.Serialize(writer, Contacts);
            writer.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool LoadFromXML()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Contact>));
            TextReader reader = new StreamReader("notebook.xml");
            Contacts = (List<Contact>)serializer.Deserialize(reader);
            reader.Close();
            return (Contacts.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    public bool SaveToJSON()
    {
        try
        {
            string json = JsonSerializer.Serialize(Contacts);
            File.WriteAllText("notebook.json", json);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool LoadFromJSON()
    {
        try
        {
            string data = File.ReadAllText("notebook.json");
            Contacts = JsonSerializer.Deserialize<List<Contact>>(data);
            return (Contacts.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    private static bool Create_SQLite()
    {
        if (!File.Exists("notebook.sqlite"))
        {
            SQLiteConnection.CreateFile("notebook.sqlite");
        }

        try
        {
            m_dbConn = new SQLiteConnection("Data Source=notebook.sqlite;Version=3;");
            m_dbConn.Open();
            m_sqlCmd = m_dbConn.CreateCommand();
            m_sqlCmd.Connection = m_dbConn;

            m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS notebook (id integer primary key autoincrement, name varchar(100), surname varchar(100), phone varchar(100), email varchar(100) )";
            m_sqlCmd.ExecuteNonQuery();
            m_sqlCmd.CommandText = "DELETE FROM notebook";
            m_sqlCmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool Connect_SQLite()
    {
        if (!File.Exists("notebook.sqlite"))
        {
            return false;
        }
        try
        {
            m_dbConn = new SQLiteConnection("Data Source=notebook.sqlite;Version=3;");
            m_dbConn.Open();
            m_sqlCmd = m_dbConn.CreateCommand();
            m_sqlCmd.Connection = m_dbConn;
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static bool Read_SQLite()
    {
        DataTable table = new DataTable();
        String sqlQuery;

        if (m_dbConn.State != ConnectionState.Open)
        {
            return false;
        }
        Contacts.Clear();
        try
        {
            sqlQuery = "SELECT * FROM notebook";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Contacts.Add(new Contact(Convert.ToString(row["name"]),Convert.ToString(row["surname"]),Convert.ToString(row["phone"]),Convert.ToString(row["email"])));
                }
            }
            else
            {
                return false;
            }
            return (Contacts.Count > 0);
        }
        catch
        {
            return false;
        }
    }

    private static bool Write_SQLite()
    {
        if (m_dbConn.State != ConnectionState.Open)
        {
            return false;
        }
        try
        {
            foreach (Contact contact in Contacts)
            {
                m_sqlCmd.CommandText = "INSERT INTO notebook ('name','surname','phone','email') values ('" + contact.Name + "','" + contact.Surname + "','" + contact.Phone + "','" + contact.Email + "')";
                m_sqlCmd.ExecuteNonQuery();
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool SaveToSQLite()
    {
        if (Create_SQLite())
        {
            return Write_SQLite();
        }
        else
        {
            return false;
        }
    }
    public bool LoadFromSQLite()
    {
        if (Connect_SQLite())
        {
            return Read_SQLite();
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        Contacts.Clear();
    }

    public int Count()
    {
        return Contacts.Count();
    }

    public bool UpdateContact(int index, String value)
    {
        String[] fields = value.Split(' ');
        if (fields.Length == 4)
        {
            Contacts[index].Name = fields[0];
            Contacts[index].Surname = fields[1];
            Contacts[index].Phone = fields[2];
            Contacts[index].Email = fields[3];
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveContact(int index)
    {
        if (index < Contacts.Count)
        {
            Contacts.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Add(string Name, string Surname, string Phone, string Email){
        if (Name.Length == 0 || Surname.Length == 0)
        {
            return false;
        }
        Contacts.Add(new Contact(Name, Surname, Phone, Email));
        return true;
    }

    public List<Contact> Search(Predicate<Contact> predicate)
    {
        return Contacts.FindAll(predicate);
    }

    public List<Contact> AllRecords
    {
        get { return Contacts; }
    }


}
