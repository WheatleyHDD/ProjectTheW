using ProjectTheW.UI;
using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Scenes
{
    internal class MenuScene : Scene
    {
        Texture2D buttonWeaponTexture = LoadedTextures.GetTexture("menu_wbuttons");
        Texture2D buttonTexture = LoadedTextures.GetTexture("menu_buttons");

        string logoText = "Project The W";
        Music music = LoadedMusic.GetMusic("main_menu");

        Texture2D panel;
        NPatchInfo panelNpatch;

        float bgHue = 0;
        int selectedWeapon = -1;

        readonly List<Button> buttons = new List<Button>();
        readonly List<Button> buttonsRightPanel = new List<Button>();
        readonly List<ToggleButton> buttonsLeftPanel = new List<ToggleButton>();

        public MenuScene() : base() { }

        public override void Ready()
        {
            base.Ready();
            CountDown.ResetTime();
            Raylib.PlayMusicStream(music);
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

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Raylib.UpdateMusicStream(music);
            bgHue += deltaTime * 4;
            if (bgHue > 360) bgHue -= 360;
            foreach (var button in buttons)
                button.Update();
            foreach (var button in buttonsRightPanel)
                button.Update();
            foreach (var button in buttonsLeftPanel)
                button.Update();
        }

        void DrawLogo(string text, int fontSize)
        {
            var mText = Raylib.MeasureText(text, fontSize);
            Raylib.DrawText(text, Raylib.GetScreenWidth() / 2 - mText / 2,
                50 - fontSize / (int)Utils.GetScale() * 2, fontSize, Color.BLACK);
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Raylib.ColorFromHSV(bgHue, 0.6f, 0.6f));
            DrawLogo(logoText, (int)(12 * Utils.GetScale()));

            DrawLeftPanel();
            if (selectedWeapon >= 0) DrawRightPanel();

            // Кнопки
            foreach (var button in buttons)
                button.Draw();
        }

        void DrawLeftPanel()
        {
            Raylib.DrawTextureNPatch(panel, panelNpatch,
                new Rectangle(100, 100, Raylib.GetScreenWidth() / 3 - 125, Raylib.GetScreenHeight() - 100 * 2),
                new Vector2(), 0, Color.WHITE);
            foreach (var button in buttonsLeftPanel)
                button.Draw();
        }

        void DrawRightPanel()
        {
            Raylib.DrawTextureNPatch(panel, panelNpatch,
                new Rectangle(Raylib.GetScreenWidth() / 3, 100,Raylib.GetScreenWidth() - Raylib.GetScreenWidth() / 3 - 100, Raylib.GetScreenHeight() - 100 * 2),
                new Vector2(), 0, Color.WHITE);

            Raylib.DrawText("Bullet Damage (1 + " + (StatsClass.BulletDamage - 1) + ")", Raylib.GetScreenWidth() / 3 + 75,
                175 + (int)(75 * Utils.GetScale() / 4) * 0, (int)(24 * Utils.GetScale() / 4), Color.BLACK);
            Raylib.DrawText("Bullet Speed (300 + " + (StatsClass.BulletSpeed - 300) + ")", Raylib.GetScreenWidth() / 3 + 75,
                175 + (int)(75 * Utils.GetScale() / 4) * 1, (int)(24 * Utils.GetScale() / 4), Color.BLACK);
            Raylib.DrawText("Bullet Knockback (0 + " + StatsClass.BulletPush + ")", Raylib.GetScreenWidth() / 3 + 75,
                175 + (int)(75 * Utils.GetScale() / 4) * 2, (int)(24 * Utils.GetScale() / 4), Color.BLACK);
            Raylib.DrawText("Player Speed (190 + " + (StatsClass.PlayerSpeed - 190) + ")", Raylib.GetScreenWidth() / 3 + 75,
                175 + (int)(75 * Utils.GetScale() / 4) * 3, (int)(24 * Utils.GetScale() / 4), Color.BLACK);

            Raylib.DrawText("Health: " + StatsClass.PlayerHealth, Raylib.GetScreenWidth() / 3 + 75,
                Raylib.GetScreenHeight() - (100 + (int)(42 * Utils.GetScale() / 4)), (int)(24 * Utils.GetScale() / 4), Color.BLACK);

            foreach (var button in buttonsRightPanel)
                button.Draw();
        }

        /// <summary>
        /// Создать кнопки
        /// </summary>
        void CreateButtons()
        {
            CreateMainButtons(buttonTexture);
            CreateWeaponButtons();
            // Кнопки добавления стат
            CreateStatAddButtons(buttonTexture);
        }

        void TakeWeapon(int type)
        {
            selectedWeapon = type;
            foreach (var w in buttonsLeftPanel)
                w.Reset();
            buttonsLeftPanel[selectedWeapon].Enable();
        }

        void CreateWeaponButtons()
        {
            Dictionary<string, Rectangle> shootgunAnims = new Dictionary<string, Rectangle>();
            shootgunAnims.Add("DEFAULT", new Rectangle(32, 0, 32, 32));
            shootgunAnims.Add("PRESSED", new Rectangle(32, 32, 32, 32));
            var shootgun = new ToggleButton(buttonWeaponTexture, shootgunAnims,
                new Vector2(175, 175 + 36 * Utils.GetScale() * 0), Raylib.GetScreenHeight() / 180,
                (x) => {
                    if (x) return;
                    Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    TakeWeapon(0);
                }, false);
            buttonsLeftPanel.Add(shootgun);

            Dictionary<string, Rectangle> rifleAnims = new Dictionary<string, Rectangle>();
            rifleAnims.Add("DEFAULT", new Rectangle(0, 0, 32, 32));
            rifleAnims.Add("PRESSED", new Rectangle(0, 32, 32, 32));
            var rifle = new ToggleButton(buttonWeaponTexture, rifleAnims,
                new Vector2(175, 175 + 36 * Utils.GetScale() * 1), Raylib.GetScreenHeight() / 180,
                (x) => {
                    if (x) return;
                    Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade")); 
                    TakeWeapon(1);
                }, false);
            buttonsLeftPanel.Add(rifle);
        }

        void CreateMainButtons(Texture2D buttonTexture)
        {
            // Кнопка "Play"
            Dictionary<string, Rectangle> playAnims = new Dictionary<string, Rectangle>();
            playAnims.Add("DEFAULT", new Rectangle(0, 0, 34, 15));
            playAnims.Add("PRESSED", new Rectangle(0, 15, 34, 15));
            var playButton = new Button(buttonTexture, playAnims, new Vector2(100, Raylib.GetScreenHeight() - 75),
                Raylib.GetScreenHeight() / 180, () => {
                    if (selectedWeapon < 0)
                    {
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                        return;
                    }
                    Raylib.StopMusicStream(music);
                    StatsClass.WeaponType = selectedWeapon;
                    Program.CurrentScene = new GameScene();
                });
            buttons.Add(playButton);

            // Кнопка "Quit"
            Dictionary<string, Rectangle> exitAnims = new Dictionary<string, Rectangle>();
            exitAnims.Add("DEFAULT", new Rectangle(34, 0, 27, 15));
            exitAnims.Add("PRESSED", new Rectangle(34, 15, 27, 15));
            var exitButton = new Button(buttonTexture, exitAnims,
                new Vector2(playButton.Destination.x + playButton.Destination.width + 15, Raylib.GetScreenHeight() - 75),
                Raylib.GetScreenHeight() / 180, () => { Program.windowAlive = false; });
            buttons.Add(exitButton);

            // Кнопка "Settings"
            Dictionary<string, Rectangle> settAnims = new Dictionary<string, Rectangle>();
            settAnims.Add("DEFAULT", new Rectangle(229, 0, 50, 15));
            settAnims.Add("PRESSED", new Rectangle(229, 15, 50, 15));
            buttons.Add(new Button(buttonTexture, settAnims,
                new Vector2(exitButton.Destination.x + exitButton.Destination.width + 15, Raylib.GetScreenHeight() - 75),
                Raylib.GetScreenHeight() / 180, () => { Program.PauseAndSetting.Show(); }));

            // Кнопка "Reset"
            Dictionary<string, Rectangle> resetAnims = new Dictionary<string, Rectangle>();
            resetAnims.Add("DEFAULT", new Rectangle(61, 0, 33, 15));
            resetAnims.Add("PRESSED", new Rectangle(61, 15, 33, 15));
            buttonsRightPanel.Add(new Button(buttonTexture, resetAnims,
                new Vector2(Raylib.GetScreenWidth() - 100 * 2 - 33 * Utils.GetScale() / 2, Raylib.GetScreenHeight() - 100 - 20 * Utils.GetScale()),
                Raylib.GetScreenHeight() / 180, () => {
                    Raylib.PlaySoundMulti(LoadedSounds.GetSound("reset"));
                    StatsClass.ResetAll();
                }));
        }

        void CreateStatAddButtons(Texture2D buttonTexture)
        {
            Dictionary<string, Rectangle> addStatAnim = new Dictionary<string, Rectangle>();
            addStatAnim.Add("DEFAULT", new Rectangle(94, 0, 15, 15));
            addStatAnim.Add("PRESSED", new Rectangle(94, 15, 15, 15));
            OnClick[] actions =
            {
                () => {
                    if (StatsClass.AddToStat(0))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                },
                () => {
                    if (StatsClass.AddToStat(1))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                },
                () => {
                    if (StatsClass.AddToStat(2))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                },
                () => {
                    if (StatsClass.AddToStat(3))
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("upgrade"));
                    else
                        Raylib.PlaySoundMulti(LoadedSounds.GetSound("error"));
                },
            };
            for (int i = 0; i < 4; i++)
            {
                buttonsRightPanel.Add(new Button(buttonTexture, addStatAnim,
                    new Vector2(Raylib.GetScreenWidth() - 100 * 2 - 20 * Utils.GetScale(), 150 + (int)(20 * Utils.GetScale()) * i),
                    Raylib.GetScreenHeight() / 180, actions[i]));
            }
        }
        public override void OnChangeScreen()
        {
            base.OnChangeScreen();
            buttons.Clear();
            buttonsLeftPanel.Clear();
            buttonsRightPanel.Clear();
            CreateButtons();
        }
    }
}
