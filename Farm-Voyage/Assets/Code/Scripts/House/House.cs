using System;
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
        private Player _player;
        
        private bool _canSleep;
        
        [Inject]
        private void Construct(Timespan.DayManager dayManager, PlayerFollowCamera playerFollowCamera, Player player)
        {
            _day = dayManager.CurrentDay;
            _playerFollowCamera = playerFollowCamera;
            _player = player;
        }

        private void OnEnable()
        {
            _day.OnDayEnded += Day_OnDayEnded;
            _player.PlayerLeftHomeEvent.OnPlayerLeftHome += Player_OnPlayerLeftHome;
        }

        private void OnDisable()
        {
            _day.OnDayEnded -= Day_OnDayEnded;
            _player.PlayerLeftHomeEvent.OnPlayerLeftHome -= Player_OnPlayerLeftHome;
        }

        private void Start()
        {
            ChangeDoorState(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canSleep) return;
            if (!other.TryGetComponent(out Player player)) return;
            
            ChangeDoorState(true);
            
            _playerFollowCamera.LooseTarget();
            _playerFollowCamera.ZoomOutOfPlayer();
            player.PlayerEnteringHomeEvent.Call(this);
        }

        private void ChangeDoorState(bool isOpen)
        {
            const float durationInSeconds = .3f;
            float targetRotation = isOpen ? -90f : 0f;

            _doorTransform.DORotate(Vector3.up * targetRotation, durationInSeconds);
        }

        private void Day_OnDayEnded()
        {
            _canSleep = true;
        }
        
        private void Player_OnPlayerLeftHome(object sender, EventArgs e)
        {
            ChangeDoorState(false);
        }
    }
}