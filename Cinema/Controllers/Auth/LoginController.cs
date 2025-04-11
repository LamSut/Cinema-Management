using System;
using System.Data;
using System.Data.SqlClient;

namespace Cinema.Controllers.Auth
{
    public static class AuthController
    {
        public static int GetUserRole(string empID, string password)
        {
            int role = -1;
            if (Database.OpenConnection())
            {
                string query = "SELECT Role FROM Employee WHERE EmployeeID = @EmployeeID AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, Database.con))
                {
                    cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar, 50).Value = empID;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;
                    object result = cmd.ExecuteScalar();
                    role = result != null ? Convert.ToInt32(result) : -1;
                }
                Database.CloseConnection();
            }
            return role;
        }
    }
}
