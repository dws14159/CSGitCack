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
using System.Windows;
using System.Xml;

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
            Chime(this, new ClockTowerEventArgs { Time = ret });
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
                            bd = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                            break;
                        case ".GIF":
                            bd = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                            break;
                        case ".JPG":
                        case ".JPEG":
                            bd = new JpegBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                            break;
                        case ".PNG":
                            bd = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
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

    class Program
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

            test39();
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
            bool done=false;
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
            Console.WriteLine(div3 ? $"inputBinary[{inputBinary}] is divisible by 3" : $"inputBinary[{inputBinary}] is not divisible by 3");
        }

        private static void test36()
        {
            string[] fields ={
                "dmDeviceName","dmSpecVersion","dmDriverVersion","dmSize","dmDriverExtra","dmFields","dmOrientation",
"dmPaperSize","dmPaperLength","dmPaperWidth","dmScale","dmCopies","dmDefaultSource","dmPrintQuality",
"dmColor","dmDuplex","dmYResolution","dmTTOption","dmCollate","dmFormName","dmLogPixels","dmBitsPerPel",
"dmPelsWidth","dmPelsHeight","dmDisplayFlags","dmDisplayFrequency","dmICMMethod","dmICMIntent","dmMediaType",
"dmDitherType","dmICCManufacturer","dmICCModel","dmPanningWidth","dmPanningHeight"
            };
            string doll = "$";
            string quot = "\"";
            string brco = "{";
            string brcc = "}";
            foreach (var s in fields)
            {
                Console.WriteLine($"if (mode1.{s} != mode2.{s})");
                Console.WriteLine($"Console.WriteLine({doll}{quot}{s} changed from [{brco}mode1.{s}{brcc}] to [{brco}mode2.{s}{brcc}]{quot});");

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
            string[] fileNames = {"Laser_Charles_Wright.ttf", "LaserCW_MB.ttf", "LaserCW_MB_3D.ttf", "Laser_Charles_Wright_3D.ttf",
                "Laser_Charles_Wright_Inline.ttf", "Laser_Charles_Wright_MC.ttf", "Laser_Charles_Wright_MC_3D.ttf",
                "Laser_Charles_Wright_MC_HL.ttf", "Laser_France.ttf", "Laser_IRL.ttf", "Laser_UK79.ttf"};
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
            var ff = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./resources/#Laser Charles Wright");
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
        static void GetTextGeometryAndFormatting(string Text, string Font, double FontSize, bool Italics, bool Bold, System.Windows.Point Location, System.Windows.Media.Color Colour, out Geometry geom, out FormattedText ft, out System.Windows.Point whitespace)
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
            ft = new FormattedText(measureString, CultureInfo.InvariantCulture, System.Windows.FlowDirection.LeftToRight, tf, FontSize, br1);

            // Problem is: we don't know what the space dimensions are.  So if we draw it at 0,0, then the bounding rectangle will start at x,y which indicates the space dimension.
            var geom1 = ft.BuildGeometry(new System.Windows.Point(0, 0));
            // geom1.Bounds.TopLeft is now the width and height of the whitespace

            // Rebuild the geometry to discard the whitespace and offset by this.(x,y), because for general use elsewhere we want geom.Bounds to indicate the drawn text area
            whitespace = geom1.Bounds.TopLeft; // draw however still needs to be able to take this into account
            System.Windows.Point offset = new System.Windows.Point(whitespace.X * -1 + Location.X, whitespace.Y * -1 + Location.Y);
            geom = ft.BuildGeometry(offset);
        }
        #endregion
        private static void test25()
        {
            Geometry geom;
            FormattedText ft;
            System.Windows.Point whitespace;
            string testStr = "";
            GetTextGeometryAndFormatting(testStr, "Arial", 12, false, false, new System.Windows.Point(0, 0), System.Windows.Media.Colors.Black, out geom, out ft, out whitespace);
            Console.WriteLine($"Width of test string '{testStr}' is {geom.Bounds.Width}");
            Console.WriteLine($"So is this smaller than a rectangle of width 20? [{(geom.Bounds.Width<20).ToString()}]");
        }

        private static void test24()
        {
            // In an infinite loop:
            // - select a random *.png filename from C:\Users\Public\Documents\CCL\lgsystems\Badges
            // - load it with MemLeak.ImageSourceFromFile
            var mem = new MemLeak();
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var files = Directory.GetFiles("C:\\Users\\Public\\Documents\\CCL\\lgsystems\\Badges");
            int count = 0;
            for (;;)
            {
                string file = files[rnd.Next(files.Length)];
                if (Path.GetExtension(file)==".png")
                {
                    var img = mem.ImageSourceFromFile(file);
                    count++;
                    if (count>99)
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
            for (int i=0; i<6561; i++)
            {
                int t = i;
                string op = "";
                string mask = "";
                for (int j=0; j<8; j++)
                {
                    char c = '.';
                    char m = '.';
                    switch (t%3)
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
                    int[] len = { 0, 0 };
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
                    Console.WriteLine($"Lengths=[{len[0]}, {len[1]}] Sum=[{len[0]+len[1]}]");
                    bool gotIt = false;
                    foreach (var v in DistinctLengths)
                    {
                        if (v[0] == len[0] && v[1] == len[1])
                            gotIt = true;
                    }
                    if (!gotIt)
                        DistinctLengths.Add(new int[] { len[0], len[1] });
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
            byte[] a = { 1, 2, 3, 4, 5 };
            byte[] b = { 1, 2, 3, 4, 5 };
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

        static int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

        static int nextPrime(int p)
        {
            for (int i=0; i<primes.Count()-1; i++)
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
                    for (int jackAge = lisaAge + 2; jackAge < 43; jackAge++) // Lisa is "the teenager" so Jack and Amy must be at least 23
                    {
                        if (isPrime(jackAge) && jackAge == nextPrime(lisaAge))
                        {
                            for (int amyAge = jackAge + 2; amyAge < 43; amyAge++)
                            {
                                if (isPrime(amyAge) && amyAge == nextPrime(jackAge))
                                {
                                    Console.WriteLine($"Testing ages L:{lisaAge},J:{jackAge},A:{amyAge}");
                                    for (int month=1; month<=12; month++)
                                    {
                                        if (isPrime(month))
                                        {
                                            for (int lisaDay=1; lisaDay<=31; lisaDay++)
                                            {
                                                if (isPrime(lisaDay))
                                                {
                                                    for (int jackDay=1; jackDay<=31; jackDay++)
                                                    {
                                                        if (isPrime(jackDay))
                                                        {
                                                            for (int amyDay=1; amyDay<=31; amyDay++)
                                                            {
                                                                if (isPrime(amyDay))
                                                                {
                                                                    int lisaCoins = lisaAge + month + lisaDay;
                                                                    int jackCoins = jackAge + month + jackDay;
                                                                    int amyCoins = amyAge + month + amyDay;
                                                                    if (isPrime(lisaCoins) && isPrime(jackCoins) && isPrime(amyCoins) && isPrime(lisaCoins+jackCoins+amyCoins))
                                                                    {
                                                                        if (lisaCoins > jackCoins && lisaCoins > amyCoins)
                                                                        {
                                                                            if (!(lisaCoins == jackCoins || lisaCoins == amyCoins || jackCoins == amyCoins))
                                                                            {
                                                                                Console.WriteLine($"Got possible solution: month={month}, days are L:{lisaDay},J:{jackDay},A:{amyDay}, coins are L:{lisaCoins},J:{jackCoins},A:{amyCoins}, total coins={lisaCoins + jackCoins + amyCoins}");
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
            for (int i=0; i<10; i++)
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
            for (int i=0; i<1000; i++)
            {
                if ((i % 50) == 0)
                    Console.Write(i);
                string foo = "Hello Bob ";
                for (int j=0; j<10000; j++)
                {
                    foo += "and Bob";
                    if ((i%50)==0)
                        if ((j%2000)==0)
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
            var john = new Person("John", t, new List<int> { 1030, 1215, 1727 });
            var sarah = new Person("Sarah", t, new List<int> { 1000, 1300, 1630 });
            var abdul = new Person("Abdul", t, new List<int> { 725, 1025, 1325, 1625, 1925 });
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
            for (int i = 0; ; i++)
            {
                fileName = String.Format("Z:\\Autogen {0:000}.lgxp", i);
                if (!File.Exists(fileName))
                    break;
            }
            Console.WriteLine($"File '{fileName}' doesn't exist");
        }

        private static void test12()
        {
            String[] ss = { "\nA\n", "  B  ", "\nC  " };
            char[] TrimChars = { ' ', '\r', '\n', '\t' };
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
            return new { Shop = "Tesco", Age = 27 };
        }
        private static void test7()
        {
            //object o = null;
            object o = GetAnonThing();

            string s = (string)o?.GetType().GetProperty("Name")?.GetValue(o, null);
            int a = (int?)o?.GetType().GetProperty("Age")?.GetValue(o, null) ?? 0;
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
                    Console.WriteLine($"The hash of '{str}' is '{BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-","")}'");
                }
            }
        }

        private static void test4()
        {
            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            string[] Layouts = {"AA11 AAA","B111 AAA","B11 AAA","B2 AAA","AAA 2B","AAA 11B","AAA 211B","2111 AA","2111B",
                    "2B","2AA","2AAA","21B","21AA","21AAA","211B","211 AA","211 AAA","B111","AA 111","AAA 111",
                    "B11","AA11","AAA 111","B2","B11","B111","B1111","AA 1111","AAA 1111","2111 AAA"};
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
