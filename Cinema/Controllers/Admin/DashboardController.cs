using System;
using Cinema.Views;
using System.Data.SqlClient;

namespace Cinema.Controllers.Admin
{
    public class DashboardController
    {
        private readonly string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=CinemaDB;Trusted_Connection=True;";

        public int GetTotalCustomers()
        {
            const string sql = "SELECT COUNT(CustomerID) FROM Customer";
            return ExecuteScalarInt(sql);
        }

        public int GetTotalBookings()
        {
            const string sql = "SELECT COUNT(BookingID) FROM Booking";
            return ExecuteScalarInt(sql);
        }

        public decimal GetTotalRevenue()
        {
            const string sql =
                "SELECT ISNULL(SUM(TotalAmount), 0) FROM Booking WHERE Status != 'Cancelled'";
            return ExecuteScalarDecimal(sql);
        }

        private int ExecuteScalarInt(string sql)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
            }
        }

        private decimal ExecuteScalarDecimal(string sql)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result == DBNull.Value) ? 0m : Convert.ToDecimal(result);
            }
        }
    }
}
