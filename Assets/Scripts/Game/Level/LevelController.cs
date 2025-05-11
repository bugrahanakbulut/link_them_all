using System;
using UnityEngine;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services;

namespace LinkThemAll.Game.Level
{
    public interface ILevelService : IService
    {
        int GetCurrentLevel();
        int GetLevelTarget();
    }
    
    public class LevelController : ILevelService, IDisposable
    {
        public BoardController Board { get; private set; }
        
        private int _currentLevel;
        private LevelConfig _currentLevelConfig;
        
        private readonly LevelConfigs _levelConfigs;
        private readonly IScoreService _scoreService = ServiceProvider.Get<IScoreService>();
        
        private const string LEVEL_KEY = "player_cur_level";
        
        public LevelController(LevelConfigs levelConfigs)
        {
            _levelConfigs = levelConfigs;
            
            LoadPlayerLevel();

            _currentLevelConfig = GetCurrentLevelConfig();
        }
        
        public void LoadCurrentLevel()
        {
            LevelConfig config = GetCurrentLevelConfig();

            Board.Initialize(config);
        }

        public void SetBoard(BoardController board)
        {
            Board = board;
            Board.OnTileLinked += OnTileLinked;
        }
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public int GetLevelTarget()
        {
            return _currentLevelConfig.TargetScore;
        }

        public void Dispose()
        {
            if (Board == null)
            {
                return;
            }

            Board.OnTileLinked -= OnTileLinked;
            Board = null;
        }

        private void OnTileLinked(ETileType tileType)
        {
            _scoreService.IncreaseScore();
        }

        private LevelConfig GetCurrentLevelConfig()
        {
            return _levelConfigs.TryGetLevelConfig(_currentLevel);
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
    }
}