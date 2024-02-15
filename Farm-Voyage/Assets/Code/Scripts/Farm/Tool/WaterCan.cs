namespace Farm.Tool
{
    public class WaterCan : Tool
    {
        private const int MaxTimesCanWater = 3;
        private int _timesToWater = MaxTimesCanWater;
        
        public WaterCan(float timeToGather, int level) : base(timeToGather, level)
        {
            
        }

        public void EmptyCan()
        {
            if (_timesToWater <= 0)
            {
                _timesToWater = 0;
                return;
            }
            
            _timesToWater--;
        }

        public void FillWaterCan()
        {
            if (_timesToWater >= MaxTimesCanWater)
            {
                _timesToWater = MaxTimesCanWater;
                return;
            }
            
            _timesToWater++;
        }

        public bool IsFullyFilled()
        {
            return _timesToWater == MaxTimesCanWater;
        }
        
        public bool HasWaterLeft()
        {
            return _timesToWater > 0;
        }
    }
}