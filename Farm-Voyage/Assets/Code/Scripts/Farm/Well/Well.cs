using System.Collections;
using Cameras;
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
        [field:SerializeField] public IconSO Icon { get; private set; }

        private Player _player;
        private PlayerInventory _playerInventory;
        private CameraController _cameraController;

        private Coroutine _extractWaterRoutine;
        
        [Inject]
        private void Construct(Player player, PlayerInventory playerInventory, CameraController cameraController)
        {
            _player = player;
            _playerInventory = playerInventory;
            _cameraController = cameraController;
        }

        public void Interact()
        {
            if (!_playerInventory.TryGetTool(out WaterCan waterCan)) return;
            if (_extractWaterRoutine != null) return;
            if (waterCan.IsFullyFilled()) return;

            _extractWaterRoutine = StartCoroutine(ExtractWaterRoutine(waterCan));
        }

        public void StopInteract()
        {
            if (_extractWaterRoutine == null) return;
            
            StopCoroutine(_extractWaterRoutine);
            _extractWaterRoutine = null;
        }

        private IEnumerator ExtractWaterRoutine(WaterCan waterCan)
        {
            _player.PlayerExtractingWaterEvent.Call(this);
            yield return new WaitForSeconds(waterCan.CalculateTimeToGatherBasedOnLevel());
            waterCan.FillWaterCan();
        }
    }
}
