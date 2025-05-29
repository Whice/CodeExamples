using UnityEngine;

namespace Utility
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Получить основной (самый что ни на есть родительский) холст для указанного <see cref="RectTransform"/>
        /// </summary>
        public static Canvas GetRootCanvas(this RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            while (rectTransform.parent != null && canvas == null)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
                rectTransform = rectTransform.parent as RectTransform;
            }
            return canvas.rootCanvas;
        }
    }
}