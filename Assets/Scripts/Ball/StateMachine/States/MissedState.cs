using UnityEngine;

namespace KNC.Ball.StateMachine.States
{
    public class MissedState : IBallState
    {
        public BallController Owner { get; set; }

        public MissedState(BallStateMachine sm) { }

        public void OnStateEnter()
        {
            Owner.StopBall();
        }

        public void Update() { }

        public void OnStateExit() { }
    }
}
