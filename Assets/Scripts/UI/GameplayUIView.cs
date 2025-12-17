using UnityEngine;
using TMPro;

namespace KNC.UI
{
    public class GameplayUIView : MonoBehaviour, IUIView
    {
        private GameplayUIController controller;

        [SerializeField] private GameObject goodCatchText;

        public void SetController(IUIController controller)
            => this.controller = controller as GameplayUIController;

        public void EnableView() => gameObject.SetActive(true);
        public void DisableView() => gameObject.SetActive(false);

        public void ShowGoodCatch()
        {
            goodCatchText.SetActive(true);
            controller.HideGoodCatchDelayed();
        }

        public void HideGoodCatch() => goodCatchText.SetActive(false);
    }
}
