using UnityEngine;

namespace KNC.Player.StateMachine.States
{
    public class TurnAndCatchState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private readonly PlayerStateMachine sm;

        public TurnAndCatchState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Owner.View.transform.localScale = new Vector3(
                -Owner.InitialScale.x,
                Owner.InitialScale.y,
                Owner.InitialScale.z
            );

            Owner.SetMovementEnabled(true);
        }

        public void Update()
        {
            Owner.Move(Time.fixedDeltaTime);
        }

        public void OnStateExit() { }
    }
}
