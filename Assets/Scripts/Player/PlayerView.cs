using UnityEngine;
using KNC.PowerBar;
using KNC.Core.Services;

namespace KNC.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private BasketView catchBasket;

        private PlayerController controller;
        private Rigidbody2D rb;

        public Rigidbody2D Rigidbody => rb;
        public PowerBarView PowerBarView => UIService.Instance.PowerBarView;    
        public PlayerController Controller => controller;

        public void InitializeView(PlayerController controller)
        {
            this.controller = controller;
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 1f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            catchBasket.Initialize(controller);
        }

        private void Update()
        {
            controller.ReadInput();
            controller.Tick();
        }
    }
}
