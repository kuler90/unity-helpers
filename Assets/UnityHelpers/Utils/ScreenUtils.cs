using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class ScreenUtils
    {
        public const float DEFAULT_SCREEN_DPI = 160.0f;
        public static float MIN_TABLET_INCHES = 6.8f;

        public static float ScreenDPI
        {
            get { return Screen.dpi == 0 ? DEFAULT_SCREEN_DPI : Screen.dpi; }
        }

        public static float ScreenScale
        {
#if UNITY_IPHONE
            get { return Mathf.Round(ScreenDPI / DEFAULT_SCREEN_DPI); }
#else
            get { return ScreenDPI / DEFAULT_SCREEN_DPI; }
#endif
        }

        public static float ScreenInches
        {
            get { return Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2)) / ScreenDPI; }
        }

        public static bool IsPhone
        {
            get { return ScreenInches < MIN_TABLET_INCHES; }
        }

        public static bool IsTablet
        {
            get { return ScreenInches >= MIN_TABLET_INCHES; }
        }

        public static int PixelFromDp(float dp)
        {
            // Convert the dps to pixels
            return (int)(dp * ScreenScale + 0.5f);
        }

        public static int DpFromPixel(float px)
        {
            // Convert the pxs to dps
            return (int)(px / ScreenScale - 0.5f);
        }
    }
}