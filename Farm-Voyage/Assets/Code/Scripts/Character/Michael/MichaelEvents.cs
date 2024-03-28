using Character.Michael.Locomotion;

namespace Character.Michael
{
    public class MichaelEvents
    {
        public MichaelLocomotionStateChangedEvent MichaelLocomotionStateChangedEvent { get; private set; }
        public MichaelWateringPlantEvent MichaelWateringPlantEvent { get; private set; }
        public MichaelHarvestingPlantEvent MichaelHarvestingPlantEvent { get; private set; }
        public MichaelPerformingGardeningActionEvent MichaelPerformingGardeningActionEvent { get; private set; }
        public MichaelSittingStateChangedEvent MichaelSittingStateChangedEvent { get; private set; }
        
        public MichaelEvents(
            MichaelLocomotionStateChangedEvent michaelLocomotionStateChangedEvent,
            MichaelWateringPlantEvent michaelWateringPlantEvent,
            MichaelHarvestingPlantEvent michaelHarvestingPlantEvent,
            MichaelPerformingGardeningActionEvent michaelPerformingGardeningActionEvent,
            MichaelSittingStateChangedEvent michaelSittingStateChangedEvent)
        {
            MichaelLocomotionStateChangedEvent = michaelLocomotionStateChangedEvent;
            MichaelWateringPlantEvent = michaelWateringPlantEvent;
            MichaelHarvestingPlantEvent = michaelHarvestingPlantEvent;
            MichaelPerformingGardeningActionEvent = michaelPerformingGardeningActionEvent;
            MichaelSittingStateChangedEvent = michaelSittingStateChangedEvent;
        }
    }
}