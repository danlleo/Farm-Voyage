using DG.Tweening;
using Sound;
using UnityEngine;

namespace Farm
{
    [DisallowMultipleComponent]
    public class GlobalStorageAnimator : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private AudioClip _storedAudioClip;
        
        private GlobalStorage _globalStorage;

        private void Awake()
        {
            _globalStorage = GetComponent<GlobalStorage>();
        }

        private void OnEnable()
        {
            _globalStorage.OnStored += GlobalStorage_OnStored;
        }

        private void OnDisable()
        {
            _globalStorage.OnStored -= GlobalStorage_OnStored;
        }

        private void AnimateGlobalStorageOnStored()
        {
            Vector3 originalScale = transform.localScale;
            
            Sequence globalStorageOnStoredSequence = DOTween.Sequence();
            globalStorageOnStoredSequence.Append(transform.DOScale(Vector3.one + Vector3.up * 0.1f, 0.2f));
            globalStorageOnStoredSequence.Append(transform.DOScale(originalScale, 0.2f));
        }
        
        private void GlobalStorage_OnStored()
        {
            AnimateGlobalStorageOnStored();
            SoundFXManager.Instance.PlaySoundFX2DClip(_storedAudioClip, 0.4f);
        }
    }
}