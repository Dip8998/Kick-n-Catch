using KNC.Ball;
using KNC.Core.Services;
using KNC.Player;
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
        private float resetDelayTime = 2f;

        [SerializeField] private RampScriptableObject rampScriptableObject;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private BallScriptableObject ballScriptableObject;
        
        public RoundState CurrentRoundState { get; set; } = RoundState.Idle;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                ScoreService.Instance.ResetScore();
            }

            InitializeControllers();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializeControllers();
            playerController.SetMovementEnabled(true);
        }

        private void InitializeControllers()
        {
            CurrentRoundState = RoundState.Idle;

            if (playerController?.View != null)
                Destroy(playerController.View.gameObject);

            if (ballController?.View != null)
                Destroy(ballController.View.gameObject);

            ballController = new BallController(ballScriptableObject);
            playerController = new PlayerController(playerScriptableObject, ballController);

            rampController = new RampController(rampScriptableObject);
            rampController.Initialize();

            ballController.Initialize();
            playerController.Initialize();

            playerStartPos = playerScriptableObject.PlayerSpawnPos;
            ballStartPos = ballController.View.transform.position;

            ballController.OnBallKicked += () => CurrentRoundState = RoundState.BallInPlay;
            ballController.OnCaught += OnBallCaught;
            ballController.OnMissed += OnBallMissed;

            Debug.Log("[GAME] Controllers initialized fresh");
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnBallCaught()
        {
            Debug.Log("[GAME] OnBallCaught fired");

            CurrentRoundState = RoundState.Resolving;

            Debug.Log("[GAME] Calling AddScore()");
            ScoreService.Instance.AddScore(1);

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
