using KNC.Utilities;
using System;
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

        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;

        protected override void Awake()
        {
            base.Awake();
            LoadHighScore();
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        public void ResetScore()
        {
            score = 0;
            OnScoreChanged?.Invoke(score);
        }

        public void AddScore(int amount)
        {
            score += amount;
            OnScoreChanged?.Invoke(score);

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
                PlayerPrefs.Save();

                OnHighScoreChanged?.Invoke(highScore);
            }
        }

        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}
