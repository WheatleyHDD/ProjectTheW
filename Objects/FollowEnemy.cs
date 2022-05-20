using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class FollowEnemy : EnemyClass
    {
        public FollowEnemy(Vector2 position, Player player)
            : base(position, new Vector2(10, 10), player)
        {
            // Анимация
            var anims = new Dictionary<string, Animation>();
            anims.Add("walk", new Animation(0, 4));

            sprite = Raylib.LoadTexture("resources/sprites/enemy1.png");
            animator = new Animator(sprite, new Vector2(16, 24), anims, "walk", 45 / 60f);
        }

        public override void Control(float dt)
        {
            if (position.X < player.position.X) velocity.X = moveSpeed;
            else velocity.X = -moveSpeed;

            if (position.Y < player.position.Y) velocity.Y = moveSpeed;
            else velocity.Y = -moveSpeed;

            base.Control(dt);
        }
    }
}
