using KNC.Player.StateMachine.States;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace KNC.Player.StateMachine
{
    public class PlayerStateMachine
    {
        private PlayerController owner;
        private IState currentState;
        protected Dictionary<PlayerState, IState> States = new();

        public PlayerStateMachine(PlayerController owner)
        {
            this.owner = owner;
            CreateState();
            AssignOwner();
            ChangeState(PlayerState.Idle);
        }

        private void AssignOwner()
        {
            foreach(var playerState in States.Values)
            {
                playerState.Owner = owner;
            }
        }

        private void CreateState()
        {
            States.Add(PlayerState.Idle, new IdleState(this));
            States.Add(PlayerState.Move, new MoveState(this));
            States.Add(PlayerState.Kick, new KickState(this));
        }

        public void Update() => currentState?.Update();

        protected void ChangeState(IState state)
        {
            currentState?.OnStateExit();
            currentState = state;
            currentState?.OnStateEnter();
        }

        public void ChangeState(PlayerState state) => ChangeState(States[state]);
    }
}
