using UnityEngine;

namespace KNC.PowerBar
{
    [CreateAssetMenu(fileName = "PowerBarSO", menuName = "SO/PowerBarSO")]
    public class PowerBarScriptableObject : ScriptableObject
    {
        public float ChargeSpeed = 1f;
        public float MaxCharge = 1f;
        public float MinCharge = 0.05f;
    }
}
