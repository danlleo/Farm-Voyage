using System;
using System.Collections;
using Attributes.WithinParent;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

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
        
        private Sprite _inProgressSprite;
        private Sprite _stoppedProgressSprite;
        private Sprite _finishedProgressSprite;

        private Coroutine _updateProgressBarRoutine;
        
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

        public void UpdateProgress(float progress, float timeToFillInSeconds = 0f)
        {
            if (progress < 0f)
                throw new ArgumentException("Progress can't be less than zero");

            CoroutineHandler.StartAndAssignIfNull(this, ref _updateProgressBarRoutine,
                UpdateProgressBarRoutine(progress, timeToFillInSeconds));
        }

        public void SetResumedProgressIcon()
        {
            _container.SetActive(true);
            _topIcon.sprite = _inProgressSprite;
        }
        
        public void SetStoppedProgressIcon()
        {
            CoroutineHandler.ClearAndStopCoroutine(this, ref _updateProgressBarRoutine);
            _container.SetActive(false);
            _topIcon.sprite = _stoppedProgressSprite;
        }
        
        private void ClearFillAmountProgress()
        {
            _foreground.fillAmount = 0f;
        }

        private IEnumerator UpdateProgressBarRoutine(float progress, float timeToFillInSeconds = 0f)
        {
            float timer = 0f;
            float startProgress = _foreground.fillAmount;
            
            _container.SetActive(true);
            _topIcon.sprite = _inProgressSprite;
            
            while (timer < timeToFillInSeconds)
            {
                float t = timer / timeToFillInSeconds;
                timer += Time.deltaTime;

                _foreground.fillAmount = Mathf.Lerp(startProgress, progress, t);
                
                yield return null;
            }

            if (_foreground.fillAmount < 1f) yield break;
            
            _container.SetActive(false);
            _topIcon.sprite = _finishedProgressSprite;
        }
    }
}