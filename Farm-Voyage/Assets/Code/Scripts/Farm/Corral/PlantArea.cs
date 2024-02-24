using System.Collections;
using Character.Player;
using Common;
using Farm.Plants;
using Farm.Tool;
using UI.Icon;
using UnityEngine;
using Utilities;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public sealed class PlantArea : MonoBehaviour, IInteractable, IDisplayIcon
    {
        [field:SerializeField] public IconSO Icon { get; private set; }
        
        [Header("Settings")]
        [SerializeField] private PlantDetails _plantDetails;
        [SerializeField, Range(0.1f, 2f)] private float _timeToDigInSeconds = 1f;
        [SerializeField, Range(0.1f, 3f)] private float _delayBeforePlantingNewTimeInSeconds = 1.4f;

        private Corral _corral;
        private Player _player;
        private PlantFactory _plantFactory;
        
        private Tool.Tool _playerTool;

        private Plant _plant;
        
        private PlayerInventory _playerInventory;
        private Day.Day _day;
        
        private BoxCollider _boxCollider;

        private Coroutine _diggingRoutine;
        private Coroutine _delayBeforePlantingNewRoutine;

        private bool _dayEnded;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, Day.Day day)
        {
            _playerInventory = playerInventory;
            _day = day;
        }
        
        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            _day.OnDayEnded += Day_OnDayEnded;
        }

        private void OnDisable()
        {
            _day.OnDayEnded -= Day_OnDayEnded;
        }

        private void Day_OnDayEnded()
        {
            _dayEnded = true;
        }

        public void Initialize(Corral corral, Player player, PlantFactory plantFactory)
        {
            _corral = corral;
            _player = player;
            _plantFactory = plantFactory;
        }

        public void Interact()
        {
            if (_dayEnded) return;
            if (_delayBeforePlantingNewRoutine != null) return;
            if (!TryAllowDigging(out Tool.Tool _)) return;
            if (!_playerInventory.HasEnoughSeedQuantity(_plantDetails.RequiredSeedType,
                    _plantDetails.RequiredSeedQuantityToPlant))
                return;
            
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
            Plant plant = _plantFactory.Create(_plantDetails.RequiredPlantType);
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
