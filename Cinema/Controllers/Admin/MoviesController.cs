using System;
using System.Data;
using System.Data.SqlClient;

namespace Cinema.Controllers.Admin
{
    public static class MovieController
    {
        public static DataTable GetAllMovies()
        {
            DataTable dt = new DataTable();

            if (Database.OpenConnection())
            {
                string query = "SELECT MovieID, Title, Genre, Duration, Director, ReleaseDate, PosterURL, Status FROM Movie";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                Database.CloseConnection();
            }

            return dt;
        }

        public static bool AddMovie(string movieID, string title, string genre, int duration, string description, string director, DateTime releaseDate, string posterURL, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool result = false;

            if (Database.OpenConnection())
            {
                string query = "INSERT INTO Movie (MovieID, Title, Genre, Duration, Description, Director, ReleaseDate, PosterURL) " +
                               "VALUES (@MovieID, @Title, @Genre, @Duration, @Description, @Director, @ReleaseDate, @PosterURL)";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Genre", genre);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Director", director);
                    cmd.Parameters.AddWithValue("@ReleaseDate", releaseDate);
                    cmd.Parameters.AddWithValue("@PosterURL", posterURL);
                    try
                    {
                        result = cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                    {
                        errorMessage = "A movie with this title already exists. Please choose a different title.";
                        result = false;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        result = false;
                    }
                }
                Database.CloseConnection();
            }
            else
            {
                errorMessage = "Cannot open database connection.";
            }
            return result;
        }

        public static bool DeleteMovie(string movieId, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool result = false;

            if (Database.OpenConnection())
            {
                string query = "DELETE FROM Movie WHERE MovieID = @MovieID";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@MovieID", movieId);
                    try
                    {
                        result = cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        result = false;
                    }
                }
                Database.CloseConnection();
            }
            else
            {
                errorMessage = "Cannot open database connection.";
            }
            return result;
        }

        public static bool UpdateMovieField(string movieID, string columnName, object value, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool result = false;

            if (Database.OpenConnection())
            {
                string query = $"UPDATE Movie SET {columnName} = @Value WHERE MovieID = @MovieID";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@Value", value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    try
                    {
                        result = cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        result = false;
                    }
                }
                Database.CloseConnection();
            }
            else
            {
                errorMessage = "Cannot open database connection.";
            }
            return result;
        }

        public static string GenerateNewMovieID()
        {
            string newID = "M001";
            if (Database.OpenConnection())
            {
                string query = "SELECT MAX(CAST(SUBSTRING(MovieID, 2, LEN(MovieID)-1) AS INT)) FROM Movie";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    object result = cmd.ExecuteScalar();
                    int next = (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 1;
                    newID = "M" + next.ToString("000");
                }
                Database.CloseConnection();
            }
            return newID;
        }
    }
}
