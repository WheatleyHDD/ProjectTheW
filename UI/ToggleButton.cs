using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.UI
{
    internal class ToggleButton : Buttons
    {
        Texture2D texture;

        Dictionary<string, Rectangle> anims;
        Vector2 position;
        readonly float scale;
        public Rectangle Destination { get; private set; }

        public bool Enabled { get; set; }
        readonly OnToggleClick action;
        bool changeValue;

        Rectangle currentFrame;

        public ToggleButton(Texture2D texture, Dictionary<string, Rectangle> spriteAnims,
            Vector2 position, float scale, OnToggleClick onClicked, bool changeValue)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            this.changeValue = changeValue;
            anims = spriteAnims;
            action = onClicked;
            if (anims != null && anims.ContainsKey("DEFAULT"))
                currentFrame = anims.GetValueOrDefault("DEFAULT");
            Destination = new Rectangle(position.X, position.Y, currentFrame.width * scale, currentFrame.height * scale);
        }

        public override void Update()
        {
            Destination = new Rectangle(position.X, position.Y, currentFrame.width * scale, currentFrame.height * scale);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Destination))
            {
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    action(Enabled);
                    if (changeValue)
                        if (!Enabled) Enable();
                        else Reset();
                }
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

        public override void Draw() => Raylib.DrawTexturePro(texture, currentFrame, Destination, new Vector2(0, 0), 0, Color.WHITE);
    }
}
