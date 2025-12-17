using UnityEngine;
using KNC.Core.Services;
using KNC.Player.StateMachine;
using KNC.PowerBar;
using KNC.Ball;
using KNC.Main;

namespace KNC.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private PlayerStateMachine stateMachine;
        private PowerBarController powerBar;
        private BallController ball;
        private PlayerScriptableObject so;

        private bool canMove = true;

        public Rigidbody2D Rigidbody => view.Rigidbody;
        public PlayerView View => view;
        public BallController BallController => ball;
        public PowerBarController PowerBar => powerBar;
        public PowerBarView PowerBarView => view.PowerBarView;
        public PlayerStateMachine StateMachine => stateMachine;

        public Vector3 InitialScale { get; private set; }
        public float MoveInput { get; private set; }
        public bool IsKickReleased { get; private set; }

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

            stateMachine = new PlayerStateMachine(this);
        }

        public void ReadInput()
        {
            if (GameService.Instance.CurrentRoundState == RoundState.Resolving)
            {
                MoveInput = 0f;
                IsKickReleased = false;
                return;
            }

            MoveInput = InputService.Instance.Horizontal;
            IsKickReleased = InputService.Instance.KickReleased;
        }

        public void Tick()
        {
            if (GameService.Instance.CurrentRoundState != RoundState.Resolving)
                stateMachine.Update();
        }

        public void Move(float dt)
        {
            if (!canMove) return;

            float targetX = Rigidbody.position.x + MoveInput * so.PlayerMoveSpeed * dt;
            targetX = Mathf.Clamp(targetX, so.MinX, so.MaxX);

            Rigidbody.MovePosition(new Vector2(targetX, Rigidbody.position.y));
        }

        public void SetMovementEnabled(bool enabled) => canMove = enabled;

        public void ExecuteKick(float force, Vector2 direction)
        {
            view.StartCoroutine(ExecuteKickSequence(force, direction));
        }

        public void EnterMoveMode()
        {
            stateMachine.ChangeState(PlayerState.Move);
        }

        public void EnterAimMode()
        {
            GameService.Instance.CurrentRoundState = RoundState.Aiming;
            stateMachine.ChangeState(PlayerState.Aim);
        }

        private System.Collections.IEnumerator ExecuteKickSequence(float force, Vector2 direction)
        {
            var playerCollider = Rigidbody.GetComponent<Collider2D>();

            ball.IgnoreCollisionWith(playerCollider, true);
            FreezePlayer(true);

            yield return new WaitForFixedUpdate();

            ball.Kick(force, direction);

            yield return new WaitForSeconds(0.1f);

            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;

            ball.IgnoreCollisionWith(playerCollider, false);
            FreezePlayer(false);
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
            if (!ball.HasBeenKicked && ball.CanKick(Rigidbody.position))
                stateMachine.ChangeState(PlayerState.Aim);
        }

        public void OnExitKickZone()
        {
            powerBar.Reset();
            PowerBarView.Hide();
            stateMachine.ChangeState(PlayerState.Move);
        }
    }
}
