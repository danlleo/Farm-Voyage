namespace Character.Michael.StateMachine.ConcreteStates
{
    public class WalkingToIdlePositionState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public WalkingToIdlePositionState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            _michael.Locomotion.HandleMoveDestination(Michael.TransformPoints.IdlePoint,
                () =>
                {
                    _michael.transform.rotation = Michael.TransformPoints.IdlePoint.rotation;
                    _stateMachine.ChangeState(_michael.StateFactory.Idle());
                });
        }
    }
}
