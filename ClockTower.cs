using System;

public class ClockTowerEventArgs : EventArgs
{
    public int Time { get; set; }
}

public delegate void ChimeEventHandler(object sender, ClockTowerEventArgs e);

namespace CSGitCack
{
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
            Chime(this, new ClockTowerEventArgs {Time = ret});
            return ret;
        }
    }
}
