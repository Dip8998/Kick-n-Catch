using KNC.Ball;
using KNC.Player;
using KNC.Player.StateMachine;
using KNC.Ramp;
using KNC.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KNC.Main
{
    public enum RoundState
    {
        Idle,
        Aiming,
        BallInPlay,
        Resolving
    }

    public class GameService : GenericMonoSingleton<GameService>
    {
        private RampController rampController;
        private PlayerController playerController;
        private BallController ballController;

        private Vector3 playerStartPos;
        private Vector3 ballStartPos;

        [SerializeField] private RampScriptableObject rampScriptableObject;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private BallScriptableObject ballScriptableObject;
        [SerializeField] private float resetDelayTime = 2f;

        public RoundState CurrentRoundState { get; set; } = RoundState.Idle;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            InitializeControllers();
        }

        private void InitializeControllers()
        {
            CurrentRoundState = RoundState.Idle;

            if (playerController?.View != null)
                Destroy(playerController.View.gameObject);

            if (ballController?.View != null)
                Destroy(ballController.View.gameObject);

            rampController = new RampController(rampScriptableObject);
            rampController.Initialize();

            ballController = new BallController(ballScriptableObject);
            ballController.Initialize();

            playerController = new PlayerController(playerScriptableObject, ballController);
            playerController.Initialize();

            playerStartPos = playerScriptableObject.PlayerSpawnPos;
            ballStartPos = ballController.View.transform.position;

            ballController.OnBallKicked += () => CurrentRoundState = RoundState.BallInPlay;
            ballController.OnCaught += OnBallCaught;
            ballController.OnMissed += OnBallMissed;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeControllers();
            playerController.SetMovementEnabled(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnBallCaught()
        {
            CurrentRoundState = RoundState.Resolving;

            playerController.PowerBar.Reset();
            playerController.PowerBarView.Hide();
            playerController.EnterMoveMode();

            StartCoroutine(DelayedResetSequence());
        }

        private void OnBallMissed()
        {
            CurrentRoundState = RoundState.Resolving;
            SceneManager.LoadScene(0);
        }

        private System.Collections.IEnumerator DelayedResetSequence()
        {
            yield return new WaitForSeconds(resetDelayTime);
            ResetPositions();
        }

        private void ResetPositions()
        {
            var prb = playerController.Rigidbody;

            ballController.SetKickZoneActive(false);

            prb.linearVelocity = Vector2.zero;
            prb.angularVelocity = 0f;
            prb.position = playerStartPos;
            prb.Sleep();

            playerController.EnterMoveMode();
            playerController.SetMovementEnabled(true);
            playerController.PowerBarView.Hide();
            playerController.View.transform.localScale = playerController.InitialScale;

            ballController.ResetBall(ballStartPos);

            StartCoroutine(ReenableKickZoneSequence());
            CurrentRoundState = RoundState.Idle;
        }

        private System.Collections.IEnumerator ReenableKickZoneSequence()
        {
            yield return null;
            ballController.SetKickZoneActive(true);
        }
    }
}
