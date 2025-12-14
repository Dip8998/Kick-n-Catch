using KNC.Ball.StateMachine.States;
using System.Collections.Generic;

namespace KNC.Ball.StateMachine
{
    public class BallStateMachine
    {
        private BallController owner;
        private IBallState currentState;
        protected Dictionary<BallState, IBallState> States = new();

        public BallStateMachine(BallController owner)
        {
            this.owner = owner;
            CreateState();
            AssignOwner();
            ChangeState(BallState.Waiting);
        }

        private void AssignOwner()
        {
            foreach(var ballState in States.Values)
            {
                ballState.Owner = owner;
            }
        }

        private void CreateState()
        {
            States.Add(BallState.Waiting, new WaitingState(this));
            States.Add(BallState.Rolling, new RollingState(this));
            States.Add(BallState.Airborne, new AirborneState(this));
            States.Add(BallState.Caught, new CaughtState(this));
            States.Add(BallState.Missed, new MissedState(this));
        }

        protected void ChangeState(IBallState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void Update() => currentState?.Update();

        public void ChangeState(BallState state) => ChangeState(States[state]); 
    }
}
