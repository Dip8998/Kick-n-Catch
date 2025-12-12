using UnityEngine;

namespace KNC.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private readonly PlayerScriptableObject so;
        private Transform parent;

        public PlayerController(PlayerScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = CreateParent("_Player");
            view = GameObject.Instantiate(so.PlayerPrefab, parent);
            view.transform.position = so.PlayerSpawnPos;
            view.InitializeView(this);
        }

        private Transform CreateParent(string name)
        {
            return new GameObject(name).transform;
        }
    }
}
