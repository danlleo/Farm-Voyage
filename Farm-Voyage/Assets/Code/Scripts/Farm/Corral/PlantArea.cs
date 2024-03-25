using System;
using System.Collections;
using Character;
using Character.Player;
using Common;
using Farm.Plants;
using Farm.Plants.Seeds;
using Farm.Tool.ConcreteTools;
using UI.Icon;
using UnityEngine;
using Utilities;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public sealed class PlantArea : MonoBehaviour, IInteractable, IStopInteractable, IDisplayProgressIcon
    {
        public static event Action<Plant> OnAnyPlantPlanted;
        public static event Action<Plant> OnAnyPlantHarvested;
        
        [field:SerializeField] public ProgressIconSO ProgressIcon { get; private set; }
        public Guid ID { get; } = Guid.NewGuid();

        [Header("Settings")] 
        [SerializeField, Range(1, 2)] private int _seedsNeededToPlant = 1;
        [SerializeField, Range(0.1f, 2f)] private float _timeToDigInSeconds = 1f;
        [SerializeField, Range(0.1f, 3f)] private float _delayBeforePlantingNewTimeInSeconds = 1.4f;

        private Corral _corral;
        private Player _player;
        private PlantFactory _plantFactory;
        
        private Tool.Tool _playerTool;

        private Plant _plant;
        private Seed _selectedSeed;
        
        private PlayerInventory _playerInventory;
        private Timespan.Day _day;
        
        private BoxCollider _boxCollider;

        private Coroutine _diggingRoutine;
        private Coroutine _delayBeforePlantingNewRoutine;

        private bool _dayEnded;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, Timespan.DayManager dayManager)
        {
            _playerInventory = playerInventory;
            _day = dayManager.CurrentDay;
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

        public void Initialize(Corral corral, Player player, PlantFactory plantFactory)
        {
            _corral = corral;
            _player = player;
            _plantFactory = plantFactory;
        }

        public void Interact(ICharacter initiator)
        {
            _player.PlayerLocomotion.HandleStickRotation(transform, 2.5f);
            
            if (_dayEnded) return;
            if (_delayBeforePlantingNewRoutine != null) return;
            if (!TryAllowDigging(out Tool.Tool _)) return;
            if (!_playerInventory.TryGetSelectedSeed(out Seed selectedSeed))
            {
                _selectedSeed = null;
                return;
            }

            _selectedSeed = selectedSeed;
            
            if (!_playerInventory.HasEnoughSeedQuantity(selectedSeed.SeedType,
                    _seedsNeededToPlant))
                return;
            
            StartDigging();
        }

        public void StopInteract()
        {
            StopDigging();
        }
        
        public void ClearPlantArea()
        {
            _boxCollider.Enable();
            _delayBeforePlantingNewRoutine ??= StartCoroutine(DelayBeforePlantingNewRoutine());
            
            // TODO: Clean this mess
            OnAnyPlantHarvested?.Invoke(_plant);
            _corral.PlantAreaClearedEvent.Call(this, new PlantAreaClearedEventArgs(_plant));
            _plant = null;
        }

        private IEnumerator DiggingRoutine()
        {
            _player.PlayerEvents.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
            
            yield return new WaitForSeconds(_timeToDigInSeconds);
            
            SpawnPlant();
            
            _playerInventory.RemoveSeedQuantity(_selectedSeed.SeedType, _seedsNeededToPlant);
            _boxCollider.Disable();
        }

        private IEnumerator DelayBeforePlantingNewRoutine()
        {
            yield return new WaitForSeconds(_delayBeforePlantingNewTimeInSeconds);
            ClearDelayBeforePlantingNewRoutine();
        }

        private void StartDigging()
        {
            _diggingRoutine ??= StartCoroutine(DiggingRoutine());
        }
        
        private void StopDigging()
        {
            ClearDiggingRoutine();
            _player.PlayerEvents.PlayerDiggingPlantAreaEvent.Call(this, new PlayerDiggingPlantAreaEventArgs(true));
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
            Plant plant = _plantFactory.Create(_selectedSeed.Plant);
            plant.Initialize(transform.position, Quaternion.identity, this, _playerInventory);
            OnAnyPlantPlanted?.Invoke(plant);
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
        
        private void Day_OnDayEnded()
        {
            _dayEnded = true;
        }
    }
}
