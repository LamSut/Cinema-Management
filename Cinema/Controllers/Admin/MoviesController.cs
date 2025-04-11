using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Admin
{
    public static class MovieController
    {
        public static List<Movie> GetAllMoviesList()
        {
            List<Movie> movies = new List<Movie>();

            if (Database.OpenConnection())
            {
                string query = "SELECT MovieID, Title, Genre, Duration, Description, Director, ReleaseDate, PosterURL, Status FROM Movie";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            movies.Add(new Movie
                            {
                                MovieID = reader["MovieID"].ToString(),
                                Title = reader["Title"].ToString(),
                                Genre = reader["Genre"].ToString(),
                                Duration = Convert.ToInt32(reader["Duration"]),
                                Description = reader["Description"].ToString(),
                                Director = reader["Director"].ToString(),
                                ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                PosterURL = reader["PosterURL"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
                Database.CloseConnection();
            }

            return movies;
        }

        public static bool AddMovie(Movie movie, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool result = false;

            if (Database.OpenConnection())
            {
                string query = "INSERT INTO Movie (MovieID, Title, Genre, Duration, Description, Director, ReleaseDate, PosterURL) " +
                               "VALUES (@MovieID, @Title, @Genre, @Duration, @Description, @Director, @ReleaseDate, @PosterURL)";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.AddWithValue("@MovieID", movie.MovieID);
                    cmd.Parameters.AddWithValue("@Title", movie.Title);
                    cmd.Parameters.AddWithValue("@Genre", movie.Genre);
                    cmd.Parameters.AddWithValue("@Duration", movie.Duration);
                    cmd.Parameters.AddWithValue("@Description", movie.Description);
                    cmd.Parameters.AddWithValue("@Director", movie.Director);
                    cmd.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
                    cmd.Parameters.AddWithValue("@PosterURL", movie.PosterURL);
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
