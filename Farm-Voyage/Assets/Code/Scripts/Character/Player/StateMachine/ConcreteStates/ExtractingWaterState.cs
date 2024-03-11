﻿using UnityEngine;

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

        public override void SubscribeToEvents()
        {
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater += Player_OnPlayerExtractingWater;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater -= Player_OnPlayerExtractingWater;
        }

        private void Player_OnPlayerExtractingWater(object sender, PlayerExtractingWaterEventArgs e)
        {
            if (e.IsExtracting) return;
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}