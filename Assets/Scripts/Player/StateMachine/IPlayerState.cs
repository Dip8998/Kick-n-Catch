namespace KNC.Player.StateMachine
{
    public interface IPlayerState
    {
        PlayerController Owner { get; set; }
        void OnStateEnter();
        void Update();
        void OnStateExit();
    }
}
