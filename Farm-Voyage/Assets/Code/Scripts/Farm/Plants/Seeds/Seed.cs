namespace Farm.Plants.Seeds
{
    public abstract class Seed
    {
        private const int MaxSeedsCanCarry = 50;
        
        public int Quantity { get; private set; }
        public readonly SeedType SeedType;
        public readonly PlantType Plant;
        
        protected Seed(SeedType seedType, PlantType plant, int quantity)
        {
            SeedType = seedType;
            Plant = plant;
            Quantity = quantity;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;

            if (Quantity > MaxSeedsCanCarry)
                Quantity = MaxSeedsCanCarry;
        }
        
        public void RemoveQuantity(int amount)
        {
            Quantity -= amount;
        }
    }
}