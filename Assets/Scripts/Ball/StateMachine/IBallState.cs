namespace KNC.Ball.StateMachine
{
    public interface IBallState
    {
        BallController Owner { get; set; }
        void OnStateEnter();
        void Update();
        void OnStateExit();
    }
}
