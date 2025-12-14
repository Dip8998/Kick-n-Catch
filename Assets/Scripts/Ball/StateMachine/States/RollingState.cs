using UnityEngine;

namespace KNC.Ball.StateMachine.States
{
    public class RollingState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine stateMachine;

        public RollingState(BallStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter()
        {
            BallStateLogger.Enter(BallState.Rolling);
        }

        public void Update()
        {
            if (Owner.IsAirborne())
                stateMachine.ChangeState(BallState.Airborne);
        }

        public void OnStateExit()
        {
            BallStateLogger.Exit(BallState.Rolling);
        }
    }
}
