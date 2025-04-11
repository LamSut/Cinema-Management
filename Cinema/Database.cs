using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Database
    {
            public static SqlConnection con;
            public static bool OpenConnection()
            {
                try
                {
                    con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB; Database=CinemaDB; Integrated Security=True;");
                    con.Open();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            public static bool CloseConnection()
            {
                try
                {
                    con.Close();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
    }
}
