using System;
using Attributes.WithinParent;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Icon
{
    [DisallowMultipleComponent]
    public class ProgressIconUpdater : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }
        
        [Header("External references")] 
        [SerializeField, WithinParent] private GameObject _container;
        [SerializeField, WithinParent] private Image _foreground;
        [SerializeField, WithinParent] private Image _topIcon;

        [Header("Settings")]
        [SerializeField, Range(0.1f, 1f)] private float _fillTimeInSeconds; 
        
        private Sprite _inProgressSprite;
        private Sprite _stoppedProgressSprite;
        private Sprite _finishedProgressSprite;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            ClearFillAmountProgress();
        }

        public void Initialize(Sprite initialProgressSprite, Sprite inProgressSprite, Sprite stoppedProgressSprite,
            Sprite finishedProgressSprite)
        {
            _inProgressSprite = inProgressSprite;
            _stoppedProgressSprite = stoppedProgressSprite;
            _finishedProgressSprite = finishedProgressSprite;
            _topIcon.sprite = initialProgressSprite;
            _container.SetActive(false);
        }

        public void UpdateProgress(float progress)
        {
            if (progress < 0f)
                throw new ArgumentException("Progress can't be less than zero");

            _foreground.DOFillAmount(progress, _fillTimeInSeconds).OnStart(() =>
            {
                _container.SetActive(true);
                _topIcon.sprite = _inProgressSprite;
            }).OnComplete(() =>
            {
                if (_foreground.fillAmount < 1f) return;

                _container.SetActive(false);
                _topIcon.sprite = _finishedProgressSprite;
            });
        }

        public void SetResumedProgressIcon()
        {
            _container.SetActive(true);
            _topIcon.sprite = _inProgressSprite;
        }
        
        public void SetStoppedProgressIcon()
        {
            _container.SetActive(false);
            _topIcon.sprite = _stoppedProgressSprite;
        }
        
        private void ClearFillAmountProgress()
        {
            _foreground.fillAmount = 0f;
        }
    }
}