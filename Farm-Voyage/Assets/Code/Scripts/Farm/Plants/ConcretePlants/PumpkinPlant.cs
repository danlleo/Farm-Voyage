namespace Farm.Plants.ConcretePlants
{
    public class PumpkinPlant : Plant
    {
        public override PlantType Type { get; protected set; } = PlantType.Pumpkin;
        
        public override void OnHarvested()
        {
            
        }
    }
}