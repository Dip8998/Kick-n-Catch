using UnityEngine;
using UnityEngine.UI;

namespace KNC.UI
{
    public class GameOverUIView : MonoBehaviour, IUIView
    {
        [SerializeField] private Button reloadButton;
        [SerializeField] private Button homeButton;

        private GameOverUIController controller;

        private void Awake()
        {
            reloadButton.onClick.AddListener(() => controller.OnReload());
            homeButton.onClick.AddListener(() => controller.OnHome());
        }

        public void SetController(IUIController controller)
        {
            this.controller = controller as GameOverUIController;
        }

        public void EnableView() => gameObject.SetActive(true);
        public void DisableView() => gameObject.SetActive(false);
    }
}
