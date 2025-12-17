using UnityEngine;
using KNC.Core.Services;
using KNC.Ball.StateMachine;
using System;

namespace KNC.Ball
{
    public class BallController
    {
        private BallStateMachine stateMachine;
        private readonly BallScriptableObject so;
        private BallView view;
        private Rigidbody2D rb;
        private Collider2D ballCollider;
        private KickZone kickZone;

        private int groundContacts;
        private bool isResolved;
        private float catchTimer;
        private float resolveTimer;

        private const float MaxKickDistance = 1.0f;
        private const float MaxResolveTime = 4f;

        public const float MissVelocityThreshold = 0.1f;

        public Rigidbody2D Rigidbody => rb;
        public Collider2D BallCollider => ballCollider;
        public BallView View => view;
        public bool HasBeenKicked { get; private set; }
        public bool IsResolving => isResolved;

        public BallController(BallScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            view = UnityEngine.Object.Instantiate(so.BallPrefab);
            view.transform.position = so.BallSpawnPos;
            view.InitializeView(this);

            rb = view.GetComponent<Rigidbody2D>();
            ballCollider = view.GetComponent<Collider2D>();

            rb.bodyType = RigidbodyType2D.Kinematic;

            kickZone = view.GetComponentInChildren<KickZone>();
            kickZone.Initialize(this);

            stateMachine = new BallStateMachine(this);
        }

        public void Tick()
        {
            stateMachine.Update();

            if (!HasBeenKicked || isResolved)
                return;

            resolveTimer += Time.deltaTime;

            if (resolveTimer >= MaxResolveTime)
                Miss();

            if (catchTimer > 0f)
                catchTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
            ClampVelocity();
        }

        public void Kick(float force, Vector2 direction)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            HasBeenKicked = true;
            isResolved = false;
            resolveTimer = 0f;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

            float impulseVariance = UnityEngine.Random.Range(0.97f, 1.03f);
            rb.linearVelocity *= impulseVariance;

            float speed = rb.linearVelocity.magnitude;
            catchTimer = Mathf.Lerp(
                2.2f,
                0.7f,
                Mathf.InverseLerp(4f, 14f, speed)
            );

            EventService.Instance.RaiseKickStarted();
            stateMachine.ChangeState(BallState.Airborne);
        }

        public bool CanBeCaught()
        {
            return !isResolved && HasBeenKicked;
        }

        public void Catch()
        {
            if (isResolved)
                return;

            Resolve();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            stateMachine.ChangeState(BallState.Caught);
            EventService.Instance.RaiseBallCaught();
        }

        public void Miss()
        {
            if (isResolved)
                return;

            Resolve();
            stateMachine.ChangeState(BallState.Missed);
            EventService.Instance.RaiseBallMissed();
        }

        private void Resolve()
        {
            isResolved = true;
            catchTimer = 0f;
        }

        public void ResetBall(Vector3 position)
        {
            SetKickZoneActive(false);

            HasBeenKicked = false;
            isResolved = false;
            resolveTimer = 0f;
            catchTimer = 0f;
            groundContacts = 0;

            stateMachine.ChangeState(BallState.Waiting);

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;

            view.transform.position = position;
            rb.position = position;
            rb.Sleep();
        }

        public bool CanKick(Vector2 playerPosition)
        {
            return Vector2.Distance(playerPosition, rb.position) <= MaxKickDistance;
        }

        public void OnGroundContact(bool enter)
        {
            groundContacts += enter ? 1 : -1;
            groundContacts = Mathf.Clamp(groundContacts, 0, 10);
        }

        public bool IsAirborne()
        {
            return groundContacts <= 0;
        }

        public void StopBall()
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        private void ClampVelocity()
        {
            rb.linearVelocity = new Vector2(
                Mathf.Clamp(rb.linearVelocity.x, -so.MaxHorizontalSpeed, so.MaxHorizontalSpeed),
                rb.linearVelocity.y
            );
        }

        public void SetKickZoneActive(bool active)
        {
            kickZone.GetComponent<Collider2D>().enabled = active;
        }

        public void IgnoreCollisionWith(Collider2D other, bool ignore)
        {
            if (other != null)
                Physics2D.IgnoreCollision(ballCollider, other, ignore);
        }
    }
}
