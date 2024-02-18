using System;
using UnityEngine;

namespace Character.George
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class GeorgeAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Seller.Seller _seller;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _seller.StartedSellingEvent.OnStartedSelling += StartedSellingEvent_OnStartedSelling;
        }

        private void OnDisable()
        {
            _seller.StartedSellingEvent.OnStartedSelling -= StartedSellingEvent_OnStartedSelling;
        }

        private void StartedSellingEvent_OnStartedSelling(object sender, EventArgs e)
        {
            int salutingAnimationLayer = _animator.GetLayerIndex(GeorgeAnimationLayers.Saluting);
            
            _animator.SetLayerWeight(salutingAnimationLayer, 1f);
            _animator.SetTrigger(GeorgeAnimationParams.OnSalute);
        }
    }
}
