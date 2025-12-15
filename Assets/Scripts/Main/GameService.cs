using KNC.Ball;
using KNC.Player;
using KNC.Player.StateMachine;
using KNC.Ramp;
using KNC.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KNC.Main
{
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
        [SerializeField] private float resetDelayTime = 2.0f;
        // In KNC.Main.GameService.cs

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
            // CLEAN OLD
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

            // SAFE: after initialization
            playerStartPos = playerScriptableObject.PlayerSpawnPos;
            ballStartPos = ballController.View.transform.position;

            // SUBSCRIBE ONCE
            ballController.OnCaught += OnBallCaught;
            ballController.OnMissed += OnBallMissed;
        }


        // **Scene Reload Handler** (This is the crucial part that reconnects inputs)
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            Debug.Log("[GAME] Scene Reloaded. Reinitializing Controllers to New Scene Views.");

            // Call the initialization function again! This will re-instantiate the BallView 
            // and PlayerView (or find the new ones) and reconnect the input listeners.
            InitializeControllers();

            // Ensure state is correct for the start of the game
            playerController.SetMovementEnabled(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Unsubscribe from the static event when the GameService is destroyed
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnBallCaught()
        {
            Debug.Log("[GAME] Ball Caught → Continue");

            playerController.PowerBar.Reset();
            playerController.PowerBarView.Hide();
            playerController.StateMachine.ChangeState(PlayerState.Move);

            StartCoroutine(DelayedResetSequence());
        }


        private void OnBallMissed()
        {
            Debug.Log("[GAME] Ball Missed → RESTARTING SCENE");

            // *** The scene reload is now safe because the miss is triggered correctly. ***
            SceneManager.LoadScene(0);
        }

        private System.Collections.IEnumerator DelayedResetSequence()
        {
            yield return new WaitForSeconds(resetDelayTime);

            Debug.Log("[GAME] Resetting Positions...");

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

            playerController.StateMachine.ChangeState(PlayerState.Move);

            playerController.SetSpeedMultiplier(1f);
            playerController.SetMovementEnabled(true);
            playerController.PowerBarView.Hide();
            playerController.View.transform.localScale = playerController.InitialScale;

            ballController.ResetBall(ballStartPos);

            StartCoroutine(ReenableKickZoneSequence());
        }

        private System.Collections.IEnumerator ReenableKickZoneSequence()
        {
            yield return null;

            Debug.Log("[GAME] KickZone Re-enabled.");
            ballController.SetKickZoneActive(true);
        }
    }
}
