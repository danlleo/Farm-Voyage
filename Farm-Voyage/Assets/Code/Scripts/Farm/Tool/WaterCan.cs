using System;

namespace Farm.Tool
{
    public class WaterCan : Tool
    {
        public event Action<int, int> OnWaterAmountChanged;
        
        private const int MaxTimesCanWater = 3;
        private int _timesCanWater = MaxTimesCanWater;
        
        public WaterCan(float timeToGather, int level) : base(timeToGather, level)
        {
            
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