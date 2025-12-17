using KNC.Core.Services;
using UnityEngine;

namespace KNC.UI
{
    public class MainMenuUIController : IUIController
    {
        private MainMenuUIView view;

        public MainMenuUIController(MainMenuUIView view)
        {
            this.view = view;
            view.SetController(this);
            Show();
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

        public void OnPlayClicked()
        {
            Hide();
            UIService.Instance.ShowGameplayUI();
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
