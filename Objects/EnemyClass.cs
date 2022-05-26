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
        protected int weight = 2;

        protected int hp = 3 * StatsClass.Level;

        public float moveSpeed = 80f;
        protected float acceleration = 500f;
        float hurtTimer = 0;

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
            Animate();
            
            if (hurtTimer > 0) hurtTimer -= deltaTime;
            Control(deltaTime);
            position = new Vector2(body.X, body.Y);
        }

        public virtual void Control(float dt)
        {
            if (hurtTimer <= 0)
                velocity = MoveToward(velocity, dir * moveSpeed, acceleration * 1/60f);
            Move(dt);
        }

        void Move(float dt)
        {
            body.Move(body.X + velocity.X * dt, body.Y + velocity.Y * dt, (collision) =>
            {
                if (collision.Other.HasTag(Tags.Solid) || collision.Other.HasTag(Tags.Enemy)) return CollisionResponses.Slide;
                if (collision.Other.HasTag(Tags.Player) && collision.Other.Data is Player pl)
                {
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
        /// <param name="directory">направление пули</param>
        public void Hurt(int removeHp, Vector2 directory)
        {
            Scenes.GameScene.AddObjectToPool(new DeathParticle(position - hitbox.Size / 2f));
            hurtTimer = 0.1f;
            velocity += directory * StatsClass.BulletPush * 30f * weight/3f;
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
            var loot = Raylib.GetRandomValue(1, 10);
            if (loot == 1) Scenes.GameScene.AddObjectToPool(new GemsLoot(position, 3));
            else if (loot <= 5) Scenes.GameScene.AddObjectToPool(new GemsLoot(position, 2));
            else
            {
                if (Raylib.GetRandomValue(0, 1) == 0) Scenes.GameScene.AddObjectToPool(new BulletLoot(position));
                else Scenes.GameScene.AddObjectToPool(new GemsLoot(position, 1));
            }
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
