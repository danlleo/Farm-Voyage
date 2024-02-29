namespace Farm.Tool
{
    public class Scythe : Tool
    {
        public sealed override string Name { get; protected set; }
        
        public Scythe(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "Scythe";
        }
    }
}