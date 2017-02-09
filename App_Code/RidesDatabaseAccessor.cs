using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

public class RidesDatabaseAccessor : DatabaseAccessor
{
    public class Filters
    {
        public string destinationID;
        public string departureID;
        public DateTime time;

        public Filters()
        {
            destinationID = "";
            departureID = "";
            time = default(DateTime);
        }
    }
    private Filters filters = new Filters();

    public void setFilters(Filters filters)
    {
        if(filters.destinationID != "")
        {
            if (Validation.isValidPlacesID(filters.destinationID) == false)
                return;
        }
        if(filters.departureID != "")
        {
            if (Validation.isValidPlacesID(filters.departureID) == false)
                return;
        }

        this.filters = filters;
    }

    public struct Entry
    {
        public string phone;
        public string destinationID;
        public string departureID;
        public DateTime time;
        public string through;
        public string comment;
    }
    
    public List<Entry> getFiltered()
    {
        List<Entry> entries = new List<Entry>();
        SqlConnection connection = null;
        SqlDataReader reader = null;
        try
        {
            connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT phone, destination, departure, time, through, comment FROM Rides";
            if(filters.departureID != "" || filters.destinationID != "" || filters.time != default(DateTime))
            {
                command.CommandText += " WHERE ";
                if (filters.departureID != "")
                {
                    command.CommandText += "departure=@DepartID ";
                    command.Parameters.AddWithValue("@DepartID", filters.departureID);
                }
                if (filters.destinationID != "")
                {
                    if (filters.departureID != "")
                    {
                        command.CommandText += " AND ";
                    }
                    command.CommandText += "destination=@DestID ";
                    command.Parameters.AddWithValue("@DestID", filters.destinationID);
                }
                if (filters.time != default(DateTime))
                {
                    if (filters.departureID != "" || filters.destinationID != "")
                    {
                        command.CommandText += " AND ";
                    }
                    if (filters.time.TimeOfDay != default(TimeSpan))
                    {
                        command.CommandText += "(time > DATEADD(hour, -3, @Time) AND Time < DATEADD(hour, 3, @Time))";
                    }
                    else
                    {
                        command.CommandText += "(time > @Time AND Time < DATEADD(hour, 24, @Time))";
                    }
                    command.Parameters.AddWithValue("@Time", filters.time);
                }
            }
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Entry entry = new Entry();
                    entry.phone = (string)reader.GetValue(0);
                    entry.destinationID = (string)reader.GetValue(1);
                    entry.departureID = (string)reader.GetValue(2);
                    entry.time = reader.GetDateTime(3);
                    entry.through = reader.GetString(4);
                    entry.comment = reader.GetString(5);
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

    public void updateEntry(Entry entry)
    {
        if(Validation.isValidPlacesID(entry.departureID) && Validation.isValidPlacesID(entry.destinationID) && (Validation.isValidComment(entry.comment) || entry.comment == "") && Validation.isValidPhoneNumber(entry.phone))
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
@"UPDATE Rides SET destination=@Dest, departure=@Depart, time=@Time, through=@Through, comment=@Comment
WHERE phone=@UName
IF (@@ROWCOUNT=0)
    INSERT INTO Rides (phone, destination, departure, time, through, comment)
    VALUES (@Uname, @Dest, @Depart, @Time, @Through, @Comment)";
                command.Parameters.AddWithValue("@UName", entry.phone);
                command.Parameters.AddWithValue("@Dest", entry.destinationID);
                command.Parameters.AddWithValue("@Depart", entry.departureID);
                command.Parameters.AddWithValue("@Time", entry.time);
                command.Parameters.AddWithValue("@Through", entry.through);
                command.Parameters.AddWithValue("@Comment", entry.comment);
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
}