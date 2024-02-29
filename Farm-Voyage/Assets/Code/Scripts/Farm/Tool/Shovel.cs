namespace Farm.Tool
{
    public class Shovel : Tool
    {
        public sealed override string Name { get; protected set; }
        
        public Shovel(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "Shovel";
        }
    }
}