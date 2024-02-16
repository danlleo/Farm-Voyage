﻿using Character.Player.StateMachine.ConcreteStates;

namespace Character.Player.StateMachine
{
    public class StateFactory
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;

        public StateFactory(Player player, StateMachine stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }
        
        public State Exploring()
        {
            return new ExploringState(_player, _stateMachine);
        }

        public State CarryingStorageBox()
        {
            return new CarryingStorageBoxState(_player, _stateMachine);
        }

        public State FoundCollectable()
        {
            return new FoundCollectableState(_player, _stateMachine);
        }
    }
}