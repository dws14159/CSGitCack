using System;

namespace CSGitCack
{
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
}
