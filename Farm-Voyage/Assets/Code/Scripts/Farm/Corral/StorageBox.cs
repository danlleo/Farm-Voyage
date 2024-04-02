using System.Collections.Generic;
using System.Linq;
using Attributes.WithinParent;
using Character;
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
        
        private readonly Dictionary<Transform, Plant> _plantsDictionary = new();

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

            _plantsDictionary[emptyPoint] = plant;
            MoveToBox(plant, emptyPoint);

            _canCarry = IsBoxFull();
        }
        
        private void InitializePlantsDictionary()
        {
            foreach (Transform storePoint in _storePoints)
            {
                _plantsDictionary[storePoint] = null;
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

                _plantsDictionary[storePoint] = null;
            }
        }
        
        private void ResetStorageBox()
        {
            _boxCollider.Enable();
            transform.SetParent(_corral.transform);
            transform.position = _initialPosition;
            _canCarry = false;
            _player.Events.CarryingStorageBoxStateChangedEvent.Call(this,
                new PlayerCarryingStorageBoxStateChangedEventArgs(this, false));
        }
        
        private bool IsBoxFull()
        {
            return _plantsDictionary.Values.All(value => value != null);
        }

        private void MoveToBox(Plant plant, Transform emptyPoint)
        {
            plant.transform.SetParent(emptyPoint);
            plant.transform.DOLocalMove(Vector3.zero, _movePlantSpeedInSeconds);
            plant.transform.DOScale(plant.PlantCompressedScale, _compressPlantScaleSpeedInSeconds);
        }

        private bool TryGetEmptyBoxPoint(out Transform emptyPoint)
        {
            emptyPoint = _storePoints.FirstOrDefault(point => _plantsDictionary[point] == null);
            return emptyPoint != null;
        }
        
        private void PlantArea_OnAnyPlantHarvested(Plant plant)
        {
            StorePlant(plant);
        }
    }
}
