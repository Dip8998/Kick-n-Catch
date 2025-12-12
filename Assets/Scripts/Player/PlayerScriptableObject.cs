using UnityEngine;

namespace KNC.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "SO/PlayerSO")]
    public class PlayerScriptableObject : ScriptableObject
    {
        public PlayerView PlayerPrefab;
        public Vector3 PlayerSpawnPos;
    }
}
