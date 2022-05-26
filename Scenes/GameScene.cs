using Raylib_cs;
using ProjectTheW.Objects;
using System.Numerics;
using Humper;

namespace ProjectTheW.Scenes
{
    internal class GameScene : Scene
    {
        static public World world = new(768, 768, 16);
        static Upgrader up;

        Player player;
        readonly Camera camera = new(new Vector2(384, 384));

        Color controlsColor = Color.WHITE;

        Music music = LoadedMusic.GetMusic("game");

        Texture2D roomTexture;

        static public List<Entity> objectPool = new();
        static List<Entity> toRemovePool = new();

        const int ForLevelMult = 10;
        public static int PlayerLevel { get; private set; }
        public static int LevelScore { get; set; }

        float stSimple = 5;
        float stFast = 15;
        float stWizard = 25;
        float stGiant = 45;
        float stLevelUp = 60;
        float stBullets = 10;

        public GameScene() : base() { }

        public override void Ready()
        {
            base.Ready();
            ResetAll();

            // Стены
            world.Create(0, 0, 768, 16).AddTags(Tags.Solid);
            world.Create(0, 16, 5, 752).AddTags(Tags.Solid);
            world.Create(763, 16, 5, 752).AddTags(Tags.Solid);
            world.Create(5, 764, 758, 4).AddTags(Tags.Solid);

            player = new Player(new Vector2(384, 384), camera);
            roomTexture = Raylib.LoadTexture("resources/rooms/room1.png");

            up = new Upgrader(player);

            Raylib.PlayMusicStream(music);
        }

        public override void Update(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)
                && !player.died && !up.Visible)
            {
                if (!Program.PauseAndSetting.Visible)
                {
                    Program.Paused = true;
                    Program.PauseAndSetting.Show();
                }
                else
                {
                    Program.Paused = false;
                    Program.PauseAndSetting.Hide();
                }
            }
            base.Update(deltaTime);
            player.Update(deltaTime);
            camera.UpdateCamera(player, deltaTime, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            up.Update(Raylib.GetFrameTime());

            if (!Program.Paused)
            {
                Raylib.UpdateMusicStream(music);
                CountDown.TimeTick(deltaTime);
                SpawnControl(deltaTime);
                if (controlsColor.a > 0)
                    controlsColor.a = (byte)Math.Max(Math.Abs(CountDown.Time - 15) * 17, 0);
            }

            foreach (var entity in objectPool.ToArray())
                entity?.Update(deltaTime);
            foreach(var del in toRemovePool)
                objectPool.Remove(del);
        }

        public void SpawnControl(float dt)
        {
            stSimple -= dt;
            stFast -= dt;
            stWizard -= dt;
            stGiant -= dt;
            stLevelUp -= dt;
            stBullets -= dt;
            if (stSimple < 0) ResetTimerAndSpawn(ref stSimple, Math.Max(5f - StatsClass.Level/10f, 1), "simple");
            if (stFast < 0) ResetTimerAndSpawn(ref stFast, Math.Max(5f - StatsClass.Level / 10f, 5), "mini");
            if (stWizard < 0) ResetTimerAndSpawn(ref stWizard, Math.Max(5f - StatsClass.Level / 10f, 8), "wizard");
            if (stGiant < 0) ResetTimerAndSpawn(ref stGiant, Math.Max(5f - StatsClass.Level / 10f, 15), "giant");
            if (stBullets < 0) ResetTimerAndSpawn(ref stBullets, Raylib.GetRandomValue(1, 15), "bullets");
            if (stLevelUp < 0)
            {
                stLevelUp = 60;
                StatsClass.Level += 1;
                Raylib.PlaySound(LoadedSounds.GetSound("levelup"));
            }
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Color.BLACK);

            Raylib.BeginMode2D(camera.Cam);

            Raylib.DrawTexture(roomTexture, 0, 0, Color.WHITE);

            Raylib.DrawTexturePro(LoadedTextures.GetTexture("tutorial"), new Rectangle(0, 0, 320, 180),
                new Rectangle(384, 384, 320, 180), new Vector2(160, 90), 0, controlsColor);

            player.Draw();
            foreach(var entity in objectPool)
                entity?.Draw();

            Raylib.EndMode2D();

            DrawTime();
            DrawProgress();
            DrawSomeInfo();

            up.Draw();

            player.DrawUI();
            //player.DebugDraw();
        }

        void DrawTime()
        {
            string time = CountDown.GetTime();
            var middle = Raylib.MeasureText(time, 12 * (int)Utils.GetScale()) / 2;
            Raylib.DrawText(time, Raylib.GetScreenWidth() / 2 - middle, 14 * (int)Utils.GetScale(), 12 * (int)Utils.GetScale(), Color.WHITE);
        }

        void DrawSomeInfo()
        {
            var txtPlayerLvl = "Player Level: " + PlayerLevel.ToString();
            var txtLvl = "Difficulty: " + StatsClass.Level.ToString();

            var mPlayerLvl = Raylib.MeasureText(txtPlayerLvl, 8 * (int)Utils.GetScale()) + (9 * (int)Utils.GetScale());
            var mLvl = Raylib.MeasureText(txtLvl, 8 * (int)Utils.GetScale()) + (9 * (int)Utils.GetScale());

            Raylib.DrawText(txtPlayerLvl, Raylib.GetScreenWidth() - mPlayerLvl,
                Raylib.GetScreenHeight() - 16 * (int)Utils.GetScale() - (6 * (int)Utils.GetScale()),
                8 * (int)Utils.GetScale(), Color.WHITE);
            Raylib.DrawText(txtLvl, Raylib.GetScreenWidth() - mLvl,
                Raylib.GetScreenHeight() - 8 * (int)Utils.GetScale() - (4 * (int)Utils.GetScale()),
                8 * (int)Utils.GetScale(), Color.WHITE);
        }

        void DrawProgress()
        {
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth(), 2 * (int)Utils.GetScale(), Color.BLACK);
            Raylib.DrawRectangle(0, 0, Raylib.GetScreenWidth() * LevelScore / (5 + ForLevelMult * PlayerLevel), 2 * (int)Utils.GetScale(), Color.DARKGREEN);
        }

        static public void AddObjectToPool(Entity entity) => objectPool.Add(entity);
        static public void RemoveObjectFromPool(Entity entity) => toRemovePool.Add(entity);

        /// <summary>
        /// Сбросить таймер и создать что-то
        /// </summary>
        /// <param name="timer">таймер (указатель)</param>
        /// <param name="time">Время сбросв</param>
        /// <param name="what">Что создать</param>
        public void ResetTimerAndSpawn(ref float timer, float time, string what)
        {
            timer = time;
            var spwn_position = new Vector2(
                Raylib.GetRandomValue(16, 752),
                Raylib.GetRandomValue(32, 736));
            switch (what) {
                case "simple":
                    AddObjectToPool(new SpawnParticle(spwn_position, new FollowEnemy(spwn_position, player)));
                    break;
                case "mini":
                    AddObjectToPool(new SpawnParticle(spwn_position, new MiniFollowEnemy(spwn_position, player)));
                    break;
                case "wizard":
                    AddObjectToPool(new SpawnParticle(spwn_position, new WizardEnemy(spwn_position, player)));
                    break;
                case "giant":
                    AddObjectToPool(new SpawnParticle(spwn_position, new GiantEnemy(spwn_position, player)));
                    break;
                case "bullets":
                    AddObjectToPool(new SpawnParticle(spwn_position, new BulletLoot(spwn_position)));
                    break;
            }
        }

        public override void OnChangeScreen()
        {
            base.OnChangeScreen();
            up.OnChangeScreen();
            camera.UpdateZoom();
        }

        /// <summary>
        /// Сброс при перезагрузке
        /// </summary>
        void ResetAll()
        {
            foreach (var obj in objectPool)
                obj.Remove();
            objectPool.Clear();
            toRemovePool.Clear();
            stSimple = 5;
            stFast = 15;
            stWizard = 25;
            stGiant = 45;
            stLevelUp = 60;
            stBullets = 10;
            PlayerLevel = 0;
            LevelScore = 0;
            Raylib.StopMusicStream(music);
        }

        public static void AddLevelScore(int score)
        {
            LevelScore += score;
            if (LevelScore >= 5 + ForLevelMult * PlayerLevel)
            {
                var diff = LevelScore - (5 + ForLevelMult * PlayerLevel);
                PlayerLevel++;
                LevelScore = diff;
                up.Show();
            }
        }
    }
}
