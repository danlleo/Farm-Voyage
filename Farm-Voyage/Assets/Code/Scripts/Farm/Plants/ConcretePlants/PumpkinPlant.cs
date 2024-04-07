namespace Farm.Plants.ConcretePlants
{
    public class PumpkinPlant : Plant
    {
        public override PlantType Type { get; } = PlantType.Pumpkin;
        
        public override void OnHarvested()
        {
            
        }
    }
}