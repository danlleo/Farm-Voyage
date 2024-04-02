using Character.Michael;
using Character.Player;
using Common;
using UnityEngine;
using Utilities;

namespace Farm.Plants
{
    public class Harvester : IVisitor
    {
        private const float TimeToHarvestInSeconds = 1.5f;
        
        private readonly Plant _plant;

        private float _harvestingTimeElapsed;

        private bool _hasFinishedHarvesting;
        
        public Harvester(Plant plant)
        {
            _plant = plant;
        }

        public void Visit(Player player)
        {
            HarvestAsPlayer(player);
        }

        public void Visit(Michael michael)
        {
            HarvestAsAny();
        }

        private void HarvestAsPlayer(Player player)
        {
            player.Events.HarvestingStateChangedEvent.Call(true);
            HarvestAsAny();
        }
        
        private void HarvestAsAny()
        {
            if (_hasFinishedHarvesting) return;
            
            if (_harvestingTimeElapsed > TimeToHarvestInSeconds)
            {
                _plant.GetComponent<BoxCollider>().Disable();
                _plant.OnHarvested();
                _plant.PlantArea.ClearPlantArea();
                _hasFinishedHarvesting = true;
                return;
            }
            
            _harvestingTimeElapsed += TimeToHarvestInSeconds;
        }
    }
}