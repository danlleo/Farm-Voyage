using System;
using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerAnimationsController : MonoBehaviour
    {
        private PlayerWalkingEvent _playerWalkingEvent;
        private PlayerIdleEvent _playerIdleEvent;

        private Animator _animator;
        private Player _player;

        private void Awake()
        {
            _playerIdleEvent = GetComponent<PlayerIdleEvent>();
            _playerWalkingEvent = GetComponent<PlayerWalkingEvent>();
            _animator = GetComponent<Animator>();
            _player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            _playerIdleEvent.OnPlayerIdle += PlayerIdleEvent_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking += PlayerWalkingEvent_OnPlayerWalking;
        }

        private void OnDisable()
        {
            _player.PlayerIdleEvent.OnPlayerIdle -= PlayerIdleEvent_OnPlayerIdle;
            _player.PlayerWalkingEvent.OnPlayerWalking -= PlayerWalkingEvent_OnPlayerWalking;
        }

        private void PlayerIdleEvent_OnPlayerIdle(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, false);
        }

        private void PlayerWalkingEvent_OnPlayerWalking(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, true);
        }
    }
}