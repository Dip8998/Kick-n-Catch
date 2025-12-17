using KNC.Utilities;
using UnityEngine;

namespace KNC.Core.Services
{
    public class ScoreService : GenericMonoSingleton<ScoreService>
    {
        private const string HIGH_SCORE_KEY = "HIGH_SCORE";

        private int score;
        private int highScore;

        public int Score => score;
        public int HighScore => highScore;

        protected override void Awake()
        {
            base.Awake();
            highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        public void ResetScore()
        {
            score = 0;
            EventService.Instance.RaiseScoreChanged(score);
        }

        public void AddScore(int amount)
        {
            score += amount;
            EventService.Instance.RaiseScoreChanged(score);

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
                PlayerPrefs.Save();
                EventService.Instance.RaiseHighScoreChanged(highScore);
            }
        }
    }
}
