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
        int GetLevelTargetScore();
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
        
        private readonly IScoreService _scoreService = ServiceProvider.Get<IScoreService>();
        
        private const string LEVEL_KEY = "player_cur_level";
        
        public Action OnMoveCountUpdated { get; set; }
        public Action OnLevelCompleted { get; set; }
        public Action OnLevelFailed { get; set; }
        
        public int LoadCurrentLevel()
        {
            LoadPlayerLevel();

            return _currentLevel;
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
            OnLevelFailed = null;
            OnLevelCompleted = null;
            OnMoveCountUpdated = null;
            
            if (Board == null)
            {
                return;
            }

            Board.OnTilesLinked -= OnTileLinked;
            Board = null;
        }

        public void LockBoard()
        {
            Board.Lock();
        }

        public void FreezeBoard()
        {
            Board.Freeze();
        }

        public void Reset()
        {
            if (Board != null)
            {
                Board.OnTilesLinked -= OnTileLinked;
            }
            
            _scoreService.Reset();
            RemainingMoveCount = 0;
        }

        public void InitializeLevel(LevelConfig config)
        {
            _currentLevelConfig = config;
            
            RemainingMoveCount = config.TargetMove;
            Board.Initialize(_currentLevelConfig);
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
                _currentLevel++;
                SaveLevel();
                
                Board.WaitForBoardFreed(() =>
                {
                    OnLevelCompleted?.Invoke();
                });
                return;
            }
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

        private void SaveLevel()
        {
            PlayerPrefs.SetInt(LEVEL_KEY, _currentLevel);
        }
    }
}