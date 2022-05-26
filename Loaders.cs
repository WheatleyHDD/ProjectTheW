using Raylib_cs;

namespace ProjectTheW
{
    internal class LoadedTextures
    {
        public static Dictionary<string, Texture2D> loadedSprites = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Загрузка всех текстур (спрайов)
        /// </summary>
        public static void LoadAll()
        {
            loadedSprites.Add("enemy1", Raylib.LoadTexture("resources/sprites/enemy1.png"));
            loadedSprites.Add("enemy2", Raylib.LoadTexture("resources/sprites/enemy2.png"));
            loadedSprites.Add("enemy3", Raylib.LoadTexture("resources/sprites/enemy3.png"));
            loadedSprites.Add("enemy4", Raylib.LoadTexture("resources/sprites/enemy4.png"));
            loadedSprites.Add("player", Raylib.LoadTexture("resources/sprites/base.png"));
            loadedSprites.Add("player_d", Raylib.LoadTexture("resources/sprites/player_died.png"));
            loadedSprites.Add("bullet", Raylib.LoadTexture("resources/sprites/bullet.png"));
            loadedSprites.Add("e_bullet", Raylib.LoadTexture("resources/sprites/e_bullet.png"));
            loadedSprites.Add("spawn_p", Raylib.LoadTexture("resources/sprites/spawn_p.png"));
            loadedSprites.Add("heart", Raylib.LoadTexture("resources/sprites/heart.png"));
            loadedSprites.Add("w_shootgun", Raylib.LoadTexture("resources/sprites/weapons/shootgun.png"));
            loadedSprites.Add("w_rifle", Raylib.LoadTexture("resources/sprites/weapons/rifle.png"));
            loadedSprites.Add("ammo_add", Raylib.LoadTexture("resources/sprites/powerups/ammo_add.png"));
            loadedSprites.Add("room1", Raylib.LoadTexture("resources/rooms/room1.png"));
            loadedSprites.Add("blue_panel", Raylib.LoadTexture("resources/sprites/panel.png"));
            loadedSprites.Add("menu_buttons", Raylib.LoadTexture("resources/sprites/menu_main_buttons.png"));
            loadedSprites.Add("menu_wbuttons", Raylib.LoadTexture("resources/sprites/menu_weapon_toggles.png"));
            loadedSprites.Add("gems1", Raylib.LoadTexture("resources/sprites/gems/1.png"));
            loadedSprites.Add("gems2", Raylib.LoadTexture("resources/sprites/gems/2.png"));
            loadedSprites.Add("gems3", Raylib.LoadTexture("resources/sprites/gems/3.png"));
            loadedSprites.Add("tutorial", Raylib.LoadTexture("resources/sprites/controls.png"));
        }

        public static Texture2D GetTexture(string name) => loadedSprites[name];

        public static void UnloadAll()
        {
            foreach (var sprite in loadedSprites.Values)
                Raylib.UnloadTexture(sprite);
        }
    }

    internal class LoadedSounds
    {
        public static Dictionary<string, Sound> loadedSounds = new Dictionary<string, Sound>();
        public static int CurrentVolume { get; private set; } = 100;

        /// <summary>
        /// Загрузка всех звуков
        /// </summary>
        public static void LoadAll()
        {
            loadedSounds.Add("pickup", Raylib.LoadSound("resources/sounds/pickup.wav"));
            loadedSounds.Add("w_shootgun", Raylib.LoadSound("resources/sounds/shootgun.wav"));
            loadedSounds.Add("w_rifle", Raylib.LoadSound("resources/sounds/fire_m4a1.wav"));
            loadedSounds.Add("error", Raylib.LoadSound("resources/sounds/error.wav"));
            loadedSounds.Add("gameover", Raylib.LoadSound("resources/sounds/gameOver.wav"));
            loadedSounds.Add("levelup", Raylib.LoadSound("resources/sounds/levelUp.wav"));
            loadedSounds.Add("reset", Raylib.LoadSound("resources/sounds/reset.wav"));
            loadedSounds.Add("upgrade", Raylib.LoadSound("resources/sounds/upgrade.wav"));
            loadedSounds.Add("defeat", Raylib.LoadSound("resources/sounds/defeat.wav"));
            loadedSounds.Add("hit", Raylib.LoadSound("resources/sounds/hit.wav"));
            loadedSounds.Add("powerup", Raylib.LoadSound("resources/sounds/powerup.wav"));
            loadedSounds.Add("coin", Raylib.LoadSound("resources/sounds/coin.wav"));
        }

        public static Sound GetSound(string name) => loadedSounds[name];

        public static void UnloadAll()
        {
            foreach (var snd in loadedSounds.Values)
                Raylib.UnloadSound(snd);
        }

        public static bool SetVolume(int vol)
        {
            if (vol > 100|| vol < 0) return false;
            foreach (var snd in loadedSounds.Values)
                Raylib.SetSoundVolume(snd, vol/100f);
            CurrentVolume = vol;
            return true;
        }
    }

    internal class LoadedMusic
    {
        public static Dictionary<string, Music> loadedMusic = new Dictionary<string, Music>();
        public static int CurrentVolume { get; private set; } = 100;

        /// <summary>
        /// Загрузка всей музыки
        /// </summary>
        public static void LoadAll()
        {
            loadedMusic.Add("main_menu", Raylib.LoadMusicStream("resources/music/TUNE13.ogg"));
            loadedMusic.Add("game", Raylib.LoadMusicStream("resources/music/end_caves.ogg"));
        }

        public static Music GetMusic(string name) => loadedMusic[name];

        public static void UnloadAll()
        {
            foreach (var music in loadedMusic.Values)
                Raylib.UnloadMusicStream(music);
        }

        public static bool SetVolume(int vol)
        {
            if (vol > 100 || vol < 0) return false;
            foreach (var music in loadedMusic.Values)
                Raylib.SetMusicVolume(music, vol/100f);
            CurrentVolume = vol;
            return true;
        }
    }
}
