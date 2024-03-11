using System;
using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;

namespace Farm.Tool.ConcreteTools
{
    public class WaterCan : Tool
    {
        public const int WaterCanCapacityAmount = 3;
        public int CurrentWaterCapacityAmount { get; private set; }
        public sealed override string Name { get; protected set; }
        public sealed override IEnumerable<ResourcePrice> ResourcesPrices { get; protected set; }

        public event Action<int, int> OnWaterAmountChanged;
        
        public WaterCan(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "Water Can";

            ResourcesPrices = new List<ResourcePrice>
            {
                new(ResourceType.Dirt, 3),
                new(ResourceType.Rock, 3),
                new(ResourceType.Wood, 3)
            }.AsEnumerable();
        }

        public void EmptyCan()
        {
            if (CurrentWaterCapacityAmount <= 0)
            {
                CurrentWaterCapacityAmount = 0;
                return;
            }
            
            CurrentWaterCapacityAmount--;
            OnWaterAmountChanged?.Invoke(CurrentWaterCapacityAmount, WaterCanCapacityAmount);
        }

        public void FillWaterCan()
        {
            if (CurrentWaterCapacityAmount >= WaterCanCapacityAmount)
            {
                CurrentWaterCapacityAmount = WaterCanCapacityAmount;
                return;
            }
            
            CurrentWaterCapacityAmount++;
            OnWaterAmountChanged?.Invoke(CurrentWaterCapacityAmount, WaterCanCapacityAmount);
        }
    }
}