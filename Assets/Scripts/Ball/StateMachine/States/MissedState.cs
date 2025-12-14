namespace KNC.Ball.StateMachine.States
{
    public class MissedState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine stateMachine;

        public MissedState(BallStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            BallStateLogger.Enter(BallState.Missed);
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit()
        {
            BallStateLogger.Exit(BallState.Missed);
        }
    }
}
