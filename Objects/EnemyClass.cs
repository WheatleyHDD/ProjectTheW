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
        protected Vector2 spriteOffset = new Vector2(-3, -12);
        protected Vector2 dir = new Vector2(0, 0);

        protected int hp = 3 * StatsClass.Level;

        public float moveSpeed = 80f;
        protected float acceleration = 500f;

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
            velocity = MoveToward(velocity, dir * moveSpeed, acceleration * 1/60f);
            body.Move(body.X + velocity.X * dt, body.Y + velocity.Y * dt, (collision) =>
            {
                if (collision.Other.HasTag(Tags.Solid) || collision.Other.HasTag(Tags.Enemy)) return CollisionResponses.Slide;
                if (collision.Other.HasTag(Tags.Player) && collision.Other.Data is Player)
                {
                    Player pl = (Player)collision.Other.Data;
                    pl.Hurt();
                    return CollisionResponses.Touch;
                }
                return CollisionResponses.None;
            });
        }

        /// <summary>
        /// Нанести урон
        /// </summary>
        /// <param name="removeHp">наносимый урон</param>
        public void Hurt(int removeHp)
        {
            Scenes.GameScene.AddObjectToPool(new DeathParticle(position - hitbox.Size / 2f));
            velocity = new Vector2(0,0);
            hp -= removeHp;
            if (hp <= 0) Die();
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

        void Die()
        {
            if (Raylib.GetRandomValue(0, 1) == 0)
                Scenes.GameScene.AddObjectToPool(new BulletLoot(position));
            Remove();
        }
        public Vector2 MoveToward(Vector2 from, Vector2 to, float delta)
        {
            Vector2 vd = to - from;
            float len = vd.Length();
            return len <= delta || len < (float)double.Epsilon ? to : from + vd / len * delta;
        }
    }
}
