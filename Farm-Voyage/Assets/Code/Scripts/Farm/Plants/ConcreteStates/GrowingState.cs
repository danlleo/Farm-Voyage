using DG.Tweening;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class GrowingState : State
    {
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;

        private readonly Transform _plantVisual;
        
        public GrowingState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
            _plantVisual = Plant.PlantVisual;
        }

        public override void OnEnter()
        {
            Grow();
        }
        
        private void Grow()
        {
            float endPartitionScale = GetGrowPartitionEndValue();
            
            _plantVisual.DOScale(endPartitionScale, _plant.PlantPartitionGrowTimeInSecond)
                .OnStart(() =>
                {
                    float progress = endPartitionScale / _plant.GrownScale;
                    _plant.PlantArea.ProgressIcon.SetProgress(_plant.PlantArea, progress,
                        _plant.PlantPartitionGrowTimeInSecond);
                })
                .OnComplete(() =>
                {
                    if (_plantVisual.localScale.z >= _plant.GrownScale)
                    {
                        _stateMachine.ChangeState(_plant.StateFactory.ReadyToHarvest());
                        return;
                    }

                    _plant.PlantArea.ProgressIcon.StopProgress(_plant.PlantArea);
                    _stateMachine.ChangeState(_plant.StateFactory.Watering());
                });
        }

        private float GetGrowPartitionEndValue()
        {
            return _plantVisual.localScale.z + (_plant.GrownScale - _plant.InitialScale) / (_plant.Partitions + 1);
        }
    }
}