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
            float rawCharge = Owner.PowerBar.Release();

            float t = Mathf.Clamp01(rawCharge / Owner.PowerBar.MaxCharge);
            float curved = Mathf.SmoothStep(0f, 1f, t);

            float minKickForce = 6f;
            float maxKickForce = 16f;

            float baseForce = Mathf.Lerp(minKickForce, maxKickForce, curved);

            float forceVariance = Mathf.Lerp(0.98f, 1.05f, curved);

            float distanceError = Mathf.Abs(
                Owner.Rigidbody.position.x - Owner.BallController.Rigidbody.position.x
            );

            float errorT = Mathf.InverseLerp(0f, 0.6f, distanceError);

            float positionalBias = Mathf.Lerp(1.05f, 0.92f, errorT);

            float finalForce =
                baseForce *
                positionalBias *
                Random.Range(0.97f, forceVariance);


            float angleVariance = Mathf.Lerp(1.5f, 6f, curved);
            float angle = Random.Range(-angleVariance, angleVariance);

            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            if (Owner.CanKickBall())
            {
                Owner.ExecuteKick(finalForce, direction);
            }

            Owner.SetMovementEnabled(true);

            InputService.Instance.ResetKickRelease();
            sm.ChangeState(PlayerState.TurnAndCatch);
        }

        public void Update() { }
        public void OnStateExit() { }
    }
}
