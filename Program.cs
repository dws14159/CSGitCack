using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// UNC paths are not supported.  Defaulting to Windows directory.
// To fix this, go to the project Properties -> Debug, change Working directory to somewhere on a local drive.
// Can't check this in; CSGitCack.csproj.user is a .gitignored file

namespace CSGitCack
{
    class Program
    {
        static void Main(string[] args)
        {
            test3();
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
