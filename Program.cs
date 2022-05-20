using Raylib_cs;

namespace ProjectTheW
{
    static class Program
    {
        public static bool windowAlive = true;
        public static Scene current_scene;
        public static void Main()
        {
            Raylib.InitWindow(1280, 720, "Project The W");
            Raylib.InitAudioDevice();

            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

             /* Для теста полноэкранного режима
            Raylib.SetWindowSize(Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor()),
                Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor()));
            Raylib.ToggleFullscreen();
             */

            current_scene = new Scenes.MenuScene();

            while (windowAlive)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE))
                    current_scene = new Scenes.MenuScene();
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO))
                    current_scene = new Scenes.GameOverScreen();

                current_scene.Update(Raylib.GetFrameTime());
                if (Raylib.WindowShouldClose()) windowAlive = false;

                Raylib.BeginDrawing();
                current_scene.Draw();
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}