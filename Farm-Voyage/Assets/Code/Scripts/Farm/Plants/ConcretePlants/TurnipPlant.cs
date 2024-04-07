namespace Farm.Plants.ConcretePlants
{
    public class TurnipPlant : Plant
    {
        public override PlantType Type { get; } = PlantType.Turnip;
        
        public override void OnHarvested()
        {
            
        }
    }
}