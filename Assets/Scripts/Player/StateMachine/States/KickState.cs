namespace KNC.Player.StateMachine.States
{
    public class KickState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public KickState(PlayerStateMachine stateMachine) => this.stateMachine = stateMachine;

        public void OnStateEnter() { }

        public void Update() { }

        public void OnStateExit() { }
    }
}
