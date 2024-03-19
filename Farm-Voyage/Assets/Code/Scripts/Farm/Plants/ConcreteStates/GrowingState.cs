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
            float endScale = GetGrowPartitionEndValue();
            
            _plantVisual.DOScale(endScale, _plant.PlantPartitionGrowTimeInSecond)
                .OnUpdate(() =>
                {
                    float progress = (_plantVisual.localScale.z - _plant.InitialScale) /
                                     (_plant.GrownScale - _plant.InitialScale);
                    _plant.PlantArea.ProgressIcon.SetProgress(_plant.PlantArea, progress);
                })
                .OnComplete(() =>
                {
                    if (_plantVisual.localScale.z >= _plant.GrownScale)
                    {
                        _stateMachine.ChangeState(_plant.StateFactory.ReadyToHarvest());
                        return;
                    }
                    
                    _stateMachine.ChangeState(_plant.StateFactory.Watering());
                });
        }

        private float GetGrowPartitionEndValue()
        {
            return _plantVisual.localScale.z + (_plant.GrownScale - _plant.InitialScale) / (_plant.Partitions + 1);
        }
    }
}