namespace Farm
{
    [System.Serializable]
    public class Tool
    {
        private const int MaxLevel = 5;
        private const int MinLevel = 1;
        
        public readonly ToolType Type;
        public readonly int Level;

        public Tool(ToolType type, int level)
        {
            Type = type;

            Level = Level switch
            {
                < MinLevel => MinLevel,
                > MaxLevel => MaxLevel,
                _ => level
            };
        }
    }
}
