namespace Farm.Plants.ConcretePlants
{
    public class TomatoPlant : Plant
    {
        public override PlantType Type { get; } = PlantType.Tomato;
    
        public override void OnHarvested()
        {
            
        }
    }
}