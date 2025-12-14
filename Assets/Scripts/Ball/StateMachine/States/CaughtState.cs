namespace KNC.Ball.StateMachine.States
{
    public class CaughtState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine stateMachine;

        public CaughtState(BallStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            BallStateLogger.Enter(BallState.Caught);
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit()
        {
            BallStateLogger.Exit(BallState.Caught);
        }
    }
}
