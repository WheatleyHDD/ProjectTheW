using Raylib_cs;
using System.Numerics;

namespace ProjectTheW.Objects.Weapons
{
    internal class Shotgun : WeaponClass
    {
        Sound shootSound = Raylib.LoadSound("resources/sounds/shootgun.wav");
        public Shotgun() => Init("Shotgun", "resources/sprites/weapons/shootgun.png", 8, new Vector2(9, 4), new Vector2(24, 3), 1f);

        public override void Shoot(Vector2 position)
        {
            if (CooldownTime > 0) return;
            if (AmmoCount > 0) Raylib.PlaySound(shootSound);
            CreateBullet(position, Rotation-10);
            CreateBullet(position, Rotation);
            CreateBullet(position, Rotation+10);
            RemoveAmmo();
            CoolDownSet();
        }
    }

    internal class Rifle : WeaponClass
    {
        public Rifle() => Init("Rifle", "resources/sprites/weapons/rifle.png", 30, new Vector2(8, 5), new Vector2(23, 4), 0.1f);

        public override void Shoot(Vector2 position)
        {
            if (CooldownTime > 0) return;
            CreateBullet(position, Rotation + Raylib.GetRandomValue(-5, 5));
            RemoveAmmo();
            CoolDownSet();
        }
    }
}
