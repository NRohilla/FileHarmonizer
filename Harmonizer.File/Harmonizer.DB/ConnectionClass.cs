using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace Harmonizer.DB
{
    public class ConnectionClass
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();

        //public  void OpenConnection()
        //{
        //    SqlConnection con = new SqlConnection();
        //    if (con.State == ConnectionState.Open)
        //        con.Close();
        //    else
        //        con.Open();
        //}

        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection(constr);
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            return conn;
        }

        public static void closeconnection(SqlConnection conn)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
    }
}
