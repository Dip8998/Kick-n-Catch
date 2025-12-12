using UnityEngine;

namespace KNC.Ball
{
    public class BallView : MonoBehaviour
    {
        private BallController controller;

        public void InitializeView(BallController controller)
        {
            this.controller = controller;
        }
    }
}
