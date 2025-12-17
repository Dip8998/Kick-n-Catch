using UnityEngine;
using KNC.PowerBar;

namespace KNC.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "SO/PlayerSO")]
    public class PlayerScriptableObject : ScriptableObject
    {
        public PlayerView PlayerPrefab;
        public Vector3 PlayerSpawnPos;

        public float PlayerMoveSpeed;
        public float MinX = -2.5f;
        public float MaxX = 2.5f;

        public PowerBarScriptableObject PowerBarSO;
    }
}
