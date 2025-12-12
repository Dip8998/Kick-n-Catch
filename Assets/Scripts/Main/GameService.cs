using KNC.Ball;
using KNC.Player;
using KNC.Ramp;
using KNC.Utilities;
using UnityEngine;

namespace KNC.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        private RampController rampController;
        private PlayerController playerController;
        private BallController ballController;

        [SerializeField] private RampScriptableObject rampScriptableObject;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private BallScriptableObject ballScriptableObject;

        protected override void Awake()
        {
            base.Awake();

            rampController = new RampController(rampScriptableObject);
            rampController.Initialize();

            playerController = new PlayerController(playerScriptableObject);
            playerController.Initialize();

            ballController = new BallController(ballScriptableObject);
            ballController.Initialize();
        }
    }
}
