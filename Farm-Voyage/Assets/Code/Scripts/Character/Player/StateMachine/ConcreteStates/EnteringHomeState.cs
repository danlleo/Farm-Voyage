using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class EnteringHomeState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public EnteringHomeState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            _player.PlayerLocomotion.HandleMoveDestination(_player.HomeStayPoint.position, Quaternion.identity);
        }
    }
}