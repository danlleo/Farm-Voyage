using System;
using Character.Michael.Locomotion;
using UnityEngine;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelIdleEvent))]
    [RequireComponent(typeof(MichaelWalkingEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MichaelAnimationsController : MonoBehaviour
    {
        private MichaelIdleEvent _michaelIdleEvent;
        private MichaelWalkingEvent _michaelWalkingEvent;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _michaelIdleEvent = GetComponent<MichaelIdleEvent>();
            _michaelWalkingEvent = GetComponent<MichaelWalkingEvent>();
        }

        private void OnEnable()
        {
            _michaelIdleEvent.OnMichaelIdle += MichaelIdleEvent_OnMichaelIdle;
            _michaelWalkingEvent.OnMichaelWalking += MichaelWalkingEvent_OnMichaelWalking;
        }

        private void OnDisable()
        {
            _michaelIdleEvent.OnMichaelIdle -= MichaelIdleEvent_OnMichaelIdle;
            _michaelWalkingEvent.OnMichaelWalking -= MichaelWalkingEvent_OnMichaelWalking;
        }

        private void MichaelWalkingEvent_OnMichaelWalking()
        {
            throw new NotImplementedException();
        }

        private void MichaelIdleEvent_OnMichaelIdle()
        {
            throw new NotImplementedException();
        }
    }
}