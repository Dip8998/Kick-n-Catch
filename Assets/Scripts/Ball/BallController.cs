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
        private BallView view;

        private int groundContacts;
        private bool isResolved;

        private float catchTimer;
        private const float CatchWindow =2f;

        public Vector2 Position => so.BallSpawnPos;
        public Collider2D BallCollider => ballCollider;
        public bool IsResolved => isResolved;
        public BallView View => view;
        public event Action OnResolved;
        public event Action OnCaught;
        public event Action OnMissed;

        private KickZone kickZone;
        private const float MaxKickDistance = 1.0f;
        public Rigidbody2D Rigidbody => rb;
        public const float MissVelocityThreshold = 0.1f;
        private float resolveTimer;
        private const float MaxResolveTime = 4f;
        public bool HasBeenKicked { get; private set; }

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
            {
                Miss();
            }

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
            catchTimer = 0f;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
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

        public void Resolve()
        {
            isResolved = true;
            catchTimer = 0f;

            SetKickZoneActive(false); 

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

        public void Catch()
        {
            if (isResolved) return;

            Debug.Log("[BALL] Catch confirmed");
            Resolve();
            sm.ChangeState(BallState.Caught);
            OnCaught?.Invoke();
        }

        public void Miss()
        {
            if (isResolved) return;

            Debug.Log("[BALL] Miss confirmed");
            Resolve();
            sm.ChangeState(BallState.Missed);
            OnMissed?.Invoke();
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
            float distance = Vector2.Distance(playerPosition, rb.position);
            return distance <= MaxKickDistance;
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

        public void IgnoreCollisionWith(Collider2D other, bool ignore)
        {
            if (other != null)
                Physics2D.IgnoreCollision(ballCollider, other, ignore);
        }
    }
}
