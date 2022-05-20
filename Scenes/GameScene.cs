using Raylib_cs;
using ProjectTheW.Objects;
using System.Numerics;
using Humper;
using System.Collections;

namespace ProjectTheW.Scenes
{
    internal class GameScene : Scene
    {
        static public World world = new World(768, 768, 16);

        Player player;
        Camera camera = new Camera();

        Texture2D roomTexture;

        static public List<Entity> objectPool = new List<Entity>();
        static List<Entity> toRemovePool = new List<Entity>();

        public GameScene() : base() { }

        public override void Ready()
        {
            base.Ready();

            // Стены
            world.Create(0, 0, 768, 16).AddTags(Tags.Solid);
            world.Create(0, 16, 5, 752).AddTags(Tags.Solid);
            world.Create(763, 16, 5, 752).AddTags(Tags.Solid);
            world.Create(5, 764, 758, 4).AddTags(Tags.Solid);

            player = new Player(new Vector2(400, 225), camera);
            roomTexture = Raylib.LoadTexture("resources/rooms/room1.png");

            AddObjectToPool(new FollowEnemy(new Vector2(500, 500), player));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            player.Update(deltaTime);
            camera.UpdateCamera(player, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            foreach(var entity in objectPool)
                entity?.Update(deltaTime);
            foreach(var del in toRemovePool)
                objectPool.Remove(del);
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.ClearBackground(Color.WHITE);

            Raylib.BeginMode2D(camera.Cam);

            Raylib.DrawTexture(roomTexture, 0, 0, Color.WHITE);
            player.Draw();
            foreach(var entity in objectPool)
                entity?.Draw();

            Raylib.EndMode2D();

            player.DrawUI();
            //player.DebugDraw();
        }

        static public void AddObjectToPool(Entity entity) => objectPool.Add(entity);
        static public void RemoveObjectFromPool(Entity entity) => toRemovePool.Add(entity);
    }
}
