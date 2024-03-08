using Character.Player;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace House
{
    [DisallowMultipleComponent]
    public class House : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Transform _doorTransform;
        
        private Timespan.Day _day;
        private PlayerFollowCamera _playerFollowCamera;
        
        private bool _canSleep;
        
        [Inject]
        private void Construct(Timespan.DayManager dayManager, PlayerFollowCamera playerFollowCamera)
        {
            _day = dayManager.CurrentDay;
            _playerFollowCamera = playerFollowCamera;
        }

        private void OnEnable()
        {
            _day.OnDayEnded += Day_OnDayEnded;
        }

        private void OnDisable()
        {
            _day.OnDayEnded -= Day_OnDayEnded;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canSleep) return;
            if (!other.TryGetComponent(out Player player)) return;
            
            OpenDoor();
            _playerFollowCamera.LooseTarget();
            _playerFollowCamera.ZoomOutOfPlayer();
            player.PlayerEnteringHomeEvent.Call(this);
        }

        private void OpenDoor()
        {
            const float durationInSeconds = 1f;
            const float targetRotation = 90f;

            _doorTransform.DORotate(Vector3.up * targetRotation, durationInSeconds);
        }
        
        private void Day_OnDayEnded()
        {
            _canSleep = true;
        }
    }
}