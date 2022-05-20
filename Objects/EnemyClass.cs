using System.Numerics;
using Humper.Responses;
using Raylib_cs;

namespace ProjectTheW.Objects
{
    internal class EnemyClass : Entity
    {
        protected Texture2D sprite;

        protected readonly Player player;

        protected Animator animator;
        Vector2 spriteOffset = new Vector2(-3, -12);

        protected int hp = 3;

        public float moveSpeed = 125f;

        public EnemyClass(Vector2 position, Vector2 size, Player player)
            : base(position, size, Tags.Enemy)
        {
            body.Data = this;
            this.player = player;
            Ready();
        }

        public override void Ready()
        {
            base.Ready();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Control(deltaTime);
            Animate();

            position = new Vector2(body.X, body.Y);
        }

        public virtual void Control(float dt)
        {
            if (velocity.X != 0 && velocity.Y != 0) velocity /= 2;

            body.Move(body.X + velocity.X * dt, body.Y + velocity.Y * dt, (collision) =>
            {
                if (collision.Other.HasTag(Tags.Solid)) return CollisionResponses.Slide;
                return CollisionResponses.None;
            });
        }

        /// <summary>
        /// Нанести урон
        /// </summary>
        /// <param name="removeHp">наносимый урон</param>
        public void Hurt(int removeHp)
        {
            hp -= removeHp;
            if (hp <= 0) Remove(sprite);
        }

        void Animate()
        {
            if (animator != null)
            {
                if (velocity.X > 0) animator.SetFlipH(false);
                else animator.SetFlipH(true);
            }
        }

        public override void Draw()
        {
            Raylib.DrawTextureRec(animator.GetTexture(), animator.GetFrame(),
                position + spriteOffset, Color.WHITE);
            base.Draw();
        }
    }
}
