using System;
using Attributes.WithinParent;
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

        public void Initialize(Sprite beforeProgressSprite, Sprite inProgressSprite, Sprite afterProgressSprite)
        {
            _beforeProgressSprite = beforeProgressSprite;
            _inProgressSprite = inProgressSprite;
            _afterProgressSprite = afterProgressSprite;

            _topIcon.sprite = beforeProgressSprite;
            _container.SetActive(false);
        }

        public void UpdateProgress(int progress)
        {
            if (progress < 0)
                throw new ArgumentException("Progress can't be less than zero");

            if (_foreground.fillAmount == 1)
            {
                _container.SetActive(false);
                _topIcon.sprite = _afterProgressSprite;
                return;
            }
            
            _container.SetActive(false);
            _topIcon.sprite = _inProgressSprite;
            _foreground.fillAmount = progress;
        }
    }
}