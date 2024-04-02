namespace Character.Player.Events
{
    public class PlayerEvents
    {
        public readonly WalkingEvent WalkingEvent = new();
        public readonly IdleEvent IdleEvent = new();
        public readonly GatheringEvent GatheringEvent = new();
        public readonly DiggingPlantAreaStateChangedEvent DiggingPlantAreaStateChangedEvent = new();
        public readonly CarryingStorageBoxStateChangedEvent CarryingStorageBoxStateChangedEvent = new();
        public readonly FoundCollectableEvent FoundCollectableEvent = new();
        public readonly ShoppingEvent ShoppingEvent = new();
        public readonly UsingWorkbenchEvent UsingWorkbenchEvent = new();
        public readonly LeftHomeEvent LeftHomeEvent = new();
        public readonly EnteringHomeEvent EnteringHomeEvent = new();
        public readonly ExtractingWaterEvent ExtractingWaterEvent = new();
        public readonly HarvestingStateChangedEvent HarvestingStateChangedEvent = new();
        public readonly WateringStateChangedEvent WateringStateChangedEvent = new();
        public readonly StartedSellingEvent StartedSellingEvent = new();
    }
}