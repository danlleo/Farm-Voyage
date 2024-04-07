using System.Collections;
using Attributes.WithinParent;
using Character.Player;
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

        private float _savedProgress;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void OnDisable()
        {
            StopProgress();
        }

        private void Update()
        {
            PositionBarNearPlayer();
        }

        public void StartProgress(float currentProgress, float maxProgress)
        {
            if (_savedProgress == currentProgress)
            {
                return;
            }

            _savedProgress = currentProgress;
            
            CoroutineHandler.StartAndAssignIfNull(this, ref _updateProgressRoutine,
                UpdateProgressRoutine(currentProgress, maxProgress));
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
            float timePassed = 0;
            while (timePassed < _barFillDurationInSeconds)
            {
                timePassed += Time.deltaTime;
                _progressBar.fillAmount = currentProgress / maxProgress;
                yield return null;
            }
            
            StopProgress();
        }
    }
}
