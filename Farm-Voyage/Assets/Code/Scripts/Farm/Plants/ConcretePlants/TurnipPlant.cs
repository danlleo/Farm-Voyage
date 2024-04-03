namespace Farm.Plants.ConcretePlants
{
    public class TurnipPlant : Plant
    {
        public override PlantType Type { get; protected set; } = PlantType.Turnip;
        
        public override void OnHarvested()
        {
            
        }
    }
}