using UnityEngine;

namespace KNC.Ramp
{
    public class RampView : MonoBehaviour
    {
        private RampController controller;

        public void InitializeView(RampController controller)
        {
            this.controller = controller;
        }
    }
}
