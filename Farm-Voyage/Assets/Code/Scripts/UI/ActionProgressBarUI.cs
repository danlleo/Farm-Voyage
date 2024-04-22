using Attributes.WithinParent;
using Character.Player;
using Misc;
using UnityEngine;
using UnityEngine.UI;
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
        
        private Player _player;

        private Observable<float> _currentProgress;

        private float _maxProgress;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void OnDisable()
        {
            _currentProgress.OnValueChanged -= CurrentProgress_OnValueChanged;
            ClearProgress();
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
            _progressBar.gameObject.SetActive(true);
            
            UpdateProgress(currentProgress.Value, maxProgress);
        }

        private void UpdateProgress(float currentProgress, float maxProgress)
        {
            if (currentProgress == 0f || currentProgress >= maxProgress)
            {
                _progressBarContainer.gameObject.SetActive(false);
                return;
            }

            if (!_progressBarContainer.gameObject.activeSelf)
                _progressBarContainer.gameObject.SetActive(true);
                
            _progressBar.fillAmount = currentProgress / maxProgress;
        }
        
        private void ClearProgress()
        {
            _currentProgress = null;
            _maxProgress = 0f;
        }
        
        private void PositionBarNearPlayer()
        {
            if (_player.transform == null) return;
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(_player.transform.position);
            _progressBarContainer.transform.position = screenPoint + _barOffset;
        }
        
        private void CurrentProgress_OnValueChanged(float currentProgress)
        {
            UpdateProgress(currentProgress, _maxProgress);
        }
    }
}
