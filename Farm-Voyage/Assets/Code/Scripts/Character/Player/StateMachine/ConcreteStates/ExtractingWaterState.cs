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
            _player.PlayerLocomotion.HandleMoveDestination(_player.WellStayPoint.position, Quaternion.identity);
        }
    }
}