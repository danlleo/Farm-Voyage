using System;
using UnityEngine;

namespace UI.Icon
{
    [CreateAssetMenu(fileName = "Icon_", menuName = "Scriptable Objects/UI/Icon")]
    public class IconSO : ScriptableObject
    {
        public static event Action<Guid, bool> OnAnyIconVisibilityChanged;
        
        [field:SerializeField] public RectTransform IconRectTransform { get; private set; }
        [field:SerializeField] public Vector3 Offset { get; private set; }

        public void SetVisibility(IDisplayIcon icon, bool isActive)
        {
            OnAnyIconVisibilityChanged?.Invoke(icon.ID, isActive);
        }
    }
}