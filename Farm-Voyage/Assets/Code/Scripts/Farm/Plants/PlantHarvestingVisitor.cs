using Character.Michael;
using Character.Player;
using UnityEngine;
using Utilities;

namespace Farm.Plants
{
    public class PlantHarvestingVisitor : IPlantVisitor
    {
        private const float TimeToHarvestInSeconds = 1.5f;
        
        private readonly Plant _plant;

        private float _harvestingTimeElapsed;

        private bool _hasFinishedHarvesting;
        
        public PlantHarvestingVisitor(Plant plant)
        {
            _plant = plant;
        }

        public void Visit(Player player)
        {
            // TODO: make it unique for player later
            Harvest();
        }

        public void Visit(Michael michael)
        {
            Harvest();
        }
        
        private void Harvest()
        {
            if (_hasFinishedHarvesting) return;
            
            if (_harvestingTimeElapsed > TimeToHarvestInSeconds)
            {
                _plant.GetComponent<BoxCollider>().Disable();
                _plant.PlantArea.ClearPlantArea();
                _hasFinishedHarvesting = true;
                return;
            }
            
            _harvestingTimeElapsed += TimeToHarvestInSeconds;
        }
    }
}