using UnityEngine;
using KNC.Utilities;
using KNC.PowerBar;
using KNC.UI;

namespace KNC.Core.Services
{
    public class UIService : GenericMonoSingleton<UIService>
    {
        [Header("Views")]
        [SerializeField] private MainMenuUIView mainMenuView;
        [SerializeField] private GameplayUIView gameplayView;
        [SerializeField] private GameOverUIView gameOverView;
        [SerializeField] private PowerBarView powerBarView;

        public PowerBarView PowerBarView => powerBarView;
        private MainMenuUIController mainMenuController;
        private GameplayUIController gameplayController;
        private GameOverUIController gameOverController;

        protected override void Awake()
        {
            base.Awake();

            mainMenuController = new MainMenuUIController(mainMenuView);
            gameplayController = new GameplayUIController(gameplayView, this);
            gameOverController = new GameOverUIController(gameOverView);

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventService.Instance.OnBallCaught.AddListener(gameplayController.OnBallCaught);
            EventService.Instance.OnBallMissed.AddListener(gameOverController.Show);
            EventService.Instance.OnGameOver.AddListener(ShowGameOver);
        }

        public void ShowMainMenu()
        {
            gameplayController.Hide();
            gameOverController.Hide();
            mainMenuController.Show();
        }

        public void ShowGameplayUI()
        {
            mainMenuController.Hide();
            gameOverController.Hide();
            gameplayController.Show();
        }

        private void ShowGameOver()
        {
            gameplayController.Hide();
            gameOverController.Show();   
        }

        public void HideGameplayUI()
        {
            gameplayController.Hide();
        }
    }
}
