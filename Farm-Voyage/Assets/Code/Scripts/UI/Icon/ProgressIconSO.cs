using System;
using UnityEngine;

namespace UI.Icon
{
    [CreateAssetMenu(fileName = "ProgressIcon_", menuName = "Scriptable Objects/UI/ProgressIcon")]
    public class ProgressIconSO : ScriptableObject
    {
        public static event Action<Guid, bool> OnAnyProgressIconVisibilityChanged;
        public static event Action<Guid, float, float> OnAnyDisplayIconProgressChanged;
        public static event Action<Guid> OnAnyDisplayIconProgressStopped;
        public static event Action<Guid> OnAnyDisplayIconProgressResumed;
        
        [field: SerializeField] public Sprite InitialProgressSprite { get; private set; }
        [field: SerializeField] public Sprite InProgressSprite { get; private set; }
        [field: SerializeField] public Sprite StoppedProgressSprite { get; private set; }
        [field: SerializeField] public Sprite FinishedProgressSprite { get; private set; }
        [field: SerializeField] public Vector3 Offset { get; private set; }

        public void SetProgress(IDisplayProgressIcon displayProgressIcon, float progress,
            float timeToFillInSeconds = 0f)
        {
            OnAnyDisplayIconProgressChanged?.Invoke(displayProgressIcon.ID, progress, timeToFillInSeconds);
        }

        public void ResumeProgress(IDisplayProgressIcon displayProgressIcon)
        {
            OnAnyDisplayIconProgressResumed?.Invoke(displayProgressIcon.ID);
        }
        
        public void StopProgress(IDisplayProgressIcon displayProgressIcon)
        {
            OnAnyDisplayIconProgressStopped?.Invoke(displayProgressIcon.ID);
        }
        
        public void SetVisibility(IDisplayIcon icon, bool isActive)
        {
            OnAnyProgressIconVisibilityChanged?.Invoke(icon.ID, isActive);
        }
    }
}