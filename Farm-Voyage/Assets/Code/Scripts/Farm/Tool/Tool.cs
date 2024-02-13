using System;

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
        
        public static Type GetToolType(ToolType toolType)
        {
            return toolType switch
            {
                ToolType.Shovel => typeof(Shovel),
                ToolType.Pickaxe => typeof(Pickaxe),
                ToolType.Axe => typeof(Axe),
                ToolType.Scythe => typeof(Scythe),
                ToolType.WaterCan => typeof(WaterCan),
                _ => throw new ArgumentOutOfRangeException(nameof(toolType), $"Unsupported tool type: {toolType}")
            };
        }
        
        private void IncreaseXp()
        {
            // TODO: Increase tool's XP            
        }
    }
}
