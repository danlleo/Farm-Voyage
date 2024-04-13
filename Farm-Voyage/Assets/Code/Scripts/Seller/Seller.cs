using Cameras;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Seller
{
    public class Seller : MonoBehaviour
    {
        public readonly SellingStateChangedEvent SellingStateChangedEvent = new();

        private Player _player;
        private CameraController _cameraController;
        
        [Inject]
        private void Construct(Player player, CameraController cameraController)
        {
            _player = player;
            _cameraController = cameraController;
        }
        
        private void OnEnable()
        {
            SellingStateChangedEvent.OnSellingStateChanged += Seller_OnSellingStateChanged;
        }

        private void OnDisable()
        {
            SellingStateChangedEvent.OnSellingStateChanged -= Seller_OnSellingStateChanged;
        }

        private void Seller_OnSellingStateChanged(bool isSelling)
        {
            if (isSelling)
            {
                _player.Events.SellingStateChangedEvent.Call(true);
                _cameraController.SwitchToCamera(CameraState.Workbench);
                return;
            }

            _player.Events.SellingStateChangedEvent.Call(false);
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}