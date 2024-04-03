using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Attributes.WithinParent;
using Character.Player;
using Character.Player.Events;
using Common;
using DG.Tweening;
using Farm.Plants;
using UnityEngine;
using Utilities;

namespace Farm.Corral
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class StorageBox : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField, WithinParent] private Transform[] _storePoints;
        [SerializeField, Range(0.1f, 2f)] private float _movePlantSpeedInSeconds;
        [SerializeField, Range(0.1f, 2f)] private float _compressPlantScaleSpeedInSeconds;
        
        private Corral _corral;
        private Player _player;
        
        private BoxCollider _boxCollider;

        private Vector3 _initialPosition;

        private readonly Dictionary<Transform, Plant> _transformToPlantMapping = new();
        private readonly Dictionary<PlantType, int> _plantQuantitiesMapping = new();
        
        private bool _canCarry;

        private void Awake()   
        {
            _boxCollider = GetComponent<BoxCollider>();
            _initialPosition = transform.position;
            InitializePlantsDictionary();
        }

        private void OnEnable()
        {
            PlantArea.OnAnyPlantHarvested += PlantArea_OnAnyPlantHarvested;
        }

        private void OnDisable()
        {
            PlantArea.OnAnyPlantHarvested -= PlantArea_OnAnyPlantHarvested;
        }

        public void Initialize(Corral corral, Player player)
        {
            _corral = corral;
            _player = player;
        }
        
        public void Interact(IVisitable initiator)
        {
            Pickup();
        }

        public ReadOnlyDictionary<PlantType, int> ReadPlantsQuantities()
        {
            return new ReadOnlyDictionary<PlantType, int>(_plantQuantitiesMapping);
        }
        
        public void ClearAndPutToInitialPosition()
        {
            ClearPlants();
            ResetStorageBox();
        }

        private void Pickup()
        {
            if (!_canCarry) return;
            
            _boxCollider.Disable();
            _player.Events.CarryingStorageBoxStateChangedEvent.Call(this,
                new PlayerCarryingStorageBoxStateChangedEventArgs(this, true));
        }
        
        private void StorePlant(Plant plant)
        {
            if (_canCarry) return;
            if (!TryGetEmptyBoxPoint(out Transform emptyPoint)) return;

            _transformToPlantMapping[emptyPoint] = plant;
            MoveToBox(plant, emptyPoint);

            _canCarry = IsBoxFull();

            if (_plantQuantitiesMapping.TryGetValue(plant.Type, out int storedQuantity))
            {
                _plantQuantitiesMapping[plant.Type] = storedQuantity + 1;
                return;
            }

            _plantQuantitiesMapping.Add(plant.Type, 1);
        }
        
        private void InitializePlantsDictionary()
        {
            foreach (Transform storePoint in _storePoints)
            {
                _transformToPlantMapping[storePoint] = null;
            }
        }

        private void ClearPlants()
        {
            foreach (Transform storePoint in _storePoints)
            {
                if (storePoint.childCount > 0)
                {
                    Destroy(storePoint.GetChild(0).gameObject);
                }

                _transformToPlantMapping[storePoint] = null;
            }
        }
        
        private void ResetStorageBox()
        {
            _boxCollider.Enable();
            transform.SetParent(_corral.transform);
            _plantQuantitiesMapping.Clear();
            
            transform.position = _initialPosition;
            _canCarry = false;
            _player.Events.CarryingStorageBoxStateChangedEvent.Call(this,
                new PlayerCarryingStorageBoxStateChangedEventArgs(this, false));
        }
        
        private bool IsBoxFull()
        {
            return _transformToPlantMapping.Values.All(value => value != null);
        }

        private void MoveToBox(Plant plant, Transform emptyPoint)
        {
            plant.transform.SetParent(emptyPoint);
            plant.transform.DOLocalMove(Vector3.zero, _movePlantSpeedInSeconds);
            plant.transform.DOScale(plant.PlantCompressedScale, _compressPlantScaleSpeedInSeconds);
        }

        private bool TryGetEmptyBoxPoint(out Transform emptyPoint)
        {
            emptyPoint = _storePoints.FirstOrDefault(point => _transformToPlantMapping[point] == null);
            return emptyPoint != null;
        }
        
        private void PlantArea_OnAnyPlantHarvested(Plant plant)
        {
            StorePlant(plant);
        }
    }
}
