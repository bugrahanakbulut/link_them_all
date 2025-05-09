using System;
using LinkThemAll.Game.Board;
using UnityEngine;

namespace LinkThemAll.Game.Level
{
    public class LevelController : IDisposable
    {
        public BoardController Board { get; private set; }
        
        private int _currentLevel;
        private readonly LevelConfigs _levelConfigs;
        
        private const string LEVEL_KEY = "player_cur_level";
        
        public LevelController(LevelConfigs levelConfigs)
        {
            _levelConfigs = levelConfigs;
            
            LoadPlayerLevel();
        }

        public void SetBoard(BoardController board)
        {
            Board = board;
        }

        public LevelConfig GetCurrentLevelConfig()
        {
            return _levelConfigs.TryGetLevelConfig(_currentLevel);
        }
        
        public void Dispose()
        {
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

        public void LoadCurrentLevel()
        {
            LevelConfig config = GetCurrentLevelConfig();

            Board.Initialize(config);
        }
    }
}