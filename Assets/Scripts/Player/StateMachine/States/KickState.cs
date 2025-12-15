using UnityEngine;
using KNC.Core.Services;

namespace KNC.Player.StateMachine.States
{
    public class KickState : IPlayerState
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine sm;

        public KickState(PlayerStateMachine sm) => this.sm = sm;

        public void OnStateEnter()
        {
            Debug.Log("[PLAYER][Kick] Enter");

            float rawCharge = Owner.PowerBar.Release();

            float t = rawCharge / Owner.PowerBar.MaxCharge;
            t = Mathf.Clamp01(t);

            float curved = Mathf.SmoothStep(0f, 1f, t);

            float minKickForce = 6f;
            float maxKickForce = 16f;

            float finalForce = Mathf.Lerp(minKickForce, maxKickForce, curved);

            finalForce += Random.Range(-0.4f, 0.4f);

            Debug.Log($"[KICK FORCE] final:{finalForce}");

            if (Owner.CanKickBall())
            {
                Owner.ExecuteKick(finalForce);
            }

            Owner.SetMovementEnabled(true);

            InputService.Instance.ResetKickRelease();
            sm.ChangeState(PlayerState.Move);
        }

        public void Update() { }

        public void OnStateExit()
        {
            Debug.Log("[PLAYER][Kick] Exit");
        }
    }
}
