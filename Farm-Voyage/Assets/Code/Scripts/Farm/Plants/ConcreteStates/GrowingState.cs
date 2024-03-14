using DG.Tweening;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class GrowingState : State
    {
        private const float MaxWateringThreshold = 1f;
        
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        private readonly Vector3 _initialScale;
        private readonly Vector3 _targetScale;
        
        private readonly float _plantPartitionGrowTimeInSeconds;

        private readonly Transform _plantVisual;
        
        public GrowingState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
            _targetScale = plant.TargetScale;
            _plantPartitionGrowTimeInSeconds = plant.PlantPartitionGrowTimeInSecond;
            _plantVisual = Plant.PlantVisual;
        }

        public override void OnEnter()
        {
            Grow();
        }
        
        private void Grow()
        {
            float nearestWateringThreshold = GetNearestWateringThreshold();
            
            if (HasReachedMaximumWateringThreshold(nearestWateringThreshold))
            {
                _stateMachine.ChangeState(_plant.StateFactory.ReadyToHarvest());
                return;
            }

            float targetPartitionScale =
                FindTargetPartitionScale(nearestWateringThreshold, _initialScale.z, _targetScale.z);

            _plantVisual.transform.DOScale(targetPartitionScale, _plantPartitionGrowTimeInSeconds).OnComplete(() =>
            {
                _stateMachine.ChangeState(_plant.StateFactory.NeedsWatering());
                _plant.PlantArea.ProgressIcon.SetProgress(_plant.PlantArea, 0.5f);
            });
        }

        private float FindTargetPartitionScale(float nearestWateringThreshold, float startScale, float endScale)
        {
            float targetPartitionScale = startScale + (endScale - startScale) * nearestWateringThreshold;
            return targetPartitionScale;
        }
        
        private float GetNearestWateringThreshold()
        {
            if (_plant.WateringThresholds.Count == 0)
            {
                return MaxWateringThreshold;
            }

            float nearestWateringThreshold = _plant.WateringThresholds[0];
            _plant.WateringThresholds.RemoveAt(0);
            
            return nearestWateringThreshold;
        }

        private bool HasReachedMaximumWateringThreshold(float nearestWateringThreshold)
        {
            return nearestWateringThreshold == MaxWateringThreshold;
        }
    }
}