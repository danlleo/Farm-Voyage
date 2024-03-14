using UnityEngine;

namespace UI.Icon
{
    public struct ProgressIconView
    {
        public readonly Vector3 Offset;
        public readonly ProgressIconUpdater ProgressIconUpdater;
        public readonly Transform OwnerTransform;

        public ProgressIconView(Vector3 offset, ProgressIconUpdater progressIconUpdater, Transform ownerTransform)
        {
            Offset = offset;
            ProgressIconUpdater = progressIconUpdater;
            OwnerTransform = ownerTransform;
        }
    }
}