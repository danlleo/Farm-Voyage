namespace Farm.Tool
{
    public abstract class Tool
    {
        public abstract string Name { get; protected set; }
        
        public readonly float TimeToGather;
        public readonly int Level;

        protected Tool(float timeToGather, int level)
        {
            TimeToGather = timeToGather;
            Level = level;
        }
        
        private void IncreaseXp()
        {
            // TODO: Increase tool's XP            
        }

        private void CalculateTimeToGather()
        {
            // TODO: calculate time to gather
        }
    }
}
