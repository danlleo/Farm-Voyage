using System.Collections.Generic;
using System.Linq;
using Attributes.Self;
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
        private const float CompressedPlantScale = 0.85f;
        
        [Header("Settings")]
        [SerializeField, Self] private Transform[] _storePoints;
        [SerializeField, Range(0.1f, 2f)] private float _movePlantSpeedInSeconds;
        [SerializeField, Range(0.1f, 2f)] private float _compressPlantScaleSpeedInSeconds;
        
        private Corral _corral;
        
        private BoxCollider _boxCollider;
        
        private Dictionary<Transform, Plant> _plantsDictionary = new();

        private bool _canCarry;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
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

        public void Initialize(Corral corral)
        {
            _corral = corral;
            _corral.PlantAreaClearedEvent.OnPlantAreaCleared += PlantAreaClearedEvent_OnPlantAreaCleared;
        }
        
        public void Interact()
        {
            if (!_canCarry) return;
            _boxCollider.Disable();
        }

        public void StopInteract()
        {
            // Decide what to do in here later
        }

        private void StorePlant(Plant plant)
        {
            if (_canCarry) return;
            if (!TryGetEmptyBoxPoint(out Transform emptyPoint)) return;

            _plantsDictionary[emptyPoint] = plant;
            MoveToBox(plant);
            
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

        private bool IsBoxFull()
        {
            return _plantsDictionary.All(keyValuePair => keyValuePair.Value != null);
        }

        private void MoveToBox(Plant plant)
        {
            if (!TryGetEmptyBoxPoint(out Transform emptyPoint))
                return;
            
            plant.transform.SetParent(emptyPoint);
            plant.transform.DOLocalMove(Vector3.zero, _movePlantSpeedInSeconds);
            plant.transform.DOScale(CompressedPlantScale, _compressPlantScaleSpeedInSeconds);
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
