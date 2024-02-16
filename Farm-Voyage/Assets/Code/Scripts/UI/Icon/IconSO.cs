using UnityEngine;

namespace UI.Icon
{
    [CreateAssetMenu(fileName = "Icon_", menuName = "Scriptable Objects/UI/Icon")]
    public class IconSO : ScriptableObject
    {
        [field:SerializeField] public RectTransform IconRectTransform { get; private set; }
        [field:SerializeField] public Vector3 Offset { get; private set; }
    }
}