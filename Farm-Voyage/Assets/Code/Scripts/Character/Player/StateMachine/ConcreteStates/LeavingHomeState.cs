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

        public override void Tick()
        {
            if (_player.PlayerLocomotion.HandleMoveDestination(_player.HomeLeavePoint.position, Quaternion.identity))
            {
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
            }
        }
    }
}