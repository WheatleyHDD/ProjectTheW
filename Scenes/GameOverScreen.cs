using Raylib_cs;

namespace ProjectTheW.Scenes
{
    internal class GameOverScreen : Scene
    {
        float timer = 0;
        string timeElapsed;

        public GameOverScreen() : base() { }

        public override void Ready()
        {
            base.Ready();
            timeElapsed = CountDown.GetTime();
            StatsClass.ResetAll();
            Raylib.PlaySoundMulti(LoadedSounds.GetSound("gameover"));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (timer < 3) timer += deltaTime;
            if (timer > 3)
                if (Raylib.IsKeyReleased(KeyboardKey.KEY_SPACE))
                    Program.CurrentScene = new MenuScene();
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Color.BLACK);

            var goWidth = Raylib.MeasureText("Game Over", 24 * (int)Utils.GetScale());
            Raylib.DrawText("Game Over", Raylib.GetScreenWidth()/2 - goWidth / 2,
                Raylib.GetScreenHeight()/2-24 * (int)Utils.GetScale(),
                24 * (int)Utils.GetScale(), Color.WHITE);

            var scoreWidth = Raylib.MeasureText("Time: " + timeElapsed, 12 * (int)Utils.GetScale());
            Raylib.DrawText("Time: " + timeElapsed, Raylib.GetScreenWidth() / 2 - scoreWidth / 2,
                Raylib.GetScreenHeight() / 2 + 12 * (int)Utils.GetScale(),
                12 * (int)Utils.GetScale(), Color.WHITE);

            if (timer > 3)
            {
                var clickToContinue = Raylib.MeasureText("Click \"Space\" To Continue", 10 * (int)Utils.GetScale());
                Raylib.DrawText("Click \"Space\" To Continue", Raylib.GetScreenWidth() / 2 - clickToContinue / 2,
                    Raylib.GetScreenHeight() - 12 * (int)Utils.GetScale(),
                    10 * (int)Utils.GetScale(), Color.WHITE);
            }
        }
    }
}
