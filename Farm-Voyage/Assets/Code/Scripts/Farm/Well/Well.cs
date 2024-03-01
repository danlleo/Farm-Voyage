using Character.Player;
using Common;
using Farm.Tool;
using Farm.Tool.ConcreteTools;
using UnityEngine;
using Zenject;

namespace Farm.Well
{
    [DisallowMultipleComponent]
    public class Well : MonoBehaviour, IInteractable
    {
        private PlayerInventory _playerInventory;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }

        public void Interact()
        {
            if (!_playerInventory.TryGetTool(out WaterCan waterCan)) return;
            if (waterCan.IsFullyFilled()) return;
            
            Debug.Log("Filled");
            waterCan.FillWaterCan();
        }

        public void StopInteract()
        {
            // TODO: stopped filling
        }
    }
}
