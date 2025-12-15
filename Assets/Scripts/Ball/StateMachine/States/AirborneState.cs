using UnityEngine;

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
            if (Owner.IsResolved)
                return;

            if (!Owner.IsAirborne())
            {
                Owner.Resolve();
                sm.ChangeState(BallState.Missed);
            }
        }

        public void OnStateExit() { }
    }
}
