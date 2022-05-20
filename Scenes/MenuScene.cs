using ProjectTheW.UI;
using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Scenes
{
    internal class MenuScene : Scene
    {
        Texture2D buttonWeaponTexture = Raylib.LoadTexture("resources/sprites/menu_weapon_toggles.png");
        Texture2D buttonTexture = Raylib.LoadTexture("resources/sprites/menu_main_buttons.png");

        string logoText = "Project The W";

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
            panel = Raylib.LoadTexture("resources/sprites/panel.png");

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
            bgHue += deltaTime * 4;
            if (bgHue > 360) bgHue -= 360;
            foreach (var button in buttons)
                button.Update();
            foreach (var button in buttonsRightPanel)
                button.Update();
            foreach (var button in buttonsLeftPanel)
                button.Update();
        }

        void DrawLogo(string text, int fontSize) =>
            Raylib.DrawText(text, Raylib.GetScreenWidth() / 2 - text.Length * (fontSize - text.Length) / 2,
                fontSize, fontSize, Color.BLACK);

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Raylib.ColorFromHSV(bgHue, 0.6f, 0.6f));
            DrawLogo(logoText, (int)(36 * Utils.GetScale() / 4));

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
                () => { TakeWeapon(0); });
            buttonsLeftPanel.Add(shootgun);

            Dictionary<string, Rectangle> rifleAnims = new Dictionary<string, Rectangle>();
            rifleAnims.Add("DEFAULT", new Rectangle(0, 0, 32, 32));
            rifleAnims.Add("PRESSED", new Rectangle(0, 32, 32, 32));
            var rifle = new ToggleButton(buttonWeaponTexture, rifleAnims,
                new Vector2(175, 175 + 36 * Utils.GetScale() * 1), Raylib.GetScreenHeight() / 180,
                () => { TakeWeapon(1); });
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
                    if (selectedWeapon < 0) return;
                    StatsClass.WeaponType = selectedWeapon;
                    Raylib.UnloadTexture(buttonTexture);
                    Raylib.UnloadTexture(buttonWeaponTexture);
                    Program.current_scene = new GameScene();
                });
            buttons.Add(playButton);

            // Кнопка "Quit"
            Dictionary<string, Rectangle> exitAnims = new Dictionary<string, Rectangle>();
            exitAnims.Add("DEFAULT", new Rectangle(34, 0, 27, 15));
            exitAnims.Add("PRESSED", new Rectangle(34, 15, 27, 15));
            buttons.Add(new Button(buttonTexture, exitAnims,
                new Vector2(playButton.Destination.x + playButton.Destination.width + 15, Raylib.GetScreenHeight() - 75),
                Raylib.GetScreenHeight() / 180, () => {
                    Raylib.UnloadTexture(buttonTexture);
                    Raylib.UnloadTexture(buttonWeaponTexture);
                    Program.windowAlive = false;
                }));

            // Кнопка "Reset"
            Dictionary<string, Rectangle> resetAnims = new Dictionary<string, Rectangle>();
            resetAnims.Add("DEFAULT", new Rectangle(61, 0, 33, 15));
            resetAnims.Add("PRESSED", new Rectangle(61, 15, 33, 15));
            buttonsRightPanel.Add(new Button(buttonTexture, resetAnims,
                new Vector2(Raylib.GetScreenWidth() - 100 * 2 - 33 * Utils.GetScale() / 2, Raylib.GetScreenHeight() - 100 - 20 * Utils.GetScale()),
                Raylib.GetScreenHeight() / 180, () => { StatsClass.ResetAll(); }));
        }

        void CreateStatAddButtons(Texture2D buttonTexture)
        {
            Dictionary<string, Rectangle> addStatAnim = new Dictionary<string, Rectangle>();
            addStatAnim.Add("DEFAULT", new Rectangle(94, 0, 15, 15));
            addStatAnim.Add("PRESSED", new Rectangle(94, 15, 15, 15));
            OnClick[] actions =
            {
                () => { StatsClass.AddToStat(0); },
                () => { StatsClass.AddToStat(1); },
                () => { StatsClass.AddToStat(2); },
                () => { StatsClass.AddToStat(3); },
            };
            for (int i = 0; i < 4; i++)
            {
                buttonsRightPanel.Add(new Button(buttonTexture, addStatAnim,
                    new Vector2(Raylib.GetScreenWidth() - 100 * 2 - 20 * Utils.GetScale(), 150 + (int)(20 * Utils.GetScale()) * i),
                    Raylib.GetScreenHeight() / 180, actions[i]));
            }
        }
    }
}
