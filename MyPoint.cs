namespace CSGitCack
{
    public struct MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
    }
}
