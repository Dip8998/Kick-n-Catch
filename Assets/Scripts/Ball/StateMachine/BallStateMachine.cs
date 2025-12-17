using System.Collections.Generic;
using KNC.Ball.StateMachine.States;

namespace KNC.Ball.StateMachine
{
    public class BallStateMachine
    {
        private IBallState currentState;
        private readonly Dictionary<BallState, IBallState> states = new();

        public BallState CurrentState { get; private set; }

        public BallStateMachine(BallController owner)
        {
            states.Add(BallState.Waiting, new WaitingState(this) { Owner = owner });
            states.Add(BallState.Rolling, new RollingState(this) { Owner = owner });
            states.Add(BallState.Airborne, new AirborneState(this) { Owner = owner });
            states.Add(BallState.Caught, new CaughtState(this) { Owner = owner });
            states.Add(BallState.Missed, new MissedState(this) { Owner = owner });

            ChangeState(BallState.Waiting);
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(BallState state)
        {
            currentState?.OnStateExit();
            currentState = states[state];
            CurrentState = state;
            currentState.OnStateEnter();
        }
    }
}
