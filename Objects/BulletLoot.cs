using System.Numerics;
using Raylib_cs;

namespace ProjectTheW.Objects
{
    internal class BulletLoot : Entity
    {
        Texture2D sprite = LoadedTextures.GetTexture("ammo_add");
        public int AmmoCount { get; private set; } = 1;

        public BulletLoot(Vector2 position) : base(position, new Vector2(12,12), Tags.Loot)
        {
            body.Data = this;
            AmmoCount = Raylib.GetRandomValue(5, 15);
            Ready();
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawTexture(sprite, (int)position.X, (int)position.Y, Color.WHITE);
        }

        public void Delete() => Remove();
    }
}
