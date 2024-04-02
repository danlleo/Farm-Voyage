namespace Character.Michael.Events
{
    public class MichaelEvents
    {
        public readonly LocomotionStateChangedEvent LocomotionStateChangedEvent = new();
        public readonly WateringPlantEvent WateringPlantEvent = new();
        public readonly HarvestingPlantEvent HarvestingPlantEvent = new();
        public readonly PerformingGardeningActionEvent PerformingGardeningActionEvent = new();
        public readonly SittingStateChangedEvent SittingStateChangedEvent = new();
    }
}