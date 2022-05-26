namespace ProjectTheW
{
    internal class CountDown
    {
        public static float Time { get; private set; } = 0;

        /// <summary>
        /// Получить пройденное время
        /// </summary>
        /// <returns>Время в строчном формате</returns>
        public static string GetTime()
        {
            var seconds = (int)Time % 60;
            var minutes = (int)Time % 3600 / 60;
            return string.Format("{0:d2}:{1:d2}", minutes, seconds);
        }

        public static void TimeTick(float dt) => Time += dt;

        public static void ResetTime() => Time = 0;
    }
}
