using KNC.Main;
using UnityEngine;

namespace KNC.Player.StateMachine.States
{
    public class MoveState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public MoveState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter() { }

        public void Update()
        {
            if (GameService.Instance.CurrentRoundState == RoundState.Resolving)
                return;

            if (Owner.MoveInput == 0f)
                sm.ChangeState(PlayerState.Idle);
            else
                Owner.Move(Time.fixedDeltaTime);
        }

        public void OnStateExit() { }
    }
}
