using KNC.Utilities;

namespace KNC.Core.Services
{
    public class InputService : GenericMonoSingleton<InputService>
    {
        private float horizontal;
        private bool kickHeld;
        private bool kickReleased;

        public float Horizontal => horizontal;
        public bool KickHeld => kickHeld;
        public bool KickReleased => kickReleased;

        public void PressLeft() => horizontal = -1f;
        public void PressRight() => horizontal = 1f;
        public void ReleaseMove() => horizontal = 0f;

        public void KickDown()
        {
            kickHeld = true;
            kickReleased = false;
        }

        public void KickUp()
        {
            kickHeld = false;
            kickReleased = true;
        }

        public void ResetKickRelease() => kickReleased = false;
    }
}
