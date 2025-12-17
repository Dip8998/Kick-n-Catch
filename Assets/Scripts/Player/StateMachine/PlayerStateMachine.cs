using System.Collections.Generic;
using KNC.Player.StateMachine.States;

namespace KNC.Player.StateMachine
{
    public class PlayerStateMachine
    {
        private IPlayerState currentState;
        private readonly Dictionary<PlayerState, IPlayerState> states = new();

        public PlayerState CurrentState { get; private set; }

        public PlayerStateMachine(PlayerController owner)
        {
            states.Add(PlayerState.Idle, new IdleState(this) { Owner = owner });
            states.Add(PlayerState.Move, new MoveState(this) { Owner = owner });
            states.Add(PlayerState.Aim, new AimState(this) { Owner = owner });
            states.Add(PlayerState.Kick, new KickState(this) { Owner = owner });
            states.Add(PlayerState.TurnAndCatch, new TurnAndCatchState(this) { Owner = owner });

            ChangeState(PlayerState.Idle);
        }

        public void Update() => currentState?.Update();

        public void ChangeState(PlayerState state)
        {
            currentState?.OnStateExit();
            currentState = states[state];
            CurrentState = state;
            currentState.OnStateEnter();
        }
    }
}
