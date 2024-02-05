namespace Farm
{
    [System.Serializable]
    public class Tool
    {
        public readonly ToolType Type;
        public readonly float TimeToGather;
        public readonly int Level;

        public Tool(ToolType type, float timeToGather, int level)
        {
            Type = type;
            TimeToGather = timeToGather;
            Level = level;
        }
    }
}
