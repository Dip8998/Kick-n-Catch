using UnityEngine;
using UnityEngine.UI;

namespace KNC.UI
{
    public class GameOverUIView : MonoBehaviour, IUIView
    {
        private GameOverUIController controller;

        [SerializeField] private Button reloadButton;
        [SerializeField] private Button homeButton;

        private void Start()
        {
            reloadButton.onClick.AddListener(controller.OnReload);
            homeButton.onClick.AddListener(controller.OnHome);
        }

        public void SetController(IUIController controller)
            => this.controller = controller as GameOverUIController;

        public void EnableView() => gameObject.SetActive(true);
        public void DisableView() => gameObject.SetActive(false);
    }
}
