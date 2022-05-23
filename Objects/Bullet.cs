using Raylib_cs;
using System.Numerics;
using Humper.Responses;

namespace ProjectTheW.Objects
{
    internal class Bullet : Entity
    {
        Texture2D sprite = LoadedTextures.GetTexture("bullet");

        float rotation = 0;
        float scale = 1;
        float moveSpeed = StatsClass.BulletSpeed;
        int dmg = StatsClass.BulletDamage;

        public Bullet(Vector2 position, Vector2 size, float rotation, float scale)
            : base(position - size/2, size, Tags.Bullet)
        {
            this.scale = scale;
            this.rotation = rotation;
            Ready();
        }

        public override void Ready()
        {
            base.Ready();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (body == null) return;
            velocity = new Vector2((float)Math.Cos(Math.PI / 180 * rotation), (float)Math.Sin(Math.PI / 180 * rotation)) * moveSpeed * scale;
            body.Move(body.X + velocity.X * dt, body.Y + velocity.Y * dt,
                (collision) => {
                    if (collision.Other.HasTag(Tags.Player)
                        || collision.Other.HasTag(Tags.Bullet)
                        || collision.Other.HasTag(Tags.Loot)) return CollisionResponses.None;
                    if (collision.Other.HasTag(Tags.Enemy) && collision.Other.Data is EnemyClass)
                    {
                        EnemyClass ec = collision.Other.Data as EnemyClass;
                        ec.Hurt(dmg);
                    }
                    Remove();
                    return CollisionResponses.Cross;
                });

            position = new Vector2(body.X, body.Y);
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawTexturePro(sprite,
                new Rectangle(0, 0, sprite.width * scale, sprite.height * scale),
                new Rectangle(position.X + hitbox.Size.X/2, position.Y + hitbox.Size.Y / 2, sprite.width, sprite.height),
                new Vector2(5,3), rotation, Color.WHITE);
        }
    }
}
