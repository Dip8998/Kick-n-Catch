using System.Collections.Generic;
using KNC.Ball.StateMachine.States;
namespace KNC.Ball.StateMachine
{
    public class BallStateMachine
    {
        private IBallState currentState;
        private BallController owner;
        private Dictionary<BallState, IBallState> states = new();

        public BallState CurrentState { get; private set; }

        public BallStateMachine(BallController owner)
        {
            this.owner = owner;

            states.Add(BallState.Waiting, new WaitingState(this));
            states.Add(BallState.Rolling, new RollingState(this));
            states.Add(BallState.Airborne, new AirborneState(this));
            states.Add(BallState.Caught, new CaughtState(this));
            states.Add(BallState.Missed, new MissedState(this));

            foreach (var s in states.Values)
                s.Owner = owner;

            ChangeState(BallState.Waiting);
        }

        public void Update() => currentState?.Update();

        public void ChangeState(BallState state)
        {
            currentState?.OnStateExit();
            currentState = states[state];
            CurrentState = state;
            currentState.OnStateEnter();
        }
    }
}
