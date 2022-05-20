using Raylib_cs;
using System.Numerics;

namespace ProjectTheW
{
    internal class Camera
    {
        public Camera2D Cam = new Camera2D();

        public Camera()
        {
            Cam.zoom = 4f;
        }

        public void UpdateCamera(Entity target, int width, int height)
        {
            Cam.target = target.position;
            Cam.offset = new Vector2(width / 2.0f, height / 2.0f);
            float minX = 0, minY = 0, maxX = 768, maxY = 768;

            Vector2 max = Raylib.GetWorldToScreen2D(new Vector2(maxX, maxY), Cam);
            Vector2 min = Raylib.GetWorldToScreen2D(new Vector2(minX, minY), Cam);

            if (max.X < width) Cam.offset.X = width - (max.X - width / 2);
            if (max.Y < height) Cam.offset.Y = height - (max.Y - height / 2);
            if (min.X > 0) Cam.offset.X = width / 2 - min.X;
            if (min.Y > 0) Cam.offset.Y = height / 2 - min.Y;
        }
    }
}
