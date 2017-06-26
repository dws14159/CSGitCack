using System;
using System.Collections.Generic;
using System.Linq;
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
            test1();
        }

        private static void test1()
        {
            string DlgText = "+237 ";
            int num;
            int.TryParse(DlgText, out num);
            string TestNum = num.ToString();
            Console.WriteLine($"DlgText=#{DlgText}#; TestNum=#{TestNum}#");
        }
    }
}
