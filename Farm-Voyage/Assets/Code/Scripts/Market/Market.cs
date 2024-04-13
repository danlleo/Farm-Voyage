using Cameras;
using Character.Player;
using Character.Player.Events;
using UnityEngine;
using Zenject;

namespace Market
{
    [DisallowMultipleComponent]
    public class Market : MonoBehaviour
    {
        public readonly ShoppingStateChangedEvent ShoppingStateChangedEvent = new();
        
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
            ShoppingStateChangedEvent.OnShoppingStateChanged += Market_OnShoppingStateChangedStateChanged;
        }

        private void OnDisable()
        {
            ShoppingStateChangedEvent.OnShoppingStateChanged -= Market_OnShoppingStateChangedStateChanged;
        }

        private void Market_OnShoppingStateChangedStateChanged(bool isShopping)
        {
            if (isShopping)
            {
                _player.Events.ShoppingEvent.Call(this, new PlayerShoppingEventArgs(true));
                _cameraController.SwitchToCamera(CameraState.Market);
                return;
            }
            
            _player.Events.ShoppingEvent.Call(this, new PlayerShoppingEventArgs(false));
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}
