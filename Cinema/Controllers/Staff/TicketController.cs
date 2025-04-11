using System;
using System.Data;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Staff
{
    public class TicketController
    {
        public Ticket GetTicketInfo(string bookingID)
        {
            const string query = @"
SELECT 
    sb.BookingID, 
    STRING_AGG(CONCAT(s.SeatRow, s.SeatNumber), ', ') AS Seats,
    sc.CategoryName AS SeatCategory,
    m.Title AS MovieTitle, 
    sh.ShowTime, 
    t.TheaterName,
    SUM(sb.Price) AS TotalPrice
FROM SeatBooking sb
JOIN Seat s ON sb.SeatID = s.SeatID
JOIN SeatCategory sc ON s.CategoryID = sc.CategoryID
JOIN Booking b ON sb.BookingID = b.BookingID
JOIN Show sh ON b.ShowID = sh.ShowID
JOIN Movie m ON sh.MovieID = m.MovieID
JOIN Theater t ON sh.TheaterID = t.TheaterID
WHERE sb.BookingID = @BookingID
GROUP BY sb.BookingID, sc.CategoryName, m.Title, sh.ShowTime, t.TheaterName";

            if (!Database.OpenConnection())
                throw new InvalidOperationException("Cannot open database connection.");

            try
            {
                using (var cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.Add("@BookingID", SqlDbType.NVarChar, 15).Value = bookingID;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Ticket
                        {
                            BookingID = reader["BookingID"].ToString(),
                            Seats = reader["Seats"].ToString(),
                            SeatCategory = reader["SeatCategory"].ToString(),
                            MovieTitle = reader["MovieTitle"].ToString(),
                            ShowTime = Convert.ToDateTime(reader["ShowTime"]),
                            TheaterName = reader["TheaterName"].ToString(),
                            TotalPrice = reader["TotalPrice"] != DBNull.Value
                                             ? Convert.ToDecimal(reader["TotalPrice"])
                                             : 0m
                        };
                    }
                }
            }
            finally
            {
                Database.CloseConnection();
            }
        }
    }
}
