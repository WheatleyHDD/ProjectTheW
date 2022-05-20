using Raylib_cs;
using System.Numerics;
using ProjectTheW.Scenes;

namespace ProjectTheW.Objects
{
    internal class WeaponClass
    {
        public int AmmoCount { get; private set; }
        public string WeaponName { get; private set; }
        Texture2D weaponTexture;

        public float Rotation { get; set; }
        public float Scale { get; set; }
        public bool FlipV { get; set; }

        Vector2 shootPoint;
        Vector2 offset;

        public float CooldownTime { get; private set; }
        float cooldownMax;

        /// <summary>
        /// Создание нового оружия
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="sprite">Путь к текстуре</param>
        /// <param name="initial_ammo">Изначальное количество патрон</param>
        /// <param name="offset">Точка, относительно которой оружие будет вращаться</param>
        /// <param name="shootPoint">Точка, из которой будут вылетать пули</param>
        /// <param name="cooldown">Кулдаун</param>
        public void Init(string name, string sprite, int initial_ammo, Vector2 offset, Vector2 shootPoint, float cooldown)
        {
            cooldownMax = cooldown;
            AmmoCount = initial_ammo;
            WeaponName = name;
            this.shootPoint = shootPoint;
            this.offset = offset;
            weaponTexture = Raylib.LoadTexture(sprite);
            Rotation = 0;
            Scale = 1;
        }

        public virtual void Shoot(Vector2 position)
        {
            if (CooldownTime > 0) return;
            CreateBullet(position, Rotation);
            RemoveAmmo();
            CoolDownSet();
        }

        /// <summary>
        /// Создать пулю
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="rotation">Угол</param>
        public void CreateBullet(Vector2 position, float rotation)
        {
            if (AmmoCount == 0) return;
            var bullet = new Bullet(GetShootPosition(position),
                new Vector2(8, 8), rotation, 200, Scale);
            GameScene.AddObjectToPool(bullet);
        }

        public void CoolDownCalc(float dt)
        {
            if (CooldownTime < 0) return ;
            CooldownTime -= dt;
        }

        public void CoolDownSet() => CooldownTime = cooldownMax;

        /// <summary>
        /// Отрисовать оружие
        /// </summary> 
        /// <param name="playerPosition"> позиция</param>
        public void Draw(Vector2 playerPosition)
        {
            Raylib.DrawTexturePro(weaponTexture,
                new Rectangle(0, 0, weaponTexture.width, weaponTexture.height * GetFlipV()),
                new Rectangle(playerPosition.X + 5, playerPosition.Y + 8, weaponTexture.width * Scale, weaponTexture.height * Scale),
                new Vector2(offset.X, offset.Y + 4 * Convert.ToInt32(FlipV)) * Scale, Rotation, Color.WHITE);

            //Raylib.DrawLineEx(new Vector2(playerPosition.X + 5, playerPosition.Y + 8), GetShootPosition(playerPosition), 3, Color.BLUE);
        }

        /// <summary>
        /// Добавить патронов
        /// </summary>
        /// <param name="count"></param>
        public void AddAmmo(int count)
        {
            AmmoCount += count;
        }

        public void RemoveAmmo() { if (AmmoCount > 0) AmmoCount--; }

        int GetFlipV() => FlipV ? -1 : 1;

        Vector2 GetShootPosition(Vector2 playerPosition) =>
            new Vector2(playerPosition.X + 5 + (float)Math.Cos(Math.PI / 180 * Rotation) * (shootPoint.X - offset.X) * GetFlipV(),
                        playerPosition.Y + 8 + (float)Math.Sin(Math.PI / 180 * Rotation) * (shootPoint.X - offset.X) * GetFlipV());
    }
}
