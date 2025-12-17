using UnityEngine;
using KNC.Core.Services;

namespace KNC.Player.StateMachine.States
{
    public class AimState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private readonly PlayerStateMachine sm;

        public AimState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            InputService.Instance.ReleaseMove();
            Owner.SetMovementEnabled(false);
            Owner.PowerBarView.Show();
        }

        public void Update()
        {
            if (InputService.Instance.KickHeld)
            {
                if (!Owner.PowerBar.IsCharging)
                    Owner.PowerBar.StartCharge();

                Owner.PowerBar.Update(Time.deltaTime);
            }

            if (Owner.IsKickReleased)
            {
                InputService.Instance.ResetKickRelease();
                sm.ChangeState(PlayerState.Kick);
            }
        }

        public void OnStateExit()
        {
            Owner.SetMovementEnabled(false);
        }
    }
}
