using UnityEngine;
using UnityEngine.UI;

namespace KNC.PowerBar
{
    public class PowerBarView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private float maxCharge;

        public void Bind(PowerBarController controller)
        {
            maxCharge = controller.MaxCharge;

            controller.OnChargeChanged += OnChargeChanged;
            controller.OnReleased += _ => Hide();
        }

        private void OnChargeChanged(float charge)
        {
            float t = Mathf.Clamp01(charge / maxCharge);

            fillImage.fillAmount = t;

            if (t < 0.5f)
                fillImage.color = Color.Lerp(Color.green, Color.yellow, t * 2f);
            else
                fillImage.color = Color.Lerp(Color.yellow, Color.red, (t - 0.5f) * 2f);
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
