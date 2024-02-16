using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class GrowingState : State
    {
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        private readonly Vector3 _initialScale;
        private readonly Vector3 _targetScale;
        
        private readonly float _plantPartitionGrowTimeInSeconds;
        private readonly float[] _wateringThresholds;

        private readonly Transform _plantVisual;
        
        public GrowingState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
            _targetScale = plant.TargetScale;
            _plantPartitionGrowTimeInSeconds = plant.PlantPartitionGrowTimeInSecond;
            _wateringThresholds = plant.WateringThresholds;
            _plantVisual = Plant.PlantVisual;
        }

        public override void OnEnter()
        {
            Grow();
        }

        private void Grow()
        {
            if (_plant.CurrentScale == _targetScale)
            {
                _stateMachine.ChangeState(_plant.StateFactory.ReadyToHarvest());
                return;
            }

            float currentScalePercentage = CalculateCurrentScalePercentage(_plant.CurrentScale.z, _targetScale.z);
            float nearestWateringThreshold = FindNearestWateringThreshold(currentScalePercentage, _wateringThresholds);

            float targetPartitionScale =
                FindTargetPartitionScale(nearestWateringThreshold, _initialScale.z, _targetScale.z);

            Debug.Log(targetPartitionScale);
            
            _plantVisual.transform.DOScale(targetPartitionScale, _plantPartitionGrowTimeInSeconds).OnComplete(() =>
            {
                _stateMachine.ChangeState(_plant.StateFactory.NeedsWatering());
            });
        }

        private float FindTargetPartitionScale(float nearestWateringThreshold, float startScale, float endScale)
        {
            float targetPartitionScale = startScale + (endScale - startScale) * nearestWateringThreshold;
            return targetPartitionScale;
        }
        
        private float CalculateCurrentScalePercentage(float currentScale, float targetScale)
        {
            float currentScalePercentage = Mathf.InverseLerp(currentScale, targetScale, 100);
            return currentScalePercentage;
        }

        private float FindNearestWateringThreshold(float currentScalePercentage, IReadOnlyList<float> thresholds)
        {
            float currentPercentageNormalized = currentScalePercentage / 100f;

            float nearestThreshold = thresholds[0];
            float smallestDifference = Mathf.Abs(currentPercentageNormalized - nearestThreshold);

            foreach (float threshold in thresholds)
            {
                float currentDifference = Mathf.Abs(currentPercentageNormalized - threshold);

                if (!(currentDifference < smallestDifference)) continue;
                
                smallestDifference = currentDifference;
                nearestThreshold = threshold;
            }

            return nearestThreshold;
        }
    }
}