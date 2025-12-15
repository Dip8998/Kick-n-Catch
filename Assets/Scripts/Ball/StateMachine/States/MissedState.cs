namespace KNC.Ball.StateMachine.States
{
    public class MissedState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine sm;

        public MissedState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
