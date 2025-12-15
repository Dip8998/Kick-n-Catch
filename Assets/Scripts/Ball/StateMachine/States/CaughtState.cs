using UnityEngine;

namespace KNC.Ball.StateMachine.States
{
    public class CaughtState : IBallState
    {
        public BallController Owner { get; set; }

        public CaughtState(BallStateMachine sm) { }

        public void OnStateEnter()
        {
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
