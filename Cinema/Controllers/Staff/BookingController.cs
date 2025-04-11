using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Staff
{
    public class BookingController
    {
        public List<Movie> GetMovies()
        {
            var list = new List<Movie>();
            const string sql = @"
                SELECT MovieID, Title, Genre, Duration, Description, PosterURL
                  FROM Movie
                 WHERE Status = 'Active'
                 ORDER BY Title";
            if (!Database.OpenConnection()) return list;
            try
            {
                using (var cmd = new SqlCommand(sql, Database.con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Movie
                        {
                            MovieID = reader["MovieID"].ToString(),
                            Title = reader["Title"].ToString(),
                            Genre = reader["Genre"].ToString(),
                            Duration = Convert.ToInt32(reader["Duration"]),
                            Description = reader["Description"].ToString(),
                            PosterURL = reader["PosterURL"] as string ?? ""
                        });
                    }
                }
            }
            finally { Database.CloseConnection(); }
            return list;
        }

        public List<Theater> GetTheaters()
        {
            var list = new List<Theater>();
            const string sql = @"
                SELECT TheaterID, TheaterName, TheaterType
                  FROM Theater
                 ORDER BY TheaterName";
            if (!Database.OpenConnection()) return list;
            try
            {
                using (var cmd = new SqlCommand(sql, Database.con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Theater
                        {
                            TheaterID = reader["TheaterID"].ToString(),
                            TheaterName = reader["TheaterName"].ToString(),
                            TheaterType = reader["TheaterType"].ToString()
                        });
                    }
                }
            }
            finally { Database.CloseConnection(); }
            return list;
        }

        public List<Show> GetShowTimes(string movieID)
        {
            var list = new List<Show>();
            const string sql = @"
                SELECT s.ShowID, s.ShowTime AS ShowTimeDt, s.Price, t.TheaterID, t.TheaterName
                  FROM [Show] s
                  JOIN Theater t ON s.TheaterID = t.TheaterID
                 WHERE s.MovieID = @MovieID AND s.Status = 'Active'
                 ORDER BY s.ShowTime";
            if (!Database.OpenConnection()) return list;
            try
            {
                using (var cmd = new SqlCommand(sql, Database.con))
                {
                    cmd.Parameters.AddWithValue("@MovieID", movieID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Show
                            {
                                ShowID = reader["ShowID"].ToString(),
                                ShowTime = Convert.ToDateTime(reader["ShowTimeDt"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                TheaterID = reader["TheaterID"].ToString(),
                                TheaterName = reader["TheaterName"].ToString()
                            });
                        }
                    }
                }
            }
            finally { Database.CloseConnection(); }
            return list;
        }

        public List<SeatCategory> GetSeatCategories()
        {
            var list = new List<SeatCategory>();
            const string sql = @"
                SELECT CategoryID, CategoryName, PriceMultiplier
                  FROM SeatCategory
                 ORDER BY PriceMultiplier";
            if (!Database.OpenConnection()) return list;
            try
            {
                using (var cmd = new SqlCommand(sql, Database.con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SeatCategory
                        {
                            CategoryID = reader["CategoryID"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            PriceMultiplier = Convert.ToDecimal(reader["PriceMultiplier"])
                        });
                    }
                }
            }
            finally { Database.CloseConnection(); }
            return list;
        }

        public List<string> GetBookedSeats(string showID)
        {
            var list = new List<string>();
            const string sql = @"
                SELECT sb.SeatID
                  FROM SeatBooking sb
                  JOIN Booking b ON sb.BookingID = b.BookingID
                 WHERE b.ShowID = @ShowID AND b.Status = 'Active'";
            if (!Database.OpenConnection()) return list;
            try
            {
                using (var cmd = new SqlCommand(sql, Database.con))
                {
                    cmd.Parameters.AddWithValue("@ShowID", showID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(reader["SeatID"].ToString());
                    }
                }
            }
            finally { Database.CloseConnection(); }
            return list;
        }
    }
}
