using KNC.Ball.StateMachine;
using System;
using UnityEngine;

namespace KNC.Ball
{
    public class BallController
    {
        private BallStateMachine sm;
        private BallScriptableObject so;
        private BallView view;
        private Rigidbody2D rb;
        private KickZone kickZone;
        private Collider2D ballCollider;
       
        private int groundContacts;
        private bool isResolved;
        private float catchTimer;
        private float resolveTimer;

        private const float MaxKickDistance = 1.0f;
        private const float CatchWindow = 2f;
        private const float MaxResolveTime = 4f;

        public Rigidbody2D Rigidbody => rb;
        public Collider2D BallCollider => ballCollider;
        public BallView View => view;
        public bool HasBeenKicked { get; private set; }
        public bool IsResolving => isResolved;
        public const float MissVelocityThreshold = 0.1f;

        public event Action OnResolved;
        public event Action OnCaught;
        public event Action OnMissed;
        public event Action OnBallKicked;

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

            sm = new BallStateMachine(this);
        }

        public void Tick()
        {
            sm.Update();

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

            StartCatchWindow(); 

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

            OnBallKicked?.Invoke();
            sm.ChangeState(BallState.Airborne);
        }

        public void SetKickZoneActive(bool active)
        {
            kickZone.GetComponent<Collider2D>().enabled = active;
        }

        public void StartCatchWindow()
        {
            catchTimer = CatchWindow;
        }

        public bool CanBeCaught()
        {
            return catchTimer > 0f && !isResolved;
        }

        private void Resolve()
        {
            isResolved = true;
            catchTimer = 0f;

            SetKickZoneActive(false);
            OnResolved?.Invoke();
        }

        private void ResolveAs(BallState state, Action callback)
        {
            if (isResolved) return;

            Resolve();
            sm.ChangeState(state);
            callback?.Invoke();
        }

        public void Catch()
        {
            Debug.Log("[BALL] Catch fired on instance " + GetHashCode());
            ResolveAs(BallState.Caught, OnCaught);
        }

        public void Miss()
        {
            ResolveAs(BallState.Missed, OnMissed);
        }

        public void ResetBall(Vector3 position)
        {
            HasBeenKicked = false;
            isResolved = false;
            resolveTimer = 0f;
            catchTimer = 0f;
            groundContacts = 0;

            sm.ChangeState(BallState.Waiting);

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

        public void IgnoreCollisionWith(Collider2D other, bool ignore)
        {
            if (other != null)
                Physics2D.IgnoreCollision(ballCollider, other, ignore);
        }
    }
}
