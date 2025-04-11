using System;
using System.Data;
using System.Data.SqlClient;

namespace Cinema
{
    public static class ShowController
    {
        public static DataTable GetMovies()
        {
            DataTable dt = new DataTable();
            if (Database.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT MovieID, Title FROM Movie", Database.con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                Database.CloseConnection();
            }
            return dt;
        }

        public static DataTable GetTheaters()
        {
            DataTable dt = new DataTable();
            if (Database.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TheaterID, TheaterName FROM Theater", Database.con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                Database.CloseConnection();
            }
            return dt;
        }

        public static DataTable GetShows()
        {
            DataTable dt = new DataTable();
            if (Database.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT s.ShowID, s.MovieID, s.TheaterID, s.ShowTime, s.Price, s.Status FROM Show s", Database.con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                Database.CloseConnection();
            }
            return dt;
        }

        public static bool UpdateShowField(string showID, string columnName, object value, out string errorMessage)
        {
            errorMessage = "";
            bool result = false;
            if (Database.OpenConnection())
            {
                string query = $"UPDATE Show SET {columnName} = @Value WHERE ShowID = @ShowID";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@Value", value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    try { result = cmd.ExecuteNonQuery() > 0; }
                    catch (Exception ex) { errorMessage = ex.Message; result = false; }
                }
                Database.CloseConnection();
            }
            else errorMessage = "Cannot open connection.";
            return result;
        }

        public static bool DeleteShow(string showID, out string errorMessage)
        {
            errorMessage = "";
            bool result = false;
            if (Database.OpenConnection())
            {
                string query = "DELETE FROM Show WHERE ShowID = @ShowID";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    try { result = cmd.ExecuteNonQuery() > 0; }
                    catch (Exception ex) { errorMessage = ex.Message; result = false; }
                }
                Database.CloseConnection();
            }
            else errorMessage = "Cannot open connection.";
            return result;
        }

        public static bool InsertShow(string showID, string movieTitle, string theaterName, DateTime showTime, decimal price, out string errorMessage)
        {
            errorMessage = "";
            bool result = false;
            string movieID = null;
            string theaterID = null;
            if (Database.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT MovieID FROM Movie WHERE Title = @Title", Database.con))
                {
                    cmd.Parameters.AddWithValue("@Title", movieTitle);
                    object obj = cmd.ExecuteScalar();
                    if (obj == null) { errorMessage = "Movie not found."; Database.CloseConnection(); return false; }
                    movieID = obj.ToString();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT TheaterID FROM Theater WHERE TheaterName = @Name", Database.con))
                {
                    cmd.Parameters.AddWithValue("@Name", theaterName);
                    object obj = cmd.ExecuteScalar();
                    if (obj == null) { errorMessage = "Theater not found."; Database.CloseConnection(); return false; }
                    theaterID = obj.ToString();
                }
                string query = "INSERT INTO Show (ShowID, MovieID, TheaterID, ShowTime, Price, Status) VALUES (@ShowID, @MovieID, @TheaterID, @ShowTime, @Price, @Status)";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                    cmd.Parameters.AddWithValue("@ShowTime", showTime);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    try { result = cmd.ExecuteNonQuery() > 0; }
                    catch (Exception ex) { errorMessage = ex.Message; result = false; }
                }
                Database.CloseConnection();
            }
            else errorMessage = "Cannot open connection.";
            return result;
        }

        public static string GenerateNewShowID()
        {
            string newID = "S001";
            if (Database.OpenConnection())
            {
                string query = "SELECT MAX(CAST(SUBSTRING(ShowID, 2, LEN(ShowID)-1) AS INT)) FROM Show";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    object obj = cmd.ExecuteScalar();
                    int next = (obj != DBNull.Value) ? Convert.ToInt32(obj) + 1 : 1;
                    newID = "S" + next.ToString("000");
                }
                Database.CloseConnection();
            }
            return newID;
        }
    }
}
