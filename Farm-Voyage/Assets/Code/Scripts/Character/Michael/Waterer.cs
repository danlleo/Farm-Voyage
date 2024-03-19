using System.Collections.Generic;
using Farm.Corral;
using Farm.Plants;
using Farm.Plants.ConcreteStates;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class Waterar : MonoBehaviour
    {
        private Dictionary<Plant, bool> _plantToNeedsWateringMapping;
        
        private void OnEnable()
        {
            PlantArea.OnAnyPlantPlanted += PlantArea_OnAnyPlantPlanted;
            WateringState.OnAnyWateringStateChanged += WateringState_OnAnyWateringStateChanged;
            ReadyToHarvestState.OnAnyPlantHarvested += ReadyToHarvestState_OnAnyPlantHarvested;
        }

        private void OnDisable()
        {
            PlantArea.OnAnyPlantPlanted -= PlantArea_OnAnyPlantPlanted;
            WateringState.OnAnyWateringStateChanged -= WateringState_OnAnyWateringStateChanged;
            ReadyToHarvestState.OnAnyPlantHarvested -= ReadyToHarvestState_OnAnyPlantHarvested;
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
        }
        
        private void PlantArea_OnAnyPlantPlanted(Plant plant)
        {
            RegisterPlant(plant);
        }
        
        private void WateringState_OnAnyWateringStateChanged(Plant plant, bool needsWatering)
        {
            SetPlantWateringValue(plant, needsWatering);
        }
        
        private void ReadyToHarvestState_OnAnyPlantHarvested(Plant plant)
        {
            DeregisterPlant(plant);
        }
    }
}