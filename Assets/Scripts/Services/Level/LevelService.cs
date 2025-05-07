using UnityEngine;

namespace LinkThemAll.Services.Level
{
    public class LevelService : ILevelService
    {
        private int _currentLevel;
        
        private const string LEVEL_KEY = "player_cur_level";
        
        public LevelService()
        {
            LoadPlayerLevel();
        }

        private void LoadPlayerLevel()
        {
            if (!PlayerPrefs.HasKey(LEVEL_KEY))
            {
                _currentLevel = 0;
                return;
            }

            _currentLevel = PlayerPrefs.GetInt(LEVEL_KEY);
        }

        public void Dispose()
        {
        }
    }
}