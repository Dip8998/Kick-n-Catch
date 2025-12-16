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
            if (ScoreService.Instance != null)
                UpdateScore(ScoreService.Instance.Score);
        }

        private void Start()
        {
            if (ScoreService.Instance == null || EventService.Instance == null)
            {
                enabled = false;
                return;
            }

            EventService.Instance.OnScoreChanged.AddListener(UpdateScore);
            UpdateScore(ScoreService.Instance.Score);
        }

        private void OnDestroy()
        {
            if (EventService.Instance != null)
                EventService.Instance.OnScoreChanged.RemoveListener(UpdateScore);
        }

        private void UpdateScore(int value)
        {
            scoreText.text = "Catch: " + value.ToString();
        }
    }
}