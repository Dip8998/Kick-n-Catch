using KNC.Player.StateMachine;

namespace KNC.Player.StateMachine.States
{
    public class IdleState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public IdleState(PlayerStateMachine sm)
        {
            this.sm = sm;
        }

        public void OnStateEnter() { }

        public void Update()
        {
            if (Owner.MoveInput != 0f)
            {
                sm.ChangeState(PlayerState.Move);
            }
        }

        public void OnStateExit() { }
    }
}
