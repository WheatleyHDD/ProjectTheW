using Raylib_cs;
using System.Numerics;

namespace ProjectTheW
{
    internal class Animator
    {
        Texture2D texture;

        Dictionary<string, Animation> animation_list;
        public string CurrentAnimation { get; private set; }

        Rectangle currFrameRect;
        int currentFrame;
        int framesCounter;
        Vector2 frameCount;

        int currFlipH = 1;
        int currFlipV = 1;

        int maxFrame = 0;

        readonly double framesSpeed = 1;

        /// <summary>
        /// Создает новый аниматор с анимациями и определенной скоростью
        /// </summary>
        public Animator(Texture2D texture, Vector2 frameSize,
            Dictionary<string, Animation> anims,
            string defaultAnim, double animationSpeed)
        {
            this.texture = texture;

            Raylib.SetTextureFilter(this.texture, TextureFilter.TEXTURE_FILTER_POINT);

            framesSpeed = animationSpeed;
            animation_list = anims;
            frameCount = new Vector2(texture.width, texture.height) / frameSize;
            currFrameRect = new Rectangle(0, 0, texture.width / frameCount.X, texture.height / frameCount.Y);
            SetAnimation(defaultAnim);
        }

        /// <summary>
        /// Ставит определенную анимация с определенным поворотом
        /// </summary>
        public void SetAnimation(string name)
        {
            CurrentAnimation = name;
            maxFrame = animation_list[name].MaxFrame;
            currFrameRect.y = animation_list[name].Line;
            currFrameRect.width = Math.Abs(currFrameRect.width) * currFlipH;
            currFrameRect.height = Math.Abs(currFrameRect.height) * currFlipV;
        }

        /// <summary>
        /// Отразить спрайт по вертикали
        /// </summary>
        /// <param name="flip"></param>
        public void SetFlipV(bool flip)
        {
            if (flip) currFlipV = -1;
            else currFlipV = 1;
            currFrameRect.height = Math.Abs(currFrameRect.height) * currFlipV;
        }

        /// <summary>
        /// Отразить спрайт по горизонтали
        /// </summary>
        /// <param name="flip"></param>
        public void SetFlipH(bool flip)
        {
            if (flip) currFlipH = -1;
            else currFlipH = 1;
            currFrameRect.width = Math.Abs(currFrameRect.width) * currFlipH;
        }

        /// <summary>
        /// Получить текущий кадр анимации
        /// </summary>
        public Rectangle GetFrame()
        {
            framesCounter++;

            if (framesCounter >= (60 / framesSpeed))
            {
                framesCounter = 0;
                currentFrame++;

                if (currentFrame > maxFrame) currentFrame = 0;

                currFrameRect.x = currentFrame * texture.width / frameCount.X;
            }

            return currFrameRect;
        }

        public Texture2D GetTexture() => texture;
    }
}
