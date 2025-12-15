namespace KNC.Ball.StateMachine.States
{
    public class AirborneState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine sm;

        public AirborneState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter() { }

        public void Update()
        {
            if (!Owner.HasBeenKicked || Owner.IsResolving)
                return;

            if (!Owner.IsAirborne())
            {
                if (Owner.Rigidbody.linearVelocity.magnitude < BallController.MissVelocityThreshold)
                {
                    Owner.Miss();
                }
            }
        }

        public void OnStateExit() { }
    }
}
