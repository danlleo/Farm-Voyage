using System.Collections;
using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class FoundCollectableState : State
    {
        private readonly float _timeToStayInCollectableState = 2f;
        
        private Player _player;
        private StateMachine _stateMachine;
        
        public FoundCollectableState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            _player.StartCoroutine(InspectCollectableItemRoutine());
        }

        private IEnumerator InspectCollectableItemRoutine()
        {
            yield return new WaitForSeconds(_timeToStayInCollectableState);
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}