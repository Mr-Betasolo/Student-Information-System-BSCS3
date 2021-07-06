using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace StudentInfromationSystem
{
    class DBConnect
    {
        // to create connection
        MySqlConnection connect = new MySqlConnection("server=localhost;;database=db_login;uid=root;password=Password;");

        // to get connection
        public MySqlConnection getconnection
        {
            get {
                return connect;
            }
        }

        // create a function to open connection
        public void openConnect() 
        {
            if (connect.State == System.Data.ConnectionState.Closed)
                connect.Open();
        }

        //create a function to close connection
        public void closeConnect()
        {
            if (connect.State == System.Data.ConnectionState.Open)
                connect.Close();
        }


    }

}
