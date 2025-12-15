using UnityEngine;

namespace KNC.Ball.StateMachine.States
{
    public class WaitingState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine sm;

        public WaitingState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Debug.Log("[BALL][Waiting] Enter");
            Owner.StopBall();
        }

        public void Update()
        {
            Debug.Log("[BALL][Waiting] Update");
        }

        public void OnStateExit()
        {
            Debug.Log("[BALL][Waiting] Exit");
        }

    }
}
