using UnityEngine;
using KNC.Player;

namespace KNC.Ball
{
    public class KickZone : MonoBehaviour
    {
        private BallController ball;

        public void Initialize(BallController controller)
        {
            ball = controller;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (ball.IsResolving)
                return;

            collider.GetComponent<PlayerView>()?.Controller.OnEnterKickZone();
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            collider.GetComponent<PlayerView>()?.Controller.OnExitKickZone();
        }
    }
}
