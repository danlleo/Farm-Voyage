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
        [Header("External references")] 
        [SerializeField, WithinParent] private GameObject _container;
        [SerializeField, WithinParent] private Image _foreground;
        [SerializeField, WithinParent] private Image _topIcon;

        [Header("Settings")]
        [SerializeField, Range(0.1f, 1f)] private float _fillTimeInSeconds; 
        
        private Sprite _beforeProgressSprite;
        private Sprite _inProgressSprite;
        private Sprite _afterProgressSprite;

        private void Awake()
        {
            ClearFillAmountProgress();
        }

        public void Initialize(Sprite beforeProgressSprite, Sprite inProgressSprite, Sprite afterProgressSprite)
        {
            _beforeProgressSprite = beforeProgressSprite;
            _inProgressSprite = inProgressSprite;
            _afterProgressSprite = afterProgressSprite;

            _topIcon.sprite = beforeProgressSprite;
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
                _topIcon.sprite = _afterProgressSprite;
            });
        }
        
        private void ClearFillAmountProgress()
        {
            _foreground.fillAmount = 0f;
        }
    }
}