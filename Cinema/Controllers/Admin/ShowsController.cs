using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Admin
{
    public static class ShowController
    {
        public static List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            if (Database.OpenConnection())
            {
                string query = "SELECT MovieID, Title FROM Movie";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        movies.Add(new Movie
                        {
                            MovieID = row["MovieID"].ToString(),
                            Title = row["Title"].ToString()
                        });
                    }
                }
                Database.CloseConnection();
            }
            return movies;
        }

        public static List<Theater> GetTheaters()
        {
            List<Theater> theaters = new List<Theater>();
            if (Database.OpenConnection())
            {
                string query = "SELECT TheaterID, TheaterName FROM Theater";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        theaters.Add(new Theater
                        {
                            TheaterID = row["TheaterID"].ToString(),
                            TheaterName = row["TheaterName"].ToString()
                        });
                    }
                }
                Database.CloseConnection();
            }
            return theaters;
        }

        public static List<Show> GetShows()
        {
            List<Show> shows = new List<Show>();
            if (Database.OpenConnection())
            {
                string query = "SELECT ShowID, MovieID, TheaterID, ShowTime, Price, Status FROM Show";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        shows.Add(new Show
                        {
                            ShowID = row["ShowID"].ToString(),
                            MovieID = row["MovieID"].ToString(),
                            TheaterID = row["TheaterID"].ToString(),
                            ShowTime = Convert.ToDateTime(row["ShowTime"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Status = row["Status"].ToString()
                        });
                    }
                }
                Database.CloseConnection();
            }
            return shows;
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
                errorMessage = "Cannot open connection.";
            }
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
                errorMessage = "Cannot open connection.";
            }
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
                    if (obj == null)
                    {
                        errorMessage = "Movie not found.";
                        Database.CloseConnection();
                        return false;
                    }
                    movieID = obj.ToString();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT TheaterID FROM Theater WHERE TheaterName = @Name", Database.con))
                {
                    cmd.Parameters.AddWithValue("@Name", theaterName);
                    object obj = cmd.ExecuteScalar();
                    if (obj == null)
                    {
                        errorMessage = "Theater not found.";
                        Database.CloseConnection();
                        return false;
                    }
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
                errorMessage = "Cannot open connection.";
            }
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
