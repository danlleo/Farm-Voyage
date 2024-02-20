using UnityEngine;

namespace UI.Icon
{
    public struct IconView
    {
        public readonly Vector3 Offset;
        public readonly RectTransform FollowRectTransform;

        public IconView(Vector3 offset, RectTransform followRectTransform)
        {
            Offset = offset;
            FollowRectTransform = followRectTransform;
        }
    }
}