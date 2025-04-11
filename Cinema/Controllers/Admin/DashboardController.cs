using System;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Admin
{
    public class DashboardController
    {
        public Dashboard GetDashboardData()
        {
            Dashboard data = new Dashboard();

            if (Database.OpenConnection())
            {
                data.TotalCustomers = ExecuteScalarInt("SELECT COUNT(CustomerID) FROM Customer");
                data.TotalBookings = ExecuteScalarInt("SELECT COUNT(BookingID) FROM Booking");
                data.TotalRevenue = ExecuteScalarDecimal("SELECT ISNULL(SUM(TotalAmount), 0) FROM Booking WHERE Status != 'Cancelled'");
                Database.CloseConnection();
            }

            return data;
        }

        private int ExecuteScalarInt(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, Database.con))
            {
                object result = cmd.ExecuteScalar();
                return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
            }
        }

        private decimal ExecuteScalarDecimal(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, Database.con))
            {
                object result = cmd.ExecuteScalar();
                return (result == DBNull.Value) ? 0m : Convert.ToDecimal(result);
            }
        }
    }
}
