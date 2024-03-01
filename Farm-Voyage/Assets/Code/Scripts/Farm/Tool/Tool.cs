using System;
using System.Collections.Generic;
using Farm.FarmResources;
using Random = UnityEngine.Random;

namespace Farm.Tool
{
    public abstract class Tool
    {
        public const int MaxLevel = 5;
        
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
        
        public int CalculateQuantityBasedOnLevel()
        {
            const int minimumRandomValue = 1;
            const int maximumRandomValue = 5;

            int calculatedQuantity = Level * Random.Range(minimumRandomValue, maximumRandomValue);
            
            return calculatedQuantity;
        }

        public float CalculateTimeToGatherBasedOnLevel()
        {
            float calculatedTimeReducer = Level == 1 ? 0 : 0.5f * Level;
            float calculatedTime = _timeToGather - calculatedTimeReducer;
            
            return calculatedTime;
        }
    }
}
