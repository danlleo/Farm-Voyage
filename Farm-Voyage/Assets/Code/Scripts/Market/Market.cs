using System;
using Cameras;
using Character.Player;
using UnityEngine;
using Zenject;

namespace Market
{
    [RequireComponent(typeof(StartedShoppingEvent))]
    [RequireComponent(typeof(StoppedShoppingEvent))]
    [DisallowMultipleComponent]
    public class Market : MonoBehaviour
    {
        public StartedShoppingEvent StartedShoppingEvent { get; private set; }
        public StoppedShoppingEvent StoppedShoppingEvent { get; private set; }
        
        private Player _player;
        private CameraController _cameraController;

        [Inject]
        private void Construct(Player player, CameraController cameraController)
        {
            _player = player;
            _cameraController = cameraController;
        }
        
        private void Awake()
        {
            StartedShoppingEvent = GetComponent<StartedShoppingEvent>();
            StoppedShoppingEvent = GetComponent<StoppedShoppingEvent>();
        }

        private void OnEnable()
        {
            StartedShoppingEvent.OnStartedShopping += StartedShoppingEvent_OnStartedShopping;
            StoppedShoppingEvent.OnStoppedShopping += StoppedShoppingEvent_OnStoppedShopping;
        }

        private void OnDisable()
        {
            StartedShoppingEvent.OnStartedShopping -= StartedShoppingEvent_OnStartedShopping;
            StoppedShoppingEvent.OnStoppedShopping -= StoppedShoppingEvent_OnStoppedShopping;
        }

        private void StartedShoppingEvent_OnStartedShopping(object sender, EventArgs e)
        {
            _player.PlayerEvents.PlayerShoppingEvent.Call(this, new PlayerShoppingEventArgs(true));
            _cameraController.SwitchToCamera(CameraState.Market);
        }
        
        private void StoppedShoppingEvent_OnStoppedShopping(object sender, EventArgs e)
        {
            _player.PlayerEvents.PlayerShoppingEvent.Call(this, new PlayerShoppingEventArgs(false));
            _cameraController.SwitchToCamera(CameraState.Main);
        }
    }
}
