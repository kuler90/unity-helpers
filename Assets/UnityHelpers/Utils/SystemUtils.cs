using System;
using UnityEngine;

namespace UnityHelpers.Utils
{
    public static class SystemUtils
    {
        public static void OpenAppStoreReviewPage(string appleId)
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/viewContentsUserReviews?id=" + appleId);
        }

        public static void OpenAppStorePage(string appleId)
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appleId);
        }

        public static void OpenGooglePlayPage(string packageName)
        {
            Application.OpenURL("market://details?id=" + packageName);
        }

        public static void OpenMailTo(string email, string subject, string body)
        {
            Application.OpenURL(
                "mailto:" + email +
                "?subject=" + Uri.EscapeUriString(subject) +
                "&body=" + Uri.EscapeUriString(body));
        }
    }
}