using UnityEngine;
using TMPro;
using KNC.Core.Services;

namespace KNC.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            UpdateScore(ScoreService.Instance.Score);
        }

        private void Awake()
        {
            EventService.Instance.OnScoreChanged.AddListener(UpdateScore);
        }

        private void OnDestroy()
        {
            if (EventService.Instance != null)
                EventService.Instance.OnScoreChanged.RemoveListener(UpdateScore);
        }

        private void UpdateScore(int value)
        {
            scoreText.text = $"Catch: {value}";
        }
    }
}
