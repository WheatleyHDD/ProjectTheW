using Raylib_cs;

namespace ProjectTheW.Scenes
{
    internal class GameOverScreen : Scene
    {
        float timer = 0;

        public GameOverScreen() => Ready();

        public override void Ready()
        {
            base.Ready();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (timer < 5) timer += deltaTime;
            if (timer > 5)
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                    Program.current_scene = new MenuScene();
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Color.BLACK);

            var goWidth = Raylib.MeasureText("Game Over", 24 * (int)Utils.GetScale());
            Raylib.DrawText("Game Over", Raylib.GetScreenWidth()/2 - goWidth / 2,
                Raylib.GetScreenHeight()/2-24 * (int)Utils.GetScale(),
                24 * (int)Utils.GetScale(), Color.WHITE);

            var scoreWidth = Raylib.MeasureText("Score: 2334728", 12 * (int)Utils.GetScale());
            Raylib.DrawText("Score: 2334728", Raylib.GetScreenWidth() / 2 - scoreWidth / 2,
                Raylib.GetScreenHeight() / 2 + 12 * (int)Utils.GetScale(),
                12 * (int)Utils.GetScale(), Color.WHITE);
        }
    }
}
