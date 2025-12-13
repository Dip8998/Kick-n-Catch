namespace KNC.Player.StateMachine
{
    public interface IState
    {
        PlayerController Owner { get; set; }
        void OnStateEnter();
        void Update();
        void OnStateExit();
    }
}
