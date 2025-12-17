namespace KNC.Ball.StateMachine.States
{
    public class WaitingState : IBallState
    {
        public BallController Owner { get; set; }
        private readonly BallStateMachine sm;

        public WaitingState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Owner.StopBall();
        }

        public void Update() { }
        public void OnStateExit() { }
    }
}
