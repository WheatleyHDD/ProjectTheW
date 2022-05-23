using Raylib_cs;

namespace ProjectTheW
{
    internal class Utils
    {
        /// <summary>
        /// Интерполяция угла (в радианах)
        /// </summary>
        /// <param name="p_from">от</param>
        /// <param name="p_to">до</param>
        /// <param name="p_weight">вес</param>
        /// <returns></returns>
        static public float LerpAngle(float p_from, float p_to, float p_weight)
        {
            float difference = p_to - p_from % (float)Math.Tau;
            float distance = (2.0f * difference % (float)Math.Tau) - difference;
            return p_from + distance * p_weight;
        }

        public static float GetScale() => Raylib.GetScreenHeight() / 180;

        public static float Lerp(float from, float to, float delta) => from + (to - from) * delta;
    }
}
