using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class FollowEnemy : EnemyClass
    {
        public FollowEnemy(Vector2 position, Player player)
            : base(position, new Vector2(10, 10), player)
        {
            acceleration = 2500f;
            // Анимация
            var anims = new Dictionary<string, Animation>();
            anims.Add("walk", new Animation(0, 4));

            sprite = LoadedTextures.GetTexture("enemy1");
            animator = new Animator(sprite, new Vector2(16, 24), anims, "walk", 5f);
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
