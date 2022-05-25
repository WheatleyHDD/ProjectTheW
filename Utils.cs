using Raylib_cs;

namespace ProjectTheW
{
    internal class Utils
    {
        /// <summary>
        /// Получить скейл окна относительно оригинального размера (320x180)
        /// </summary>
        /// <returns>Коэффициэнт скейла</returns>
        public static float GetScale() => Raylib.GetScreenHeight() / 180;

        /// <summary>
        /// Интерполяция значения
        /// </summary>
        /// <param name="from">От какого значения</param>
        /// <param name="to">До какого значения</param>
        /// <param name="delta">Дельта</param>
        /// <returns>Интерполированное значени</returns>
        public static float Lerp(float from, float to, float delta) => from + (to - from) * delta;
    }
}
