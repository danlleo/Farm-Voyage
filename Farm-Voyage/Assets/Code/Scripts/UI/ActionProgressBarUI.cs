using System.Collections;
using Attributes.WithinParent;
using Character.Player;
using Misc;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class ActionProgressBarUI : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private GameObject _progressBarContainer;
        [SerializeField, WithinParent] private Image _progressBar;

        [Header("Settings")]
        [SerializeField] private Vector2 _barOffset;
        [SerializeField] private float _barFillDurationInSeconds = 0.75f;
        
        private Player _player;

        private Coroutine _updateProgressRoutine;

        private Observable<float> _currentProgress;

        private float _maxProgress;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void OnDisable()
        {
            StopProgress();

            _currentProgress.OnValueChanged -= CurrentProgress_OnValueChanged;
        }

        private void Update()
        {
            PositionBarNearPlayer();
        }

        public void StartProgress(Observable<float> currentProgress, float maxProgress)
        {
            _maxProgress = maxProgress;
            _currentProgress = currentProgress;
            _currentProgress.OnValueChanged += CurrentProgress_OnValueChanged;

            UpdateProgress(currentProgress, maxProgress);
        }

        private void UpdateProgress(Observable<float> currentProgress, float maxProgress)
        {
            CoroutineHandler.StartAndAssignIfNull(this, ref _updateProgressRoutine,
                UpdateProgressRoutine(currentProgress.Value, maxProgress));
        }

        private void StopProgress()
        {
            CoroutineHandler.ClearAndStopCoroutine(this, ref _updateProgressRoutine);
        }
        
        private void PositionBarNearPlayer()
        {
            if (_player.transform == null) return;
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(_player.transform.position);
            _progressBarContainer.transform.position = screenPoint + _barOffset;
        }
        
        private IEnumerator UpdateProgressRoutine(float currentProgress, float maxProgress)
        {
            float startFillAmount = _progressBar.fillAmount;
            float endFillAmount = currentProgress / maxProgress;

            float timer = 0;
            while (timer < _barFillDurationInSeconds)
            {
                timer += Time.deltaTime;
                float t = timer / _barFillDurationInSeconds;
        
                _progressBar.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, t);

                yield return null;
            }

            _progressBar.fillAmount = endFillAmount;

            StopProgress();
        }
        
        private void CurrentProgress_OnValueChanged(float currentProgress)
        {
            CoroutineHandler.ReassignAndRestart(this, ref _updateProgressRoutine,
                UpdateProgressRoutine(currentProgress, _maxProgress));
        }
    }
}
