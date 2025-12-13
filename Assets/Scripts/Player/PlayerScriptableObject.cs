using UnityEngine;

namespace KNC.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "SO/PlayerSO")]
    public class PlayerScriptableObject : ScriptableObject
    {
        [Header("PlayerPrefab & Position")]
        public PlayerView PlayerPrefab;
        public Vector3 PlayerSpawnPos;

        [Header("Movement")]
        public float PlayerMoveSpeed;

        [Header("Horizontal Bounds")]
        public float MinX = -2.5f;
        public float MaxX = 2.5f;
    }
}
