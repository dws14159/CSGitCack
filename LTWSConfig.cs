using System;
using System.IO;
using System.Xml.Serialization;

namespace CSGitCack
{
    public class LTWSConfig
    {
        [XmlElement(Order = 1)] public string FileVersion = "1.0";
        [XmlIgnore] public bool LoadOK;
        [XmlElement(Order = 2)] public string DebugLevel;

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

        [XmlElement(Order = 5)] public string Sentinel; // only used if present
        [XmlElement(Order = 6)] public string ExitSentinel;
        [XmlElement(Order = 7)] public string DeleteOlderThan;
        [XmlElement(Order = 8)] public DataSource SourceDB; // Currently only supports ODBC
        [XmlElement(Order = 9)] public DataSource DestinationDB; // Currently only supports ".Net SqlClient Data Provider" aka "SqlClient";

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
                    var ret = (LTWSConfig) ser.Deserialize(reader);
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
}
