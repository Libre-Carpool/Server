using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class FailsDatabaseAccessor : DatabaseAccessor
{
    public bool isBlocked(string ip)
    {
        SqlConnection connection = null;
        SqlDataReader reader = null;
        try
        {
            connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Fails WHERE (Ip=@IP AND TheTime > DATEADD(hour, -1, GETDATE()))";
            command.Parameters.AddWithValue("@IP", ip);
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                Int32 count = 0;
                if(reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                if(count > 5)
                {
                    return true;
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
        return false;
    }

    public void addFail(string ip)
    {
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Fails (Ip, TheTime) VALUES (@IP, GETDATE())";
            command.Parameters.AddWithValue("@IP", ip);
            command.ExecuteNonQuery();
            // TODO: Delete expired fails
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