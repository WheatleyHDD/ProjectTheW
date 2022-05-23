using ProjectTheW.Scenes;
using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class DeathParticle : Entity
    {
        protected Texture2D sprite;

        protected Animator animator;

        public DeathParticle(Vector2 position)
            : base(position, Vector2.Zero, Tags.Other)
        {
            body.Data = this;
            Ready();

            // Анимация
            var anims = new Dictionary<string, Animation>();
            anims.Add("default", new Animation(0, 19));

            sprite = LoadedTextures.GetTexture("spawn_p");
            animator = new Animator(sprite, new Vector2(32, 32), anims, "default", 6f);
        }

        public override void Ready()
        {
            base.Ready();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (animator.GetFrame().x > 600)
                Remove();
        }

        public override void Draw()
        {
            Raylib.DrawTextureRec(animator.GetTexture(), animator.GetFrame(), position, Color.RED);
            base.Draw();
        }
    }
}
