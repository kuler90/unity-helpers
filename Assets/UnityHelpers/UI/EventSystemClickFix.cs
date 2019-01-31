using UnityEngine;
using UnityEngine.EventSystems;
using UnityHelpers.Utils;

namespace UnityHelpers.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemClickFix : MonoBehaviour
    {
        public int dragThresholdInDP = 5;

        void Start()
        {
            GetComponent<EventSystem>().pixelDragThreshold = ScreenUtils.PixelFromDp(this.dragThresholdInDP);
        }

        void OnValidate()
        {
            Start();
        }
    }
}