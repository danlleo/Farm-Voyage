using Character.Michael.Locomotion;
using UnityEngine;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MichaelAnimationsController : MonoBehaviour
    {
        private MichaelLocomotionStateChangedEvent _michaelLocomotionStateChangedEvent;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _michaelLocomotionStateChangedEvent = GetComponent<MichaelLocomotionStateChangedEvent>();
        }

        private void OnEnable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking += Michael_OnMichaelLocomotionStateChanged;
        }

        private void OnDisable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking -= Michael_OnMichaelLocomotionStateChanged;
        }

        private void Michael_OnMichaelLocomotionStateChanged(bool isWalking)
        {
            _animator.SetBool(MichaelAnimationsParams.IsWalking, isWalking);
        }
    }
}