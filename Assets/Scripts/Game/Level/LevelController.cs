using System;
using UnityEngine;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services;

namespace LinkThemAll.Game.Level
{
    public interface ILevelService : IService
    {
        int RemainingMoveCount { get; }

        int GetCurrentLevel();
        int GetLevelTargetScore();
        
        Action OnMoveCountUpdated { get; set; }
    }

    public interface IMoveService : IService
    {
        int RemainingMoveCount { get; }
        Action OnMoveCountUpdated { get; set; }
    }
    
    public class LevelController : ILevelService, IMoveService
    {
        public BoardController Board { get; private set; }
        public int RemainingMoveCount { get; private set; }
        
        private int _currentLevel;
        private LevelConfig _currentLevelConfig;
        
        private readonly LevelConfigs _levelConfigs;
        private readonly IScoreService _scoreService = ServiceProvider.Get<IScoreService>();
        
        private const string LEVEL_KEY = "player_cur_level";
        
        public Action OnMoveCountUpdated { get; set; }
        public Action OnLevelCompleted { get; set; }
        public Action OnLevelFailed { get; set; }
        
        public LevelController(LevelConfigs levelConfigs)
        {
            _levelConfigs = levelConfigs;
            
            LoadPlayerLevel();

            _currentLevelConfig = GetCurrentLevelConfig();
            RemainingMoveCount = _currentLevelConfig.TargetMove;
        }
        
        public void LoadCurrentLevel()
        {
            LevelConfig config = GetCurrentLevelConfig();

            Board.Initialize(config);
        }

        public void SetBoard(BoardController board)
        {
            Board = board;
            Board.OnTilesLinked += OnTileLinked;
        }
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public int GetLevelTargetScore()
        {
            return _currentLevelConfig.TargetScore;
        }

        public void Dispose()
        {
            if (Board == null)
            {
                return;
            }

            Board.OnTilesLinked -= OnTileLinked;
            Board = null;
        }

        private void OnTileLinked(int linkLength, ETileType tileType)
        {
            _scoreService.IncreaseScore(linkLength);

            RemainingMoveCount--;
            
            OnMoveCountUpdated?.Invoke();

            CheckLevelEnd();
        }

        private void CheckLevelEnd()
        {
            if (RemainingMoveCount == 0 && _scoreService.CurrentScore < GetLevelTargetScore())
            {
                OnLevelFailed?.Invoke();
                return;
            }

            if (_scoreService.CurrentScore >= GetLevelTargetScore())
            {
                OnLevelCompleted?.Invoke();
                return;
            }
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