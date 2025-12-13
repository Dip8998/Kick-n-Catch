using UnityEngine;

namespace KNC.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        private PlayerController controller;
        private Rigidbody2D rb;

        public Rigidbody2D Rigidbody => rb;

        public void InitializeView(PlayerController controller)
        {
            this.controller = controller;
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 1f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Update()
        {
            controller?.ReadInput();
        }

        private void FixedUpdate()
        {
            controller?.FixedTick(Time.fixedDeltaTime); 
        }
    }
}
