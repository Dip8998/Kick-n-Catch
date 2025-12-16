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
            if (ScoreService.Instance == null)
                return;

            ScoreService.Instance.OnHighScoreChanged += UpdateHighScore;
            UpdateHighScore(ScoreService.Instance.HighScore);
        }

        private void OnDestroy()
        {
            if (ScoreService.Instance != null)
                ScoreService.Instance.OnHighScoreChanged -= UpdateHighScore;
        }

        private void UpdateHighScore(int value)
        {
            highScoreText.text = $"Total Catch: {value}";
        }
    }
}
