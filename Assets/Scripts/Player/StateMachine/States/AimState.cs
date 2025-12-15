using UnityEngine;
using KNC.Core.Services;

namespace KNC.Player.StateMachine.States
{
    public class AimState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public AimState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            if (Owner.BallController.IsResolved)
            {
                sm.ChangeState(PlayerState.Move);
                return;
            }

            InputService.Instance.ReleaseMove();
            Owner.SetMovementEnabled(false);
            Owner.PowerBarView.Show();
        }

        public void Update()
        {
            Debug.Log("[PLAYER][Aim] Update | Charging: " + Owner.PowerBar.IsCharging);

            if (InputService.Instance.KickHeld)
            {
                if (!Owner.PowerBar.IsCharging)
                {
                    Debug.Log("[PLAYER][Aim] Start Charging");
                    Owner.PowerBar.StartCharge();
                }

                Owner.PowerBar.Update(Time.deltaTime);
            }

            if (Owner.IsKickReleased)
            {
                Debug.Log("[PLAYER][Aim] Kick Released");
                InputService.Instance.ResetKickRelease();
                sm.ChangeState(PlayerState.Kick);
            }
        }

        public void OnStateExit()
        {
            Debug.Log("[PLAYER][Aim] Exit");
            Owner.SetMovementEnabled(false);
        }

    }
}
