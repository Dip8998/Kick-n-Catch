using UnityEngine;
using KNC.Utilities;

namespace KNC.Core.Services
{
    public class InputService : GenericMonoSingleton<InputService>
    {
        private float horizontal;

        public float Horizontal => horizontal;

        public void PressLeft() => horizontal = -1f;
        public void PressRight() => horizontal = 1f;
        public void Release() => horizontal = 0f;
    }
}
