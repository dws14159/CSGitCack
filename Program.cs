using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Printing;
using System.Printing.Interop;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Windows.Media;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;

// UNC paths are not supported.  Defaulting to Windows directory.
// To fix this, go to the project Properties -> Debug, change Working directory to somewhere on a local drive.
// Can't check this in; CSGitCack.csproj.user is a .gitignored file

namespace CSGitCack
{
    #region XYstuff

    public class XYBase
    {
        public virtual void Speak()
        {
            Console.WriteLine("XYBase");
        }
    }

    public class XYLevel1A : XYBase
    {
        public override void Speak()
        {
            Console.WriteLine("XYLevel1A");
        }
    }

    public class XYLevel1B : XYBase
    {
        public override void Speak()
        {
            Console.WriteLine("XYLevel1B");
        }
    }

    public class XYLevel2 : XYLevel1A
    {
        public override void Speak()
        {
            Console.WriteLine("XYLevel2");
        }
    }

    public static class XYExtensions
    {
        public static void UpSpeak(this XYBase foo)
        {
            Console.WriteLine("XYBase UpSpeak");
        }

        public static void UpSpeak(this XYLevel1A foo)
        {
            Console.WriteLine("XYLevel1A UpSpeak");
        }

        public static void UpSpeak(this XYLevel1B foo)
        {
            Console.WriteLine("XYLevel1B UpSpeak");
        }

        public static void UpSpeak(this XYLevel2 foo)
        {
            Console.WriteLine("XYLevel2 UpSpeak");
        }
    }

    #endregion

    #region Clock tower event stuff

    public class Person
    {
        private string name;
        private ClockTower tower;

        private List<int> InterestedTimes;

        //private
        public Person(string n, ClockTower c, List<int> it)
        {
            name = n;
            tower = c;
            InterestedTimes = it;

            tower.Chime += (object sender, ClockTowerEventArgs args) =>
            {
                if (InterestedTimes.Contains(args.Time))
                {
                    Console.WriteLine($"{name} responding to chime at {args.Time}");
                }
            };

            string s = $"Initialising new Person({name}, ClockTower, times[{string.Join(",", it)}]";
            Console.WriteLine(s);
        }
    }

    public class ClockTowerEventArgs : EventArgs
    {
        public int Time { get; set; }
    }

    public delegate void ChimeEventHandler(object sender, ClockTowerEventArgs e);

    public class ClockTower
    {
        public event ChimeEventHandler Chime;
        private int hours, mins;

        public ClockTower()
        {
            hours = mins = 0;
        }

        public int Tick()
        {
            mins++;
            if (mins > 59)
            {
                mins = 0;
                hours++;
                if (hours > 23)
                    hours = 0;
            }

            int ret = hours * 100 + mins;
            Chime(this, new ClockTowerEventArgs {Time = ret});
            return ret;
        }
    }

    #endregion

    #region Z-order stuff

    public class ZedThing
    {
        public string message;
        public int zPos;

        public ZedThing(string message, int zPos)
        {
            this.message = message;
            this.zPos = zPos;
        }
    }

    #endregion

    #region test24 memleak stuff

    public class MemLeak
    {
        public BitmapImage ImageSourceFromFile(string myImageFile)
        {
            try
            {
                if (string.IsNullOrEmpty(myImageFile))
                    return null;

                var image = new BitmapImage();
                var ms = new MemoryStream();
                BitmapDecoder bd;
                using (FileStream fs = File.OpenRead(myImageFile))
                {
                    string ext = Path.GetExtension(myImageFile).ToUpper();
                    switch (ext)
                    {
                        case ".BMP":
                            bd = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".GIF":
                            bd = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".JPG":
                        case ".JPEG":
                            bd = new JpegBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        case ".PNG":
                            bd = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat,
                                BitmapCacheOption.OnLoad);
                            break;
                        default:
                            throw new Exception($"Unknown file extension '{ext}'");
                    }

                    var png = new PngBitmapEncoder();
                    png.Frames.Add(bd.Frames[0]);
                    png.Save(ms);

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }

                return image;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"File Open error: '{ex.Message}'");
                return null;
            }
        }

    }

    #endregion

    #region Lock test stuff

    public class LockTest1
    {
        //private object _lock = "lock";
        //private string _lock = "lock";

        public IEnumerable<int> GetNum(object foo)
        {
            lock (foo)
            {
                yield return 1;
                yield return 2;
            }
        }
    }

    public class LockTest2
    {
        //private object _lock = "lock";
        //private string _lock = "lock";

        public IEnumerable<int> GetNum(object foo)
        {
            lock (foo)
            {
                yield return 1;
                yield return 2;
            }
        }
    }

    #endregion

    public class EndPointInfo
    {
        public string IPAddr { get; set; }
        public int Port { get; set; }
    }

    public class ReaderInfo
    {
        public EndPointInfo EndPoint { get; set; }
        public string FriendlyName { get; set; }
    }

    public class TagFriendlyName
    {
        public string TagID { get; set; }
        public string Name { get; set; }
    }

    public class ZoneInfo
    {
        public string Name { get; set; }
        public string OverlayImage { get; set; }
        public List<ReaderInfo> Readers { get; set; }
    }

    public class Config
    {
        public string FileVersion = "DrillRigDemo Config V1.0";
        [XmlIgnore]
        public bool LoadOK;
        public List<ZoneInfo> ZonesInfo { get; set; } = new List<ZoneInfo>();
        //public List<TagFriendlyName> TagFriendlyNames { get; set; } = new List<TagFriendlyName>();
        //public List<string> TagsSeen { get; set; } = new List<string>();
        //public string BackgroundImage { get; set; }
        //public List<string> OverlayFiles { get; set; } = new List<string>();

        private string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DrillRigDemo.xml");
        }
    }

    public class Transform
    {
        [XmlElement(Order = 1)]
        public string DestTables;
        [XmlElement(Order = 2)]
        public string QueryText;
        [XmlArray(Order = 3)]
        [XmlArrayItem(ElementName = "Task")]
        public List<string> TaskList;
    }
    public class DataSource
    {
        [XmlElement(Order = 1)]
        public string ConnectionString;
        [XmlElement(Order = 2)]
        public List<Transform> Transformation;
    }

    public class LTWSConfig
    {
        [XmlElement(Order = 1)]
        public string FileVersion = "1.0";
        [XmlIgnore] public bool LoadOK;
        [XmlElement(Order = 2)]
        public string DebugLevel;

        [XmlIgnore] private string _shortSleep;
        [XmlElement(Order = 3)]
        public string ShortSleep
        {
            get => _shortSleep;
            set
            {
                bool parseOK = int.TryParse(value, out shortSleep);
                if (parseOK && shortSleep >= 1)
                {
                    // TODO: log acceptable shortSleep
                    Console.WriteLine($"ShortSleep [{shortSleep}] validated");
                    _shortSleep = value;
                }
                else
                {
                    // Didn't parse or is <1 so let's set it to 1.
                    // TODO: log the fact that we did this
                    shortSleep = 1;
                    _shortSleep = "1";
                    Console.WriteLine($"ShortSleep set in config but invalid; defaulting to {shortSleep} second(s)");
                }
            }
        }

        [XmlIgnore] private string _intervalSeconds;

        [XmlElement(Order = 4)]
        public string IntervalSeconds
        {
            get => _intervalSeconds;
            set
            {
                bool parseOK = int.TryParse(value, out intervalSec);
                if (parseOK && intervalSec >= 1)
                {
                    // TODO: log acceptable intervalSeconds
                    Console.WriteLine($"IntervalSeconds [{intervalSec}] validated");
                    _intervalSeconds = value;
                }
                else
                {
                    // Didn't parse so let's default to 120 -- needs setting because TryParse craps all over the out variable if it doesn't parse.
                    // TODO: log the fact that we did this
                    intervalSec = defaultInterval;
                    _intervalSeconds = defaultInterval.ToString();
                    Console.WriteLine($"IntervalSeconds set in config but invalid; defaulting to {intervalSec} seconds");
                }
            }
        }
        [XmlElement(Order = 5)]
        public string Sentinel; // only used if present
        [XmlElement(Order = 6)]
        public string ExitSentinel;
        [XmlElement(Order = 7)]
        public string DeleteOlderThan;
        [XmlElement(Order = 8)]
        public DataSource SourceDB; // Currently only supports ODBC
        [XmlElement(Order = 9)]
        public DataSource DestinationDB; // Currently only supports ".Net SqlClient Data Provider" aka "SqlClient";

        [XmlIgnore] private const int defaultInterval = 15; // TODO: 120 for release
        [XmlIgnore] public int shortSleep = 0;
        // IntervalSeconds should exist, so if it doesn't, set it to default
        [XmlIgnore] public int intervalSec = defaultInterval;



        private static string GetPath()
        {
            return "C:\\S3\\TransformationServiceXXX.xml";
        }

        public static LTWSConfig Load()
        {
            try
            {
                var ser = new XmlSerializer(typeof(LTWSConfig));
                using (var reader = new StreamReader(GetPath()))
                {
                    var ret = (LTWSConfig)ser.Deserialize(reader);
                    ret.LoadOK = true;
                    return ret;
                }
            }
            catch
            {
                // TODO: exception handler - write to event log
                throw;
            }
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            //foreach (string s in args)
            //{
            //    Console.WriteLine($"Argument: '{s}'");
            //}

            Assembly thisAssem = typeof(Program).Assembly;
            AssemblyName thisAssemName = thisAssem.GetName();

            Version ver = thisAssemName.Version;

            // https://blog.soltysiak.it/en/2017/07/attach-git-commit-sha1-hash-to-your-assembly/
            // Added AutoT4 extension
            // Console.WriteLine($"Git info [{CSGitCack.GitInfo.HeadShaShort}]");
            // Console.WriteLine($"This is version [{ver}] of [{thisAssemName.Name}] aka [{thisAssemName.FullName}].");
            try
            {
                test60();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("\n\nHit any key to continue");
                Console.ReadLine();
            }
        }

        private static SqlConnection createConnection()
        {
            String myConnectionString = "Data Source=devsql01\\sitedb;Initial Catalog=ATS;User Id=dev; Password=Automation15;Connect Timeout=5;Connection Lifetime=30;Pooling=false";
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
        private static void closeConnection(SqlConnection SQLcon)
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
        private static DataTable getDataTable(String command)
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
        private static Image convertByteToImage(Object pictureByte)
        {
            byte[] imageByte = new byte[0];
            imageByte = (byte[])pictureByte;
            MemoryStream memStream = new MemoryStream(imageByte);
            Image returnImage = Image.FromStream(memStream);

            return returnImage;
        }

        private static void retreiveData(System.Windows.Forms.DataGridView pobUnAcknowledgedGridView, System.Windows.Forms.DataGridView pobOnSiteGridView)
        {
            DataRetreiver dataretreiver = new DataRetreiver();
            int gateID = 82;

            // populate grids
            dataretreiver.populateUnacknowledgedPersonnel(pobUnAcknowledgedGridView, gateID);
            dataretreiver.populateAcknowledgedPersonnel(pobOnSiteGridView, gateID);
            String tagId = "";
            DataTable allTags = dataretreiver.getFirstUndisplayedTag(gateID);
            // loop through grid
            if (allTags.Rows.Count != 0)
                foreach (DataRow tag in allTags.Rows)
                {
                    tagId = allTags.Rows[0][0].ToString();
                    // retrieve personnel details
                    DataTable displayedData = dataretreiver.retrieveDisplayPersonnel(tag["ubisensetagid"].ToString());
                    if (displayedData.Rows.Count != 0)
                    {
                        foreach (DataRow row in displayedData.Rows)
                        {
                            String dob = row["dob"].ToString();
                            dob = dob.Substring(0, dob.IndexOf(":") - 3);

                            if (row["photograph"].ToString().Length > 0 && row["photograph"].ToString() != null)
                            {
                                var personnelPicture_BackgroundImage = new Bitmap(convertByteToImage(row["photograph"]));
                            }
                            else
                            {
                                var personnelPicture_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/PicNotAvailable.png");
                            }

                            if (row["batterystatus"].ToString() != "OK")
                            {
                                var batteryStatusPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/BatteryEmpty.png");
                                var stopGoPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/stopSignOn.jpg");
                            }
                            else
                            {
                                var batteryStatusPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/BatteryFull.png");
                                var stopGoPictureBox_BackgroundImage = new Bitmap(dataretreiver.getResourcesFile() + "/goSignOn.jpg");
                            }
                        }
                    }
                    // check if tag is assigned to vehicle and diplay appropriate label
                    else if (displayedData.Rows.Count == 0)
                    {
                        DataTable vehicles = dataretreiver.getVehicleTag(tag["ubisensetagid"].ToString());
                        if (vehicles.Rows.Count != 0)
                        {
                            foreach (DataRow row in vehicles.Rows)
                            {
                                var regLabel_Text = row["registration"].ToString();
                                var descLabel_Text = row["details"].ToString();
                            }
                        }
                    }
                }
            // there is no queue - display front screen logo
            if (allTags.Rows.Count == 0)
            {
                tagId = "";
            }
        }


        private static int test60a() // Does not leak
        {
            int timer = 2000;
            RegistryKey registryKey = Registry.LocalMachine;
            registryKey = registryKey.OpenSubKey(@"Software\IslandD\Checkpoint");
            timer = int.Parse(registryKey.GetValue("Gate App Timer").ToString());
            return timer;
        }

        private static int test60b() // Does not leak
        {
            int ret = 1;
            try
            {
                String myConnectionString = "Data Source=devsql01\\sitedb;Initial Catalog=ATS;User Id=dev; Password=Automation15;Connect Timeout=5;Connection Lifetime=30;Pooling=false";
                SqlConnection con = new SqlConnection(myConnectionString);
                con.Open();
                con.Close();
            }
            catch (Exception e)
            {
                ret = 0;
                Console.WriteLine(e);
            }

            return ret;
        }

        private static int test60c() // Does not leak
        {
            String command = "SELECT maximum FROM viewsitemaximum";
            return getTotalNumber(command);
        }

        private static int test60d() // Does not leak
        {
            try
            {
                var dt = getDataTable("SELECT name,dob,company,position,thumbnail FROM viewpersonneltagunacknowledge");
                return dt.Rows.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }
        private static int test60e()
        {
            try
            {
                var pobUnAcknowledgedGridView = new System.Windows.Forms.DataGridView();
                var pobOnSiteGridView = new System.Windows.Forms.DataGridView();
                retreiveData(pobUnAcknowledgedGridView, pobOnSiteGridView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }
        private static void test60()
        {
            for (;;)
            {
                int total = 0;
                for (int i = 0; i < 1000; i++)
                    total += test60e();
            }
        }
        private static void test59()
        {
            string code = "RFID";
            string technologyCode = "RFID";
            if (code==technologyCode)
                Console.WriteLine("true");
        }

        private static void test58()
        {
            Console.WriteLine("test58 entry");
            Console.WriteLine("test58 exit");
        }

        private static List<string> RecombineQuotedStrings(string[] tokens)
        {
            var ret = new List<string>();
            char quoteChar = '"';
            int state = 0;
            string newtok = "";
            foreach (var t in tokens)
            {
                switch (state)
                {
                    case 0: // looking for first to combine, e.g. _"task_ or _'task_
                        if (t[0] == '"' || t[0] == '\'')
                        {
                            quoteChar = t[0];
                        }

                        if (t[0] == quoteChar && t[t.Length - 1] != quoteChar)
                        {
                            state = 1;
                            newtok = t;
                        }
                        else
                        {
                            ret.Add(t);
                        }

                        break;

                    case 1: // recombining; looking for last to combine, e.g. _task"_ or _task'_
                        newtok += "," + t;
                        if (t[t.Length - 1] == quoteChar)
                        {
                            state = 0;
                            ret.Add(newtok);
                            newtok = "";
                        }

                        break;
                }
            }

            if (state == 1) // then we've got to the end of the list without a closing quote, e.g. [task1,\"task2a,b,c]
            {
                // It's anyone's guess what the user actually meant here, so let's just bung a quote at the end and leave them to figure out what's wrong
                newtok += quoteChar;
                ret.Add(newtok);
            }
            return ret;
        }

        // Split(',') even splits quoted strings - need a function to recombine them
        private static void test57()
        {
            //string task = "task1,\"task2a,b,c\",\"task3\",task4";
            // What if the last one isn't closed?
            // First let's make sure the last one being closed works OK: [task1 - "task2a,b,c"] yep
            //string task = "task1,\"task2a,b,c\"";
            string task = "task1,\"task2a,b,c"; // 
            // Output: just task1. After extra code: [task1 - "task2a,b,c"] OK
            var tokens = RecombineQuotedStrings(task.Split(','));
            foreach (var t in tokens)
            {
                Console.WriteLine(t);
            }
        }

        // Are these two blocks identical?
        // Loop can be converted into LINQ-expression
        private static void test56()
        {
            var LHS = new List<string>() { "A", "B", "C", "D", "E" };
            var RHS = new List<string>() { "1", "2", "3", "4", "5" };
            var joins = new List<string>();
            string msg1 = "The list is: ";
            for (int i = 0; i < LHS.Count; i++) // "Loop can be converted into LINQ-expression" - can it?
            {
                joins.Add(LHS[i] + "=" + RHS[i]);
            }

            Console.WriteLine(msg1 + string.Join(";", joins));
            // Output: The list is: A = 1; B = 2; C = 3; D = 4; E = 5

            // From previous code:
            // var updateList = columnList.Select((t, i) => t + "=" + valueList[i]).ToList();
            // var joins = LHS.Select((t, i) => t + "=" + RHS[i]).ToList();
            var msg2 = LHS.Select((t, i) => t + "=" + RHS[i]).ToList();
            Console.WriteLine(msg2);
            // Output: System.Collections.Generic.List`1[System.String] - Not like this!

            Console.WriteLine(string.Join(";", msg2));
            // Output: A=1;B=2;C=3;D=4;E=5 - That's better

            var msg3 = LHS.Select((t, i) => t + "=" + RHS[i]).ToList();
            Console.WriteLine(msg1 + string.Join(";", msg3));
            // Output: The list is: A = 1; B = 2; C = 3; D = 4; E = 5 - That's the original result
        }

        // Why is a DataRowCollection a collection of object not a collection of DataRow?
        // Answer: https://stackoverflow.com/a/2325792/5937167
        private static void test55()
        {
            // Create new DataTable.
            DataTable table = new DataTable();

            // Declare DataColumn and DataRow variables.

            // Create new DataColumn, set DataType, ColumnName
            // and add to DataTable.    
            var column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "id"
            };
            table.Columns.Add(column);

            // Create second column.
            column = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "item"
            };
            table.Columns.Add(column);

            // Create new DataRow objects and add to DataTable.    
            for (int i = 0; i < 10; i++)
            {
                var row = table.NewRow();
                row["id"] = i;
                row["item"] = "item " + i;
                table.Rows.Add(row);
            }

            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"id=[{(row["id"])}]; item=[{(row["item"])}]");
            }

            // foreach (var row in table.Rows)
            // {
            //     Console.WriteLine($"id=[{row["id"]}]; item=[{row["item"]}]");
            //     //                       ~~~~~~~~~           ~~~~~~~~~~~
            //     // Cannot apply indexing with [] to an expression of type 'object'
            // }
        }

        private static void test54a()
        {
            var cfg = new LTWSConfig();
            string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var ser = new XmlSerializer(typeof(LTWSConfig));
            var inputFile = Path.Combine(MyDocs, "TestXformSvc.xml");
            using (var reader = new StreamReader(inputFile))
            {
                cfg = (LTWSConfig)ser.Deserialize(reader);
                Debugger.Break(); // rather than loads of Console.WriteLine junk, just inspect it in the debugger
            }
        }

        // Another XML config file generator
        private static void test54()
        {
            // nicked from test47; test47a deserialises to a breakpoint
            var taskList = new List<string>()
            {
                "UniqueID,PERSONNEL_ID",
                "SimpleCopy,FORENAMES,rw_pob.first_name",
                "SimpleCopy,SURNAME,rw_pob.last_name",
                "SimpleCopy,BIRTH_DATE,rw_pob.dob",
                "SimpleCopy,COMPANY_NAME,rw_pob.company",
                "SimpleCopy,BED,rw_pob.bed_no",
                "DestFK,SPECIAL_DUTY,rw_pob.id_emergency_team,ro_emergency_team,id,name",
                "Ignore,MUSTER_POINT",
                "DestFK,LIFEBOAT,rw_pob.id_lifeboatpoint,ro_lifeboatpoint,id,name",
                "SimpleCopy,SHIFT,rw_pob.shift",
                "DestValue,rw_pob.checked_in,1",
                "DestValue,rw_pob.checked_out,0"
            };
            var txList = new List<Transform>()
            {
                new Transform()
                {
                    DestTables = "rw_POB",
                    QueryText =
                        "SELECT PERSONNEL_ID, FORENAMES, SURNAME, BIRTH_DATE, COMPANY_NAME, BED, SPECIAL_DUTY, MUSTER_POINT, LIFEBOAT, SHIFT FROM vantage.dbo.s3_pob WHERE LOCATION='GEYE'",
                    TaskList = taskList
                },
                new Transform()
                {
                    DestTables="foo,bar",
                    QueryText="wibble"
                }
            };
            var cfg = new LTWSConfig()
            {
                DebugLevel = "Full",
                ShortSleep = "1",
                IntervalSeconds = "10",
                Sentinel = @"C:\S3\LogTX_DeleteToContinue.txt",
                ExitSentinel = @"C:\S3\LogTX_DeleteToExit.txt",
                DeleteOlderThan = "01-JAN-1998",
                SourceDB = new DataSource()
                {
                    ConnectionString = "DSN=Vantage;UID=s3idapp;PWD=Automation1",
                    Transformation = txList
                },
                DestinationDB = new DataSource()
                {
                    ConnectionString = "ODBC=wibble;FOO=wobble"
                },
            };
            string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var outputFile = Path.Combine(MyDocs, "TestXformSvc.xml");
            var ser = new XmlSerializer(typeof(LTWSConfig));
            Console.WriteLine(outputFile);
            using (var writer = new FileStream(outputFile, FileMode.Create))
            {
                ser.Serialize(writer, cfg);
            }
        }

        // Type cast is redundant? Really?
        private static void test53()
        {
            int a = 5;
            int b = 2;
            double c = (double)a / (double)b; // R# says cast is redundant but we get the right result 2.5
            double d = (double) a / b; // No R# whinge; 2.5
            double e = a / (double) b; // No R# whinge; 2.5
            double f = a / b; // No R# whinge, "Possible loss of fraction" prob from VS; result 2
            double g = (double) (a / b); // No R# whinge, "Possible loss of fraction" prob from VS; result 2
            Console.WriteLine($"a[{a}] / b[{b}] = c[{c}], d[{d}], e[{e}], f[{f}], g[{g}]");
            // Result: a[5] / b[2] = c[2.5], d[2.5], e[2.5], f[2], g[2]
        }

        // Access to modified closure?
        // public delegate void ClosureEventHandler(object sender, EventArgs e);
        // public static event ClosureEventHandler ev1;
        // Chime(this, new ClockTowerEventArgs {Time = ret});
        private static void test52()
        {
            var nums = new List<string>() {"One", "Two", "Three"};
            var acts = new List<Action>();
            // foreach (var num in nums)
            // {
            //     acts.Add(delegate { Console.WriteLine(num); });
            // }
            for (int i = 0; i < nums.Count; i++)
            {
                acts.Add(delegate { Console.WriteLine(nums[i]); });
                // acts.Add(delegate  "{ Console.WriteLine(nums[i]); }"  );
            }
            // ev1.Invoke("foo", new EventArgs());
            // Console.WriteLine($"i={i}"); // error CS0103: The name 'i' does not exist in the current context
            acts[0](); // System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
        }
        private static void test51()
        {
            int len = 30;
            string str = $"{len:D3}";
            Console.WriteLine($"Length = [{str}]");

            Byte[] hello = { 72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100 };
            //string plop = new string(Encoding.ASCII.GetChars(hello, 0, 5));
            Console.WriteLine($"hello: [{new string(Encoding.ASCII.GetChars(hello, 0, 5))}]");
        }

        private static async Task<int> test50a()
        {
            Console.WriteLine("Enter 50a");
            Task<int> t = Task.Run(() =>
            {
                Thread.Sleep(1000);
                var client = new HttpClient();
                var mm = client.GetStringAsync("http://msdn.microsoft.com");
                return 17;
            });
            Console.WriteLine("Before 50a await");
            int ret = await t;
            Console.WriteLine("Exit 50a");
            return ret;
        }
        private static async Task<int> test50b()
        {
            Console.WriteLine("Enter 50b");
            Task<int> t = Task.Run(() =>
            {
                Thread.Sleep((2000));
                return 25;
            });
            Console.WriteLine("Before 50b await");
            int ret = await t;
            Console.WriteLine("Exit 50b");
            return ret;
        }

        private static void test50()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("How do we solve this with async/await?");
            Console.WriteLine("Calling func 1");
            var a = test50a();
            Console.WriteLine("Calling func 2");
            var b = test50b();
            Console.WriteLine("Starting infinite loop");
            for (; ; )
            {
                if (a.IsCompleted && b.IsCompleted)
                    break;

                Console.WriteLine("Something isn't ready yet");
                Thread.Sleep(500);
            }
            //Console.WriteLine("Doing my own thing for 3000");
            //Thread.Sleep(3000);
            Console.WriteLine($"The sum of a({a.Result}) and b({b.Result}) is {a.Result + b.Result}");
            Console.WriteLine($"That took {sw.ElapsedMilliseconds} ms to run");
        }

        private static async Task<int> test49a()
        {
            Console.WriteLine("Enter 49a");
            Task<int> t = Task.Run(() => test48a());
            int ret = await t;
            Console.WriteLine("Exit 49a");
            return ret;
        }
        private static async Task<int> test49b()
        {
            Console.WriteLine("Enter 49b");
            Task<int> t = Task.Run(() => test48b());
            int ret = await t;
            Console.WriteLine("Exit 49b");
            return ret;
        }

        private static void test49()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("How do we solve this with async/await?");
            Console.WriteLine("Calling func 1");
            var a = test49a();
            Console.WriteLine("Calling func 2");
            var b = test49b();
            for (; ; )
            {
                if (a.IsCompleted && b.IsCompleted)
                    break;

                Console.WriteLine("Something isn't ready yet");
                Thread.Sleep(500);
            }
            //Console.WriteLine("Doing my own thing for 3000");
            //Thread.Sleep(3000);
            Console.WriteLine($"The sum of a({a.Result}) and b({b.Result}) is {a.Result + b.Result}");
            Console.WriteLine($"That took {sw.ElapsedMilliseconds} ms to run");
        }

        private static int test48a()
        {
            // Long running operation 1
            Console.WriteLine("Enter 48a");
            Thread.Sleep(1000);
            Console.WriteLine("Exit 48a");
            return 17;
        }

        private static int test48b()
        {
            // Long running operation 2
            Console.WriteLine("Enter 48b");
            Thread.Sleep(2000);
            Console.WriteLine("Exit 48b");
            return 25;
        }
        private static void test48()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("How do we solve this with async/await?");
            Console.WriteLine("Calling func 1");
            var a = Task.Run(() => test48a());
            Console.WriteLine("Calling func 2");
            var b = Task.Run(() => test48b());
            for (; ; )
            {
                if (a.IsCompleted && b.IsCompleted)
                    break;

                Console.WriteLine("Something isn't ready yet");
                Thread.Sleep(500);
            }
            //Console.WriteLine("Doing my own thing for 3000");
            //Thread.Sleep(3000);
            Console.WriteLine($"The sum of a({a.Result}) and b({b.Result}) is {a.Result + b.Result}");
            Console.WriteLine($"That took {sw.ElapsedMilliseconds} ms to run");
        }

        private static void test47a()
        {
            var cfg = new Config();
            string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var ser = new XmlSerializer(typeof(Config));
            var inputFile = Path.Combine(MyDocs, "DrillRigDemo.xml");
            using (var reader = new StreamReader(inputFile))
            {
                cfg = (Config)ser.Deserialize(reader);
                Debugger.Break(); // rather than loads of Console.WriteLine junk, just inspect it in the debugger
            }
        }

        private static void test47()
        {
            Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "DrillRigDemo.xml"));
            string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            for (int i = 0; i < 2; i++)
            {
                var cfg = new Config();
                cfg.ZonesInfo.Add(new ZoneInfo()
                {
                    Name = "Zone1",
                    OverlayImage = Path.Combine(MyDocs, "DS Zone1 overlay.png"),
                    Readers = new List<ReaderInfo>()
                    {
                        new ReaderInfo()
                        {
                            FriendlyName = "Zone 1 near window",
                            EndPoint = new EndPointInfo() {IPAddr = "192.168.1.124", Port = 6101}
                        },
                        new ReaderInfo()
                        {
                            FriendlyName = "Zone 1 above entry door",
                            EndPoint = new EndPointInfo() {IPAddr = "192.168.1.124", Port = 6102}
                        }
                    }
                });
                cfg.ZonesInfo.Add(new ZoneInfo()
                {
                    Name = "Zone2",
                    OverlayImage = Path.Combine(MyDocs, "DS Zone2 overlay.png"),
                    Readers = new List<ReaderInfo>()
                    {
                        new ReaderInfo()
                        {
                            FriendlyName = "Zone 2 ceiling",
                            EndPoint = new EndPointInfo() {IPAddr = "192.168.1.124", Port = 6103}
                        }
                    }
                });
                //cfg.TagFriendlyNames.Add(new TagFriendlyName() {TagID = "5333494430303031", Name = "S3ID0001"});
                //cfg.TagFriendlyNames.Add(new TagFriendlyName() {TagID = "5333494430303032", Name = "S3ID0002"});
                //cfg.TagsSeen.Add("5333494430303031");
                //cfg.TagsSeen.Add("5333494430303032");
                //cfg.TagsSeen.Add("5333494430303033");
                //cfg.TagsSeen.Add("5333494430303034");
                //cfg.BackgroundImage = Path.Combine(MyDocs, "DS Roughneck Schematics.png");
                //cfg.OverlayFiles.Add(Path.Combine(MyDocs, "DS Zone1 overlay.png"));
                //cfg.OverlayFiles.Add(Path.Combine(MyDocs, "DS Zone2 overlay.png"));

                var ser = new XmlSerializer(typeof(Config));
                var outputFile = Path.Combine(MyDocs, "DrillRigDemo.xml");
                using (var writer = new FileStream(outputFile, FileMode.Create))
                {
                    ser.Serialize(writer, cfg);
                }
            }
        }


        // What are the chances of 3 tags clashing in a 25ms cycle? Extended to 5 cos the numbers seem wrong
        // This seems wrong -> Found 186445 clashes out of 194481 possible cases; P(Clash)=95
        // 00000 AAAAA BBBBB CCCCC DDDDD with 4 combinations for A, 3 for B, 2 for C, that should mean there are 24 non-clashes out of 20^4=160000 cases
        private static void test46()
        {
            int PossibleCases = 0;
            int ClashesFound = 0;
            // Tag0 always transmits at T=0
            for (int tag1 = 0; tag1 <= 20; tag1++)
            {
                for (int tag2 = 0; tag2 <= 20; tag2++)
                {
                    for (int tag3 = 0; tag3 <= 20; tag3++)
                    {
                        for (int tag4 = 0; tag4 <= 20; tag4++)
                        {
                            PossibleCases++;
                            bool clash = false;
                            // Tag1 overlaps Tag0 if it starts broadcasting at 0-4
                            if (tag1 >= 0 && tag1 <= 4)
                            {
                                clash = true;
                            }

                            // Tag2 overlaps Tag0 if it starts broadcasting at 0-4
                            if (tag2 >= 0 && tag2 <= 4)
                            {
                                clash = true;
                            }

                            // Tag2 overlaps Tag1 if it starts broadcasting at tag1 to tag1+4
                            if (tag2 >= tag1 && tag2 <= tag1 + 4)
                            {
                                clash = true;
                            }

                            if (tag3 >= 0 && tag3 <= 4 || // Tag3 overlaps Tag0
                                tag3 >= tag1 && tag3 <= tag1 + 4 || // Tag3 overlaps Tag1
                                tag3 >= tag2 && tag3 <= tag2 + 4 // Tag3 overlaps Tag2
                            )
                                clash = true;

                            if (tag4 >= 0 && tag4 <= 4 || // Tag4 overlaps Tag0
                                tag4 >= tag1 && tag4 <= tag1 + 4 || // Tag4 overlaps Tag1
                                tag4 >= tag2 && tag4 <= tag2 + 4 || // Tag4 overlaps Tag2
                                tag4 >= tag3 && tag4 <= tag3 + 4 // Tag4 overlaps Tag2
                            )
                                clash = true;
                            if (clash)
                                ClashesFound++;
                        }
                    }
                }
            }

            Console.WriteLine(
                $"Found {ClashesFound} clashes out of {PossibleCases} possible cases; P(Clash)={ClashesFound * 100 / PossibleCases}");
        }

        private static void test45()
        {
            //object ServiceValue1 = 15;
            //object ServiceValue2 = null;
            //object ServiceValue3 = 0;
            //object ServiceValue4 = "Hello";
            //int param = (ServiceValue4 as int?) ?? 5;
            //if (param == 0)
            //    param = 5;
            //Console.WriteLine($"param = {param}");
        }

        // Testing behaviour of lock
        // Expected to deadlock, but it doesn't because locks are re-entrant within the same thread.  We'd need to do something in a background thread for this to deadlock.
        private static void test44()
        {
            object foo = "foo";
            var t1 = new LockTest1().GetNum(foo).GetEnumerator();
            var t2 = new LockTest2().GetNum(foo).GetEnumerator();
            Console.WriteLine($"Get a number from t1: {t1.Current}");
            t1.MoveNext();
            Console.WriteLine($"Get a number from t2: {t2.Current}");
            t2.MoveNext();
            Console.WriteLine($"Get a number from t1: {t1.Current}");
            t1.MoveNext();
            Console.WriteLine($"Get a number from t2: {t2.Current}");
            t2.MoveNext();
            Console.WriteLine($"Get a number from t1: {t1.Current}");
            t1.MoveNext();
            Console.WriteLine($"Get a number from t2: {t2.Current}");
            t2.MoveNext();
            Console.WriteLine($"Get a number from t1: {t1.Current}");
            t1.MoveNext();
            Console.WriteLine($"Get a number from t2: {t2.Current}");
            t2.MoveNext();
        }

        // Does this return expected behaviour?
        // return !Service?.Selection?.FirstOrDefault()?.checked_out ?? false;
        // Simplify: !check ?? false;
        // Then compare with !(check ?? false);
        private static void test43()
        {
            bool? check;
            bool result;

            check = true;
            result = !(check ?? false);
            Console.WriteLine($"For input true, output is {result}");

            check = false;
            result = !(check ?? false);
            Console.WriteLine($"For input false, output is {result}");

            check = null;
            result = !(check ?? false);
            Console.WriteLine($"For input null, output is {result}");

        }

        // List triangular numbers up to MAXTN
        private static void test42()
        {
            int MAXTN = 15500;
            int Tn = 1, diff = 2;
            while (Tn <= MAXTN)
            {
                Tn += diff;
                diff++;
                Console.WriteLine($"{Tn}");
            }
        }

        // Brute force solver for the missing leaf problem https://www.youtube.com/watch?v=GYFHMD_ja7c
        // Cannot remove first leaf, whether 1 or 12, because there is no pair of triangular numbers that differs by 1 or 3.
        // -- consider T4 and T6. Difference is 5+6=11.  If we go larger or take a bigger gap, the difference 11 will only increase.
        // -- Yet we must go larger, because the remaining pages add up to 15000.
        // -- The only Tn that differ by 3 are 1+2 and 1+2+3. No Tn differ by 1 (unless you count T0 and T1). None of these are 15K.
        // -- Does T_n=15001 or 15003 exist?  No; T_n around 15000 are 14706 14878 * 15051 15225 15400 (source: test42())
        // Cannot remove last leaf whether single or double sided, because the remaining pages add up to a triangular number, and there is no T_n=15000.
        private static void test41a() // expanded version of test41
        {
            const int MAXLEAF = 250000;
            // In this function we consider the wider set of possibilities.  We'll work with leaves this time, starting at 4.
            // Pages could be 12 34 56 78, _1 23 45 67, 12 34 56 7_, _1 23 45 6_. Clearly some are equivalent since we can't remove first or last, but we'll check them anyway.

            for (int leaf = 4; leaf <= MAXLEAF; leaf++)
                //for (int leaf = 86; leaf <= 88; leaf++) // Video solutions are 174 pages remove 112/113; 173 pages remove 25/26; 174 pages=87 leaves so we go from 85 to 89
            {
                // Reducing duplicate code
                for (int leafRemoved = 1; leafRemoved < leaf - 1; leafRemoved++)
                {
                    int firstPage = 0, lastPage = 0;
                    string eg = "";
                    for (int blanks = 0; blanks < 4; blanks++)
                    {
                        switch (blanks)
                        {
                            case 0:
                                // No blanks
                                eg = "12 34 56 78";
                                lastPage = leaf * 2; // last page of the book
                                firstPage = 3 + (leafRemoved - 1) * 2; // first page of the removed leaf
                                break;

                            case 1:
                                // Blank on the first leaf
                                eg = "_1 23 45 67";
                                firstPage = leafRemoved * 2; // first page of the removed leaf - clearly 2,4,6 in the eg
                                lastPage = leaf * 2 - 1; // see eg: 4 leaves *2=8; -1=7
                                break;

                            case 2:
                                // Blank on the last leaf
                                eg = "12 34 56 7_";
                                firstPage = leafRemoved * 2 +
                                            1; // first page of the removed leaf - clearly 3,5,7 in the eg
                                lastPage = leaf * 2 - 1; // see eg: 4 leaves *2=8; -1=7
                                break;

                            case 3:
                                // Blank on the first and last leaf
                                eg = "_1 23 45 6_";
                                firstPage = leafRemoved *
                                            2; // first page of the removed leaf - clearly 2,4(,6...) in the eg
                                lastPage = (leaf - 1) * 2; // see eg: 4 leaves *2=8; -1=7
                                break;
                        }

                        int leafSum = firstPage + firstPage + 1;
                        int totalPages = lastPage * (lastPage + 1) / 2;
                        if (totalPages - leafSum != 15000)
                        {
                            // Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} (blanks:{eg}) containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(!=15K -> no soln)");
                        }
                        else
                        {
                            Console.WriteLine(
                                $"Removing leaf {leafRemoved} of {leaf} (blanks:{eg}) containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(=15K-> solution!)");
                            // Console.ReadLine();
                        }
                    }
                }

                // //~~~~~~~
                // // No blanks
                // int lastPage = leaf * 2; // last page of the book
                // Console.WriteLine($"Considering book of {leaf} leaves, no blanks, pages 1-{lastPage} (eg 12 34 56 78)");
                // for (int leafRemoved=1; leafRemoved<leaf-1; leafRemoved++)
                // {
                //     int firstPage = 3 + (leafRemoved - 1) * 2; // first page of the removed leaf
                //     int leafSum = firstPage + firstPage + 1;
                //     int totalPages = lastPage * (lastPage + 1); // 4 leaves=36; *2=72 which is 8*9, i.e. last page*(last page+1)
                //     if (totalPages - leafSum != 15000)
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(!=15K -> no soln)");
                //     }
                //     else
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(=15K-> solution!)");
                //         Console.ReadLine();
                //     }
                // }
                // 
                // // Blank on the first leaf
                // lastPage = leaf * 2 - 1; // see eg: 4 leaves *2=8; -1=7
                // Console.WriteLine($"Considering book of {leaf} leaves, blank on first leaf, pages 1-{lastPage} (eg _1 23 45 67)");
                // for (int leafRemoved = 1; leafRemoved < leaf - 1; leafRemoved++)
                // {
                //     int firstPage = leafRemoved * 2; // first page of the removed leaf - clearly 2,4,6 in the eg
                //     int leafSum = firstPage + firstPage + 1;
                //     int totalPages = lastPage * (lastPage + 1); // 4 leaves=28; *2=56 which is 7*8, i.e. last page*(last page+1)
                //     if (totalPages - leafSum != 15000)
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(!=15K -> no soln)");
                //     }
                //     else
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(=15K-> solution!)");
                //         Console.ReadLine();
                //     }
                // }
                // 
                // // Blank on the last leaf
                // lastPage = leaf * 2 - 1; // see eg: 4 leaves *2=8; -1=7
                // Console.WriteLine($"Considering book of {leaf} leaves, blank on last leaf, pages 1-{lastPage} (eg 12 34 56 7_)");
                // for (int leafRemoved = 1; leafRemoved < leaf - 1; leafRemoved++)
                // {
                //     int firstPage = leafRemoved * 2 + 1; // first page of the removed leaf - clearly 3,5,7 in the eg
                //     int leafSum = firstPage + firstPage + 1; // Not removing last page so we don't need to worry about 7_
                //     int totalPages = lastPage * (lastPage + 1); // 4 leaves=28; *2=56 which is 7*8, i.e. last page*(last page+1)
                //     if (totalPages - leafSum != 15000)
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(!=15K -> no soln)");
                //     }
                //     else
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(=15K-> solution!)");
                //         Console.ReadLine();
                //     }
                // }
                // 
                // // Blank on the first and last leaf
                // lastPage = (leaf - 1) * 2; // see eg: 4 leaves *2=8; -1=7
                // Console.WriteLine($"Considering book of {leaf} leaves, blank on first and last leaf, pages 1-{lastPage} (eg _1 23 45 6_)");
                // for (int leafRemoved = 1; leafRemoved < leaf - 1; leafRemoved++)
                // {
                //     int firstPage = leafRemoved * 2; // first page of the removed leaf - clearly 2,4(,6...) in the eg
                //     int leafSum = firstPage + firstPage + 1; // Not removing last page so we don't need to worry about 6_
                //     int totalPages = lastPage * (lastPage + 1); // 4 leaves=21; *2=42 which is 6*7, i.e. last page*(last page+1)
                //     if (totalPages - leafSum != 15000)
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(!=15K -> no soln)");
                //     }
                //     else
                //     {
                //         Console.WriteLine($"Removing leaf {leafRemoved} of {leaf} containing pages {firstPage} and {firstPage + 1}; page sum decreases from {totalPages} to {totalPages - leafSum}(=15K-> solution!)");
                //         Console.ReadLine();
                //     }
                // }
            }
        }

        private static void test41()
        {
            const int MAXPAGES = 250000;
            Console.WriteLine(
                "Finding solutions for pages numbered [2|3], leaves numbered both sides, not removing first or last leaf");
            // There must be at least 3 leaves in this case
            // Each leaf contains 2 pages 12 34 56 78 etc
            //int firstFew = 10;
            for (int numPages = 6; numPages <= MAXPAGES; numPages += 2)
            {
                // Calculate the sum of the pages - don't forget we are looking at the larger of the two page numbers
                int sumPages = (numPages - 1) * numPages / 2;

                // The sum of the remaining leaves is 15000 so there's no point doing anything until we get to this point
                if (sumPages >= 15000)
                {
                    //if (firstFew > 0)
                    //{
                    //    Console.WriteLine($"{sumPages}");
                    //    firstFew--;
                    //}

                    // So we now need to look for a page numbered from 3 to numPage-3 inclusive
                    for (int tornPage = 3; tornPage <= numPages - 3; tornPage += 2)
                    {
                        int tornLeaf = tornPage + (tornPage + 1);

                        // If the sum of the pages minus the tornLeaf is exactly 15000, we have a solution
                        if (sumPages - tornLeaf == 15000)
                        {
                            Console.WriteLine(
                                $"Found a solution; book has {numPages} pages, and leaf numbered {tornPage} and {tornPage + 1} was removed");
                        }

                        //if (sumPages == 15051)
                        //{
                        //    Console.WriteLine($"The case where sumPages=={sumPages} exists");
                        //}
                    }
                }
            }
        }

        // Check if Random%6 is biased.  If it is then the average value over a lot of runs will be significantly
        // different from 2.5.  It isn't.
        private static void test40()
        {
            var RNG = new Random();
            int total = 0, count = 0;
            while (count < 5000)
            {
                int num = RNG.Next() % 6;
                total += num;
                double average = (double) total / (double) count;
                count++;
                Console.WriteLine($"num={num}; total={total}; count={count}; average={average}");
                //total += RNG.Next() % 6;
                //count++;
                //
                //if (count % 50 == 0)
                //{
                //    Console.WriteLine($"Total={total}; Count={count}; Average={total/count}");
                //}
            }
        }

        private static void test39()
        {
            int num = 27;
            string str1 = num.ToString("X");
            string str2 = num.ToString("X6");
            Console.WriteLine($"str1:{str1}, str2:{str2}");
        }

        // Example from https://msdn.microsoft.com/en-us/library/system.xml.xmldocument.createtextnode(v=vs.110).aspx
        private static void test38()
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" +
                        "<title>Pride And Prejudice</title>" +
                        "</book>");

            //Create a new node and add it to the document.
            //The text node is the content of the price element.
            XmlElement elem = doc.CreateElement("price");
            XmlText text = doc.CreateTextNode("19.95");
            doc.DocumentElement.AppendChild(elem);
            doc.DocumentElement.LastChild.AppendChild(text);

            Console.WriteLine("Display the modified XML...");
            doc.Save(Console.Out);
        }

        // https://practice.geeksforgeeks.org/problems/is-binary-number-multiple-of-3/0
        private static void test37()
        {
            string inputBinary = "1001011";
            int ix = 1;
            bool done = false;
            bool div3 = false;
            string partial = $"{inputBinary[0]}";
            while (!done)
            {
                while (partial.Length < 3)
                    partial = "0" + partial;
                switch (partial)
                {
                    case "000":
                    case "011":
                    case "110":
                        div3 = true; // 0,3,6
                        partial = "00";
                        break;

                    case "001":
                    case "100":
                    case "111":
                        div3 = false; // 1,4,7
                        partial = "01";
                        break;

                    case "010":
                    case "101":
                        div3 = false; // 2,5
                        partial = "10";
                        break;
                }

                // get next bit
                if (ix < inputBinary.Length)
                {
                    partial += inputBinary[ix];
                    ix++;
                }
                else
                    done = true;
            }

            Console.WriteLine(div3
                ? $"inputBinary[{inputBinary}] is divisible by 3"
                : $"inputBinary[{inputBinary}] is not divisible by 3");
        }

        private static void test36()
        {
            string[] fields =
            {
                "dmDeviceName", "dmSpecVersion", "dmDriverVersion", "dmSize", "dmDriverExtra", "dmFields",
                "dmOrientation",
                "dmPaperSize", "dmPaperLength", "dmPaperWidth", "dmScale", "dmCopies", "dmDefaultSource",
                "dmPrintQuality",
                "dmColor", "dmDuplex", "dmYResolution", "dmTTOption", "dmCollate", "dmFormName", "dmLogPixels",
                "dmBitsPerPel",
                "dmPelsWidth", "dmPelsHeight", "dmDisplayFlags", "dmDisplayFrequency", "dmICMMethod", "dmICMIntent",
                "dmMediaType",
                "dmDitherType", "dmICCManufacturer", "dmICCModel", "dmPanningWidth", "dmPanningHeight"
            };
            string doll = "$";
            string quot = "\"";
            string brco = "{";
            string brcc = "}";
            foreach (var s in fields)
            {
                Console.WriteLine($"if (mode1.{s} != mode2.{s})");
                Console.WriteLine(
                    $"Console.WriteLine({doll}{quot}{s} changed from [{brco}mode1.{s}{brcc}] to [{brco}mode2.{s}{brcc}]{quot});");

                // if (mode1.dmDeviceName != mode2.dmDeviceName)
                // Console.WriteLine($"dmDeviceName changed from [{mode1.dmDeviceName}] to [{mode2.dmDeviceName}]");

            }
        }

        private static void test35()
        {
            Console.WriteLine($"Temp directory is [{Path.GetTempPath()}]"); // C:\Users\dspencer\AppData\Local\Temp\
        }

        private static void test34()
        {
            int x = 0;
            var printServer = new LocalPrintServer();
            var queue = printServer.GetPrintQueue("OKI-C532-BE907F");
            var queueStatus = queue.QueueStatus;
            var jobStatus = queue.GetPrintJobInfoCollection().FirstOrDefault().JobStatus;
            x++;
        }

        private static void test33()
        {
            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");

                Console.WriteLine("{0} (Status: {1}, Default: {2}, Network: {3}",
                    name, status, isDefault, isNetworkPrinter);
                foreach (var pp in printer.Properties)
                {
                    Console.WriteLine($"- Property name [{pp.Name}] Value [{pp.Value}]");
                }
            }
        }

        private static void test32()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine(printer);
            }
        }

        // Can I access UNC directories? (Yes)
        private static void test31()
        {
            DirectoryInfo d = new DirectoryInfo(@"\\DESKTOP-VQNL9IV\Content\JJ\AV");

            foreach (var file in d.GetFiles("*.*"))
            {
                string filename = file.ToString();
                Console.WriteLine(filename);
            }
        }

        // Basic DataTable stuff
        static void test30a(DataTable t)
        {
            DataColumn column;
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "moo";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            t.Columns.Add(column);
        }

        static DataTable test30b(DataTable t)
        {
            DataColumn column;
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "oink";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            t.Columns.Add(column);
            return t;
        }

        private static void test30()
        {
            DataTable table = new DataTable("Fred");
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            test30a(table);
            table = test30b(table);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ParentItem";
            column.AutoIncrement = false;
            column.Caption = "ParentItem";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create three new DataRow objects and add 
            // them to the DataTable
            for (int i = 0; i <= 2; i++)
            {
                row = table.NewRow();
                row["id"] = i;
                row["moo"] = i;
                row["oink"] = i;
                row["ParentItem"] = "ParentItem " + i;
                table.Rows.Add(row);
            }

            table.AcceptChanges();
            DataTable table2 = table.Copy();
            Console.WriteLine("Blat!");
        }

        // Check multiline string literals work as expected
        private static void test29()
        {
            string test = "Hello-" +
                          "There-" +
                          "Everyone";
            Console.WriteLine(test);
        }

        // Base 64 encode a named file
        private static void test28()
        {
            string[] fileNames =
            {
                "Laser_Charles_Wright.ttf", "LaserCW_MB.ttf", "LaserCW_MB_3D.ttf", "Laser_Charles_Wright_3D.ttf",
                "Laser_Charles_Wright_Inline.ttf", "Laser_Charles_Wright_MC.ttf", "Laser_Charles_Wright_MC_3D.ttf",
                "Laser_Charles_Wright_MC_HL.ttf", "Laser_France.ttf", "Laser_IRL.ttf", "Laser_UK79.ttf"
            };
            foreach (var fn in fileNames)
            {
                string fnNoExt = fn.Replace(".ttf", "");
                Console.WriteLine($"        static string {fnNoExt} = ");

                string f2enc = $"D:\\MiscJunk\\fonts\\{fn}";
                var b64 = Convert.ToBase64String(File.ReadAllBytes(f2enc));
                int count = 0, chars = 0, lines = 0;
                foreach (char c in b64)
                {
                    if (count == 0)
                        Console.Write("            \"");
                    Console.Write(c);
                    chars++;
                    count++;
                    if (count > 119)
                    {
                        Console.WriteLine("\" +");
                        lines++;
                        count = 0;
                    }
                }

                Console.WriteLine("\";\n");
                // Console.WriteLine($"Lines: {lines}; Characters: {chars}");
            }
        }

        // test27: get resource font -- doesn't work
        private static void test27()
        {
            var foo = new Uri("pack://application:,,,/");
            var ff = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"),
                "./resources/#Laser Charles Wright");
            //new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./resources/#Laser Charles Wright");
        }

        // test26: is a font installed?
        private static void test26()
        {
            string strLaser = "Laser Charles Wright";
            var fntLaser = new Font(strLaser, 8);
            bool instLaser = strLaser.Equals(fntLaser.Name, StringComparison.InvariantCultureIgnoreCase);

            string strWibble = "Wibble";
            var fntWibble = new Font(strWibble, 8);
            bool instWibble = strWibble.Equals(fntWibble.Name, StringComparison.InvariantCultureIgnoreCase);

            Console.WriteLine($"'{strLaser}' installed: [{instLaser}]; '{strWibble}' installed: [{instWibble}]");
        }

        // test25: What does GetTextGeometryAndFormatting do if the text is empty?
        // Ans: Bounds.Width=-Inf.  Bounds.Width < 20 returns TRUE.

        #region GetTextGeometryAndFormatting

        static void GetTextGeometryAndFormatting(string Text, string Font, double FontSize, bool Italics, bool Bold,
            System.Windows.Point Location, System.Windows.Media.Color Colour, out Geometry geom, out FormattedText ft,
            out System.Windows.Point whitespace)
        {
            var tf = new Typeface(new System.Windows.Media.FontFamily(Font),
                Italics ? FontStyles.Italic : FontStyles.Normal,
                Bold ? FontWeights.Bold : FontWeights.Normal,
                FontStretches.Normal);
            var br1 = new SolidColorBrush();
            try
            {
                br1.Color = Colour;
            }
            catch
            {
                br1.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
            }

            string measureString = Text;
            ft = new FormattedText(measureString, CultureInfo.InvariantCulture,
                System.Windows.FlowDirection.LeftToRight, tf, FontSize, br1);

            // Problem is: we don't know what the space dimensions are.  So if we draw it at 0,0, then the bounding rectangle will start at x,y which indicates the space dimension.
            var geom1 = ft.BuildGeometry(new System.Windows.Point(0, 0));
            // geom1.Bounds.TopLeft is now the width and height of the whitespace

            // Rebuild the geometry to discard the whitespace and offset by this.(x,y), because for general use elsewhere we want geom.Bounds to indicate the drawn text area
            whitespace = geom1.Bounds.TopLeft; // draw however still needs to be able to take this into account
            System.Windows.Point offset =
                new System.Windows.Point(whitespace.X * -1 + Location.X, whitespace.Y * -1 + Location.Y);
            geom = ft.BuildGeometry(offset);
        }

        #endregion

        private static void test25()
        {
            Geometry geom;
            FormattedText ft;
            System.Windows.Point whitespace;
            string testStr = "";
            GetTextGeometryAndFormatting(testStr, "Arial", 12, false, false, new System.Windows.Point(0, 0),
                System.Windows.Media.Colors.Black, out geom, out ft, out whitespace);
            Console.WriteLine($"Width of test string '{testStr}' is {geom.Bounds.Width}");
            Console.WriteLine(
                $"So is this smaller than a rectangle of width 20? [{(geom.Bounds.Width < 20).ToString()}]");
        }

        private static void test24()
        {
            // In an infinite loop:
            // - select a random *.png filename from C:\Users\Public\Documents\CCL\lgsystems\Badges
            // - load it with MemLeak.ImageSourceFromFile
            var mem = new MemLeak();
            Random rnd = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            var files = Directory.GetFiles("C:\\Users\\Public\\Documents\\CCL\\lgsystems\\Badges");
            int count = 0;
            for (;;)
            {
                string file = files[rnd.Next(files.Length)];
                if (Path.GetExtension(file) == ".png")
                {
                    var img = mem.ImageSourceFromFile(file);
                    count++;
                    if (count > 99)
                    {
                        count = 0;
                        Console.Write(".");
                    }
                }
            }
        }

        private static void test23()
        {
            // Plates: every position can be X I or space.  8 characters.
            // Ternary column values: 1 3 9 27 81 243 729 2187 6561

            string[] ValidMasks =
            {
                // One letter one number &vv
                "NN......",
                // 1-3 letters, 1-3 numbers &vv not 1&1
                "NNN.NNN.", "NNN.NN..", "NNN.N...", "NN.NNN..", "NN.NN...", "NN.N....", "N.NNN...", "N.NN....",
                // Post 2001  AB12 CDE
                "NNNN.NNN.",
                // Prefix D123 ABC
                "NNNN.NNN.", "NNN.NNN.", "NN.NNN..",
                // Suffix ABC 123D
                "NNN.NNNN.", "NNN.NNN..", "NNN.NN..",
                // 4 digits 1 or 2 letters &vv
                "NNNN.N..", "NNNN.NN.", "N.NNNN..", "NN.NNNN.",
                // 3 letters 4 numbers &vv
                "NNN.NNNN", "NNNN.NNN"

            };
            var DistinctLengths = new List<int[]>();

            int lines = 0;
            int count = 1;
            for (int i = 0; i < 6561; i++)
            {
                int t = i;
                string op = "";
                string mask = "";
                for (int j = 0; j < 8; j++)
                {
                    char c = '.';
                    char m = '.';
                    switch (t % 3)
                    {
                        case 1:
                            c = 'I';
                            m = 'N';
                            break;
                        case 2:
                            c = 'X';
                            m = 'N';
                            break;
                    }

                    op += c;
                    mask += m;
                    t /= 3;
                }

                if (ValidMasks.Contains(mask))
                {
                    Console.Write($"{op} {count} ");
                    count++;
                    lines++;

                    op = op.Replace('.', ' ').Trim();
                    op += ' ';
                    int which = 0;
                    int[] len = {0, 0};
                    for (int k = 0; k < op.Length - 1; k++)
                    {
                        switch (op[k])
                        {
                            case 'I':
                                if (op[k + 1] == ' ')
                                    len[which] += 46;
                                else
                                    len[which] += 82;
                                break;

                            case 'X':
                                if (op[k + 1] == ' ')
                                    len[which] += 168;
                                else
                                    len[which] += 204;
                                break;

                            case ' ':
                                which = 1;
                                break;
                        }
                    }

                    Console.WriteLine($"Lengths=[{len[0]}, {len[1]}] Sum=[{len[0] + len[1]}]");
                    bool gotIt = false;
                    foreach (var v in DistinctLengths)
                    {
                        if (v[0] == len[0] && v[1] == len[1])
                            gotIt = true;
                    }

                    if (!gotIt)
                        DistinctLengths.Add(new int[] {len[0], len[1]});
                    if (lines > 40)
                    {
                        //Console.Read();
                        lines = 0;
                    }
                }
            }

            Console.WriteLine("Distinct lengths:");
            foreach (var v1 in DistinctLengths)
            {
                Console.WriteLine($"{v1[0]} {v1[1]}");
            }

            Console.WriteLine("End");
        }

        private static void test22()
        {
            Console.WriteLine($"This is what happens if you try Console WriteLine null --> [{null}]");
            Console.WriteLine($"How to replace the missing result with string NULL --> [{null ?? ("NULL")}]");
        }

        // Compare byte arrays
        private static void test21()
        {
            bool equal = true;
            byte[] a = {1, 2, 3, 4, 5};
            byte[] b = {1, 2, 3, 4, 5};
            if (a.Length != b.Length)
                equal = false;
            for (int len = 0; len < a.Length && equal == true; len++)
                if (a[len] != b[len])
                    equal = false;
            Console.WriteLine("Test with a for loop");
            Console.WriteLine(equal ? "Arrays are equal" : "Arrays are not equal");
            Console.WriteLine("Test with ==");
            Console.WriteLine(a == b ? "Arrays are equal" : "Arrays are not equal");
        }

        static int[] primes =
            {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};

        static int nextPrime(int p)
        {
            for (int i = 0; i < primes.Count() - 1; i++)
            {
                if (primes[i] == p)
                    return primes[i + 1];
            }

            Console.WriteLine($"nextPrime({p}) couldn't return a value");
            return 0;
        }

        static bool isPrime(int p)
        {
            foreach (var v in primes)
            {
                if (p == v)
                    return true;
            }

            return false;
        }

        // Brute force solver for https://puzzling.stackexchange.com/questions/56369/how-many-coins-did-mrs-jones-have
        private static void test20()
        {
            for (int lisaAge = 13; lisaAge <= 19; lisaAge++) // Lisa is teenage
            {
                if (isPrime(lisaAge))
                {
                    for (int jackAge = lisaAge + 2;
                        jackAge < 43;
                        jackAge++) // Lisa is "the teenager" so Jack and Amy must be at least 23
                    {
                        if (isPrime(jackAge) && jackAge == nextPrime(lisaAge))
                        {
                            for (int amyAge = jackAge + 2; amyAge < 43; amyAge++)
                            {
                                if (isPrime(amyAge) && amyAge == nextPrime(jackAge))
                                {
                                    Console.WriteLine($"Testing ages L:{lisaAge},J:{jackAge},A:{amyAge}");
                                    for (int month = 1; month <= 12; month++)
                                    {
                                        if (isPrime(month))
                                        {
                                            for (int lisaDay = 1; lisaDay <= 31; lisaDay++)
                                            {
                                                if (isPrime(lisaDay))
                                                {
                                                    for (int jackDay = 1; jackDay <= 31; jackDay++)
                                                    {
                                                        if (isPrime(jackDay))
                                                        {
                                                            for (int amyDay = 1; amyDay <= 31; amyDay++)
                                                            {
                                                                if (isPrime(amyDay))
                                                                {
                                                                    int lisaCoins = lisaAge + month + lisaDay;
                                                                    int jackCoins = jackAge + month + jackDay;
                                                                    int amyCoins = amyAge + month + amyDay;
                                                                    if (isPrime(lisaCoins) && isPrime(jackCoins) &&
                                                                        isPrime(amyCoins) &&
                                                                        isPrime(lisaCoins + jackCoins + amyCoins))
                                                                    {
                                                                        if (lisaCoins > jackCoins &&
                                                                            lisaCoins > amyCoins)
                                                                        {
                                                                            if (!(lisaCoins == jackCoins ||
                                                                                  lisaCoins == amyCoins ||
                                                                                  jackCoins == amyCoins))
                                                                            {
                                                                                Console.WriteLine(
                                                                                    $"Got possible solution: month={month}, days are L:{lisaDay},J:{jackDay},A:{amyDay}, coins are L:{lisaCoins},J:{jackCoins},A:{amyCoins}, total coins={lisaCoins + jackCoins + amyCoins}");
                                                                                //Console.ReadLine();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Restrict a value using min/max
        private static void test19()
        {
            for (int i = 0; i < 10; i++)
            {
                int least = 2, most = 8;
                int disp = Math.Min(Math.Max(i, least), most);
                Console.WriteLine(i + " -> " + disp);
            }
        }

        // Test18: testing z-order stuff
        // https://stackoverflow.com/questions/3309188/how-to-sort-a-listt-by-a-property-in-the-object/3309230#3309230
        private static void test18()
        {
            var theList = new List<ZedThing>();
            theList.Add(new ZedThing("dog.", 4));
            theList.Add(new ZedThing("brown fox ", 1));
            theList.Add(new ZedThing("the lazy ", 3));
            theList.Add(new ZedThing("The quick ", 0));
            theList.Add(new ZedThing("jumps over ", 2));
            //theList.Add(new ZedThing("", ));

            foreach (var v in theList.OrderBy(o => o.zPos))
            {
                Console.Write(v.message);
            }

            Console.WriteLine("");
        }

        // Is StringBuilder really more performant than string?  1000x10000
        // string test took 63897 ms
        // StringBuilder test took 147 ms
        private static void test17()
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                if ((i % 50) == 0)
                    Console.Write(i);
                string foo = "Hello Bob ";
                for (int j = 0; j < 10000; j++)
                {
                    foo += "and Bob";
                    if ((i % 50) == 0)
                        if ((j % 2000) == 0)
                            Console.Write(".");
                }
            }

            Console.WriteLine($"string test took {sw.ElapsedMilliseconds} ms");
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                if ((i % 50) == 0)
                    Console.Write(i);
                var bar = new StringBuilder("Hello Bob ");
                for (int j = 0; j < 10000; j++)
                {
                    bar.Append("and Bob");
                    if ((i % 50) == 0)
                        if ((j % 2000) == 0)
                            Console.Write(".");
                }
            }

            Console.WriteLine($"StringBuilder test took {sw.ElapsedMilliseconds} ms");
        }

        private static void test16()
        {
            // Is it possible to create a file with a question mark in the name?  Not this way; it throws an exception.
            // System.ArgumentException: Illegal characters in path
            string fileName = "D:\\Hello?World.txt";
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine("Greetings earthling");
            }
        }

        // 10 11 12 13 14 15
        private static void test15()
        {
            var t = new ClockTower();
            var john = new Person("John", t, new List<int> {1030, 1215, 1727});
            var sarah = new Person("Sarah", t, new List<int> {1000, 1300, 1630});
            var abdul = new Person("Abdul", t, new List<int> {725, 1025, 1325, 1625, 1925});
            for (int i = 0; i < 24 * 60; i++)
            {
                int ticks = t.Tick();
                //Console.WriteLine($"At step {i} the time advanced to {ticks}");
            }
        }

        private static void test14()
        {
            Console.WriteLine("List of XYBase");
            var BaseList = new List<XYBase>();
            BaseList.Add(new XYBase());
            BaseList.Add(new XYLevel1A());
            BaseList.Add(new XYLevel1B());
            BaseList.Add(new XYLevel2());

            foreach (XYBase b in BaseList)
            {
                //b.UpSpeak(); // doesn't work: everything says "XYBase UpSpeak"
                XYExtensions.UpSpeak(b as dynamic); // works but isn't in the form "object.verb()"
            }

            var L1AList = new List<XYLevel1A>();
            L1AList.Add(new XYLevel1A());
            L1AList.Add(new XYLevel2());

            Console.WriteLine("List of XYLevel1A foreach XYBase");
            foreach (XYBase b in L1AList)
            {
                //b.UpSpeak();
                XYExtensions.UpSpeak(b as dynamic);
            }

            Console.WriteLine("List of XYLevel1A foreach XYLevel1A");
            foreach (XYLevel1A b in L1AList)
            {
                //b.UpSpeak();
                XYExtensions.UpSpeak(b as dynamic);
            }
        }

        private static void test13()
        {
            string fileName = "";
            for (int i = 0;; i++)
            {
                fileName = String.Format("Z:\\Autogen {0:000}.lgxp", i);
                if (!File.Exists(fileName))
                    break;
            }

            Console.WriteLine($"File '{fileName}' doesn't exist");
        }

        private static void test12()
        {
            String[] ss = {"\nA\n", "  B  ", "\nC  "};
            char[] TrimChars = {' ', '\r', '\n', '\t'};
            //foreach(char c in TrimChars)
            //{
            //    Console.WriteLine(Char.IsWhiteSpace(c) ? "TRUE" : "FALSE");
            //}
            foreach (String p1 in ss)
            {
                string p = p1.Trim();
                //p.TrimStart(TrimChars);
                //p.TrimEnd(TrimChars);
                Console.WriteLine($"p='{p}'");
            }
        }

        private static void test11()
        {
            int foo = 27;
            int.TryParse("bob", out foo);
            Console.WriteLine($"foo after TryParse='{foo}'");
        }

        private static void foo(int x, int y)
        {
            Console.WriteLine($"x={x}, y={y}");
        }

        private static void test10()
        {
            int a = 5;
            Console.WriteLine($"{++a + ++a + ++a + ++a + ++a}");
            foo(y: ++a, x: --a);
            foo(x: ++a, y: --a);
        }

        private static void test9()
        {
            RectangleF Bound = new RectangleF(0, 0, 200, 150);
            float infX = -0.05F * Bound.Width;
            float infY = -0.05F * Bound.Height;
            RectangleF Margin = RectangleF.Inflate(Bound, infX, infY);
            Console.WriteLine($"BoundLRTB = '{Bound.Left} {Bound.Right} {Bound.Top} {Bound.Bottom}'");
            Console.WriteLine($"Inflate by {infX} {infY}");
            Console.WriteLine($"MarginLRTB = '{Margin.Left} {Margin.Right} {Margin.Top} {Margin.Bottom}'");
        }

        private static void test8()
        {
            string s = "     Hello";
            string s1 = s.TrimStart();
            Console.WriteLine($"TrimStart(spc) '{s}' -> '{s1}'");
        }

        private static object GetAnonThing()
        {
            return new {Shop = "Tesco", Age = 27};
        }

        private static void test7()
        {
            //object o = null;
            object o = GetAnonThing();

            string s = (string) o?.GetType().GetProperty("Name")?.GetValue(o, null);
            int a = (int?) o?.GetType().GetProperty("Age")?.GetValue(o, null) ?? 0;
            Console.WriteLine($"Name:{s} Age:{a}");
        }

        private static void test6()
        {
            // CSV preprocesor:
            // (1) quotes and commas  ["a,b",c] -> [a b,c] so that "a,b" means one parameter [a b]
            // (2) Make sure last (11th, 0-based) field is five non-zero digits
            string inFile = "C:\\Users\\dspencer\\Downloads\\MOCK_DATA_1.csv";
            string outFile = "C:\\Users\\dspencer\\Downloads\\MOCK_DATA_2.csv";
            string[] lines = File.ReadAllLines(inFile);
            FileStream of = File.Create(outFile);
            StreamWriter sw = new StreamWriter(of);
            bool inQuotes = false;
            foreach (string Line in lines)
            {
                string outLine = "";
                int field = 0;
                Console.WriteLine($"Starting string: [{Line}]");
                foreach (char c in Line)
                {
                    if (inQuotes)
                    {
                        if (c == '\"')
                            inQuotes = false;
                        else
                        {
                            if (c == ',')
                                outLine += ' '; // replace commas with spaces
                            else
                                outLine += c;
                        }
                    }
                    else
                    {
                        if (c == ',')
                            field++;

                        if (c == '\"')
                        {
                            inQuotes = true;
                            // don't copy quote to outLine
                        }
                        else
                        {
                            if (field == 11 && c == '0')
                                outLine += '1';
                            else
                                outLine += c;
                        }
                    }
                }

                Console.WriteLine($"Ending string  : [{outLine}]");
                sw.WriteLine(outLine);
            }

            sw.Close();
            of.Close();
        }

        private static void test5()
        {
            for (;;)
            {
                Console.WriteLine("Enter string to hash");
                string str = Console.ReadLine();
                using (var sha1 = new SHA1Managed())
                {
                    Console.WriteLine(
                        $"The hash of '{str}' is '{BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "")}'");
                }
            }
        }

        private static void test4()
        {
            Random r = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            string[] Layouts =
            {
                "AA11 AAA", "B111 AAA", "B11 AAA", "B2 AAA", "AAA 2B", "AAA 11B", "AAA 211B", "2111 AA", "2111B",
                "2B", "2AA", "2AAA", "21B", "21AA", "21AAA", "211B", "211 AA", "211 AAA", "B111", "AA 111", "AAA 111",
                "B11", "AA11", "AAA 111", "B2", "B11", "B111", "B1111", "AA 1111", "AAA 1111", "2111 AAA"
            };
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string charsNoOIZU = "ABCDEFGHJKLMNPQRSTVWXY";
            string digits = "0123456789";
            string digitsNo01 = "23456789";
            string newplates = "";

            for (int i = 0; i < 50; i++)
            {
                string layout = Layouts[r.Next(Layouts.Length)];

                foreach (char c in layout)
                {
                    switch (c)
                    {
                        case 'A':
                            newplates += chars[r.Next(chars.Length)];
                            break;

                        case 'B':
                            newplates += charsNoOIZU[r.Next(charsNoOIZU.Length)];
                            break;

                        case '1':
                            newplates += digits[r.Next(digits.Length)];
                            break;

                        case '2':
                            newplates += digitsNo01[r.Next(digitsNo01.Length)];
                            break;

                        case ' ':
                            newplates += ' ';
                            break;
                    }
                }

                if (i < 49)
                    newplates += ',';
            }

            Console.WriteLine(newplates);
        }

        private static void test3()
        {
            string teststr = "";
            for (int i = 129; i <= 255; i++)
            {
                teststr += Convert.ToChar(i);
            }

            Console.WriteLine(teststr);
        }

        private static void test2()
        {
            Rectangle r1 = new Rectangle(10, 15, 100, 150);
            Rectangle r2 = new Rectangle(20, 25, 200, 250);
            r2.Inflate(-50, -50);
            r2.Offset(-60, -60);
            Console.WriteLine("Rectangle r1=" + r1.ToString());
            Console.WriteLine("Rectangle r2=" + r2.ToString());
            if (r1 == r2)
            {
                Console.WriteLine("Rectangles are equal");
            }
            else
            {
                Console.WriteLine("Rectangles are not equal");
            }
        }

        private static void test1([CallerMemberName] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            string DlgText = "+237 ";
            int num;
            int.TryParse(DlgText, out num);
            string TestNum = num.ToString();
            Console.WriteLine($"DlgText=#{DlgText}#; TestNum=#{TestNum}#");
            Console.WriteLine($"DlgText=#{DlgText}#; TestNum=#{TestNum}#");
            Console.WriteLine($"CalledBy={filePath} at line {lineNumber}");
        }
    }
}
