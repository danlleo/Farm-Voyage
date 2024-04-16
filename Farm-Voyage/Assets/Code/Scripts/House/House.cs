using System;
using Character.Player;
using DG.Tweening;
using Sound;
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
        
        private PlayerFollowCamera _playerFollowCamera;
        private Player _player;
        
        private bool _canSleep;
        
        [Inject]
        private void Construct(Timespan.TimeManager timeManager, PlayerFollowCamera playerFollowCamera, Player player)
        {
            _playerFollowCamera = playerFollowCamera;
            _player = player;
        }

        private void OnEnable()
        {
            _player.Events.LeftHomeEvent.OnPlayerLeftHome += OnLeftHome;
        }

        private void OnDisable()
        {
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
                SoundFXManager.Instance.PlaySoundFX2DClip(_doorOpenAudioClip, 0.4f);

            SoundFXManager.Instance.PlaySoundFX2DClip(_doorCloseAudioClip, 0.4f);
            _doorTransform.DORotate(Vector3.up * targetRotation, durationInSeconds);
        }
        
        private void OnLeftHome(object sender, EventArgs e)
        {
            ChangeDoorState(false);
        }
    }
}