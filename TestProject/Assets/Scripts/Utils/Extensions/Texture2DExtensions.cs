using UnityEngine;

namespace Utility
{
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Создать новую текстуру с правильными параметрами глубины цвета.
        /// </summary>
        public static Texture2D CreateNewTexture2D(int width, int height)
        {
            return new Texture2D(width, height, TextureFormat.ARGB32, false, true);
        }

        /// <summary>
        /// Получить дубликат текстуры, чтобы она была читаемой.
        /// </summary>
        public static Texture2D Duplicate(this Texture2D source, Vector2Int? newSize = null)
        {
            int width = source.width;
            int height = source.height;
            if (newSize != null && newSize.Value.x > 0 && newSize.Value.y > 0)
            {
                width = newSize.Value.x;
                height = newSize.Value.y;
            }
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        width,
                        height,
                        0,
                        RenderTextureFormat.ARGB32,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = CreateNewTexture2D(width, height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        /// <summary>
        /// Получить спрайт из этой текстуры.
        /// </summary>
        public static Sprite CreateSprite(this Texture2D source, Rect rect)
        {
            return Sprite.Create(source, rect, new Vector2(0.5f, 0.5f));
        }
    }
}