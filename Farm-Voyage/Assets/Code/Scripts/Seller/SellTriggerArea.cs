using Cameras;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Seller
{
    [DisallowMultipleComponent]
    public class SellTriggerArea : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Seller _seller;
        
        private CameraController _cameraController;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _cameraController = cameraController;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            _seller.StartedSellingEvent.Call(this);
            _cameraController.SwitchToCamera(CameraState.Seller);
        }
    }
}