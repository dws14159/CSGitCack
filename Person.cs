using System;
using System.Collections.Generic;

namespace CSGitCack
{
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
}
