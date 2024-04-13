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

        private Player _player;
        
        [Inject]
        private void Construct(CameraController cameraController, Player player)
        {
            _cameraController = cameraController;
            _player = player;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;

            _seller.SellingStateChangedEvent.Call(this);
            _cameraController.SwitchToCamera(CameraState.Seller);
            _player.Events.SellingStateChangedEvent.Call(true);
        }
    }
}