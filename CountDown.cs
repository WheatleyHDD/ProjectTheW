namespace ProjectTheW
{
    internal class CountDown
    {
        static float time = 0;

        public static string GetTime()
        {
            var seconds = (int)time % 60;
            var minutes = (int)time % 3600 / 60;
            return string.Format("{0:d2}:{1:d2}", minutes, seconds);
        }

        public static void TimeTick(float dt) => time += dt;

        public static void ResetTime() => time = 0;
    }
}
