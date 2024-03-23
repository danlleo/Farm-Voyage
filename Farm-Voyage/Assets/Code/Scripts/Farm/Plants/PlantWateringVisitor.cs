using Character.Michael;
using Character.Player;
using Farm.Tool.ConcreteTools;
using UnityEngine;

namespace Farm.Plants
{
    public class PlantWateringVisitor : IPlantVisitor
    {
        private const float TimeToWaterInSeconds = 3f;
        
        private readonly Plant _plant;
        private readonly WaterCan _waterCan;

        private float _wateringTimeElapsed;
        
        public PlantWateringVisitor(Plant plant, PlayerInventory playerInventory)
        {
            _plant = plant;
            playerInventory.TryGetTool(out _waterCan);
        }

        public void Visit(Player player)
        {
            if (_waterCan == null) return;
            if (!(_waterCan.CurrentWaterCapacityAmount > 0)) return;
            WaterPlant();
        }

        public void Visit(Michael michael)
        {
            WaterPlant();
        }

        private void WaterPlant()
        {
            if (_wateringTimeElapsed >= TimeToWaterInSeconds)
            {
                _plant.PlantFinishedWateringEvent.Call();
                return;
            }
            
            _wateringTimeElapsed += Time.deltaTime;
        }
    }
}