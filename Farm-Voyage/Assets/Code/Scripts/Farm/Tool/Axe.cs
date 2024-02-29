namespace Farm.Tool
{
    public class Axe : Tool
    {
        public sealed override string Name { get; protected set; }
        
        public Axe(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "axe";
        }
    }
}