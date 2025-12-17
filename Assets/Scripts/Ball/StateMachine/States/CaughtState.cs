namespace KNC.Ball.StateMachine.States
{
    public class CaughtState : IBallState
    {
        public BallController Owner { get; set; }
        private readonly BallStateMachine sm;

        public CaughtState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Owner.StopBall();
        }

        public void Update() { }
        public void OnStateExit() { }
    }
}
