using Farm.Plants.Seeds;

namespace Farm.Plants
{
    [System.Serializable]
    public struct PlantDetails
    {
        public PlantType RequiredPlantType;
        public SeedType RequiredSeedType;
        public int RequiredSeedQuantityToPlant;
    }
}