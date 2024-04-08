using System;
using Character.Player;
using DG.Tweening;
using Sound;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using Zenject;

namespace House
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class House : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Transform _doorTransform;
        [SerializeField] private AudioClip _doorOpenAudioClip;
        [SerializeField] private AudioClip _doorCloseAudioClip;
        
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
            _player.Events.LeftHomeEvent.OnPlayerLeftHome += OnLeftHome;
        }

        private void OnDisable()
        {
            _day.OnDayEnded -= Day_OnDayEnded;
            _player.Events.LeftHomeEvent.OnPlayerLeftHome -= OnLeftHome;
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
            player.Events.EnteringHomeEvent.Call(this);
        }

        private void ChangeDoorState(bool isOpen)
        {
            const float durationInSeconds = .3f;
            float targetRotation = isOpen ? -90f : 0f;

            if (isOpen)
                SoundFXManager.Instance.PlaySoundFXClip(_doorOpenAudioClip, transform, 0.4f);

            SoundFXManager.Instance.PlaySoundFXClip(_doorCloseAudioClip, transform, 0.4f);
            _doorTransform.DORotate(Vector3.up * targetRotation, durationInSeconds);
        }

        private void Day_OnDayEnded()
        {
            _canSleep = true;
        }
        
        private void OnLeftHome(object sender, EventArgs e)
        {
            ChangeDoorState(false);
        }
    }
}