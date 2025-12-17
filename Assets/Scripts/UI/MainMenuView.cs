using UnityEngine;
using UnityEngine.UI;

namespace KNC.UI
{
    public class MainMenuUIView : MonoBehaviour, IUIView
    {
        private MainMenuUIController controller;

        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(() => controller.OnPlayClicked());
            quitButton.onClick.AddListener(() => controller.OnQuitClicked());
        }

        public void SetController(IUIController controller)
            => this.controller = controller as MainMenuUIController;

        public void EnableView() => gameObject.SetActive(true);
        public void DisableView() => gameObject.SetActive(false);
    }
}
