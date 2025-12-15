using UnityEngine;

namespace KNC.Player.StateMachine.States
{
    public class MoveState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public MoveState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Debug.Log("[PLAYER][Move] Enter");
        }

        public void Update()
        {
            Debug.Log("[PLAYER][Move] Update | MoveInput: " + Owner.MoveInput);

            if (Owner.MoveInput == 0f)
                sm.ChangeState(PlayerState.Idle);
            else
                Owner.Move(Time.fixedDeltaTime);
        }

        public void OnStateExit()
        {
            Debug.Log("[PLAYER][Move] Exit");
        }

    }
}
