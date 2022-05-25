using Raylib_cs;
using System.Numerics;

namespace ProjectTheW
{
    internal class Camera
    {
        public Camera2D Cam = new Camera2D();

        float shakeTimer = 0;
        int shakeIntensity = 0;

        float actualZoom;

        public Camera(Vector2 startPos)
        {
            Cam.target = startPos;
            Cam.zoom = 20;
            UpdateZoom();
        }

        /// <summary>
        /// Обновление положения камеры
        /// </summary>
        /// <param name="player">игрок</param>
        /// <param name="dt">дельта</param>
        /// <param name="width">Длина комнаты</param>
        /// <param name="height">Ширина комнаты</param>
        public void UpdateCamera(Entity player, float dt, int width, int height)
        {
            if (shakeTimer > 0)
                shakeTimer -= dt;
            else
                shakeIntensity = 0;

            Cam.zoom = Utils.Lerp(Cam.zoom, actualZoom, 5 * dt);

            Cam.offset = new Vector2(
                width / 2.0f + Raylib.GetRandomValue(-shakeIntensity, shakeIntensity),
                height / 2.0f + Raylib.GetRandomValue(-shakeIntensity, shakeIntensity)
            );
            float minX = 0, minY = 0, maxX = 768, maxY = 768;

            Cam.target.X = Utils.Lerp(Cam.target.X, player.position.X, 16 * dt);
            Cam.target.Y = Utils.Lerp(Cam.target.Y, player.position.Y, 16 * dt);

            Vector2 max = Raylib.GetWorldToScreen2D(new Vector2(maxX, maxY), Cam);
            Vector2 min = Raylib.GetWorldToScreen2D(new Vector2(minX, minY), Cam);

            if (max.X < width) Cam.offset.X = width - (max.X - width / 2);
            if (max.Y < height) Cam.offset.Y = height - (max.Y - height / 2);
            if (min.X > 0) Cam.offset.X = width / 2 - min.X;
            if (min.Y > 0) Cam.offset.Y = height / 2 - min.Y;
        }

        public void UpdateZoom() => actualZoom = Utils.GetScale() * 3.5f / 4f;

        public void Shake(float time, int intensity)
        {
            shakeTimer = time;
            shakeIntensity = intensity;
        }
    }
}
