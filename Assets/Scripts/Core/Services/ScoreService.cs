using KNC.Utilities;
using System;
using UnityEngine;

namespace KNC.Core.Services
{
    public class ScoreService : GenericMonoSingleton<ScoreService>
    {
        private int score;

        public int Score => score;

        public event Action<int> OnScoreChanged;

        public void ResetScore()
        {
            score = 0;
            OnScoreChanged?.Invoke(score);
        }

        public void AddScore(int amount)
        {
            score += amount;
            Debug.Log("[SCORE] New Score = " + score);
            OnScoreChanged?.Invoke(score);
        }
    }
}
