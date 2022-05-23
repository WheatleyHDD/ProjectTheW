using ProjectTheW.UI;
using Raylib_cs;
using System.Numerics;
using System.Reflection;

namespace ProjectTheW.Scenes
{
    internal class PauseSettingAbout : Scene
    {
        Texture2D panel;
        NPatchInfo panelNpatch;

        string aboutText;
        bool aboutVisible;

        readonly List<Buttons> buttons = new List<Buttons>();
        ToggleButton fsCheck;

        const int MARGIN = 50;
        const int PADDING = 25;

        public bool Visible { get; private set; }
        public bool Fullscreen { get; private set; }

        public PauseSettingAbout() : base() { }

        public override void Ready()
        {
            base.Ready();

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources", "about.txt");
            aboutText = File.ReadAllText(path);
            aboutText = aboutText.Replace("\r", "");

            panel = LoadedTextures.GetTexture("blue_panel");
            panelNpatch = new NPatchInfo
            {
                source = new Rectangle(0, 0, 32, 32),
                left = 6,
                top = 6,
                right = 6,
                bottom = 6,
                layout = NPatchLayout.NPATCH_NINE_PATCH
            };

            CreateButtons();
        }

        void CreateButtons()
        {
            var resume = SettButton(109, 41,
                new Vector2(MARGIN + PADDING, Raylib.GetScreenHeight() - (MARGIN + (15 * Utils.GetScale()) + PADDING)),
                () => {
                    Program.Paused = false;
                    Hide();
                });
            var quit = SettButton(34, 27,
                new Vector2(resume.Destination.x + resume.Destination.width + PADDING,
                Raylib.GetScreenHeight() - (MARGIN + (15 * Utils.GetScale()) + PADDING)),
                () => {
                    Program.Paused = false;
                    Hide();
                    Program.CurrentScene = new MenuScene();
                });
            var about = SettButton(180, 34, new Vector2(MARGIN + PADDING,
                Raylib.GetScreenHeight() - (MARGIN + (15 * Utils.GetScale() + PADDING) * 2)),
                () => { aboutVisible = !aboutVisible; });

            fsCheck = SettCheckButton(214, 15,
                new Vector2(MARGIN + PADDING, MARGIN + PADDING),
                (e) => {
                    if (Fullscreen)
                    {
                        Raylib.ToggleFullscreen();
                        Raylib.SetWindowSize(1280, 720);
                    } else
                    {
                        Raylib.SetWindowSize(Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor()),
                            Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor()));
                        Raylib.ToggleFullscreen();
                    }
                    OnChangeScreen();
                    Program.CurrentScene.OnChangeScreen();
                    Fullscreen = !Fullscreen;
                });

            var volSndMinus = SettButton(165, 15,
                new Vector2(MARGIN + PADDING, fsCheck.Destination.y + fsCheck.Destination.height + PADDING),
                () => {
                    if (LoadedSounds.SetVolume(LoadedSounds.CurrentVolume - 10))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("reset"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                });
            var volSndText = (int)(18 * 1.5f * Utils.GetScale()) +
                Raylib.MeasureText("Sounds Volume: " + LoadedSounds.CurrentVolume + "%", 7 * (int)Utils.GetScale());
            var volSndPlus = SettButton(150, 15,
                new Vector2(MARGIN + PADDING + volSndText, fsCheck.Destination.y + fsCheck.Destination.height + PADDING),
                () => {
                    if (LoadedSounds.SetVolume(LoadedSounds.CurrentVolume + 10))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                });

            var volMusicMinus = SettButton(165, 15,
                new Vector2(MARGIN + PADDING, volSndMinus.Destination.y + volSndMinus.Destination.height + PADDING),
                () => {
                    if (LoadedMusic.SetVolume(LoadedMusic.CurrentVolume - 10))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("reset"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                });
            var volMusicPlus = SettButton(150, 15,
                new Vector2(MARGIN + PADDING + volSndText, volSndMinus.Destination.y + volSndMinus.Destination.height + PADDING),
                () => {
                    if (LoadedMusic.SetVolume(LoadedMusic.CurrentVolume + 10))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                });
        }

        public override void Update(float deltaTime)
        {
            if (!Visible) return;
            base.Update(deltaTime);

            if (Fullscreen) fsCheck.Enable();
            else fsCheck.Reset();

            foreach (var button in buttons.ToArray())
                button.Update();
        }

        public override void Draw()
        {
            if (!Visible) return;
            base.Draw();
            if (!(Program.CurrentScene is GameScene))
                Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()+200, Color.BLACK);
            DrawLeftPanel();
            if (aboutVisible) DrawRightPanel();

            foreach (var button in buttons)
                button.Draw();
        }

        void DrawLeftPanel()
        {
            Raylib.DrawTextureNPatch(panel, panelNpatch,
                new Rectangle(MARGIN, MARGIN, Raylib.GetScreenWidth() / 2 - (MARGIN + PADDING), Raylib.GetScreenHeight() - MARGIN * 2),
                new Vector2(), 0, Color.WHITE);

            Raylib.DrawText("Is Fullscreen?", MARGIN + PADDING + 18 * (int)Utils.GetScale(),
                MARGIN + PADDING + 4 * (int)Utils.GetScale(), 7 * (int)Utils.GetScale(), Color.BLACK);
            Raylib.DrawText("Sounds Volume: " + LoadedSounds.CurrentVolume + "%", MARGIN + PADDING + 18 * (int)Utils.GetScale(),
                MARGIN + PADDING + 4 * (int)Utils.GetScale() + ((int)fsCheck.Destination.height + PADDING) * 1,
                7 * (int)Utils.GetScale(), Color.BLACK);
            Raylib.DrawText("Music Volume: " + LoadedMusic.CurrentVolume + "%", MARGIN + PADDING + 18 * (int)Utils.GetScale(),
                MARGIN + PADDING + 4 * (int)Utils.GetScale() + ((int)fsCheck.Destination.height + PADDING) * 2,
                7 * (int)Utils.GetScale(), Color.BLACK);
        }

        void DrawRightPanel()
        {
            Raylib.DrawTextureNPatch(panel, panelNpatch,
                new Rectangle(Raylib.GetScreenWidth() / 2, MARGIN,
                Raylib.GetScreenWidth() - Raylib.GetScreenWidth() / 2 - MARGIN,
                Raylib.GetScreenHeight() - MARGIN * 2),
                new Vector2(), 0, Color.WHITE);

            var i = -1;
            foreach(string t in aboutText.Split("\n"))
            {
                i++;
                Raylib.DrawText(t, Raylib.GetScreenWidth() / 2 + PADDING, MARGIN + PADDING + 5 * (int)Utils.GetScale() * i,
                    5 * (int)Utils.GetScale(), Color.BLACK);
            }
        }

        public void Show() => Visible = true;
        public void Hide() => Visible = false;

        Button SettButton(int spriteXPos, int bWidth, Vector2 position, OnClick action)
        {
            Dictionary<string, Rectangle> anims = new()
            {
                { "DEFAULT", new Rectangle(spriteXPos, 0, bWidth, 15) },
                { "PRESSED", new Rectangle(spriteXPos, 15, bWidth, 15) }
            };
            var button = new Button(LoadedTextures.GetTexture("menu_buttons"), anims, position,
                Raylib.GetScreenHeight() / 180, action);
            buttons.Add(button);
            return button; 
        }

        ToggleButton SettCheckButton(int spriteXPos, int bWidth, Vector2 position, OnToggleClick action)
        {
            Dictionary<string, Rectangle> anims = new()
            {
                { "DEFAULT", new Rectangle(spriteXPos, 0, bWidth, 15) },
                { "PRESSED", new Rectangle(spriteXPos, 15, bWidth, 15) }
            };
            var button = new ToggleButton(LoadedTextures.GetTexture("menu_buttons"), anims, position,
                Raylib.GetScreenHeight() / 180, action, false);
            buttons.Add(button);
            return button;
        }

        public override void OnChangeScreen()
        {
            base.OnChangeScreen();
            buttons.Clear();
            CreateButtons();
        }
    }
}
