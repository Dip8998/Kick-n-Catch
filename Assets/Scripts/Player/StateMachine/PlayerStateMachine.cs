using System.Collections.Generic;
using KNC.Player.StateMachine.States;
using UnityEngine;

namespace KNC.Player.StateMachine
{
    public class PlayerStateMachine
    {
        private IPlayerState currentState;
        private PlayerController owner;

        public PlayerState CurrentState { get; private set; }

        private Dictionary<PlayerState, IPlayerState> states = new();

        public PlayerStateMachine(PlayerController owner)
        {
            this.owner = owner;
            states.Add(PlayerState.Idle, new IdleState(this));
            states.Add(PlayerState.Move, new MoveState(this));
            states.Add(PlayerState.Aim, new AimState(this));
            states.Add(PlayerState.Kick, new KickState(this));

            foreach (var s in states.Values)
                s.Owner = owner;

            ChangeState(PlayerState.Idle);
        }

        public void Update() => currentState?.Update();

        public void ChangeState(PlayerState state)
        {
            Debug.Log($"[PLAYER SM] {CurrentState} → {state}");

            currentState?.OnStateExit();
            currentState = states[state];
            CurrentState = state;
            currentState.OnStateEnter();
        }
    }
}
