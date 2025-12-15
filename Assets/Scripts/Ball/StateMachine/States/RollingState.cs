using UnityEngine;

namespace KNC.Ball.StateMachine.States
{
    public class RollingState : IBallState
    {
        public BallController Owner { get; set; }
        private BallStateMachine sm;

        public RollingState(BallStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Debug.Log("[BALL][Rolling] Enter");
        }

        public void Update()
        {
            if (Owner.IsResolved)
                return;

            if (Owner.IsAirborne())
            {
                Debug.Log("[BALL][Rolling] Became Airborne");
                Owner.StartCatchWindow(); 
                sm.ChangeState(BallState.Airborne);
            }
        }

        public void OnStateExit()
        {
            Debug.Log("[BALL][Rolling] Exit");
        }
    }
}
