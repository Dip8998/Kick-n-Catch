using UnityEngine;

namespace KNC.UI
{
    public class GameplayUIView : MonoBehaviour, IUIView
    {
        [SerializeField] private GameObject goodCatchText;

        private GameplayUIController controller;

        public void SetController(IUIController controller)
        {
            this.controller = controller as GameplayUIController;
        }

        public void EnableView() => gameObject.SetActive(true);
        public void DisableView() => gameObject.SetActive(false);

        public void ShowGoodCatch()
        {
            goodCatchText.SetActive(true);
        }

        public void HideGoodCatch()
        {
            goodCatchText.SetActive(false);
        }
    }
}
