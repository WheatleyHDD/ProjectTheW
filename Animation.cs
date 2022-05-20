namespace ProjectTheW
{
    internal class Animation
    {
        public int Line { get; private set; }
        public int MaxFrame { get; private set; }

        public Animation(int line, int maxFrame)
        {
            Line = line;
            MaxFrame = maxFrame;
        }
    }
}
