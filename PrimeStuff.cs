using System;
using System.Linq;

namespace CSGitCack
{
    public class PrimeStuff
    {
        public static int[] primes =
            {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97};

        public static int nextPrime(int p)
        {
            for (int i = 0; i < primes.Count() - 1; i++)
            {
                if (primes[i] == p)
                    return primes[i + 1];
            }

            Console.WriteLine($"nextPrime({p}) couldn't return a value");
            return 0;
        }

        public static bool isPrime(int p)
        {
            foreach (var v in primes)
            {
                if (p == v)
                    return true;
            }

            return false;
        }
    }
}
