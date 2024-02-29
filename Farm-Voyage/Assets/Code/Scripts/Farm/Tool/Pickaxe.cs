namespace Farm.Tool
{
    public class Pickaxe : Tool
    {
        public sealed override string Name { get; protected set; }

        public Pickaxe(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "Pickaxe";
        }
    }
}