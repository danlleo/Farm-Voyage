using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EmmaShop
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public class EmmaShopUI : MonoBehaviour
    {
        public event Action OnClosed; 
        
        [Header("External references")]
        [SerializeField] private Button _closeButton;
        
        [Header("Settings")]
        [SerializeField, Min(0)] private float _timeToFadeInSeconds = 0.35f;
        [SerializeField, Range(0f, 1f)] private float _endFadeValue = 1f;

        private float _startFadeValue = 0f;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetDefaultCanvasParams();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseUI);
            PlayFadeAnimation(_endFadeValue);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            KillFadeAnimation();
        }

        private void CloseUI()
        {
            _closeButton.onClick.RemoveAllListeners();
            PlayFadeAnimation(_startFadeValue, () => OnClosed?.Invoke());
        }
        
        private void SetDefaultCanvasParams()
        {
            _canvasGroup.alpha = 0f;
        }
        
        private void PlayFadeAnimation(float targetValue, Action onComplete = null)
        {
            _canvasGroup.DOFade(targetValue, _timeToFadeInSeconds).OnComplete(() => onComplete?.Invoke());
        }

        private void KillFadeAnimation()
        {
            SetDefaultCanvasParams();
            _canvasGroup.DOKill();
        }
    }
}
