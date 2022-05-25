using Raylib_cs;

namespace ProjectTheW
{
    internal class Scene
    {
        public Scene() => Ready();
        public virtual void Ready() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Draw() { }

        /// <summary>
        /// Необходима для перестройки UI под новое разрешение экрана
        /// </summary>
        public virtual void OnChangeScreen() { }
    }
}
