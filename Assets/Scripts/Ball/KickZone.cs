using UnityEngine;
using KNC.Player;

namespace KNC.Ball
{
    public class KickZone : MonoBehaviour
    {
        private BallController ball;

        public void Initialize(BallController b) => ball = b;

        private void OnTriggerEnter2D(Collider2D c)
        {
            if (ball.IsResolving)
                return;

            c.GetComponent<PlayerView>()?.Controller.OnEnterKickZone();
        }

        private void OnTriggerExit2D(Collider2D c)
        {
            c.GetComponent<PlayerView>()?.Controller.OnExitKickZone();
        }
    }
}
