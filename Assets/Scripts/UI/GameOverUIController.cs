using KNC.Core.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KNC.UI
{
    public class GameOverUIController : IUIController
    {
        private GameOverUIView view;

        public GameOverUIController(GameOverUIView view)
        {
            this.view = view;
            view.SetController(this);
            Hide();
        }

        public void Show()
        {
            Time.timeScale = 0f;
            view.EnableView();
        }

        public void Hide()
        {
            view.DisableView();
        }

        public void OnReload()
        {
            UIService.Instance.HideGameplayUI();

            Time.timeScale = 1f;

            view.StartCoroutine(ShowGameplayNextFrame());
        }

        private System.Collections.IEnumerator ShowGameplayNextFrame()
        {
            yield return new WaitForSeconds(.85f); 
            UIService.Instance.ShowGameplayUI();
            ScoreService.Instance.ResetScore();
            EventService.Instance.RaiseGameReset();
        }


        public void OnHome()
        {
            Hide();
            UIService.Instance.ShowMainMenu();
        }
    }
}
