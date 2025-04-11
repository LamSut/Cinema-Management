using System;
using System.Data;
using System.Data.SqlClient;
using Cinema.Models;

namespace Cinema.Controllers.Auth
{
    public static class AuthController
    {
        public static Employee Authenticate(string empID, string password)
        {
            Employee employee = null;
            if (Database.OpenConnection())
            {
                string query = "SELECT EmployeeID, Role FROM Employee WHERE EmployeeID = @EmployeeID AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 50).Value = empID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeID = reader["EmployeeID"].ToString(),
                                Role = Convert.ToInt32(reader["Role"])
                            };
                        }
                    }
                }
                Database.CloseConnection();
            }
            return employee;
        }
    }
}
