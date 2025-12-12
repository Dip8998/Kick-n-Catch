using UnityEngine;

namespace KNC.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerController controller;

        public void InitializeView(PlayerController controller)
        {
            this.controller = controller;
        }
    }
}
