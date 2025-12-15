using UnityEngine;

namespace KNC.Ball
{
    public class BallView : MonoBehaviour
    {
        private BallController controller;
        public BallController Controller => controller;

        public void InitializeView(BallController controller)
        {
            this.controller = controller;
        }

        private void Update()
        {
            controller.Tick();
        }

        private void FixedUpdate()
        {
            controller.FixedTick();
        }

        private void OnCollisionEnter2D(Collision2D c)
        {
            if (c.collider.CompareTag("Ramp"))
                controller.OnGroundContact(true);
        }

        private void OnCollisionExit2D(Collision2D c)
        {
            if (c.collider.CompareTag("Ramp"))
                controller.OnGroundContact(false);
        }

    }
}
