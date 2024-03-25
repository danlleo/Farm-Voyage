using System;
using System.Collections.Generic;
using System.Linq;
using Farm.Corral;
using Farm.Plants;
using Farm.Plants.ConcreteStates;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class WaterStateObserver : MonoBehaviour
    {
        public event Action<Plant, bool> OnPlantWateringStateChanged;
        
        private Dictionary<Plant, bool> _plantToNeedsWateringMapping;
        
        private void Awake()
        {
            _plantToNeedsWateringMapping = new Dictionary<Plant, bool>();
        }

        private void OnEnable()
        {
            PlantArea.OnAnyPlantPlanted += PlantArea_OnAnyPlantPlanted;
            WateringState.OnAnyWateringStateChanged += WateringState_OnAnyWateringStateChanged;
            PlantArea.OnAnyPlantHarvested += Plant_OnAnyPlantHarvested;
        }

        private void OnDisable()
        {
            PlantArea.OnAnyPlantPlanted -= PlantArea_OnAnyPlantPlanted;
            WateringState.OnAnyWateringStateChanged -= WateringState_OnAnyWateringStateChanged;
            PlantArea.OnAnyPlantHarvested -= Plant_OnAnyPlantHarvested;
        }

        public bool TryGetPlantToWater(out Plant plant)
        {
            if (_plantToNeedsWateringMapping.Count == 0)
            {
                plant = null;
                return false;
            }

            plant = _plantToNeedsWateringMapping.First().Key;
            return true;
        }
        
        private void RegisterPlant(Plant plant)
        {
            if (!_plantToNeedsWateringMapping.TryAdd(plant, false))
            {
                Debug.LogWarning("There's already registered plant in dictionary.");
            }
        }

        private void DeregisterPlant(Plant plant)
        {
            _plantToNeedsWateringMapping.Remove(plant);
        }
        
        private void SetPlantWateringValue(Plant plant, bool needsWatering)
        {
            _plantToNeedsWateringMapping[plant] = needsWatering;
            OnPlantWateringStateChanged?.Invoke(plant, needsWatering);
        }
        
        private void PlantArea_OnAnyPlantPlanted(Plant plant)
        {
            RegisterPlant(plant);
        }
        
        private void WateringState_OnAnyWateringStateChanged(Plant plant, bool needsWatering)
        {
            SetPlantWateringValue(plant, needsWatering);
        }
        
        private void Plant_OnAnyPlantHarvested(Plant plant)
        {
            DeregisterPlant(plant);
        }
    }
}