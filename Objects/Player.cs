using Raylib_cs;
using System.Numerics;
using Humper;
using Humper.Responses;

namespace ProjectTheW.Objects
{
    internal class Player : Entity
    {
        Texture2D heartTexture = Raylib.LoadTexture("resources/sprites/heart.png");

        Animator animator;

        public float moveSpeed = StatsClass.PlayerSpeed;

        Rectangle animFrame;
        Vector2 spriteOffset;

        WeaponClass currentWeapon;

        Camera currentCamera;

        public Player(Vector2 position, Camera cam)
            : base(position, new Vector2(10, 10), Tags.Player)
        {
            currentCamera = cam;
            Ready();
        }

        public override void Ready()
        {
            base.Ready();

            var anims = new Dictionary<string, Animation>();
            anims.Add("idle", new Animation(24, 4));
            anims.Add("walk", new Animation(48, 4));

            Texture2D texture = Raylib.LoadTexture("resources/sprites/base.png");
            animator = new Animator(texture, new Vector2(16, 24), anims, "idle", 45/60f);

            spriteOffset = new Vector2(-3, -12);

            switch (StatsClass.WeaponType)
            {
                case 0:
                    currentWeapon = new Weapons.Shotgun();
                    break;
                case 1:
                    currentWeapon = new Weapons.Rifle();
                    break;
            }
        }

        public override void Update(float dt)
        {
            animFrame = animator.GetFrame();
            base.Update(dt);
            Controls(dt);
            Animate();
            
            position = new Vector2(body.X, body.Y);
        }

        void Controls(float dt)
        {
            // Горизонтальное движение
            var horizontal =
                Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D)) -
                Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A));

            // Вертикальное движение
            var vertical =
                Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S)) -
                Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W));

            velocity = new Vector2(horizontal, vertical) * moveSpeed;
            if (velocity.X != 0 && velocity.Y != 0) velocity /= 3/2;

            body.Move(body.X + velocity.X * dt, body.Y + velocity.Y * dt, (collision) =>
            {
                if (collision.Other.HasTag(Tags.Solid)) return CollisionResponses.Slide;
                return CollisionResponses.None;
            });

            // Вращение оружия
            if (currentWeapon != null)
            {
                var globalMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), currentCamera.Cam);
                var katet1 = globalMousePos.X - position.X - (hitbox.Size.X / 2);
                var katet2 = globalMousePos.Y - position.Y - (hitbox.Size.Y / 2);
                currentWeapon.Rotation = (float)(180 * Math.Atan(Convert.ToDouble(katet2 / katet1)) / Math.PI);
                if (katet1 < 0)
                {
                    currentWeapon.Scale = -1;
                    currentWeapon.FlipV = true;
                }
                else
                {
                    currentWeapon.Scale = 1;
                    currentWeapon.FlipV = false;
                }
            }

            // стрельба
            currentWeapon?.CoolDownCalc(dt);
            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                currentWeapon?.Shoot(position);
        }
        
        /// <summary>
        /// Метод для анимирования
        /// </summary>
        void Animate()
        {
            if (animator != null)
            {
                if (velocity == Vector2.Zero) animator.SetAnimation("idle");
                else animator.SetAnimation("walk");

                var globalMousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), currentCamera.Cam);
                if (globalMousePos.X - position.X < 0)
                    animator.SetFlipH(true);
                else if (globalMousePos.X - position.X > 0)
                    animator.SetFlipH(false);
            }
        }

        public override void Draw()
        {
            Raylib.DrawTextureRec(animator.GetTexture(), animFrame,
                position + spriteOffset, Color.WHITE);

            if (currentWeapon != null)
                currentWeapon.Draw(position);

            base.Draw();
        }

        public void DebugDraw()
        {
            Raylib.DrawText("Velocity: " + velocity.ToString(), 12, 12, 20, Color.WHITE);
            Raylib.DrawText("Position: " + position.ToString(), 12, 32, 20, Color.WHITE);
            Raylib.DrawText("currFrameRect: " + animator.GetFrame().ToString(), 12, 52, 20, Color.WHITE);
            Raylib.DrawText("Weapon Rotation: " + currentWeapon.Rotation, 12, 72, 20, Color.WHITE);
        }

        public void DrawUI()
        {
            Raylib.DrawText(currentWeapon.WeaponName, 12, Raylib.GetScreenHeight()-42, 32, Color.WHITE);
            Raylib.DrawText("Ammo: " + currentWeapon.AmmoCount.ToString(), 12, Raylib.GetScreenHeight() - 74, 20, Color.WHITE);

            var scale = Utils.GetScale() * 2 / 3;
            for (int heartI = 0; heartI < StatsClass.PlayerHealth; heartI++)
                Raylib.DrawTextureEx(heartTexture, new Vector2(25 + 24 * scale * heartI, 25), 0, scale, Color.WHITE);
        }
    }
}
