using System.Collections;
using UnityEngine;

namespace KNC.UI
{
    public class GameplayUIController : IUIController
    {
        private GameplayUIView view;
        private MonoBehaviour coroutineRunner;

        public GameplayUIController(GameplayUIView view, MonoBehaviour runner)
        {
            this.view = view;
            this.coroutineRunner = runner;
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
        }

        public void HideGoodCatchDelayed()
        {
            coroutineRunner.StartCoroutine(HideGoodCatchRoutine());
        }

        private IEnumerator HideGoodCatchRoutine()
        {
            yield return new WaitForSeconds(1.2f);
            view.HideGoodCatch();
        }
    }
}
