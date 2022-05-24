using Raylib_cs;
using System.Numerics;
using ProjectTheW.UI;
using ProjectTheW.Objects;

namespace ProjectTheW.Scenes
{
    internal class Upgrader : Scene
    {
        Player player;

        Texture2D panel;
        NPatchInfo panelNpatch;
        float panelY = Raylib.GetScreenHeight() + MARGIN;

        List<Button> buttons = new List<Button>();

        public bool Visible { get; private set; }

        readonly string[] texts =
        {
            "Bullet Damage ({0} + 1)",
            "Bullet Speed ({0} + 50)",
            "Bullet Knockback ({0} + 1)",
            "Player Speed ({0} + 10)",
            "Player Health ({0} + 1)",
            "Add {0} Ammo"
        };

        OnClick[] acts = new OnClick[6];

        const int MARGIN = 100;
        const int PADDING = 25;

        public Upgrader(Player player) : base() => this.player = player;

        public override void Ready()
        {
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

            acts[0] = () => {
                StatsClass.AddToStatInGame(0);
                Hide();
            };
            acts[1] = () => {
                StatsClass.AddToStatInGame(1);
                Hide();
            };
            acts[2] = () => {
                StatsClass.AddToStatInGame(2);
                Hide();
            };
            acts[3] = () => {
                StatsClass.AddToStatInGame(3);
                Hide();
            };
            acts[4] = () => {
                StatsClass.AddToStatInGame(4);
                Hide();
            };
            acts[5] = () => {
                player.AddWeaponAmmo(player.GetWeaponAmmo());
                Hide();
            };

            CreateButtons();
        }

        public override void Update(float deltaTime)
        {
            if (!Visible)
            {
                foreach (var button in buttons.ToArray())
                    button.position.Y = Utils.Lerp(button.position.Y,
                        button.startPosition.Y + Raylib.GetScreenHeight() + (MARGIN * (int)Utils.GetScale()),
                        16 * deltaTime);
                panelY = Utils.Lerp(panelY, Raylib.GetScreenHeight() + (MARGIN * (int)Utils.GetScale()), 16 * deltaTime);
                return;
            }
            if (Visible) panelY = Utils.Lerp(panelY, MARGIN, 16 * deltaTime);

            foreach (var button in buttons.ToArray())
            {
                if (Visible) button.position.Y = Utils.Lerp(button.position.Y, button.startPosition.Y, 16 * deltaTime);
                button.Update();
            }
        }

        public override void Draw()
        {
            if (!Visible) return;

            Raylib.DrawTextureNPatch(panel, panelNpatch,
                new Rectangle(Raylib.GetScreenWidth() / 2 - Raylib.GetScreenWidth() / 4, panelY,
                Raylib.GetScreenWidth()/2, Raylib.GetScreenHeight() - MARGIN * 2), new Vector2(), 0, Color.WHITE);

            for (int i = 0; i < texts.Length; i++)
                Raylib.DrawText(string.Format(texts[i], GetStatInfo(i)),
                    Raylib.GetScreenWidth() / 2 - Raylib.GetScreenWidth() / 4 + PADDING,
                    (int)panelY + PADDING + 4 * (int)Utils.GetScale() + (15 * (int)Utils.GetScale() + PADDING) * i,
                    7 * (int)Utils.GetScale(), Color.BLACK);

            foreach (var button in buttons)
                button.Draw();
        }

        Button SettButton(Vector2 position, OnClick action)
        {
            Dictionary<string, Rectangle> anims = new()
            {
                { "DEFAULT", new Rectangle(94, 0, 15, 15) },
                { "PRESSED", new Rectangle(94, 15, 15, 15) }
            };
            var button = new Button(LoadedTextures.GetTexture("menu_buttons"), anims, position,
                Raylib.GetScreenHeight() / 180, action);
            button.position.Y = Raylib.GetScreenHeight() + (MARGIN * (int)Utils.GetScale()) + button.position.Y;
            buttons.Add(button);
            return button;
        }

        public string GetStatInfo(int num)
        {
            switch (num) {
                case 0: return StatsClass.BulletDamage.ToString();
                case 1: return StatsClass.BulletSpeed.ToString();
                case 2: return StatsClass.BulletPush.ToString();
                case 3: return StatsClass.BulletSpeed.ToString();
                case 4: return StatsClass.PlayerHealth.ToString();
                case 5: return player.GetWeaponAmmo().ToString();
            }
            return "";
        }

        void CreateButtons()
        {
            for (int i = 0; i < texts.Length; i++)
                SettButton(new Vector2(
                    Raylib.GetScreenWidth() / 2 + Raylib.GetScreenWidth() / 4 - 15 * (int)Utils.GetScale() - PADDING,
                    MARGIN + PADDING + (15 * (int)Utils.GetScale() + PADDING) * i), acts[i]);
        }

        public void Show()
        {
            Visible = true;
            Program.Paused = true;
        }

        public void Hide()
        {
            Visible = false;
            Program.Paused = false;
        }

        public override void OnChangeScreen()
        {
            buttons.Clear();
            CreateButtons();
        }
    }
}
