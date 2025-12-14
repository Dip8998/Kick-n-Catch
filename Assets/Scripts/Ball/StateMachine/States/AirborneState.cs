namespace KNC.Ball.StateMachine.States
{
    public class AirborneState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine stateMachine;

        public AirborneState(BallStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            BallStateLogger.Enter(BallState.Airborne);
        }

        public void Update()
        {
            if (Owner.HasLanded())
                stateMachine.ChangeState(BallState.Rolling);
        }

        public void OnStateExit()
        {
            BallStateLogger.Exit(BallState.Airborne);
        }
    }
}
