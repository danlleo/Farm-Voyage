using System.Collections;
using Character.Player;
using Common;
using Farm.Plants;
using Farm.Tool;
using UnityEngine;
using Utilities;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public sealed class PlantArea : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private PlantType _plantType;
        [SerializeField, Range(0.1f, 2f)] private float _timeToDigInSeconds = 1f;
        [SerializeField, Range(0.1f, 3f)] private float _delayBeforePlantingNewTimeInSeconds = 1.4f;

        private Corral _corral;
        private Player _player;
        private PlantFactory _plantFactory;
        
        private Tool.Tool _playerTool;

        private Plant _plant;
        private PlayerInventory _playerInventory;
        
        private BoxCollider _boxCollider;

        private Coroutine _diggingRoutine;
        private Coroutine _delayBeforePlantingNewRoutine;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        public void Initialize(Corral corral, Player player, PlantFactory plantFactory)
        {
            _corral = corral;
            _player = player;
            _plantFactory = plantFactory;
        }

        public void Interact()
        {
            if (!TryAllowDigging(out Tool.Tool _)) return;
            if (_delayBeforePlantingNewRoutine != null) return;
            
            _diggingRoutine ??= StartCoroutine(DiggingRoutine());
        }

        public void StopInteract()
        {
            StopDigging();
        }
        
        public void ClearPlantArea()
        {
            _boxCollider.Enable();
            _delayBeforePlantingNewRoutine ??= StartCoroutine(DelayBeforePlantingNewRoutine());
            _corral.PlantAreaClearedEvent.Call(this, new PlantAreaClearedEventArgs(_plant));
            _plant = null;
        }

        private IEnumerator DiggingRoutine()
        {
            _player.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
            yield return new WaitForSeconds(_timeToDigInSeconds);
            SpawnPlant();
            _boxCollider.Disable();
        }

        private IEnumerator DelayBeforePlantingNewRoutine()
        {
            yield return new WaitForSeconds(_delayBeforePlantingNewTimeInSeconds);
            ClearDelayBeforePlantingNewRoutine();
        }
        
        private void StopDigging()
        {
            ClearDiggingRoutine();
            _player.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
        }

        private bool TryAllowDigging(out Tool.Tool playerTool)
        {
            if (!_playerInventory.TryGetTool(out Shovel shovel))
            {
                playerTool = null;
                return false;
            }
            
            playerTool = shovel;

            return true;
        }
        
        private void SpawnPlant()
        {
            Plant plant = _plantFactory.Create(_plantType);
            plant.Initialize(transform.position, Quaternion.identity, this, _playerInventory);
            _plant = plant;
        }

        private void ClearDiggingRoutine()
        {
            if (_diggingRoutine == null) return;
            
            StopCoroutine(_diggingRoutine);
            _diggingRoutine = null;
        }

        private void ClearDelayBeforePlantingNewRoutine()
        {
            if (_delayBeforePlantingNewRoutine == null) return;
            
            StopCoroutine(_delayBeforePlantingNewRoutine);
            _delayBeforePlantingNewRoutine = null;
        }
    }
}
