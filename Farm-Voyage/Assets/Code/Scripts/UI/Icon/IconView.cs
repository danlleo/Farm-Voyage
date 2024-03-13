using UnityEngine;

namespace UI.Icon
{
    public struct IconView
    {
        public readonly Vector3 Offset;
        public readonly RectTransform FollowRectTransform;
        public readonly Transform OwnerTransform;
        
        public IconView(Vector3 offset, RectTransform followRectTransform, Transform ownerTransform)
        {
            Offset = offset;
            FollowRectTransform = followRectTransform;
            OwnerTransform = ownerTransform;
        }
    }
}