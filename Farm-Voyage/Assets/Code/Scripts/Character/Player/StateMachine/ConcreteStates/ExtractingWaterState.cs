using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class ExtractingWaterState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public ExtractingWaterState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            _player.Locomotion.HandleMoveDestination(_player.TransformPoints.WellStayPoint.position,
                Quaternion.identity);
        }

        public override void OnExit()
        {
            _player.Locomotion.StopAllMovement();
        }

        public override void SubscribeToEvents()
        {
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged +=
                Player_OnExtractingWaterStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged -=
                Player_OnExtractingWaterStateChanged;
        }

        private void Player_OnExtractingWaterStateChanged(bool isExtractingWater)
        {
            if (isExtractingWater) return;
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}