using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class TransformUtils
    {
        public static void SetPositionX(this Transform t, float x)
        {
            Vector3 p = t.position; p.x = x; t.position = p;
        }

        public static void SetPositionY(this Transform t, float y)
        {
            Vector3 p = t.position; p.y = y; t.position = p;
        }

        public static void SetPositionZ(this Transform t, float z)
        {
            Vector3 p = t.position; p.z = z; t.position = p;
        }

        public static void SetLocalPositionX(this Transform t, float x)
        {
            Vector3 lp = t.localPosition; lp.x = x; t.localPosition = lp;
        }

        public static void SetLocalPositionY(this Transform t, float y)
        {
            Vector3 lp = t.localPosition; lp.y = y; t.localPosition = lp;
        }

        public static void SetLocalPositionZ(this Transform t, float z)
        {
            Vector3 lp = t.localPosition; lp.z = z; t.localPosition = lp;
        }

        public static void SetEulerAnglesX(this Transform t, float x)
        {
            Vector3 ea = t.eulerAngles; ea.x = x; t.eulerAngles = ea;
        }

        public static void SetEulerAnglesY(this Transform t, float y)
        {
            Vector3 ea = t.eulerAngles; ea.y = y; t.eulerAngles = ea;
        }

        public static void SetEulerAnglesZ(this Transform t, float z)
        {
            Vector3 ea = t.eulerAngles; ea.z = z; t.eulerAngles = ea;
        }

        public static void SetLocalScaleX(this Transform t, float x)
        {
            Vector3 ls = t.localScale; ls.x = x; t.localScale = ls;
        }

        public static void SetLocalScaleY(this Transform t, float y)
        {
            Vector3 ls = t.localScale; ls.y = y; t.localScale = ls;
        }

        public static void SetLocalScaleZ(this Transform t, float z)
        {
            Vector3 ls = t.localScale; ls.z = z; t.localScale = ls;
        }

        public static void SetWidth(this RectTransform rt, float width)
        {
            Vector2 sd = rt.sizeDelta; sd.x = width; rt.sizeDelta = sd;
        }

        public static void SetHeight(this RectTransform rt, float height)
        {
            Vector2 sd = rt.sizeDelta; sd.y = height; rt.sizeDelta = sd;
        }

        public static void SetPivot(this RectTransform rt, Vector2 pivot)
        {
            Vector2 size = rt.rect.size;
            Vector2 deltaPivot = rt.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
            rt.pivot = pivot;
            rt.localPosition -= deltaPosition;
        }
    }
}