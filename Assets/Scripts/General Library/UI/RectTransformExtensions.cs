using UnityEngine;

namespace WSoft.UI
{
    /// <summary>
    /// Copied from https://answers.unity.com/questions/888257/access-left-right-top-and-bottom-of-recttransform.html
    /// and https://forum.unity.com/threads/very-useful-recttransform-extension-methods.285483/
    /// </summary>
    public static class RectTransformExtensions
    {
        public static void AnchorToCorners(this RectTransform transform)
        {
            if (transform == null)
                throw new System.ArgumentNullException("transform");

            if (transform.parent == null)
                return;

            var parent = transform.parent.GetComponent<RectTransform>();

            Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                              transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

            Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                              transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

            transform.anchorMin = newAnchorsMin;
            transform.anchorMax = newAnchorsMax;
            transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
        }

        public static void SetDefaultScale(this RectTransform transform)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        public static void SetPivotAndAnchors(this RectTransform transform, Vector2 aVec)
        {
            transform.pivot = aVec;
            transform.anchorMin = aVec;
            transform.anchorMax = aVec;
        }

        public static Vector2 GetSize(this RectTransform transform)
        {
            return transform.rect.size;
        }

        public static float GetWidth(this RectTransform transform)
        {
            return transform.rect.width;
        }

        public static float GetHeight(this RectTransform transform)
        {
            return transform.rect.height;
        }

        public static void SetPositionOfPivot(this RectTransform transform, Vector2 newPos)
        {
            transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
        }

        public static void SetLeftBottomPosition(this RectTransform transform, Vector2 newPos)
        {
            transform.localPosition = new Vector3(newPos.x + (transform.pivot.x * transform.rect.width), newPos.y + (transform.pivot.y * transform.rect.height), transform.localPosition.z);
        }

        public static void SetLeftTopPosition(this RectTransform transform, Vector2 newPos)
        {
            transform.localPosition = new Vector3(newPos.x + (transform.pivot.x * transform.rect.width), newPos.y - ((1f - transform.pivot.y) * transform.rect.height), transform.localPosition.z);
        }

        public static void SetRightBottomPosition(this RectTransform transform, Vector2 newPos)
        {
            transform.localPosition = new Vector3(newPos.x - ((1f - transform.pivot.x) * transform.rect.width), newPos.y + (transform.pivot.y * transform.rect.height), transform.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform transform, Vector2 newPos)
        {
            transform.localPosition = new Vector3(newPos.x - ((1f - transform.pivot.x) * transform.rect.width), newPos.y - ((1f - transform.pivot.y) * transform.rect.height), transform.localPosition.z);
        }

        public static void SetSize(this RectTransform transform, Vector2 newSize)
        {
            Vector2 oldSize = transform.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            transform.offsetMin = transform.offsetMin - new Vector2(deltaSize.x * transform.pivot.x, deltaSize.y * transform.pivot.y);
            transform.offsetMax = transform.offsetMax + new Vector2(deltaSize.x * (1f - transform.pivot.x), deltaSize.y * (1f - transform.pivot.y));
        }

        public static void SetWidth(this RectTransform transform, float newSize)
        {
            SetSize(transform, new Vector2(newSize, transform.rect.size.y));
        }

        public static void SetHeight(this RectTransform transform, float newSize)
        {
            SetSize(transform, new Vector2(transform.rect.size.x, newSize));
        }

        public static void SetFillParent(this RectTransform transform)
        {
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(1, 1);
        }
    }
}
