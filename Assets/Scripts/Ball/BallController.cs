using UnityEngine;

namespace KNC.Ball
{
    public class BallController
    {
        private BallView view;
        private readonly BallScriptableObject so;
        private Transform parent;

        public BallController(BallScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = CreateParent("_Ball");
            view = GameObject.Instantiate(so.BallPrefab, parent);
            view.transform.position = so.BallSpawnPos;
            view.InitializeView(this);
        }

        private Transform CreateParent(string name)
        {
            return new GameObject(name).transform;
        }
    }
}
