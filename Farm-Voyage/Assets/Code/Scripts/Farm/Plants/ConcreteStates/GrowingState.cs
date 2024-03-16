﻿using DG.Tweening;
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
            if (_plantVisual.localScale.z >= _plant.GrownScale)
            {
                _stateMachine.ChangeState(_plant.StateFactory.ReadyToHarvest());
                return;
            }

            _plantVisual.DOScale(GetGrowPartitionEndValue(), _plant.PlantPartitionGrowTimeInSecond)
                .OnUpdate(() =>
                {
                    _plant.PlantArea.ProgressIcon.SetProgress(_plant.PlantArea,
                        _plantVisual.localScale.z / _plant.GrownScale);
                })
                .OnComplete(() =>
                {
                    _stateMachine.ChangeState(_plant.StateFactory.NeedsWatering());
                });
        }

        private float GetGrowPartitionEndValue()
        {
            return _plantVisual.localScale.z + (_plant.GrownScale - _plant.InitialScale) / (_plant.Partitions + 1);
        }
    }
}