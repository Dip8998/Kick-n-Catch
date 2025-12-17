using UnityEngine;
using KNC.Core.Services;

namespace KNC.Player.StateMachine.States
{
    public class KickState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private readonly PlayerStateMachine sm;

        public KickState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            float raw = Owner.PowerBar.Release();
            float t = Mathf.Clamp01(raw / Owner.PowerBar.MaxCharge);
            float curved = Mathf.SmoothStep(0f, 1f, t);

            float force = Mathf.Lerp(6f, 16f, curved);
            float angle = Random.Range(-Mathf.Lerp(1.5f, 6f, curved), Mathf.Lerp(1.5f, 6f, curved));

            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            if (Owner.CanKickBall())
                Owner.ExecuteKick(force, direction);

            Owner.SetMovementEnabled(true);
            InputService.Instance.ResetKickRelease();
            sm.ChangeState(PlayerState.TurnAndCatch);
        }

        public void Update() { }
        public void OnStateExit() { }
    }
}
