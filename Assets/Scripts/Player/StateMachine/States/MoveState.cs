using UnityEngine;
using KNC.Player.StateMachine;

namespace KNC.Player.StateMachine.States
{
    public class MoveState : IState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public MoveState(PlayerStateMachine sm)
        {
            this.sm = sm;
        }

        public void OnStateEnter() { }

        public void Update()
        {
            if (Owner.MoveInput == 0f)
            {
                sm.ChangeState(PlayerState.Idle);
                return;
            }

            Owner.Move(Time.fixedDeltaTime);
        }

        public void OnStateExit() { }
    }
}
