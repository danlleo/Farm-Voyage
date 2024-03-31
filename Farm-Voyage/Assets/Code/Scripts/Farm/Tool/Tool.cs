using System;
using System.Collections.Generic;
using Farm.FarmResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Farm.Tool
{
    public abstract class Tool
    {
        public const int MaxLevel = 5;
        private const float TimeReductionPerLevel = 0.5f;
        public event Action<int> OnLevelUp;
        
        public abstract string Name { get; protected set; }
        public abstract IEnumerable<ResourcePrice> ResourcesPrices { get; protected set; }
        
        public int Level { get; protected set; }
        
        private readonly float _timeToGather;

        protected Tool(float timeToGather, int level)
        {
            _timeToGather = timeToGather;
            Level = level;
        }

        public void IncreaseLevel()
        {
            if (Level == MaxLevel) return;
            
            Level++;
            OnLevelUp?.Invoke(Level);
        }
        
        public int GetPriceByRecourseType(ResourceType resourceType)
        {
            foreach (ResourcePrice shopItemResourcePrice in ResourcesPrices)
            {
                if (shopItemResourcePrice.ResourceType == resourceType)
                {
                    return shopItemResourcePrice.Price;
                }
            }

            return 0;
        }
        
        public int CalculateRandomQuantityBasedOnLevel()
        {
            const int minimumRandomValue = 1;
            const int maximumRandomValuePlusOne = 5;

            int calculatedQuantity = Level * Random.Range(minimumRandomValue, maximumRandomValuePlusOne);
            
            return calculatedQuantity;
        }

        public float CalculateReducedGatherTime()
        {
            if (Level <= 1) return _timeToGather;
    
            float calculatedTimeReducer = (Level - 1) * TimeReductionPerLevel;
            float calculatedTime = _timeToGather - calculatedTimeReducer;
    
            return Mathf.Max(calculatedTime, 0);
        }
    }
}
