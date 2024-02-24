using Farm.Plants.Seeds;

namespace Farm.Plants
{
    [System.Serializable]
    public class PlantDetails
    {
        public PlantType RequiredPlantType;
        public SeedType RequiredSeedType;
        public int RequiredSeedQuantityToPlant;
    }
}