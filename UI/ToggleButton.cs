using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.UI
{
    internal class ToggleButton
    {
        Texture2D texture;

        Dictionary<string, Rectangle> anims;
        Vector2 position;
        readonly float scale;
        public Rectangle Destination { get; private set; }

        public bool Enabled { get; set; }
        readonly OnClick action;

        Rectangle currentFrame;

        public ToggleButton(Texture2D texture, Dictionary<string, Rectangle> spriteAnims,
            Vector2 position, float scale, OnClick onClicked)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            anims = spriteAnims;
            action = onClicked;
            if (anims != null && anims.ContainsKey("DEFAULT"))
                currentFrame = anims.GetValueOrDefault("DEFAULT");
            Destination = new Rectangle(position.X, position.Y, currentFrame.width * scale, currentFrame.height * scale);
        }

        public void Update()
        {
            Destination = new Rectangle(position.X, position.Y, currentFrame.width * scale, currentFrame.height * scale);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Destination))
            {
                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    if (!Enabled) action();
            }
        }

        public void Enable()
        {
            if (anims != null && anims.ContainsKey("PRESSED"))
                currentFrame = anims.GetValueOrDefault("PRESSED");
            Enabled = true;
        }

        public void Reset()
        {
            if (anims != null && anims.ContainsKey("DEFAULT"))
                currentFrame = anims.GetValueOrDefault("DEFAULT");
            Enabled = false;
        }

        public void Draw() => Raylib.DrawTexturePro(texture, currentFrame, Destination, new Vector2(0, 0), 0, Color.WHITE);
    }
}
