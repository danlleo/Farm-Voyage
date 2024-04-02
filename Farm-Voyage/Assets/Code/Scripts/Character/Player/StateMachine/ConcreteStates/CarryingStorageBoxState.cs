using Character.Player.Events;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class CarryingStorageBoxState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public CarryingStorageBoxState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                OnCarryingStorageBoxStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                OnCarryingStorageBoxStateChanged;
        }

        public override void Tick()
        {
            _player.Locomotion.HandleGroundedMovement();
            _player.Locomotion.HandleRotation();
        }

        private void OnCarryingStorageBoxStateChanged(object sender, PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying)
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}