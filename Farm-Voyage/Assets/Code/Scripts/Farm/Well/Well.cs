using System;
using Character.Player;
using Common;
using Farm.Tool.ConcreteTools;
using UI.Icon;
using UnityEngine;
using Zenject;

namespace Farm.Well
{
    [RequireComponent(typeof(WaterCanFilledEvent))]
    [DisallowMultipleComponent]
    public class Well : MonoBehaviour, IInteractable, IDisplayIcon
    {
        public WaterCanFilledEvent WaterCanFilledEvent { get; private set; }
        
        [field:SerializeField] public IconSO Icon { get; private set; }

        private Player _player;
        private PlayerInventory _playerInventory;
        private PlayerFollowCamera _playerFollowCamera;
        
        private bool _isExtractingWater;
        
        [Inject]
        private void Construct(Player player, PlayerInventory playerInventory, PlayerFollowCamera playerFollowCamera)
        {
            _player = player;
            _playerInventory = playerInventory;
            _playerFollowCamera = playerFollowCamera;
        }

        private void Awake()
        {
            WaterCanFilledEvent = GetComponent<WaterCanFilledEvent>();
        }

        private void OnEnable()
        {
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater += Player_OnPlayerExtractingWater;
        }

        private void OnDisable()
        {
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater -= Player_OnPlayerExtractingWater;
        }

        public void Interact()
        {
            if (!_playerInventory.TryGetTool(out WaterCan waterCan)) return;
            if (_isExtractingWater) return;
            if (waterCan.CurrentWaterCapacityAmount == WaterCan.WaterCanCapacityAmount) return;
            
            _isExtractingWater = true;
            _player.PlayerExtractingWaterEvent.Call(this, new PlayerExtractingWaterEventArgs(true));
            _playerFollowCamera.ZoomIn();
        }

        public void StopInteract()
        {
            
        }
        
        private void Player_OnPlayerExtractingWater(object sender, PlayerExtractingWaterEventArgs e)
        {
            if (e.IsExtracting) return;
            _playerFollowCamera.ZoomOut();
        }
    }
}
