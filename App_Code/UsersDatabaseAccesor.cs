using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class UsersDatabaseAccessor : DatabaseAccessor
{
    private string username;
    private string password;
    private bool loggedIn;
    private bool canAdd;
    private bool canDelete;
    private bool canModify;

    public struct Entry
    {
        public string phone;
        public bool canAdd;
        public bool canMod;
        public bool canDel;
    }

    public bool isLoggedIn()
    {
        return loggedIn;
    }
    public bool CanAdd()
    {
        return canAdd;
    }
    public bool CanDelete()
    {
        return canDelete;
    }
    public bool CanModify()
    {
        return canModify;
    }

    public static bool login(string username, string password)
    {
        if (Validation.isValidPhoneNumber(username) && Validation.isValidPassword(password))
        {
            bool isSuccess = false;
            FailsDatabaseAccessor fails = new FailsDatabaseAccessor();

            if(fails.isBlocked(HttpContext.Current.Request.UserHostAddress) == false)
            {
                SqlConnection connection = null;
                SqlDataReader reader = null;
                try
                {
                    connection = new SqlConnection(CONNECTION_STRING);
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT salt FROM Users WHERE phone=@UName";
                    command.Parameters.AddWithValue("@UName", username);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string salt = reader.Read().ToString();

                        reader.Close();
                        command = connection.CreateCommand();
                        command.CommandText = "SELECT * FROM Users WHERE phone=@UName AND password=HASHBYTES('SHA2_256', @PWord)";
                        command.Parameters.AddWithValue("@UName", username);
                        command.Parameters.AddWithValue("@PWord", password); // TODO: Salt
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            isSuccess = true;
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
            if(isSuccess == false)
            {
                fails.addFail(HttpContext.Current.Request.UserHostAddress);
            }
            return isSuccess;
        }
        else
        {
            return false;
        }
    }

    public List<Entry> getAll()
    {
        if(loggedIn)
        {
            List<Entry> entries = new List<Entry>();
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT phone, canadd, canmod, candel FROM Users";
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Entry entry;
                        entry.phone = (string)reader.GetValue(0);
                        entry.canAdd = reader.GetBoolean(1);
                        entry.canMod = reader.GetBoolean(2);
                        entry.canDel = reader.GetBoolean(3);
                        entries.Add(entry);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return entries;
        }
        else
        {
            return null;
        }
    }

    public void deleteUser(string username)
    {
        if(canDelete && Validation.isValidPhoneNumber(username))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }

    public void addUser(string username, string password)
    {
        if(canAdd && Validation.isValidPhoneNumber(username) && Validation.isValidPassword(password))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();
                
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.HasRows == false)
                    {
                        reader.Close();
                        //reader = null;

                        string salt = "123456"; // TODO: Create salt
                        
                        command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO Users (phone, password, salt, canadd, candel, canmod) VALUES(@UName, HASHBYTES('SHA2_256', @PWord), @Salt, '0', '0', '0')";
                        command.Parameters.AddWithValue("@UName", username);
                        command.Parameters.AddWithValue("@PWord", password);
                        command.Parameters.AddWithValue("@Salt", salt);
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if(reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }

    public void switchAdd(string username)
    {
        if (canModify && Validation.isValidPhoneNumber(username))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT canadd FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        bool canAdd = false;
                        while (reader.Read())
                        {
                            canAdd = reader.GetBoolean(0);
                        }
                        reader.Close();

                        canAdd = !canAdd;

                        char canAddChar;
                        if(canAdd)
                        {
                            canAddChar = '1';
                        }
                        else
                        {
                            canAddChar = '0';
                        }

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE Users SET canadd='" + canAddChar + "' WHERE phone=@UName";
                        command.Parameters.AddWithValue("@UName", username);
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if(reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }

    public void switchDelete(string username)
    {
        if (canModify && Validation.isValidPhoneNumber(username))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT candel FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        bool canDel = false;
                        while (reader.Read())
                        {
                            canDel = reader.GetBoolean(0);
                        }
                        reader.Close();

                        canDel = !canDel;

                        char canDelChar;
                        if (canDel)
                        {
                            canDelChar = '1';
                        }
                        else
                        {
                            canDelChar = '0';
                        }

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE Users SET candel='" + canDelChar + "' WHERE phone=@UName";
                        command.Parameters.AddWithValue("@UName", username);
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }

    public void switchModify(string username)
    {
        if (canModify && Validation.isValidPhoneNumber(username))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT canmod FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        bool canMod = false;
                        while (reader.Read())
                        {
                            canMod = reader.GetBoolean(0);
                        }
                        reader.Close();

                        canMod = !canMod;

                        char canModChar;
                        if (canMod)
                        {
                            canModChar = '1';
                        }
                        else
                        {
                            canModChar = '0';
                        }

                        command = connection.CreateCommand();
                        command.CommandText = "UPDATE Users SET canmod='" + canModChar + "' WHERE phone=@UName";
                        command.Parameters.AddWithValue("@UName", username);
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            finally
            {
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }

    public UsersDatabaseAccessor(string username, string password)
    {
        if (Validation.isValidPhoneNumber(username) && Validation.isValidPassword(password))
        {
            this.username = username;
            this.password = password;
            this.loggedIn = login(username, password);

            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT canadd, candel, canmod FROM Users WHERE phone=@UName";
                command.Parameters.AddWithValue("@UName", username);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        this.canAdd = reader.GetBoolean(0);
                        this.canDelete = reader.GetBoolean(1);
                        this.canModify = reader.GetBoolean(2);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
        else
        {
            throw (new ArgumentException("Username or Password is invalid"));
        }
    }
}