using KNC.Utilities;
using UnityEngine;

namespace KNC.Core.Services
{
    public class UIService : GenericMonoSingleton<UIService>
    {
        [SerializeField] private KNC.UI.MainMenuUIView mainMenuView;
        [SerializeField] private KNC.UI.GameplayUIView gameplayView;
        [SerializeField] private KNC.UI.GameOverUIView gameOverView;
        [SerializeField] private KNC.PowerBar.PowerBarView powerBarView;

        public KNC.PowerBar.PowerBarView PowerBarView => powerBarView;

        private KNC.UI.MainMenuUIController mainMenuController;
        private KNC.UI.GameplayUIController gameplayController;
        private KNC.UI.GameOverUIController gameOverController;

        protected override void Awake()
        {
            base.Awake();

            mainMenuController = new(mainMenuView);
            gameplayController = new(gameplayView, this);
            gameOverController = new(gameOverView);

            var es = EventService.Instance;
            es.OnBallCaught.AddListener(gameplayController.OnBallCaught);
            es.OnGameOver.AddListener(ShowGameOver);
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

        public void HideGameplayUI() => gameplayController.Hide();
    }
}
