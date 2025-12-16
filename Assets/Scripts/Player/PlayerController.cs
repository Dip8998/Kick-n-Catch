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
        private PlayerStateMachine sm;
        private PowerBarController powerBar;
        private BallController ball;
        private PlayerScriptableObject so;

        private bool canMove = true;

        public BallController BallController => ball;
        public Rigidbody2D Rigidbody => view.Rigidbody;
        public PowerBarController PowerBar => powerBar;
        public PowerBarView PowerBarView => view.PowerBarView;
        public PlayerView View => view;
        public PlayerStateMachine StateMachine => sm;

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
        }

        public void ExecuteKick(float force, Vector2 direction)
        {
            view.StartCoroutine(ExecuteKickSequence(force, direction));
        }

        public void EnterMoveMode()
        {
            sm.ChangeState(PlayerState.Move);
        }

        public void EnterAimMode()
        {
            GameService.Instance.CurrentRoundState = RoundState.Aiming;
            sm.ChangeState(PlayerState.Aim);
        }

        private System.Collections.IEnumerator ExecuteKickSequence(float force, Vector2 direction)
        {
            var playerCol = Rigidbody.GetComponent<Collider2D>();

            ball.IgnoreCollisionWith(playerCol, true);
            FreezePlayer(true);

            yield return new WaitForFixedUpdate();

            ball.Kick(force, direction);

            view.StartCoroutine(ReenableAfterSeparation(playerCol));
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
