namespace Character.Michael.Events
{
    public class MichaelEvents
    {
        public readonly MichaelLocomotionStateChangedEvent MichaelLocomotionStateChangedEvent = new();
        public readonly MichaelWateringPlantEvent MichaelWateringPlantEvent = new();
        public readonly MichaelHarvestingPlantEvent MichaelHarvestingPlantEvent = new();
        public readonly MichaelPerformingGardeningActionEvent MichaelPerformingGardeningActionEvent = new();
        public readonly MichaelSittingStateChangedEvent MichaelSittingStateChangedEvent = new();
    }
}