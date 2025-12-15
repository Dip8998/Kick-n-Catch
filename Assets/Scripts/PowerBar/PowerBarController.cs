using System;
using UnityEngine;

namespace KNC.PowerBar
{
    public class PowerBarController
    {
        private readonly PowerBarScriptableObject so;
        private float currentCharge;
        private bool isCharging;
        public float MaxCharge => so.MaxCharge;

        public bool IsCharging => isCharging;

        public event Action<float> OnChargeChanged;
        public event Action<float> OnReleased;

        public PowerBarController(PowerBarScriptableObject so)
        {
            this.so = so;
        }

        public void StartCharge()
        {
            currentCharge = 0f;
            isCharging = true;
            OnChargeChanged?.Invoke(currentCharge);
        }

        public void Update(float dt)
        {
            if (!isCharging) return;

            currentCharge += so.ChargeSpeed * dt;
            currentCharge = Mathf.Clamp(currentCharge, 0f, so.MaxCharge);
            OnChargeChanged?.Invoke(currentCharge);
        }

        public float Release()
        {
            if (!isCharging) return 0f;

            isCharging = false;
            float value = Mathf.Max(currentCharge, so.MinCharge);
            OnReleased?.Invoke(value);
            return value;
        }

        public void Reset()
        {
            currentCharge = 0f;
            isCharging = false;
            OnChargeChanged?.Invoke(0f);
        }

    }
}
