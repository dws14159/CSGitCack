using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace CSGitCack
{
    public class DataRetreiver
    {
        // populate personnel that have not been displayed in tag checkpoint (queue)
        public void populateUnacknowledgedPersonnel(System.Windows.Forms.DataGridView pobUnAcknowledgedGridView, int gateID)
        {
            String command = "SELECT name,dob,company,position,thumbnail FROM viewpersonneltagunacknowledge";
            pobUnAcknowledgedGridView.DataSource = getDataTable(command);
        }

        // populate personnel that have been displayed in tag checkpoint
        public void populateAcknowledgedPersonnel(System.Windows.Forms.DataGridView pobOnSiteGridView, int gateID)
        {
            String command = "SELECT name,dob,company,position,thumbnail FROM viewpersonneltagacknowledge ORDER BY displayedwhen DESC";
            pobOnSiteGridView.DataSource = getDataTable(command);
        }

        // retrieve personnel details that is currently displayed
        public DataTable retrieveDisplayPersonnel(String tag)
        {
            String command = "SELECT ubisensetagid,name,dob,company,position,photograph,batterystatus,description FROM viewpersonnelwithtag WHERE ubisensetagid=" + tag;
            return getDataTable(command);
        }

        // return first person in queue
        public DataTable getFirstUndisplayedTag(int gateID)
        {
            String command = "SELECT TOP 1 ubisensetagid FROM viewubisensetagchecking WHERE displayed=0 AND id_gate=" + gateID;
            return getDataTable(command);
        }

        // retrieve vehiocle details that has that tag
        public DataTable getVehicleTag(String tag)
        {
            String command = "SELECT * FROM viewvehicle WHERE ubisensetagid=" + tag;
            return getDataTable(command);
        }

        // retrieve tag details
        public DataTable getTagBatteryStatus(String tag)
        {
            String command = "SELECT batterystatus,description FROM viewtag where id=" + tag;
            return getDataTable(command);
        }

        // retrieve maximum count allowed on site
        internal int getMaximumOnSite()
        {
            String command = "SELECT maximum FROM viewsitemaximum";
            return getTotalNumber(command);
        }

        // retrieve current count on plant
        public int retrieveTotalCount()
        {
            String countCommand = "SELECT COUNT(id) FROM viewpersonnelonplant";
            return getTotalNumber(countCommand);
        }

        /* 
         * send data
         */

        // set flag that person has been displayed
        public void saveAcknowledgedPOB(String tagID)
        {
            if (tagID != "")
            {
                String saveData = "UPDATE ubisensetagchecking SET displayed=1,displayedwhen=current_timestamp WHERE ubisensetagid=" + tagID;

                // Create Connection
                SqlConnection SQLcon = createConnection();
                try
                {
                    SqlCommand sqlComand = new SqlCommand(saveData, SQLcon);
                    sqlComand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                //close connection
                closeConnection(SQLcon);
            }
        }

        /*
         * common SQL methods
         */

        // get total number
        public int getTotalNumber(String command)
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

        // retreive data table
        private DataTable getDataTable(String command)
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

        // create sql connection
        private SqlConnection createConnection()
        {
            String myConnectionString = getConnectionStringName();
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

        //close sql connection
        private void closeConnection(SqlConnection SQLcon)
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

        /*
         * Registry access methods
         */
        private String getConnectionStringName()
        {
            String connection = "";
            try
            {
                RegistryKey registryKey = Registry.LocalMachine;
                registryKey = registryKey.OpenSubKey(@"Software\IslandD");
                connection = registryKey.GetValue("Server Connection String").ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return connection;
        }

        public int getGateAppTimer()
        {
            int timer = 2000;
            try
            {
                RegistryKey registryKey = Registry.LocalMachine;
                registryKey = registryKey.OpenSubKey(@"Software\IslandD\Checkpoint");
                timer = int.Parse(registryKey.GetValue("Gate App Timer").ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return timer;
        }

        public String getResourcesFile()
        {
            String resourceDirectory = "";
            try
            {

                RegistryKey registryKey = Registry.LocalMachine;
                registryKey = registryKey.OpenSubKey(@"Software\IslandD\Checkpoint");
                resourceDirectory = registryKey.GetValue("Resources File").ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return resourceDirectory;
        }

        public String getReportDirectory()
        {
            String reportDirectory = "";
            try
            {

                RegistryKey registryKey = Registry.LocalMachine;
                registryKey = registryKey.OpenSubKey(@"Software\IslandD\Checkpoint");
                reportDirectory = registryKey.GetValue("Report Location").ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return reportDirectory;
        }
    }
}
