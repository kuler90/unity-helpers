using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityHelpers.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FlexibleSizeFitter : UIBehaviour, ILayoutSelfController
    {
        [SerializeField]
        private bool controlWidth;
        [SerializeField]
        private float preferredLeft;
        [SerializeField]
        private float preferredRight;
        [SerializeField]
        private float minWidth;
        [SerializeField]
        private float maxWidth;

        [SerializeField]
        private bool controlHeight;
        [SerializeField]
        private float preferredTop;
        [SerializeField]
        private float preferredBottom;
        [SerializeField]
        private float minHeight;
        [SerializeField]
        private float maxHeight;

        private DrivenRectTransformTracker drivenTracker;

        public virtual void SetLayoutHorizontal()
        {
            UpdateRectTransform();
        }

        public virtual void SetLayoutVertical()
        {
            UpdateRectTransform();
        }

        protected override void OnEnable()
        {
            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }

        protected override void OnDisable()
        {
            drivenTracker.Clear();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (IsActive())
            {
                LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
            }
        }
#endif
        public bool ControlWidth
        {
            get { return controlWidth; }
            set { controlWidth = value; UpdateRectTransform(); }
        }

        public float PreferredLeft
        {
            get { return preferredLeft; }
            set { preferredLeft = value; UpdateRectTransform(); }
        }

        public float PreferredRight
        {
            get { return preferredRight; }
            set { preferredRight = value; UpdateRectTransform(); }
        }

        public float MinWidth
        {
            get { return minWidth; }
            set { minWidth = value; UpdateRectTransform(); }
        }

        public float MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; UpdateRectTransform(); }
        }

        public bool ControlHeight
        {
            get { return controlHeight; }
            set { controlHeight = value; UpdateRectTransform(); }
        }

        public float PreferredTop
        {
            get { return preferredTop; }
            set { preferredTop = value; UpdateRectTransform(); }
        }

        public float PreferredBottom
        {
            get { return preferredBottom; }
            set { preferredBottom = value; UpdateRectTransform(); }
        }

        public float MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; UpdateRectTransform(); }
        }

        public float MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; UpdateRectTransform(); }
        }

        private void UpdateRectTransform()
        {
            RectTransform parentRectTransform = transform.parent as RectTransform;
            RectTransform rectTransform = transform as RectTransform;

            // Update driven properties
            drivenTracker.Clear();
            DrivenTransformProperties drivenProperties = DrivenTransformProperties.None;
            if (controlWidth)
            {
                drivenProperties = drivenProperties
                    | DrivenTransformProperties.AnchorMinX
                    | DrivenTransformProperties.AnchorMaxX
                    | DrivenTransformProperties.AnchoredPositionX
                    | DrivenTransformProperties.SizeDeltaX;
            }
            if (controlHeight)
            {
                drivenProperties = drivenProperties
                    | DrivenTransformProperties.AnchorMinY
                    | DrivenTransformProperties.AnchorMaxY
                    | DrivenTransformProperties.AnchoredPositionY
                    | DrivenTransformProperties.SizeDeltaY;

            }
            drivenTracker.Add(this, rectTransform, drivenProperties);

            // Update anchors
            Vector2 anchorMin = rectTransform.anchorMin;
            Vector2 anchorMax = rectTransform.anchorMax;
            if (controlWidth)
            {
                anchorMin.x = 0;
                anchorMax.x = 1;
            }
            if (controlHeight)
            {
                anchorMin.y = 0;
                anchorMax.y = 1;
            }
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

            // Update offsets
            Vector2 offsetMin = rectTransform.offsetMin;
            Vector2 offsetMax = rectTransform.offsetMax;
            if (controlWidth)
            {
                float requiredWidth = parentRectTransform.rect.width - preferredLeft - preferredRight;
                if (requiredWidth < minWidth)
                {
                    float delta = requiredWidth - minWidth;
                    offsetMin.x = preferredLeft + delta * rectTransform.pivot.x;
                    offsetMax.x = -preferredRight - delta * (1 - rectTransform.pivot.x);
                }
                else if (requiredWidth > maxWidth)
                {
                    float delta = maxWidth - requiredWidth;
                    offsetMin.x = preferredLeft - delta * rectTransform.pivot.x;
                    offsetMax.x = -preferredRight + delta * (1 - rectTransform.pivot.x);
                }
                else
                {
                    offsetMin.x = preferredLeft;
                    offsetMax.x = -preferredRight;
                }
            }
            if (controlHeight)
            {
                float requiredHeight = parentRectTransform.rect.height - preferredTop - preferredBottom;
                if (requiredHeight < minHeight)
                {
                    float delta = requiredHeight - minHeight;
                    offsetMin.y = preferredBottom + delta * rectTransform.pivot.y;
                    offsetMax.y = -preferredTop - delta * (1 - rectTransform.pivot.y);
                }
                else if (requiredHeight > maxHeight)
                {
                    float delta = maxHeight - requiredHeight;
                    offsetMin.y = preferredBottom - delta * rectTransform.pivot.y;
                    offsetMax.y = -preferredTop + delta * (1 - rectTransform.pivot.y);
                }
                else
                {
                    offsetMin.y = preferredBottom;
                    offsetMax.y = -preferredTop;
                }
            }
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }
}