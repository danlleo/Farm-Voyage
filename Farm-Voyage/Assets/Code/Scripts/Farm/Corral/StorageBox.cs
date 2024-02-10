using System.Collections.Generic;
using System.Linq;
using Attributes.Self;
using Character.Player;
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
        [SerializeField, Self] private Transform[] _storePoints;
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
            if (_corral != null)    
                _corral.PlantAreaClearedEvent.OnPlantAreaCleared += PlantAreaClearedEvent_OnPlantAreaCleared;
        }

        private void OnDisable()
        {
            _corral.PlantAreaClearedEvent.OnPlantAreaCleared -= PlantAreaClearedEvent_OnPlantAreaCleared;
        }

        public void Initialize(Corral corral, Player player)
        {
            _corral = corral;
            _player = player;
            _corral.PlantAreaClearedEvent.OnPlantAreaCleared += PlantAreaClearedEvent_OnPlantAreaCleared;
        }
        
        public void Interact()
        {
            if (!_canCarry) return;
            Pickup();
        }

        public void StopInteract()
        {
            // Decide what to do in here later
        }

        public void ClearAndPutToInitialPosition()
        {
            ClearFromPlants();
            ClearPlantsDictionary();
            _boxCollider.Enable();
            transform.position = _initialPosition;
            _player.PlayerCarryingStorageBoxStateChangedEvent.Call(this,
                new PlayerCarryingStorageBoxStateChangedEventArgs(false));
        }
        
        private void Pickup()
        {
            _boxCollider.Disable();
            _player.CarryStorageBox(this);
        }
        
        private void StorePlant(Plant plant)
        {
            if (_canCarry) return;
            if (!TryGetEmptyBoxPoint(out Transform emptyPoint)) return;

            _plantsDictionary[emptyPoint] = plant;
            MoveToBox(plant, emptyPoint);
            
            if (IsBoxFull())
                _canCarry = true;
        }
        
        private void InitializePlantsDictionary()
        {
            foreach (Transform storePoint in _storePoints)
            {
                _plantsDictionary.Add(storePoint, null);
            }
        }

        private void ClearPlantsDictionary()
        {
            foreach (KeyValuePair<Transform, Plant> keyValuePair in _plantsDictionary)
            {
                _plantsDictionary[keyValuePair.Key] = null;
            }   
        }

        private void ClearFromPlants()
        {
            foreach (KeyValuePair<Transform, Plant> keyValuePair in _plantsDictionary)
            {
                Destroy(_plantsDictionary[keyValuePair.Key].gameObject);
            }   
        }
        
        private bool IsBoxFull()
        {
            return _plantsDictionary.All(keyValuePair => keyValuePair.Value != null);
        }

        private void MoveToBox(Plant plant, Transform emptyPoint)
        {
            plant.transform.SetParent(emptyPoint);
            plant.transform.DOLocalMove(Vector3.zero, _movePlantSpeedInSeconds);
            plant.transform.DOScale(plant.PlantCompressedScale, _compressPlantScaleSpeedInSeconds);
        }

        private bool TryGetEmptyBoxPoint(out Transform emptyPoint)
        {
            foreach (Transform storePoint in _storePoints)
            {
                if (!_plantsDictionary.TryGetValue(storePoint, out Plant plant)) continue;
                if (plant != null) continue;
                
                emptyPoint = storePoint;
                return true;
            }

            emptyPoint = null;
            return false;
        }
        
        private void PlantAreaClearedEvent_OnPlantAreaCleared(object sender, PlantAreaClearedEventArgs e)
        {
            StorePlant(e.Plant);
        }
    }
}
