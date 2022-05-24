using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class GiantEnemy : EnemyClass
    {
        public GiantEnemy(Vector2 position, Player player)
            : base(position, new Vector2(18, 28), player)
        {
            weight = 3;
            acceleration = 2500f;
            spriteOffset = new Vector2(-7, -3);
            hp = 12 * StatsClass.Level;
            moveSpeed = 50f;

            // Анимация
            var anims = new Dictionary<string, Animation>();
            anims.Add("walk", new Animation(0, 4));

            sprite = LoadedTextures.GetTexture("enemy3");
            animator = new Animator(sprite, new Vector2(32, 34), anims, "walk", 5f);
        }

        public override void Control(float dt)
        {
            if (position.X < player.position.X) dir.X = 1;
            else dir.X = -1;

            if (position.Y < player.position.Y) dir.Y = 1;
            else dir.Y = -1;

            base.Control(dt);
        }
    }
}
