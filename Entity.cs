using System.Numerics;
using Raylib_cs;
using Humper;

namespace ProjectTheW
{
    internal class Entity
    {
        public Vector2 position;
        protected readonly Hitbox hitbox;
        public Vector2 velocity;

        public readonly IBox body;

        public Entity(Vector2 position, Vector2 hitboxSize, Enum tag)
        {
            this.position = position;
            hitbox = new Hitbox(position, hitboxSize);
            body = Scenes.GameScene.world.Create(position.X, position.Y, hitbox.Size.X, hitbox.Size.Y).AddTags(tag);
        }

        public virtual void Ready() { }

        public virtual void Draw()
        {
            //hitbox.DebugDraw(Color.SKYBLUE);
        }

        public virtual void Update(float deltaTime)
        {
            hitbox.Position = position;
        }

        public void Remove()
        {
            Scenes.GameScene.world.Remove(body);
            Scenes.GameScene.RemoveObjectFromPool(this);
        }
    }
}
