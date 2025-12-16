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
            if (ScoreService.Instance == null)
            {
                Debug.LogError("ScoreService missing in scene");
                enabled = false;
                return;
            }

            ScoreService.Instance.OnScoreChanged += UpdateScore;
            UpdateScore(ScoreService.Instance.Score);
        }

        private void OnDestroy()
        {
            if (ScoreService.Instance != null)
                ScoreService.Instance.OnScoreChanged -= UpdateScore;
        }

        private void UpdateScore(int value)
        {
            scoreText.text = "Catch: " + value.ToString();
        }
    }
}
