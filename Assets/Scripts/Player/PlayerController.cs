using UnityEngine;
using KNC.Core.Services;
using KNC.Player.StateMachine;
using KNC.PowerBar;
using KNC.Ball;

namespace KNC.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private PlayerStateMachine sm;
        private bool canMove = true;

        private PowerBarController powerBar;
        private BallController ball;

        public float MoveInput { get; private set; }
        public bool IsKickReleased { get; private set; }
        public BallController BallController => ball;
        public Rigidbody2D Rigidbody => view.Rigidbody;
        public PowerBarController PowerBar => powerBar;
        public PowerBarView PowerBarView => view.PowerBarView;
        public PlayerView View => view;
        public PlayerStateMachine StateMachine => sm;
        private PlayerScriptableObject so;
        private float speedMultiplier = 1f;
        public Vector3 InitialScale { get; private set; }

        public PlayerController(PlayerScriptableObject so, BallController ball)
        {
            this.so = so;
            this.ball = ball;
        }

        public void Initialize()
        {
            view = Object.Instantiate(so.PlayerPrefab);
            view.transform.position = so.PlayerSpawnPos;
            view.InitializeView(this);
            InitialScale = view.transform.localScale;

            powerBar = new PowerBarController(so.PowerBarSO);
            view.PowerBarView.Bind(powerBar);

            sm = new PlayerStateMachine(this);
        }

        public void ReadInput()
        {
            MoveInput = InputService.Instance.Horizontal;
            IsKickReleased = InputService.Instance.KickReleased;
        }

        public void Tick() => sm.Update();

        public void Move(float fixedDeltaTime)
        {
            if (!canMove) return;

            float targetX =
                Rigidbody.position.x +
                MoveInput * so.PlayerMoveSpeed * fixedDeltaTime;

            targetX = Mathf.Clamp(targetX, so.MinX, so.MaxX);

            Rigidbody.MovePosition(
                new Vector2(targetX, Rigidbody.position.y)
            );
        }

        public void SetMovementEnabled(bool v) => canMove = v;

        private System.Collections.IEnumerator ReenableAfterSeparation(Collider2D col)
        {
            yield return new WaitForSeconds(0.1f);

            Rigidbody.linearVelocity = Vector2.zero; 
            Rigidbody.angularVelocity = 0f;         

            ball.IgnoreCollisionWith(col, false);

            FreezePlayer(false);

            SetSpeedMultiplier(1f);
        }

        public void ExecuteKick(float force)
        {
            view.StartCoroutine(ExecuteKickSequence(force));
        }

        private System.Collections.IEnumerator ExecuteKickSequence(float force)
        {
            var playerCol = Rigidbody.GetComponent<Collider2D>();

            ball.IgnoreCollisionWith(playerCol, true);
            SetSpeedMultiplier(0.75f);
            FreezePlayer(true);

            yield return new WaitForFixedUpdate();

            Vector2 dir = Vector2.right;
            ball.Kick(force, dir);

            view.StartCoroutine(ReenableAfterSeparation(playerCol));
        }
        public void SetSpeedMultiplier(float value)
        {
            speedMultiplier = value;
        }

        public void FreezePlayer(bool freeze)
        {
            Rigidbody.constraints = freeze
                ? RigidbodyConstraints2D.FreezeAll
                : RigidbodyConstraints2D.FreezeRotation;
        }

        public bool CanKickBall()
        {
            return ball.CanKick(Rigidbody.position);
        }

        public void OnEnterKickZone()
        {
            if (ball.HasBeenKicked)
                return;

            if (!ball.CanKick(Rigidbody.position))
                return;

            sm.ChangeState(PlayerState.Aim);
        }

        public void OnExitKickZone()
        {
            PowerBar.Reset();
            PowerBarView.Hide();
            sm.ChangeState(PlayerState.Move);
        }
    }
}
