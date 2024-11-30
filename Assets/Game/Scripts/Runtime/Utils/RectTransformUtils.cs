using UnityEngine;

namespace GameMain
{
    public static class RectTransformUtils
    {
        public static float GetPosX(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.x;
        }

        public static float GetPosY(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.y;
        }

        public static Vector2 GetPos(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition;
        }

        public static void SetPosX(this RectTransform rectTransform, float x)
        {
            var ap = rectTransform.anchoredPosition3D;
            ap.x = x;
            rectTransform.anchoredPosition3D = ap;
        }

        public static void SetPosY(this RectTransform rectTransform, float y)
        {
            var ap = rectTransform.anchoredPosition;
            ap.y = y;
            rectTransform.anchoredPosition = ap;
        }

        public static void SetPos(this RectTransform rectTransform, Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
        }
    }
}