using System;
using Character.Player;
using Common;
using Farm.Tool.ConcreteTools;
using UI.Icon;
using UnityEngine;
using Zenject;

namespace Farm.Well
{
    [DisallowMultipleComponent]
    public class Well : MonoBehaviour, IInteractable, IDisplayIcon
    {
        public readonly WaterCanFilledEvent WaterCanFilledEvent = new();
        
        [field:SerializeField] public IconSO Icon { get; private set; }
        public Guid ID { get; } = Guid.NewGuid();

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
        
        private void OnEnable()
        {
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged +=
                Player_OnExtractingWaterStateChanged;
        }

        private void OnDisable()
        {
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged -=
                Player_OnExtractingWaterStateChanged;
        }

        public void Interact(IVisitable initiator)
        {
            if (!_playerInventory.TryGetTool(out WaterCan waterCan)) return;
            if (_isExtractingWater) return;
            if (waterCan.CurrentWaterCapacityAmount == WaterCan.WaterCanCapacityAmount) return;
            
            _isExtractingWater = true;
            _player.Events.ExtractingWaterStateChangedEvent.Call(true);
            _playerFollowCamera.ZoomIn();
        }
        
        private void Player_OnExtractingWaterStateChanged(bool isExtractingWater)
        {
            if (isExtractingWater) return;
            _playerFollowCamera.ZoomOut();
            _isExtractingWater = false;
        }
    }
}
