using UnityEngine;

namespace KNC.Ramp
{
    public class RampController
    {
        private RampView view;
        private readonly RampScriptableObject so;

        private Transform parent;

        public RampController(RampScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = CreateParent("_Ramp");
            view = GameObject.Instantiate(so.RampPrefab, parent);
            view.InitializeView(this);
        }

        private Transform CreateParent(string name)
        {
            var go = new GameObject(name);
            return go.transform;
        }
    }
}
