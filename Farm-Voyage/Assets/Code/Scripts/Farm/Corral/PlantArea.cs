using System;
using System.Collections;
using Character.Player;
using Character.Player.Events;
using Common;
using Farm.Plants;
using Farm.Plants.Seeds;
using Farm.Tool.ConcreteTools;
using Misc;
using Sound;
using UI.Icon;
using UnityEngine;
using Utilities;
using Zenject;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public sealed class PlantArea : MonoBehaviour, IInteractable, IStopInteractable, IDisplayProgressIcon, IInteractDisplayProgress
    {
        public static event Action<Plant> OnAnyPlantPlanted;
        public static event Action<Plant> OnAnyPlantHarvested;
        
        [field:SerializeField] public ProgressIconSO ProgressIcon { get; private set; }
        public Guid ID { get; } = Guid.NewGuid();
        public float MaxClampedProgress => 1f;
        public Observable<float> CurrentClampedProgress { get; } = new();

        [Header("External references")]
        [SerializeField] private AudioClip[] _plantSpawnAudioClips;
        
        [Header("Settings")] 
        [SerializeField, Range(1, 2)] private int _seedsNeededToPlant = 1;
        [SerializeField, Range(0.1f, 2f)] private float _timeToDigInSeconds = 1f;
        [SerializeField, Range(0.1f, 3f)] private float _delayBeforePlantingNewTimeInSeconds = 1.4f;

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

        public void Initialize(Player player, PlantFactory plantFactory)
        {
            _player = player;
            _plantFactory = plantFactory;
        }

        public void Interact(IVisitable initiator)
        {
            _player.Locomotion.StartStickRotation(transform, 2.5f);
            
            if (ConstrainDigging()) return;

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
            OnAnyPlantHarvested?.Invoke(_plant);
            ProgressIcon.RefreshProgress(this);
            _plant = null;
        }

        private bool ConstrainDigging()
        {
            if (_dayEnded) return true;
            if (_delayBeforePlantingNewRoutine != null) return true;
            if (!TryAllowDigging(out Tool.Tool _)) return true;
            if (!_playerInventory.TryGetSelectedSeed(out Seed selectedSeed))
            {
                _selectedSeed = null;
                return true;
            }

            _selectedSeed = selectedSeed;
            
            return !_playerInventory.HasEnoughSeedQuantity(selectedSeed.SeedType,
                _seedsNeededToPlant);
        }

        private void StartDigging()
        {
            CoroutineHandler.StartAndAssignIfNull(this, ref _diggingRoutine, DiggingRoutine());
        }
        
        private void StopDigging()
        {
            CoroutineHandler.ClearAndStopCoroutine(this, ref _diggingRoutine);
            _player.Events.DiggingPlantAreaStateChangedEvent.Call(this, 
                new PlayerDiggingPlantAreaEventArgs(false));
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
            if (_selectedSeed == null) return;
            
            Plant plant = _plantFactory.Create(_selectedSeed.Plant);
            plant.Initialize(transform.position, Quaternion.identity, this, _playerInventory);
            OnAnyPlantPlanted?.Invoke(plant);
            _plant = plant;
            SoundFXManager.Instance.PlayRandomSoundFXClip(_plantSpawnAudioClips, transform, 0.3f);
        }
        
        private IEnumerator DiggingRoutine()
        {
            _player.Events.DiggingPlantAreaStateChangedEvent.Call(this, 
                new PlayerDiggingPlantAreaEventArgs(true));

            float timer = 0f;

            while (timer <= _timeToDigInSeconds)
            {
                timer += Time.deltaTime;
                CurrentClampedProgress.Value = timer / _timeToDigInSeconds;
                yield return null;
            }
            
            SpawnPlant();
            
            _playerInventory.RemoveSeedQuantity(_selectedSeed.SeedType, _seedsNeededToPlant);
            _boxCollider.Disable();

            CurrentClampedProgress.Value = 0f;
        }

        private IEnumerator DelayBeforePlantingNewRoutine()
        {
            yield return new WaitForSeconds(_delayBeforePlantingNewTimeInSeconds);
            CoroutineHandler.ClearAndStopCoroutine(this, ref _delayBeforePlantingNewRoutine);
        }
        
        private void Day_OnDayEnded()
        {
            _dayEnded = true;
        }
    }
}
