using UnityEngine;
using UnityEngine.UI;
using KNC.Core.Services;

namespace KNC.PowerBar
{
    public class PowerBarView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private float maxCharge;

        public void Bind(PowerBarController controller)
        {
            maxCharge = controller.MaxCharge;

            EventService.Instance.OnPowerChanged.AddListener(OnChargeChanged);
            EventService.Instance.OnPowerReleased.AddListener(Hide);
        }

        private void OnDestroy()
        {
            if (EventService.Instance != null)
            {
                EventService.Instance.OnPowerChanged.RemoveListener(OnChargeChanged);
                EventService.Instance.OnPowerReleased.RemoveListener(Hide);
            }
        }

        private void OnChargeChanged(float charge)
        {
            float t = Mathf.Clamp01(charge / maxCharge);

            fillImage.fillAmount = t;
        }

        public void Show()
        {
            fillImage.fillAmount = 0f;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}