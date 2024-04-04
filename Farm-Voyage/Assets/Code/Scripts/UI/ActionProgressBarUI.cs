using System.Collections;
using Attributes.WithinParent;
using Character.Player;
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
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void Update()
        {
            PositionBarNearPlayer();
        }

        private void PositionBarNearPlayer()
        {
            if (_player.transform == null) return;
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(_player.transform.position);
            _progressBarContainer.transform.position = screenPoint + _barOffset;
        }
        
        private void StartProgress(float duration)
        {
            _progressBar.fillAmount = 0;
            StartCoroutine(UpdateProgress(duration));
        }

        private void StopProgress()
        {
            StopAllCoroutines();
            _progressBar.gameObject.SetActive(false);
        }

        private IEnumerator UpdateProgress(float duration)
        {
            float timePassed = 0;
            while (timePassed < duration)
            {
                timePassed += Time.deltaTime;
                _progressBar.fillAmount = timePassed / duration;
                yield return null;
            }
        }
    }
}
