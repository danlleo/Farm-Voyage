using Character.Player.Locomotion;

namespace Character.Player.Events
{
    public class PlayerEvents
    {
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerGatheringEvent PlayerGatheringEvent { get; private set; }
        public PlayerDiggingPlantAreaEvent PlayerDiggingPlantAreaEvent { get; private set; }
        public PlayerCarryingStorageBoxStateChangedEvent PlayerCarryingStorageBoxStateChangedEvent { get; private set; }
        public PlayerFoundCollectableEvent PlayerFoundCollectableEvent { get; private set; }
        public PlayerShoppingEvent PlayerShoppingEvent { get; private set; }
        public PlayerUsingWorkbenchEvent PlayerUsingWorkbenchEvent { get; private set; }
        public PlayerLeftHomeEvent PlayerLeftHomeEvent { get; private set; }
        public PlayerEnteringHomeEvent PlayerEnteringHomeEvent { get; private set; }
        public PlayerExtractingWaterEvent PlayerExtractingWaterEvent { get; private set; }

        public PlayerEvents(
            PlayerWalkingEvent playerWalkingEvent,
            PlayerIdleEvent playerIdleEvent,
            PlayerGatheringEvent playerGatheringEvent,
            PlayerDiggingPlantAreaEvent playerDiggingPlantAreaEvent,
            PlayerCarryingStorageBoxStateChangedEvent playerCarryingStorageBoxStateChangedEvent,
            PlayerFoundCollectableEvent playerFoundCollectableEvent,
            PlayerShoppingEvent playerShoppingEvent,
            PlayerUsingWorkbenchEvent playerUsingWorkbenchEvent,
            PlayerLeftHomeEvent playerLeftHomeEvent,
            PlayerEnteringHomeEvent playerEnteringHomeEvent,
            PlayerExtractingWaterEvent playerExtractingWaterEvent)
        {
            PlayerWalkingEvent = playerWalkingEvent;
            PlayerIdleEvent = playerIdleEvent;
            PlayerGatheringEvent = playerGatheringEvent;
            PlayerDiggingPlantAreaEvent = playerDiggingPlantAreaEvent;
            PlayerCarryingStorageBoxStateChangedEvent = playerCarryingStorageBoxStateChangedEvent;
            PlayerFoundCollectableEvent = playerFoundCollectableEvent;
            PlayerShoppingEvent = playerShoppingEvent;
            PlayerUsingWorkbenchEvent = playerUsingWorkbenchEvent;
            PlayerLeftHomeEvent = playerLeftHomeEvent;
            PlayerEnteringHomeEvent = playerEnteringHomeEvent;
            PlayerExtractingWaterEvent = playerExtractingWaterEvent;
        }
    }
}