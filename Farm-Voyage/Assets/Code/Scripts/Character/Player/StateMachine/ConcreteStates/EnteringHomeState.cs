using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class EnteringHomeState : State
    {
        private Player _player;
        private StateMachine _stateMachine;
        
        public EnteringHomeState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void Tick()
        {
            _player.PlayerLocomotion.HandleMoveDestination(_player.HomeStayPoint.position, Quaternion.identity);
        }
    }
}