using System;
using LinkThemAll.Services;

namespace LinkThemAll.Game
{
    public interface IScoreService : IService
    {
        int CurrentScore { get; }
        Action OnScoreUpdated { get; set; }
        void IncreaseScore();
        void Reset();
    }
    
    public class ScoreService : IScoreService
    {
        public int CurrentScore { get; private set; }
        
        public Action OnScoreUpdated { get; set; }

        public void IncreaseScore()
        {
            CurrentScore++;
            OnScoreUpdated?.Invoke();
        }

        public void Reset()
        {
            CurrentScore = 0;
            OnScoreUpdated?.Invoke();
        }
        
        public void Dispose()
        {
            OnScoreUpdated = null;
        }
    }
}