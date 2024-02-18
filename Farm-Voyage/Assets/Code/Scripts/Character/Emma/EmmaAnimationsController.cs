using System;
using UnityEngine;

namespace Character.Emma
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class EmmaAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market.Market _market;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _market.StartedShoppingEvent.OnStartedShopping += StartedShoppingEvent_OnStartedShopping;
        }

        private void OnDisable()
        {
            _market.StartedShoppingEvent.OnStartedShopping -= StartedShoppingEvent_OnStartedShopping;
        }

        private void StartedShoppingEvent_OnStartedShopping(object sender, EventArgs e)
        {
            _animator.SetTrigger(EmmaAnimationsParams.OnGreeting);
        }
    }
}