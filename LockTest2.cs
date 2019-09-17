using System.Collections.Generic;

namespace CSGitCack
{
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
}
