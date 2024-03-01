using System;
using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;

namespace Farm.Tool.ConcreteTools
{
    public class WaterCan : Tool
    {
        public sealed override string Name { get; protected set; }
        public sealed override IEnumerable<ResourcePrice> ResourcesPrices { get; protected set; }

        public event Action<int, int> OnWaterAmountChanged;
        
        private const int MaxTimesCanWater = 3;
        private int _timesCanWater = MaxTimesCanWater;
        
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
            if (_timesCanWater <= 0)
            {
                _timesCanWater = 0;
                return;
            }
            
            _timesCanWater--;
            OnWaterAmountChanged?.Invoke(_timesCanWater, MaxTimesCanWater);
        }

        public void FillWaterCan()
        {
            if (_timesCanWater >= MaxTimesCanWater)
            {
                _timesCanWater = MaxTimesCanWater;
                return;
            }
            
            _timesCanWater++;
            OnWaterAmountChanged?.Invoke(_timesCanWater, MaxTimesCanWater);
        }

        public bool IsFullyFilled()
        {
            return _timesCanWater == MaxTimesCanWater;
        }
        
        public bool HasWaterLeft()
        {
            return _timesCanWater > 0;
        }
    }
}