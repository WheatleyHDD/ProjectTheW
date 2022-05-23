using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class WizardEnemy : EnemyClass
    {
        float spellTimer = 5;
        public WizardEnemy(Vector2 position, Player player)
            : base(position, new Vector2(12, 12), player)
        {
            acceleration = 2500f;
            spriteOffset = new Vector2(-2, -5);
            hp = 4 * StatsClass.Level;
            moveSpeed = 30f;

            // Анимация
            var anims = new Dictionary<string, Animation>();
            anims.Add("walk", new Animation(0, 4));

            sprite = LoadedTextures.GetTexture("enemy4");
            animator = new Animator(sprite, new Vector2(16, 20), anims, "walk", 5f);
        }

        public override void Control(float dt)
        {
            spellTimer -= dt;
            if (spellTimer <= 0) Spell();

            if (position.X < player.position.X) dir.X = 1;
            else dir.X = -1;

            if (position.Y < player.position.Y) dir.Y = 1;
            else dir.Y = -1;

            base.Control(dt);
        }
        
        void Spell()
        {
            spellTimer = 5;

            var katet1 = player.position.X - position.X - (hitbox.Size.X / 2);
            var katet2 = player.position.Y - position.Y - (hitbox.Size.Y / 2);
            var rotation = (float)(180 * Math.Atan(Convert.ToDouble(katet2 / katet1)) / Math.PI);
            var scale = 1;

            if (katet1 < 0) scale = -1;

            var bullet = new EnemyBullet(position, new Vector2(8, 8), rotation, scale);
            Scenes.GameScene.AddObjectToPool(bullet);
        }
    }
}
