using UnityEngine;
using KNC.PowerBar;
using KNC.Utilities;

namespace KNC.Core.Services
{
    public class UIService : GenericMonoSingleton<UIService>
    {
        [SerializeField] private PowerBarView powerBarView;

        public PowerBarView PowerBarView => powerBarView;
    }
}
