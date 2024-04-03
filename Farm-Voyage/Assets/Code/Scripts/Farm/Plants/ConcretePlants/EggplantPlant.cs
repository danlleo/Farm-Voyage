namespace Farm.Plants.ConcretePlants
{
    public class EggplantPlant : Plant
    {
        public override PlantType Type { get; protected set; } = PlantType.Eggplant;
        
        public override void OnHarvested()
        {
            
        }
    }
}