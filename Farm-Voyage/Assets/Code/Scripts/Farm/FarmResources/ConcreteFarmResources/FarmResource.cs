namespace Farm.FarmResources.ConcreteFarmResources
{
    public abstract class FarmResource
    {
        public int Quantity { get; private set; }
        public readonly ResourceType Type;

        protected FarmResource(int quantity, ResourceType type)
        {
            Quantity = quantity;
            Type = type;
        }
        
        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        public void RemoveQuantity(int amount)
        {
            Quantity -= amount;
        }
    }
}