using KNC.Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace KNC.Core.Services
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSound : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundService.Instance.Play(SoundService.Sounds.BUTTONCLICK);
            });
        }
    }
}
