using UnityEngine;

namespace KNC.Ramp
{
    [CreateAssetMenu(fileName = "RampSO", menuName = "SO/RampSO")]
    public class RampScriptableObject : ScriptableObject
    {
        public RampView RampPrefab;
    }
}
