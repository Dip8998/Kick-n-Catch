using UnityEngine;

namespace KNC.Ball
{
    [CreateAssetMenu(fileName = "BallSO", menuName = "SO/BallSO")]
    public class BallScriptableObject : ScriptableObject
    {
        public BallView BallPrefab;
        public Vector3 BallSpawnPos;
    }
}
