﻿using System;
using Farm;
using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class PlayerAnimationsController : MonoBehaviour
    {
        private PlayerWalkingEvent _playerWalkingEvent;
        private PlayerIdleEvent _playerIdleEvent;
        private PlayerGatheringEvent _playerGatheringEvent;

        private Animator _animator;

        private void Awake()
        {
            _playerIdleEvent = GetComponent<PlayerIdleEvent>();
            _playerWalkingEvent = GetComponent<PlayerWalkingEvent>();
            _playerGatheringEvent = GetComponent<PlayerGatheringEvent>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerIdleEvent.OnPlayerIdle += PlayerIdleEvent_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking += PlayerWalkingEvent_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering += PlayerGatheringEvent_OnPlayerGathering;
        }

        private void OnDisable()
        {
            _playerIdleEvent.OnPlayerIdle -= PlayerIdleEvent_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking -= PlayerWalkingEvent_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering -= PlayerGatheringEvent_OnPlayerGathering;
        }

        private void PlayerIdleEvent_OnPlayerIdle(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, false);
        }

        private void PlayerWalkingEvent_OnPlayerWalking(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, true);
        }
        
        private void PlayerGatheringEvent_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            _animator.speed = e.GatheringSpeed;
            
            switch (e.ResourceType)
            {
                case ResourceType.Rock:
                    _animator.SetBool(PlayerAnimationParams.IsMining, e.IsGathering);
                    break;
                case ResourceType.Wood:
                    break;
                case ResourceType.Dirt:
                    _animator.SetBool(PlayerAnimationParams.IsDigging, e.IsGathering);
                    break;
                case ResourceType.Water:
                    break;
                case ResourceType.Wheat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}