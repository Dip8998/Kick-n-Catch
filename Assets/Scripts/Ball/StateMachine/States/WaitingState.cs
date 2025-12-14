namespace KNC.Ball.StateMachine.States
{
    public class WaitingState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine stateMachine;

        public WaitingState(BallStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            BallStateLogger.Enter(BallState.Waiting);
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit()
        {
            BallStateLogger.Exit(BallState.Waiting);
        }
    }
}
