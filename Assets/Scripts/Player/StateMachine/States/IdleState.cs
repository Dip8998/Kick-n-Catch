using UnityEngine;

namespace KNC.Player.StateMachine.States
{
    public class IdleState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public IdleState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Debug.Log("[PLAYER][Idle] Enter");
        }

        public void Update()
        {
            Debug.Log("[PLAYER][Idle] Update");

            if (Owner.MoveInput != 0f)
                sm.ChangeState(PlayerState.Move);
        }

        public void OnStateExit()
        {
            Debug.Log("[PLAYER][Idle] Exit");
        }

    }
}
