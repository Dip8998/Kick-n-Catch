using KNC.Ball.StateMachine;
using System;
using UnityEngine;

namespace KNC.Ball
{
    public class BallController
    {
        private Rigidbody2D rb;
        private Collider2D ballCollider;
        private BallStateMachine sm;
        private BallScriptableObject so;

        private int groundContacts;
        private bool isResolved;

        private float catchTimer;
        private const float CatchWindow = 0.45f;

        public Vector2 Position => rb.position;
        public Collider2D BallCollider => ballCollider;
        public bool IsResolved => isResolved;

        public event Action OnResolved;

        public BallController(BallScriptableObject so)
        {
            this.so = so;

            BallView view = UnityEngine.Object.Instantiate(so.BallPrefab);
            view.transform.position = so.BallSpawnPos;
            view.InitializeView(this);

            rb = view.GetComponent<Rigidbody2D>();
            ballCollider = view.GetComponent<Collider2D>();

            view.GetComponentInChildren<KickZone>().Initialize(this);
            sm = new BallStateMachine(this);
        }

        public void Tick()
        {
            sm.Update();

            if (catchTimer > 0f)
                catchTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
            ClampVelocity();
        }

        public void Kick(float force, Vector2 direction)
        {
            isResolved = false;
            catchTimer = 0f;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
            sm.ChangeState(BallState.Rolling);
        }

        public void StartCatchWindow()
        {
            catchTimer = CatchWindow;
        }

        public bool CanBeCaught()
        {
            return catchTimer > 0f && !isResolved;
        }

        public void Resolve()
        {
            isResolved = true;
            catchTimer = 0f;
            OnResolved?.Invoke();
        }

        void ClampVelocity()
        {
            rb.linearVelocity = new Vector2(
                Mathf.Clamp(rb.linearVelocity.x, -so.MaxHorizontalSpeed, so.MaxHorizontalSpeed),
                rb.linearVelocity.y
            );
        }

        public void StopBall()
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        public bool CanKick(Vector2 playerPosition)
        {
            return Mathf.Abs(playerPosition.y - rb.position.y) <= 0.3f;
        }

        public void OnGroundContact(bool enter)
        {
            groundContacts += enter ? 1 : -1;
            groundContacts = Mathf.Max(groundContacts, 0);
        }

        public bool IsAirborne()
        {
            return groundContacts == 0;
        }

        public void IgnoreCollisionWith(Collider2D other, bool ignore)
        {
            if (other != null)
                Physics2D.IgnoreCollision(ballCollider, other, ignore);
        }
    }
}
