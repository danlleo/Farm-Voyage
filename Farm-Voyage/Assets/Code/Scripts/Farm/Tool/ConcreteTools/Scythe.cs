using System.Collections.Generic;
using System.Linq;
using Farm.FarmResources;

namespace Farm.Tool.ConcreteTools
{
    public class Scythe : Tool
    {
        public sealed override string Name { get; protected set; }
        public sealed override IEnumerable<ResourcePrice> ResourcesPrices { get; protected set; }

        public Scythe(float timeToGather, int level) : base(timeToGather, level)
        {
            Name = "Scythe";
            
            ResourcesPrices = new List<ResourcePrice>
            {
                new(ResourceType.Dirt, 3),
                new(ResourceType.Rock, 3),
                new(ResourceType.Wood, 3)
            }.AsEnumerable();
        }
    }
}