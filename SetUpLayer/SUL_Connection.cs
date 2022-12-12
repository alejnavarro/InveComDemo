using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetUpLayer
{
    public class SUL_Connection
    {
        private readonly SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial catalog=InveCom; Integrated security=true;");
        //private readonly SqlConnection con = new SqlConnection("Network Address=192.168.0.102 ; Initial catalog=InveCom; Integrated security=false; Password=InVeCoMsErVeR@; User ID=SA");


        public SqlConnection Connect()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
                return con;
            }
            return con;

        }

        public SqlConnection CloseConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                return con;
            }
            return con;

        }
    }
}
