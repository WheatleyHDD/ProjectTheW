using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.UI
{
    delegate void OnClick();
    delegate void OnToggleClick(bool enabled);

    internal class Buttons
    { 
        public virtual void Update() { }
        public virtual void Draw() { }
    }

    internal class Button : Buttons
    {
        Texture2D texture;
        public readonly Vector2 startPosition;

        Dictionary<string, Rectangle> anims;
        public Vector2 position;
        readonly float scale;
        public Rectangle Destination { get; private set; }
        readonly OnClick action;

        Rectangle currentFrame;

        public Button(Texture2D texture, Dictionary<string, Rectangle> spriteAnims,
            Vector2 position, float scale, OnClick onClicked)
        {
            this.texture = texture;
            startPosition = position;
            this.position = position;
            this.scale = scale;
            action = onClicked;
            anims = spriteAnims;
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
                    if (action != null) action();

                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    if (anims != null && anims.ContainsKey("PRESSED"))
                        currentFrame = anims.GetValueOrDefault("PRESSED");
                } else
                {
                    if (anims != null && anims.ContainsKey("DEFAULT"))
                        currentFrame = anims.GetValueOrDefault("DEFAULT");
                }
            } else
            {
                if (anims != null && anims.ContainsKey("DEFAULT"))
                    currentFrame = anims.GetValueOrDefault("DEFAULT");
            }
        }

        public override void Draw() => Raylib.DrawTexturePro(texture, currentFrame, Destination, new Vector2(0, 0), 0, Color.WHITE);
    }
}
