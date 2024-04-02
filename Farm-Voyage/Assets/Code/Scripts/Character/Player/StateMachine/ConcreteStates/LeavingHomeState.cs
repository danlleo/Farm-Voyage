using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class LeavingHomeState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public LeavingHomeState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            _player.Locomotion.HandleMoveDestination(_player.TransformPoints.HomeLeavePoint.position,
                Quaternion.identity, () =>
                {
                    _stateMachine.ChangeState(_player.StateFactory.Exploring());
                    _player.Events.LeftHomeEvent.Call(this);
                });
        }
    }
}