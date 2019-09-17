using System;

namespace CSGitCack
{
    public class ParityStuff
    {
        // Test the ParityChecker stuff
        public enum Parity
        {
            IsEven,
            IsOdd
        };

        public static bool ParityCheck(byte b, Parity type, bool pbit)
        {
            byte bits = 0;
            byte b_orig = b;
            while (b != 0)
            {
                // We don't care about the actual number of bits, just flip this flag each time we encounter a 1
                bits ^= (byte) (b & 1);
                b /= 2;
            }

            // This looks fairly unintuitive. Taking the above example 101001, it has three 1-bits, so in an Even Parity world the parity bit must be a 1.
            // So if pbit==1 we can return true.
            bool ret = pbit == ((bits == 1 && type == Parity.IsEven) || (bits == 0 && type == Parity.IsOdd));
            Console.WriteLine($"In ParityCheck(byte b={b_orig}, Parity type={type.ToString()}, bool pbit=={pbit}); calculated parity bit={bits}; returning {ret}");
            return ret;
        }

        public static bool ParityBit(byte b, Parity type)
        {
            byte bits = 0;
            while (b != 0)
            {
                // We don't care about the actual number of bits, just flip this flag each time we encounter a 1
                bits ^= (byte) (b & 1);
                b /= 2;
            }

            // If we want even parity, then we return 1 if the bit count is odd (so the overall parity is even)
            return (type == Parity.IsEven && bits == 1) || (type == Parity.IsOdd && bits == 0);
        }

        public static int BitCount(int n)
        {
            int ret = 0;
            while (n != 0)
            {
                ret += (n & 1);
                n /= 2;
            }

            return ret;
        }
    }
}
