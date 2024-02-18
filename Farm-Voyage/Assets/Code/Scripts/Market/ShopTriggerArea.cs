using Cameras;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Market
{
    [DisallowMultipleComponent]
    public class ShopTriggerArea : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market _market;
        
        private CameraController _cameraController;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _cameraController = cameraController;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _cameraController.SwitchToCamera(CameraState.Market);
            _market.StartedShoppingEvent.Call(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}
