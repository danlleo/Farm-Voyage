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

        private bool _hasFinishedWatering;
        
        public PlantWateringVisitor(Plant plant, PlayerInventory playerInventory)
        {
            _plant = plant;
            playerInventory.TryGetTool(out _waterCan);
        }

        public void Visit(Player player)
        {
            WaterPlantAsPlayer();
        }

        public void Visit(Michael michael)
        {
            WaterPlantAsAny();
        }

        private void WaterPlantAsPlayer()
        {
            if (_waterCan == null) return;
            if (!(_waterCan.CurrentWaterCapacityAmount > 0)) return;
            WaterPlantAsAny();
        }
        
        private void WaterPlantAsAny()
        {
            if (_hasFinishedWatering) return;
            
            if (_wateringTimeElapsed >= TimeToWaterInSeconds)
            {
                _plant.PlantFinishedWateringEvent.Call();
                _hasFinishedWatering = true;
                return;
            }
            
            _wateringTimeElapsed += Time.deltaTime;
        }
    }
}