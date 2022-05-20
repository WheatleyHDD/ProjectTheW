using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.UI
{
    delegate void OnClick();

    internal class Button
    {
        Texture2D texture;

        Dictionary<string, Rectangle> anims;
        Vector2 position;
        readonly float scale;
        public Rectangle Destination { get; private set; }
        readonly OnClick action;

        Rectangle currentFrame;

        public Button(Texture2D texture, Dictionary<string, Rectangle> spriteAnims,
            Vector2 position, float scale, OnClick onClicked)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            action = onClicked;
            anims = spriteAnims;
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

        public void Draw() => Raylib.DrawTexturePro(texture, currentFrame, Destination, new Vector2(0, 0), 0, Color.WHITE);
    }
}
