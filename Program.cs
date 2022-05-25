using ProjectTheW.Scenes;
using Raylib_cs;

namespace ProjectTheW
{
    static class Program
    {
        public static bool windowAlive = true;
        public static Scene CurrentScene;
        public static PauseSettingAbout PauseAndSetting { get; private set; }
        public static bool Paused { get; set; }

        public static void Main()
        {
            Raylib.InitWindow(1280, 720, "Project The W");
            Raylib.InitAudioDevice();

            // Сбивается UI, но игра запускается без рамок
            // Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_UNDECORATED);

            Raylib.SetTargetFPS(60);

            Raylib.SetExitKey(KeyboardKey.KEY_NULL);

            // Загрузка ресурсов в память
            LoadedTextures.LoadAll();
            LoadedSounds.LoadAll();
            LoadedMusic.LoadAll();

            PauseAndSetting = new PauseSettingAbout();
            CurrentScene = new MenuScene();

            while (windowAlive)
            {
                CurrentScene.Update(Paused? 0 : Raylib.GetFrameTime());
                PauseAndSetting.Update(Raylib.GetFrameTime());

                if (Raylib.WindowShouldClose()) windowAlive = false;

                Raylib.BeginDrawing();
                CurrentScene.Draw();
                PauseAndSetting.Draw();
                Raylib.EndDrawing();
            }

            // Выгрузка ресурсов из памяти
            LoadedTextures.UnloadAll();
            LoadedSounds.UnloadAll();
            LoadedMusic.UnloadAll();

            Raylib.CloseAudioDevice();

            Raylib.CloseWindow();
        }
    }
}