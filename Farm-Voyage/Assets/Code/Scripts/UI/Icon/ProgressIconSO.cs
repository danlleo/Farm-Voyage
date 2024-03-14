using System;
using UnityEngine;

namespace UI.Icon
{
    [CreateAssetMenu(fileName = "ProgressIcon_", menuName = "Scriptable Objects/UI/ProgressIcon")]
    public class ProgressIconSO : ScriptableObject
    {
        public static event Action<Guid, float> OnAnyDisplayIconProgressChanged;
        
        [field: SerializeField] public Sprite BeforeProgressSprite { get; private set; }
        [field: SerializeField] public Sprite InProgressSprite { get; private set; }
        [field: SerializeField] public Sprite AfterProgressSprite { get; private set; }
        [field:SerializeField] public Vector3 Offset { get; private set; }

        public void SetProgress(IDisplayProgressIcon displayProgressIcon, float progress)
        {
            OnAnyDisplayIconProgressChanged?.Invoke(displayProgressIcon.ID, progress);
        }
    }
}