using Raylib_cs;

namespace ProjectTheW
{
    internal class Scene
    {
        public Scene() => Ready();
        public virtual void Ready() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Draw() { }

        public virtual void OnChangeScreen() { }
    }
}
