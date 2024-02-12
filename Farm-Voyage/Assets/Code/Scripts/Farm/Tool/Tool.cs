namespace Farm.Tool
{
    [System.Serializable]
    public abstract class Tool
    {
        public readonly float TimeToGather;
        public readonly int Level;

        public Tool(float timeToGather, int level)
        {
            TimeToGather = timeToGather;
            Level = level;
        }
        
        private void IncreaseXp()
        {
            // TODO: Increase tool's XP            
        }
    }
}
