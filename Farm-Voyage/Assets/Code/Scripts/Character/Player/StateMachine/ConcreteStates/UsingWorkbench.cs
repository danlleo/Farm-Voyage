using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class UsingWorkbenchState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public UsingWorkbenchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.Events.UsingWorkbenchStateChangedEvent.OnPlayerUsingWorkbenchStateChanged +=
                Player_OnUsingWorkbenchStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.UsingWorkbenchStateChangedEvent.OnPlayerUsingWorkbenchStateChanged -=
                Player_OnUsingWorkbenchStateChanged;
        }

        public override void OnEnter()
        {
            _player.Locomotion.HandleMoveDestination(_player.TransformPoints.WorkbenchStayPoint.position,
                Quaternion.identity);
        }
        
        private void Player_OnUsingWorkbenchStateChanged(bool isUsingWorkbench)
        {
            if (isUsingWorkbench) return;

            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}