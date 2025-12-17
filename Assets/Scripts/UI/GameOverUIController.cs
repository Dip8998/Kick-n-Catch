using System.Collections;
using UnityEngine;
using KNC.Core.Services;

namespace KNC.UI
{
    public class GameOverUIController : IUIController
    {
        private readonly GameOverUIView view;

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

        private IEnumerator ShowGameplayNextFrame()
        {
            yield return new WaitForSeconds(0.7f);
            ScoreService.Instance.ResetScore();
            EventService.Instance.RaiseGameReset();
            UIService.Instance.ShowGameplayUI();
        }

        public void OnHome()
        {
            Hide();

            Time.timeScale = 1f;

            ScoreService.Instance.ResetScore();
            EventService.Instance.RaiseGameReset();

            UIService.Instance.ShowMainMenu();
        }
    }
}
