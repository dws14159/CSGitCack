﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Printing;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ExcelDataReader;
using Microsoft.Win32;
using System.Net;
using Microsoft.Data.Sqlite;

#pragma warning disable IDE1006 // Naming Styles

// UNC paths are not supported.  Defaulting to Windows directory.
// To fix this, go to the project Properties -> Debug, change Working directory to somewhere on a local drive.
// Can't check this in; CSGitCack.csproj.user is a .gitignored file

namespace CSGitCack
{
    //interface MiObservable
    //{
    //    void Notify();
    //}
    //public class ConcreteObservable : MiObservable
    //{
    //    // Non-public method 'Notify' cannot implement method from interface MiObservable
    //    private void Notify() { } 
    //}


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
                test95();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("\n\nHit any key to continue");
                Console.ReadLine();
            }
        }

        // https://www.youtube.com/watch?v=siyw_yBnFww
        // N is a 3-digit number. The digits of N can be swapped around to make five more numbers
        // If those five numbers are added up they make 2022. What is N? Ans: 642
        private static void test95()
        {
            for (int a=1; a<=9; a++)
                for (int b=1; b<=9; b++)
                    for (int c=1; c<=9; c++)
                    {
                        if ((a+2*b+2*c)*100 + 
                            (a*2+b+c*2)*10 +
                            (a*2+b*2+c) == 2022)
                            Console.WriteLine($"Found a solution a={a}, b={b}, c={c}");
                    }
        }
            
        private static void test94()
        {
            // TDF page generator
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine($"Page {i:D2}: &start={15 * i}");
            }
        }

        private static void test93()
        {
            using (var stream = File.Open("C:\\Users\\david.spencer\\Documents\\S3 ID PTS Excel Import Template.xlsx",
                FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                int ix = 0;
                ix++;
                ix++;
                var result = reader.AsDataSet();
                var table0 = result.Tables[0];
                foreach (DataRow row in table0.Rows)
                {
                    string s = "";
                    foreach (var v in row.ItemArray)
                    {
                        s += v.ToString() + " - ";
                    }

                    Console.WriteLine(s);
                    ix++;
                    ix++;
                    ix++;
                    ix++;
                }
                var table1 = result.Tables[1];
                foreach (DataRow row in table1.Rows)
                {
                }
            }
        }
    
        // Benoit's age and house number
        // https://www.youtube.com/watch?v=i3zOm7_e9xU
        private static void test92()
        {
            for (int age = 1; age < 150; age++)
            {
                for (int numHouses = 3; numHouses < 500; numHouses++)
                {
                    for (int bensHouse = 1; bensHouse <= numHouses; bensHouse++)
                    {
                        // Sum of house numbers
                        double SHN = 0.0;
                        for (int i = 1; i <= numHouses; i++)
                            SHN += i;

                        // Deduct Ben's house
                        SHN -= bensHouse;

                        // Average of all houses except Ben's
                        SHN /= (numHouses - 1);

                        // Add Ben's age
                        SHN += age;

                        if (NearlyEqual(SHN, 20.16, 0.0001))
                        {
                            Console.WriteLine($"Found possible solution: age={age}; numHouses={numHouses}; Ben's house={bensHouse}; SHN={SHN}");
                        }
                    }
                }
            }
        }

        private static void test91()
        {
            string filePath = @"C:\YourFile.txt";

            // Create a StreamWriter to write to the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < 10000; i++)
                {
                    writer.WriteLine($"{i:D4}"); // use leading zeros
                }
            }
        }

        // Blood Magic: suppose we have N capacity&augmented capacity runes.
        // What is the maximum number M of capacity runes that gives the highest total capacity?
        // Max is 108 runes and we want others, but we could go all the way up there. But we only need 200k cap at T5
        private static void test90()
        {
            // n is the total number of C and AC runes
            for (double n = 1; n <= 108; n++)
            {
                // Reset maxCap for each new total number of runes
                double maxCap = 0.0;
                double maxC = 0.0, maxAC = 0.0;

                // c is the number of capacity runes
                for (double c = 0; c <= n; c++)
                {
                    // ac is the number of augmented capacity runes
                    double ac = n - c;

                    // Capacity is (10K plus 2000*m) * 1.075^(n-m)
                    double totalCap = (10000.0 + 2000.0 * c) * Math.Pow(1.075, ac);

                    if (totalCap > maxCap)
                    {
                        maxCap = totalCap;
                        maxC = c;
                        maxAC = ac;
                    }
                }

                Console.WriteLine($"For {n} runes the maximum capacity {maxCap} is achieved for {maxC} cap runes and {maxAC} augcap runes");
            }
        }

        private static void test89()
        {
            int page = 0;

            for (int a = 0; a <= 2021; a++)
                for (int b = 0; b <= 2021; b++)
                    for (int c = 0; c <= 2021; c++)
                    {
                        if (a * b + c == 2020 && a + b * c == 2021)
                        {
                            Console.WriteLine($"A={a}; B={b}; C={c}; a*b+c={a*b+c}; a+b*c={a+b*c}");
                            page++;
                            if (page > 30)
                            {
                                page = 0;
                                Console.ReadLine();
                            }
                        }
                    }
        }

        // LC Hard 233 - number of digit 1's
        private static int CountDigitOneBasic(int n)
        {
            int num1s = 0;
            for (int i = 1; i <= n; i++)
            {
                int junk = i;
                while (junk != 0)
                {
                    if ((junk % 10) == 1)
                    {
                        num1s++;
                    }
                    junk /= 10;
                }
            }
            return num1s;
        }

        private static int CountDigitOneVia999NoWorkee(int n)
        {
            int num1s = 0;
            int mult = 1;
            // We separately established there are 300 1's in 1-999 so let's use that.
            if (n>999)
            {
                num1s = 300;
                n /= 1000;
                mult *= 1000;
            }
            while (n>999)
            {
                num1s *= 300*mult;
                n /= 1000;
                mult *= 1000;
            }

            // So let's now count what's left
            for (int i = 1; i <= n; i++)
            {
                int junk = i;
                while (junk != 0)
                {
                    if ((junk % 10) == 1)
                    {
                        num1s+=mult;
                    }
                    junk /= 10;
                }
            }
            return num1s;
        }

        private static int CountDigitOneVia99(int n)
        {
            int num1s = 0;
            int mult = 1;

            // The number of 1s in 00-99 is 20.
            while (n>99)
            {
                num1s += 20 * mult;
                mult *= 100;
                n /= 100;
            }
            if (n < 10) num1s += mult;
            else if (n < 20) num1s += (n - 10) * mult;
            else num1s += (n / 10 - 2) * mult;

            return num1s;
        }

        /* Ladies and gentlemen we have a winner
         * Number of 1s we get from the units digit is what's on its left, plus 1 if the units digit is >=1
         * Number of 1s we get from the 10s digit is what's on its left *10 plus: ???
         * Number of 1s we get from the 100s digit is what's on its left *100 plus: nothing if it's 0; 100 if it's >1; (%100)+1 if it's 1
         *
         * So: number of 1s we get from the N digit is what's on its left * N plus: nothing if it's 0; N if it's >1; (orig % N)+1 if it's 1
         */

        private static int CountDigitOneViaReels(int n)
        {
            int num1s = 0;
            for (int mult=1; mult<=n; mult*=10)
            {
                // Number of 1s we get from the "mult" digit is what's on its left...
                int left = n / (mult * 10);
                num1s += left*mult;

                int right= n % (mult);

                int digit = (n - left * mult * 10 - right) / mult;

                if (digit>1)
                {
                    // ... if digit>1 then plus mult
                    num1s += mult;
                }
                else if (digit==1)
                {
                    // ... if digit==1 then plus (orig%mult)+1
                    num1s += (n % mult) + 1;
                }
            }
            return num1s;
        }

        private static void test85()
        {
            int[] tests = { 0, 1, 2, 15684, 100000 }; //, 15084, 15184, 15101, 9999999, 99999999 };

            using (var logFile = File.AppendText("C:\\test85.log"))
            {
                logFile.AutoFlush = true;

                foreach (int test in tests)
                {
                    Console.WriteLine($"Basic check: Number of 1s up to {test} is {CountDigitOneBasic(test)}");
                    Console.WriteLine($"Reels check: Number of 1s up to {test} is {CountDigitOneViaReels(test)}");
                    Console.WriteLine();

                    logFile.WriteLine($"Basic check: Number of 1s up to {test} is {CountDigitOneBasic(test)}");
                    logFile.WriteLine($"Reels check: Number of 1s up to {test} is {CountDigitOneViaReels(test)}");
                    logFile.WriteLine();
                }
            }
            ;
            //for (int j = 10; j < 100000; j *= 10)
            //{
            //    for (int i = 1; i < 10; i++)
            //    {
            //        Console.WriteLine($"Basic check: Number of 1s up to {i * j} is {CountDigitOneBasic(i * j)}");
            //    }
            //}
            //for (int i = 100; i <= 5000; i += 100)
            //{
            //    Console.WriteLine($"Basic check: Number of 1s up to {i} is {CountDigitOneBasic(i)}");
            //}
            //Console.WriteLine();
            //for (int i = 198; i<212; i++)
            //{
            //    Console.WriteLine($"Basic check: Number of 1s up to {i} is {CountDigitOneBasic(i)}");
            //}

            //Console.WriteLine($"Basic check: Number of 1s up to 9 is {CountDigitOneBasic(9)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 99 is {CountDigitOneBasic(99)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 999 is {CountDigitOneBasic(999)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 9999 is {CountDigitOneBasic(9999)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 99999 is {CountDigitOneBasic(99999)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 999999 is {CountDigitOneBasic(999999)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 9999999 is {CountDigitOneBasic(9999999)}");
            //Console.WriteLine();
            //Console.WriteLine($"Basic check: Number of 1s up to 10 is {CountDigitOneBasic(10)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 100 is {CountDigitOneBasic(100)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 1000 is {CountDigitOneBasic(1000)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 10000 is {CountDigitOneBasic(10000)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 100000 is {CountDigitOneBasic(100000)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 1000000 is {CountDigitOneBasic(1000000)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 10000000 is {CountDigitOneBasic(10000000)}");
            //Console.WriteLine($"Basic check: Number of 1s up to 100000000 is {CountDigitOneBasic(100000000)}");
            //Console.WriteLine();
            //Console.WriteLine($"Basic check: Number of 1s up to 15684 is {CountDigitOneBasic(15684)}");


            //int target = 99;
            //int result = 20;
            //Console.WriteLine($"Basic check: Number of 1s up to {target} is {CountDigitOneBasic(target)} (should be {result})");
            //Console.WriteLine($"Number of 1s up to {target} is {CountDigitOneVia99(target)} (should be {result})");
            //Console.WriteLine();

            //target = 100;
            //result = 21;
            //Console.WriteLine($"Basic check: Number of 1s up to {target} is {CountDigitOneBasic(target)} (should be {result})");
            //Console.WriteLine($"Number of 1s up to {target} is {CountDigitOneVia99(target)} (should be {result})");
            //Console.WriteLine();


            //Console.WriteLine($"Basic check: Number of 1s up to 999 is {CountDigitOneBasic(999)} (should be 300)");

            //Console.WriteLine($"Number of 1s up to 999 is {CountDigitOne(999)} (should be 300)");
            //Console.WriteLine($"Number of 1s up to 1,500,000 is {CountDigitOne(1500000)} (should be ..?)");
            //Console.WriteLine($"Number of 1s up to 13 is {CountDigitOne(13)} (should be 6)");
            //Console.WriteLine($"Number of 1s up to 0 is {CountDigitOne(0)} (should be 0)");
            //Console.WriteLine($"Basic check: Number of 1s up to 1,000,000 is {CountDigitOneBasic(1000000)} (should be 9(0...?)1");
            //Console.WriteLine($"Number of 1s up to 1,000,000 is {CountDigitOne(1000000)} (should be 9(0...?)1");
            //Console.WriteLine($"Number of 1s up to 1,000,000,000 is {CountDigitOne(1000000000)} (should be 9(0...?)1");
        }

        public static int FirstMissingPositive(int[] nums)
        {
            var flags = new int[10000];
            foreach (int num in nums)
            {
                if (num >= 1 && num <= 10000)
                    flags[num - 1] = 1;
            }
            int foundAt = -1;
            for (int i=0; i<10000; i++)
            {
                if (flags[i]==0)
                {
                    foundAt = i + 1;
                    break;
                }
            }
            return foundAt == -1 ? 10001 : foundAt;
        }

        // First missing positive, Leetcode 41
        // Testcase 171 is incorrect because of the constraint "1 <= nums.length <= 10^5" but the input contains 99,999 commas, exceeding the constraint by a factor of 10.
        private static void test84()
        {
            Console.WriteLine($"First missing positive for [1,2,0] is {FirstMissingPositive(new int[] { 1, 2, 0 })}");
            Console.WriteLine($"First missing positive for [3,4,-1,1] is {FirstMissingPositive(new int[] { 3, 4, -1, 1 })}");
            Console.WriteLine($"First missing positive for [7,8,9,11,12] is {FirstMissingPositive(new int[] { 7, 8, 9, 11, 12 })}");
        }

        // Find all positive integers for a,b and c for abc=a+b+c+7
        private static void test83()
        {
            for (int a=1; a<500; a++)
                for (int b=1; b<500; b++)
                    for (int c=1; c<500; c++)
                    {
                        if (a*b*c == a+b+c+7)
                            Console.WriteLine($"a={a}, b={b}, c={c}");
                    }
        }

        // There are 112 grams of powder. Two people need to split the cost. Person #1 takes 3.84 grams each time,
        // person #2 takes 3.04 grams each time until there is no powder left. How much of the bag did each person
        // take, percentage wise maybe?

        // 112/3=38
        private static void test82()
        {
            for (int x=1; x<=38; x++)
            {
                for (int y=1; y<=38; y++)
                {
                    double val = (double)x * 3.84 + (double)y * 3.04;
                    if (NearlyEqual(val, 112, 0.0001))
                    {
                        Console.WriteLine($"Found solution x={x} * 3.84 + y={y} * 3.04 = {val}");
                    }
                }
            }
        }

        private static void test81()
        {
            long cachedSize = 1023 * 1048576;
            long fi_length = 2 * 1048576;
            int MaxMB = 1024;


            if (cachedSize + fi_length > MaxMB * 1048576)
            {
                Console.WriteLine("SmallThing is too big");
            }
            ;
        }

        private static string GetRandomFileName(string template)
        {
            var rng = new Random();
            while(true)
            {
                string filename = string.Format(template, rng.Next(100000, 1000000));
                if (!File.Exists(filename))
                    return filename;
            }
        }

        private static int GapSpotter(List<int> list)
        {
            int current = 1;
            foreach (int ext in list)
            {
                if (current != ext)
                    return current;

                current++;
            }
            return current;
        }

        // Check the gap spotter works
        private static void test80()
        {
            var emptyList = new List<int>();
            Console.WriteLine($"Empty list should return 1 --> {GapSpotter(emptyList)}");

            var List1 = new List<int>() { 1 };
            Console.WriteLine($"List1 should return 2 --> {GapSpotter(List1)}");

            var List12 = new List<int>() { 1, 2 };
            Console.WriteLine($"List12 should return 3 --> {GapSpotter(List12)}");

            var List1245 = new List<int>() { 1, 2, 4, 5 };
            Console.WriteLine($"List1245 should return 3 --> {GapSpotter(List1245)}");

            var List2345 = new List<int>() { 2, 3, 4, 5 };
            Console.WriteLine($"List2345 should return 1 --> {GapSpotter(List2345)}");

            var List12345 = new List<int>() { 1, 2, 3, 4, 5 };
            Console.WriteLine($"List12345 should return 6 --> {GapSpotter(List12345)}");

            var List125689 = new List<int>() { 1, 2, 5, 6, 8, 9 };
            Console.WriteLine($"List125689 should return 3 --> {GapSpotter(List125689)}");
        }


        // Can I cast a boxed double directly to an int?
        // No, it throws System.InvalidCastException - Specified cast is not valid.
        private static void test79()
        {
            double d = 3;
            object boxedDouble = d;

            int unboxedInt = (int)(double)boxedDouble;
            Console.WriteLine($"unboxed int={unboxedInt}");

            var now = DateTime.Now;
            object boxedDate = now;

            double d2 = 3.14159;
            object boxedDouble2 = d2;

            string str = "Hello";
            object boxedStr = str;

            string str1=boxedDate.ToString();
            string str2=boxedDouble2.ToString();
            string str3 = boxedStr.ToString();

            ;
        }

        // Understanding SQLite
        private static void test78()
        {
            try
            {
                var fn = GetRandomFileName("F:\\MiscJunk\\_0\\TempDBFiles\\sqlite{0}.db");
                Console.WriteLine($"File name: {fn}");

                var con = new SqliteConnection("Data Source={fn}");
                var cmd=new SqliteCommand("CREATE TABLE PEOPLE()", con);
                // System.Data.SqlClient System.Data.Sqlite.Core Dapper
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        // Understanding the Registry
        private static void test77()
        {
            ;
            var subKeyPath = "HKEY_CURRENT_USER\\SOFTWARE\\Me\\NoSuchProject";
            var nsp = Registry.GetValue(subKeyPath, "TestValue", 23);
            Console.WriteLine($"Registry setting {subKeyPath}\\TestValue is {nsp ?? "(null)"} - Me\\NoSuchProject doesn't exist, so should be null (we don't get the default value here)");

            subKeyPath = "HKEY_CURRENT_USER\\SOFTWARE\\Me\\CSGitCack";
            Console.WriteLine($"Creating new value {subKeyPath}\\NewValue=1");
            Registry.SetValue(subKeyPath, "NewValue", 1);

            Console.WriteLine($"Check value exists: {Registry.GetValue(subKeyPath, "NewValue", -1)} - should be the created value 1"); // it was

            var nsv = Registry.GetValue(subKeyPath, "NoSuchValue", 45);
            Console.WriteLine($"Registry setting {subKeyPath}\\NoSuchValue is {nsv} - should be 45 this time because Me\\CSGitCack *does* exist"); // it was
        }

        private static void test76()
        {
            // Does List.Add add a copy?
            var myList = new List<SimpleListThing>();

            var myThing = new SimpleListThing()
            {
                id = 1,
                Name = "Bob",
                Flag = false
            };
            Console.WriteLine($"myThing.Flag = {myThing.Flag}");
            myList.Add(myThing);
            // Now modify the thing in the list
            myList[0].Flag = true;
            Console.WriteLine($"myList[0].Flag = {myList[0].Flag}");
            Console.WriteLine($"myThing.Flag = {myThing.Flag} - if true, List contains a reference; if false, List contains a copy"); // It was True!

            // So if I modify myThing that should also change the thing in the list
            myThing.Flag = false;
            Console.WriteLine($"myList[0].Flag = {myList[0].Flag} - should be false"); // It was!
        }

        private static bool NearlyEqual(double a,double b,double epsilon=0.01)
        {
            return Math.Abs(Math.Abs(a) - Math.Abs(b)) < epsilon;
        }

        private static decimal Sqrt(decimal square)
        {
            if (square < 0) return 0;

            decimal root = square / 3;
            int i;
            for (i = 0; i < 32; i++)
                root = (root + square / root) / 2;
            return root;
        }

        private static decimal AplusBsqrtC(decimal a, decimal b, decimal c)
        {
            decimal ret = a + b * Sqrt(c);
            //Console.WriteLine($"AplusBsqrtC(a={a}, b={b}, c={c}) returns {ret}");
            return ret;
        }

        // Calculate from the inside term outwards
        // sqrt(G+F.sqrt(E+D.sqrt(C+B.sqrt(A))))
        // Number of terms must be odd and at least 3
        private static decimal test75a(int limit)
        {
            //Console.WriteLine($"Entering test75a(limit={limit})");

            if (limit<3 || ((limit & 1) == 0))
            {
                //Console.WriteLine("Number of terms must be odd and at least 3");
                return 0;
            }

            // Start with C+B.sqrt(A)
            decimal dlimit = limit;
            decimal total = AplusBsqrtC(dlimit - 2, dlimit - 1, dlimit);

            // The next term is E+D.sqrt(previous result), hence -4
            dlimit -= 4;
            while (dlimit>=0)
            {
                total = AplusBsqrtC(dlimit, dlimit + 1, total);
                // Next term is G+F.sqrt(previous result), hence -2
                dlimit -= 2;
            }
            return total;
        }

        private static void testtest75a()
        {
            Console.WriteLine($"0 should throw an error and return 0: {test75a(0)}\n");
            Console.WriteLine($"1 should throw an error and return 0: {test75a(1)}\n");
            Console.WriteLine($"2 should throw an error and return 0: {test75a(2)}\n");
            Console.WriteLine($"3 should return {1.0+2.0*Math.Sqrt(3.0)}: {test75a(3)}\n");
            Console.WriteLine($"4 should throw an error and return 0: {test75a(4)}\n");
            // .0+.0*Math.Sqrt(.0)
            double res5 = 1.0 + 2.0 * Math.Sqrt(3.0 + 4.0 * Math.Sqrt(5.0));
            Console.WriteLine($"5 should return {res5}: {test75a(5)}\n");
            Console.WriteLine($"6 should throw an error and return 0: {test75a(6)}\n");
            double res7 = 1.0 + 2.0 * Math.Sqrt(3.0 + 4.0 * Math.Sqrt(5.0 + 6.0 * Math.Sqrt(7.0)));
            Console.WriteLine($"7 should return {res7}: {test75a(7)}\n");
            Console.WriteLine($"8 should throw an error and return 0: {test75a(8)}\n");
            double res9 = 1.0 + 2.0 * Math.Sqrt(3.0 + 4.0 * Math.Sqrt(5.0 + 6.0 * Math.Sqrt(7.0 + 8.0 * Math.Sqrt(9.0))));
            Console.WriteLine($"9 should return {res9}: {test75a(9)}\n");
            Console.WriteLine($"10 should throw an error and return 0: {test75a(10)}\n");
            double res11 = 1.0 + 2.0 * Math.Sqrt(3.0 + 4.0 * Math.Sqrt(5.0 + 6.0 * Math.Sqrt(7.0 + 
                8.0 * Math.Sqrt(9.0 + 10.0 * Math.Sqrt(11.0)))));
            Console.WriteLine($"11 should return {res11}: {test75a(11)}\n");
            Console.WriteLine($"12 should throw an error and return 0: {test75a(12)}\n");
            double res13 = 1.0 + 2.0 * Math.Sqrt(3.0 + 4.0 * Math.Sqrt(5.0 + 6.0 * Math.Sqrt(7.0 +
                8.0 * Math.Sqrt(9.0 + 10.0 * Math.Sqrt(11.0 + 12.0 * Math.Sqrt(13.0))))));
            Console.WriteLine($"13 should return {res13}: {test75a(13)}\n");
        }

        // Infinitely nested square roots
        // sqrt(1+2*sqrt(3+4*sqrt(5+6*sqrt(...
        // Problem is: we have to calculate it from the inmost term
        private static void test75()
        {
            //testtest75a();
            for (int i = 5; i < 500; i += 2)
            {
                Console.WriteLine($"test75a({i}) returns {test75a(i)}");
            }
        }

        // What if I'm the one doing a long running op?
        private static async Task<int> test74e(int sleepTime)
        {
            Console.WriteLine("74e: Enter");
            int result = 0;
            await Task.Run(() =>
            {
                Console.WriteLine("74e: In the task");
                for (int i = 0; i < 70; i++)
                {
                    if ((i % 15) == 0)
                        Console.WriteLine($"74e: Loop up to {i}");
                    Thread.Sleep(sleepTime); //simulate long calculation
                    result += i;
                }
                Console.WriteLine("74e: After loop");
            });
            Console.WriteLine("74e: After the task");
            return result;
        }

        private static async Task<int> test74d(int sleepTime)
        {
            Console.WriteLine("74d: Enter");
            int result = 0;
            await Task.Run(() =>
            {
                Console.WriteLine("74d: In the task");
                for (int i = 0; i < 150; i++)
                {
                    if ((i % 20) == 0)
                        Console.WriteLine($"74d: Loop up to {i}");
                    Thread.Sleep(sleepTime); //simulate long calculation
                    result += i;
                }
                Console.WriteLine("74d: After loop");
            });
            Console.WriteLine("74d: After the task");
            return result;
        }

        private static async Task<int> test74c(int sleepTime)
        {
            Console.WriteLine("74c: Enter");
            int result = 0;
            await Task.Run(() =>
            {
                Console.WriteLine("74c: In the task");
                for (int i = 0; i < 100; i++)
                {
                    if ((i % 10) == 0)
                        Console.WriteLine($"74c: Loop up to {i}");
                    Thread.Sleep(sleepTime); //simulate long calculation
                    result += i;
                }
                Console.WriteLine("74c: After loop");
            });
            Console.WriteLine("74c: After the task");
            return result;
        }

        private static async Task<string> test74b()
        {
            Console.WriteLine("In the async");
            var wc = new WebClient();
            Console.WriteLine("Before the await");
            var dsta = await wc.DownloadStringTaskAsync("http://www.google.com");
            Console.WriteLine("After the await");
            int len = dsta.Length;
            return $"{len}";
        }

        private static void test74a()
        {
            // With async/await instead
            int sleepTime = 30;
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Start 74a");
            var taskList = new List<Task<int>>();
            Console.WriteLine("Call 74c");
            taskList.Add(test74c(sleepTime));
            Console.WriteLine("Back from 74c");
            Console.WriteLine("Call 74d");
            taskList.Add(test74d(sleepTime));
            Console.WriteLine("Back from 74d");
            Console.WriteLine("Call 74e");
            taskList.Add(test74e(sleepTime));
            Console.WriteLine("Back from 74e");
            Task.WaitAll(taskList.ToArray());
            sw.Stop();
            long elapsed = sw.ElapsedMilliseconds;
            long expected = sleepTime * 150; // that's d's iterations
            Console.WriteLine($"Expected={expected}, elapsed={elapsed}, diff={elapsed - expected}");

            foreach (Task<int> t in taskList)
            {
                Console.WriteLine($"Tasklist result: {t.Result}");
            }
        }

        private static void test74()
        {
            var wc = new WebClient();
            string page = "";
            var tsk = Task.Run(() => page = wc.DownloadString("http://www.google.com"));
            tsk.Wait();

            Console.WriteLine($"Length is {page.Length}");
        }


        private static void test73a()
        {
            // This is how I'd do it ...
            for (int i = 1; i < 35; i++)
            {
                string output = "";

                if ((i % 3) == 0)
                    output += "Fizz";

                if ((i % 5) == 0)
                    output += "Buzz";

                // ... because being able to add in something like this is much easier
                if ((i % 6) == 0)
                    output += "Gronk";

                if (output == "")
                    output = $"{i}";

                Console.WriteLine(output);
            }
        }

        private static void test73()
        {
            // note: I wouldn't actually do it this way. This is quite inflexible,
            // because these keys could get quite long when we add more features.
            // Fizz       f..f..f..f..f..
            // Buzz       b....b....b....
            // Wazz       w......
            string key = "x..f.bf..fb.f..";
            for (int i = 1; i <= 25; i++)
            {
                switch (key[i % key.Length])
                {
                    case 'f':
                        Console.WriteLine("Fizz");
                        break;
                    case 'b':
                        Console.WriteLine("Buzz");
                        break;
                    case 'x':
                        Console.WriteLine("FizzBuzz");
                        break;
                    default:
                        Console.WriteLine(i);
                        break;
                }
            }
        }

        private static void test72()
        {
            for (int i = 7; i < 12; i++)
            {
                string hex = i.ToString("X2");
                Console.WriteLine($"i:[{i}] hex:[{hex}]");
            }
        }

        private static void test71()
        {
            //var sut = new Thing();
            //sut.ListFields();
            //Console.WriteLine($"Returned query is [{XXXXXThingXXXXX.GetQuery(typeof(XXXXXThingXXXXX))}]");
            //Console.WriteLine($"Returned query is [{XXXXXThingXXXXX.BuildQuery()}]");
            var h = new Holder();
            //Console.WriteLine($"Returned SQL is [{h.UpdateWriter<Thing>("1=1")}]");
            Console.WriteLine($"Returned Select is [{h.SqlWriter<Thing>(SqlTypes.SELECT, "")}]");
            Console.WriteLine($"Returned Select/where is [{h.SqlWriter<Thing>(SqlTypes.SELECT, "bananas are pink")}]");
            Console.WriteLine($"Returned Insert is [{h.SqlWriter<Thing>(SqlTypes.INSERT, "")}]");
            Console.WriteLine($"Returned Update(\"\") is [{h.SqlWriter<Thing>(SqlTypes.UPDATE, "")}]");
            Console.WriteLine($"Returned Update(\"1=1\") is [{h.SqlWriter<Thing>(SqlTypes.UPDATE, "1=1")}]");
        }

        public class Roles
        {
            public const string OverviewOnly = "S3IDOverviewOnlyUser";
            public const string ReadOnlyUser = "S3IDReadOnlyUser";
            public const string MusterController = "S3IDMusterController";
            public const string TagAndMusterManager = "S3IDTagAndMusterManager";
            public const string SystemAdministrator = "S3IDSystemAdministrator";
            public const string EngineeringAdministrator = "S3IDEngineeringAdministrator";
        }

        private static int UserLevel(string role)
        {
            switch (role)
            {
                case Roles.OverviewOnly: return 1;
                case Roles.ReadOnlyUser: return 2;
                case Roles.MusterController: return 3;
                case Roles.TagAndMusterManager: return 4;
                case Roles.SystemAdministrator: return 5;
                case Roles.EngineeringAdministrator: return 6;
                default: return -1;
            }
        }

        public static bool AuthorizedForRole(string role, string UserGroup)
        {
            var currentLevel = UserLevel(UserGroup);
            var desiredLevel = UserLevel(role);
            if (currentLevel == -1 || desiredLevel == -1)
            {
                Console.WriteLine($"Calling AuthorizedForFeature({role})");
                return false;
            }
            else
                return currentLevel >= desiredLevel;
        }

        private static void test70()
        {
            string myGroup;

            myGroup = Roles.SystemAdministrator;
            Console.WriteLine("Can a sysadm get a MC feature? - " + (AuthorizedForRole(Roles.MusterController, myGroup) ? "YES" : "NO"));

            myGroup = Roles.MusterController;
            Console.WriteLine("Can an MC get a sysadm feature? - " + (AuthorizedForRole(Roles.SystemAdministrator, myGroup) ? "YES" : "NO"));

            myGroup = Roles.TagAndMusterManager;
            Console.WriteLine("Can a TAMM get a TAMM feature? - " + (AuthorizedForRole(Roles.TagAndMusterManager, myGroup) ? "YES" : "NO"));

            myGroup = "wibble";
            Console.WriteLine("My group is junk, can I get an OO feature? - " + (AuthorizedForRole(Roles.OverviewOnly, myGroup) ? "YES" : "NO"));
            Console.WriteLine("My group is junk, can I get an SA feature? - " + (AuthorizedForRole(Roles.SystemAdministrator, myGroup) ? "YES" : "NO"));

            myGroup = Roles.MusterController;
            Console.WriteLine("My group is MC, can I get a junk feature? - " + (AuthorizedForRole("wibble", myGroup) ? "YES" : "NO"));
        }

        private static void test69()
        {
            int results = 0;
            for (int a = 1; a <= 2450; a++)
            {
                for (int b = a; b <= 2450; b++)
                {
                    int c = 2450 / (a * b);
                    if (a * b * c == 2450 && a <= b && b <= c && c < 150)
                    {
                        int halfsum = (a + b + c) / 2;
                        if (halfsum == 32)
                        {
                            Console.WriteLine($"Ages {a} {b} {c} prod={a * b * c} sum={a + b + c} halfsum={halfsum}");
                            results++;
                        }
                        // Sum must be twice one of the ages
                        //
                        //if (halfsum == a || halfsum == b || halfsum == c)
                        //{
                        //    Console.WriteLine($"Ages {a} {b} {c} prod={a * b * c} sum={a + b + c}");
                        //    results++;
                        //}
                    }
                }
            }
            Console.WriteLine($"There were {results} results");
        }

        private static void test68()
        {
            int ix = 0;
            ix++;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            ix++;
            var _principal = new WindowsPrincipal(identity);
            ix++;
            ix++;
            ix++;
            ix++;
        }

        private static void test67()
        {
            System.Media.SystemSounds.Beep.Play();
        }

        private static void test66() // need to run VS as Administrator
        {
            using (var searcher = new ManagementObjectSearcher(new SelectQuery("Win32_Keyboard")))
            {
                using (ManagementObjectCollection objs = searcher.Get())
                {
                    foreach (var obj in objs)
                    {
                        if (!(obj is ManagementObject))
                        {
                            continue;
                        }
                        var propData = obj.Properties.Cast<PropertyData>();
                        int props = obj.Properties.Cast<PropertyData>().Count(e => e.Name == "PortName" || e.Name == "InstanceName");
                        if (props != 2) continue;

                        Console.WriteLine("Keyboard Port found: " + obj["PortName"] + "\n" + obj["InstanceName"]);
                        //_logService.LogEntry(0, "Keyboard Port found: " + obj["PortName"] + "\n" + obj["InstanceName"]);
                        if (obj["InstanceName"].ToString().Contains(@"HID\VID_0C27&PID_3BFA"))
                        {
                            Console.WriteLine("Mifare Port added:" + obj["PortName"] + "\n" + obj["InstanceName"]);
                            //_logService.LogEntry(0, "Mifare Port added:" + obj["PortName"] + "\n" + obj["InstanceName"]);
                            //if (_mifarePort == null || (!_mifarePort.IsOpen || _mifarePort.IsDisposed))
                            //{
                            //    _mifarePorts.Add(new DesktopReaderPort(obj["PortName"].ToString()),
                            //        obj["PortName"].ToString());
                            //}
                        }
                    }
                }
            }
        }

        private static void testJson(string hid, string pid)
        {
            var addr = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\S3ID\\S3Configuration", "DesktopReaderUDPAddress", "")?.ToString();
            var port = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\S3ID\\S3Configuration", "DesktopReaderUDPPort", "")?.ToString();
            if (string.IsNullOrEmpty(addr) || string.IsNullOrEmpty(port))
            {
                Console.WriteLine("addr or port is blank");
            }
            Console.WriteLine($"Reg UDP Address = '{addr}'");
            Console.WriteLine($"Reg UDP Port = '{port}'");

            string json = $"{{ \"HID\": \"{hid}\", \"PID\": \"{pid}\" }}";
            Console.WriteLine(json);
        }

        private static void test65()
        {
            testJson("481", "50765");
        }

        private static string SplitLongWord(string s)
        {
            string ret = s;
            if (s.Length > 8)
            {
                ret = "";
                foreach (var c in s)
                {
                    ret += c;
                    ret += '-'; // (char)0xAD; // soft hyphen
                }
            }

            return ret;
        }

        // Split long strings
        private static void test64()
        {
            string pfn = "Herman Barend Cornelis";
            string pln = "Nieuwenhuiduiluiandbobthebuilderzen";
            string s = string.Format("{0} {1}", pfn, pln);
            // string s = "Phthaoulmnoarhquiaunkc The Unpronounceable";
            var s1 = s.Split(' ');
            var s2 = "";
            foreach (var v in s1)
            {
                s2 += SplitLongWord(v) + " ";
            }

            Console.WriteLine(s2);
        }

        private static void test63()
        {
            var ext= new String[]{ "cs","xaml","config","js","resx","tt","wxi","csproj","manifest","settings","wxs","edmx"};
            int n = 1;
            foreach (var str in ext)
            {
                Console.WriteLine($"grep -i \\.{str}[^^a-z] {n}.txt > {n}.{str}.txt");
                Console.WriteLine($"grep -v -i \\.{str}[^^a-z] {n}.txt > {n+1}.txt");
                //Console.WriteLine($"ext='{str}'");
                n++;
            }
        }

        private static void test62()
        {
            string code = "RFID";
            string technologyCode = "RFID";
            if (code == technologyCode)
                Console.WriteLine("true");
        }

        private static void test61()
        {
            Console.WriteLine("test58 entry");
            Console.WriteLine("test58 exit");
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
                String myConnectionString =
                    "Data Source=devsql01\\sitedb;Initial Catalog=ATS;User Id=dev; Password=Automation15;Connect Timeout=5;Connection Lifetime=30;Pooling=false";
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
            return DBInterface.getTotalNumber(command);
        }

        private static int test60d() // Does not leak
        {
            try
            {
                var dt = DBInterface.getDataTable("SELECT name,dob,company,position,thumbnail FROM viewpersonneltagunacknowledge");
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
                DataRetreiver_Test.retreiveData(pobUnAcknowledgedGridView, pobOnSiteGridView);
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

        private static void test59a()
        {
            for (int i = 0; i < 10; i++)
            {
                string bitReport = "";
                int numBits = ParityStuff.BitCount(i);
                switch (numBits)
                {
                    case 0:
                        bitReport = "no '1' bits";
                        break;
                    case 1:
                        bitReport = "one '1' bit";
                        break;
                    default:
                        bitReport = $"{numBits} '1' bits";
                        break;
                }

                Console.WriteLine(
                    $"Number {i} has {bitReport}; PE bit {ParityStuff.ParityBit((byte) i, ParityStuff.Parity.IsEven)}; PO bit {ParityStuff.ParityBit((byte) i, ParityStuff.Parity.IsOdd)}");
            }
        }

        private static void test59()
        {
            byte[] arr;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        arr = new byte[] {0b00000011, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b11000000};
                        break;
                    case 1:
                        arr = new byte[] {0b00000100, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b01000000};
                        break;
                    case 2:
                        arr = new byte[] {0b00000110, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b00000000};
                        break;
                    case 3:
                        arr = new byte[] {0b00001000, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b10000000};
                        break;
                    case 4:
                        arr = new byte[] {0b00001010, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b11000000};
                        break;
                    default:
                        arr = new byte[] {0b00001101, 0b00000000, 0b00000000, 0b01010110, 0b00010110, 0b01000000};
                        break;
                }

                byte pe = (byte) ((arr[0] & 0b10110110) ^ (arr[1] & 0b01101101) ^ (arr[2] & 0b11011011) ^ (arr[3] & 0b10110110) ^ (arr[4] & 0b01101101) ^ (arr[5] & 0b00011011));
                byte po = (byte) ((arr[0] & 0b01101100) ^ (arr[1] & 0b11011011) ^ (arr[2] & 0b10110110) ^ (arr[3] & 0b01101101) ^ (arr[4] & 0b11011011) ^ (arr[5] & 0b00110110));
                byte po2 = (byte) (arr[0] ^ arr[1] ^ arr[2] ^ arr[3] ^ arr[4] ^ (arr[5] & 0x7f));
                Console.WriteLine(
                    $"Bits {i}: pe={ParityStuff.ParityCheck(pe, ParityStuff.Parity.IsEven, (arr[5] & 64) == 64)}; po={ParityStuff.ParityCheck(po, ParityStuff.Parity.IsOdd, (arr[0] & 1) == 1)}; po2={ParityStuff.ParityCheck(po2, ParityStuff.Parity.IsOdd, (arr[5] & 128) == 128)}");
            }
        }

        private static void test58()
        {
            byte val1 = 0x81;
            Console.WriteLine("Val1=" + val1);

            sbyte val2 = 64;
            val2 <<= 1;
            val2++;
            Console.WriteLine("Val2=" + val2);

            int val3 = (int) val2;
            Console.WriteLine("Val3=" + val3);
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
            var tokens = Conversions.RecombineQuotedStrings(task.Split(','));
            foreach (var t in tokens)
            {
                Console.WriteLine(t);
            }
        }

        // Are these two blocks identical?
        // Loop can be converted into LINQ-expression
        private static void test56()
        {
            var LHS = new List<string>() {"A", "B", "C", "D", "E"};
            var RHS = new List<string>() {"1", "2", "3", "4", "5"};
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
                cfg = (LTWSConfig) ser.Deserialize(reader);
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
                    DestTables = "foo,bar",
                    QueryText = "wibble"
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
            double c = (double) a / (double) b; // R# says cast is redundant but we get the right result 2.5
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

            Byte[] hello = {72, 101, 108, 108, 111, 32, 119, 111, 114, 108, 100};
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
            for (;;)
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
            for (;;)
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
            for (;;)
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
                cfg = (Config) ser.Deserialize(reader);
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

        //private static void test25()
        //{
        //    Geometry geom;
        //    FormattedText ft;
        //    System.Windows.Point whitespace;
        //    string testStr = "";
        //    Conversions.GetTextGeometryAndFormatting(testStr, "Arial", 12, false, false, new System.Windows.Point(0, 0),
        //        System.Windows.Media.Colors.Black, out geom, out ft, out whitespace);
        //    Console.WriteLine($"Width of test string '{testStr}' is {geom.Bounds.Width}");
        //    Console.WriteLine(
        //        $"So is this smaller than a rectangle of width 20? [{(geom.Bounds.Width < 20).ToString()}]");
        //}

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


        // Brute force solver for https://puzzling.stackexchange.com/questions/56369/how-many-coins-did-mrs-jones-have
        private static void test20()
        {
            for (int lisaAge = 13; lisaAge <= 19; lisaAge++) // Lisa is teenage
            {
                if (PrimeStuff.isPrime(lisaAge))
                {
                    for (int jackAge = lisaAge + 2;
                        jackAge < 43;
                        jackAge++) // Lisa is "the teenager" so Jack and Amy must be at least 23
                    {
                        if (PrimeStuff.isPrime(jackAge) && jackAge == PrimeStuff.nextPrime(lisaAge))
                        {
                            for (int amyAge = jackAge + 2; amyAge < 43; amyAge++)
                            {
                                if (PrimeStuff.isPrime(amyAge) && amyAge == PrimeStuff.nextPrime(jackAge))
                                {
                                    Console.WriteLine($"Testing ages L:{lisaAge},J:{jackAge},A:{amyAge}");
                                    for (int month = 1; month <= 12; month++)
                                    {
                                        if (PrimeStuff.isPrime(month))
                                        {
                                            for (int lisaDay = 1; lisaDay <= 31; lisaDay++)
                                            {
                                                if (PrimeStuff.isPrime(lisaDay))
                                                {
                                                    for (int jackDay = 1; jackDay <= 31; jackDay++)
                                                    {
                                                        if (PrimeStuff.isPrime(jackDay))
                                                        {
                                                            for (int amyDay = 1; amyDay <= 31; amyDay++)
                                                            {
                                                                if (PrimeStuff.isPrime(amyDay))
                                                                {
                                                                    int lisaCoins = lisaAge + month + lisaDay;
                                                                    int jackCoins = jackAge + month + jackDay;
                                                                    int amyCoins = amyAge + month + amyDay;
                                                                    if (PrimeStuff.isPrime(lisaCoins) && PrimeStuff.isPrime(jackCoins) &&
                                                                        PrimeStuff.isPrime(amyCoins) &&
                                                                        PrimeStuff.isPrime(lisaCoins + jackCoins + amyCoins))
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
