using System.Collections;
using UnityEngine;

namespace KNC.UI
{
    public class GameplayUIController : IUIController
    {
        private readonly GameplayUIView view;
        private readonly MonoBehaviour coroutineRunner;

        public GameplayUIController(GameplayUIView view, MonoBehaviour runner)
        {
            this.view = view;
            coroutineRunner = runner;
            view.SetController(this);
            Hide();
        }

        public void Show()
        {
            Time.timeScale = 1f;
            view.EnableView();
        }

        public void Hide()
        {
            view.DisableView();
        }

        public void OnBallCaught()
        {
            view.ShowGoodCatch();
            coroutineRunner.StartCoroutine(HideGoodCatchRoutine());
        }

        private IEnumerator HideGoodCatchRoutine()
        {
            yield return new WaitForSeconds(1.2f);
            view.HideGoodCatch();
        }
    }
}
