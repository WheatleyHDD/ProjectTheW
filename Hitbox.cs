using System.Numerics;
using Raylib_cs;

namespace ProjectTheW
{
    internal class Hitbox
    {
        public Vector2 Position { get; set; }
        public readonly Vector2 Size;

        public Hitbox(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public void DebugDraw(Color debugColor)
        {
            Rectangle rec = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
            Raylib.DrawRectangleLinesEx(rec, 1, debugColor);
        }
    }
}
