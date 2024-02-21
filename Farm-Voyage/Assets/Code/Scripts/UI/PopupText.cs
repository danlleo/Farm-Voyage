using System;
using Attributes.WithinParent;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    [DisallowMultipleComponent]
    public class PopupText : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField, WithinParent] private Transform _textAnchor;
        [SerializeField, WithinParent] private RectTransform _textRectTransform;
        [SerializeField, WithinParent] private TextMeshProUGUI _text;

        [Header("Options")]
        [SerializeField, Range(0.1f, 3f)] private float _animationTimeInSeconds;
        [SerializeField, Range(1f, 150f)] private float _minimalMagnitudeValue;
        [SerializeField, Range(1f, 150f)] private float _maximumMagnitudeValue;
        
        private Action _onComplete;
        
        private void Start()
        {
            PlayTextMoveAnimation();
        }

        public void Initialize(string message, Action onComplete)
        {
            _text.SetText(message);
            _textRectTransform.localScale = Vector3.zero;
            _onComplete = onComplete;
        }

        public void Initialize(int value, Action onComplete, bool isDecreasing = false)
        {
            _text.SetText(isDecreasing ? "-{}" : "+{}", value);
            _textRectTransform.localScale = Vector3.zero;
            _onComplete = onComplete;
        }

        private void PlayTextMoveAnimation()
        {
            Vector2 randomDirection = -CalculateRandomDirection();

            Vector2 startAnchoredPosition = _textRectTransform.anchoredPosition;
            Vector2 targetPosition = startAnchoredPosition + randomDirection;

            int sequencesAmount = 4;
            float durationPerSequence = _animationTimeInSeconds / sequencesAmount; 
            
            Sequence textAnimationSequence = DOTween.Sequence();
            textAnimationSequence.Join(_textRectTransform.DOScale(Vector3.one, durationPerSequence));
            textAnimationSequence.Join(_textRectTransform.DOAnchorPos(targetPosition, durationPerSequence));
            textAnimationSequence.Append(_text.DOFade(0.1f, durationPerSequence));
            textAnimationSequence.Join(_textRectTransform.DOScale(Vector3.zero, durationPerSequence));
            textAnimationSequence.OnComplete(() => _onComplete?.Invoke());
        }

        private Vector2 CalculateRandomDirection()
        {
            float angle = Random.Range(-75f, 75f);
            float angleInRadians = angle * Mathf.Deg2Rad;

            Vector2 direction = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            
            float magnitude = Random.Range(_minimalMagnitudeValue, _maximumMagnitudeValue);

            return direction * magnitude;
        }
    }
}
