using UnityEngine;
using TMPro;
using KNC.Core.Services;

namespace KNC.UI
{
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreText;

        private void Start()
        {
            if (ScoreService.Instance == null || EventService.Instance == null)
                return;

            EventService.Instance.OnHighScoreChanged.AddListener(UpdateHighScore);
            UpdateHighScore(ScoreService.Instance.HighScore);
        }

        private void OnDestroy()
        {
            if (EventService.Instance != null)
                EventService.Instance.OnHighScoreChanged.RemoveListener(UpdateHighScore);
        }

        private void UpdateHighScore(int value)
        {
            highScoreText.text = $"Total Catch: {value}";
        }
    }
}