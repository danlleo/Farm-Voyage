namespace Farm.Plants.Seeds
{
    public abstract class Seed
    {
        private const int MaxSeedsCanCarry = 50;
        
        public int Quantity { get; private set; }
        public readonly SeedType SeedType;

        protected Seed(SeedType seedType, int quantity)
        {
            SeedType = seedType;
            Quantity = quantity;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;

            if (Quantity > MaxSeedsCanCarry)
                Quantity = MaxSeedsCanCarry;
        }
        
        public bool TryRemoveQuantity(int amount)
        {
            if (Quantity < amount)
            {
                return false;
            }

            Quantity -= amount;
            return true;
        }
    }
}