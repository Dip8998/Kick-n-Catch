namespace KNC.Ball.StateMachine.States
{
    public class RollingState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine sm;

        public RollingState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter() { }

        public void Update()
        {
            if (Owner.IsResolving)
                return;

            if (Owner.IsAirborne())
            {
                sm.ChangeState(BallState.Airborne);
            }
        }

        public void OnStateExit() { }
    }
}
