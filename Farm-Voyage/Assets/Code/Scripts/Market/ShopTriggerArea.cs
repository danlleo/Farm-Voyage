using Cameras;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Market
{
    [DisallowMultipleComponent]
    public class ShopTriggerArea : MonoBehaviour
    {
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
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}
