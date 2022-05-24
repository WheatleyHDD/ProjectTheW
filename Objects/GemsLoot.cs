using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects
{
    internal class GemsLoot : Entity
    {
        Texture2D sprite;
        public int Cost { get; private set; } = 1;

        public GemsLoot(Vector2 position, int cost) : base(position, new Vector2(12, 12), Tags.Loot)
        {
            body.Data = this;
            Cost = Math.Clamp(cost, 1, 3);
            sprite = LoadedTextures.GetTexture("gems" + Cost);
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
