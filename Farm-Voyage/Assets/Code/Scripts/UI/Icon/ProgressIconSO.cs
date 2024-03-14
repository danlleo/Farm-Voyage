using UnityEngine;

namespace UI.Icon
{
    [CreateAssetMenu(fileName = "ProgressIcon_", menuName = "Scriptable Objects/UI/ProgressIcon")]
    public class ProgressIconSO : ScriptableObject
    {
        [field: SerializeField] public Sprite BeforeProgressSprite { get; private set; }
        [field: SerializeField] public Sprite InProgressSprite { get; private set; }
        [field: SerializeField] public Sprite AfterProgressSprite { get; private set; }
        [field:SerializeField] public Vector3 Offset { get; private set; }
    }
}