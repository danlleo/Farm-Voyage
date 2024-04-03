namespace Farm.Plants.ConcretePlants
{
    public class CornPlant : Plant
    {
        public override PlantType Type { get; protected set; } = PlantType.Corn;
        
        public override void OnHarvested()
        {
            
        }
    }
}