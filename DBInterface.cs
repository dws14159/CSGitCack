using System;
using System.Data;
using System.Data.SqlClient;

namespace CSGitCack
{
    public class DBInterface
    {
        public static SqlConnection createConnection()
        {
            String myConnectionString =
                "Data Source=devsql01\\sitedb;Initial Catalog=ATS;User Id=dev; Password=Automation15;Connect Timeout=5;Connection Lifetime=30;Pooling=false";
            SqlConnection SQLcon = new SqlConnection(myConnectionString);
            try
            {
                SQLcon.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            return SQLcon;
        }

        public static void closeConnection(SqlConnection SQLcon)
        {
            try
            {
                SQLcon.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static int getTotalNumber(String command)
        {
            // Create Connection
            SqlConnection SQLcon = createConnection();
            int returnValue = 0;

            try
            {
                if (SQLcon.State == ConnectionState.Open)
                {
                    // retrieve total number of personnel on board
                    SqlCommand pobCommand = new SqlCommand();
                    pobCommand.CommandText = command;
                    pobCommand.Connection = SQLcon;
                    returnValue = int.Parse(pobCommand.ExecuteScalar().ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            //close connection
            closeConnection(SQLcon);

            return returnValue;
        }

        public static DataTable getDataTable(String command)
        {
            DataTable dtable = new DataTable();
            // Create Connection
            SqlConnection SQLcon = createConnection();

            try
            {
                //retrieve dataTable
                SqlDataAdapter executeCommand = new SqlDataAdapter(command, SQLcon);
                executeCommand.Fill(dtable);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            //close connection
            closeConnection(SQLcon);

            return dtable;
        }
    }
}
