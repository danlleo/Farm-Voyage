namespace Farm.Plants.ConcretePlants
{
    public class CarrotPlant : Plant
    {
        public override PlantType Type { get; protected set; } = PlantType.Carrot;

        public override void OnHarvested()
        {
            
        }
    }
}