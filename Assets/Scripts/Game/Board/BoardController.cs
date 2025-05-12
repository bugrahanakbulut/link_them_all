using System;
using System.Collections.Generic;
using DG.Tweening;
using LinkThemAll.Common.Tasks;
using LinkThemAll.Game.Level;
using UnityEngine;
using LinkThemAll.Game.Tile;
using LinkThemAll.Game.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Game.Board
{
    public static class BoardConstants
    {
        public const float TILE_WIDTH = 2.56f;
        public const float TILE_HEIGHT = 2.56f;

        public const int MIN_BOARD_WIDTH = 3;
        public const int MIN_BOARD_HEIGHT = 3;

        public const int MIN_LINK_LENGTH = 3;
    }
    
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private BackgroundPool _bgTilePool;
        [SerializeField] private BoardTilePool _boardTilePool;
        [SerializeField] private BoardTileConfigs _configs;
        [SerializeField] private InputController _inputController;
        [SerializeField] private LinkDrawer _linkDrawer;
        
        private BoardTile[] _boardTiles;
        public IReadOnlyList<BoardTile> BoardTiles => _boardTiles;
        
        public bool Freezed { get; private set; }
        public bool NeedToShuffle { get; private set; } public Vector2Int Dimensions { get; private set; }

        private TaskRunner _taskRunner;
        private LinkController _linkController;
        
        public Action<int, ETileType> OnTilesLinked { get; set; }
        
        public void Initialize(LevelConfig config)
        {
            _taskRunner = new TaskRunner();
            _taskRunner.AddTask(new InitializeBoardTask(this, config));
            _taskRunner.AddTask(new DrawBoardBackgroundTask(Dimensions, _bgTilePool));
            _taskRunner.AddTask(new DrawBoardTilesTask(_boardTiles, Dimensions, _boardTilePool, _configs));
            _taskRunner.AddTask(new AdjustCameraTask(Dimensions));
            _taskRunner.AddTask(new ValidateBoardTask(this));
            _taskRunner.AddTask(new ShuffleBoardTask(this));
        }
        
        public void InitializeBoard(Vector2Int dimensions)
        {
            Dimensions = dimensions;
            _boardTiles = new BoardTile[Dimensions.x * Dimensions.y];
            
            _linkController = new LinkController(_inputController, this);
            _linkDrawer.Initialize(_linkController);
            
            _linkController.OnLinkCompleted += OnLinkCompleted;
        }

        public BoardTile GetTileByIndex(int index)
        {
            if (_boardTiles == null || index < 0 || index >= _boardTiles.Length)
            {
                return null;
            }

            return _boardTiles[index];
        }

        public BoardTile GetTileByBoardPos(Vector2Int boardPos)
        {
            if (boardPos.x < 0 || boardPos.x >= Dimensions.x ||
                boardPos.y < 0 || boardPos.y >= Dimensions.y)
            {
                return null;
            }
            
            int index = BoardUtils.BoardPosToIndex(boardPos.x, boardPos.y, Dimensions);
            return GetTileByIndex(index);
        }

        public void WaitForBoardFreed(Action action)
        {
            _taskRunner.AddTask(new ExecuteActionTask(() =>
            {
                action?.Invoke();
            }));
        }

        public void TilesLinked(IReadOnlyList<BoardTile> linkedTiles)
        {
            foreach (BoardTile tile in linkedTiles)
            {
                Vector2Int boardPos = tile.BoardPos;
                int index = BoardUtils.BoardPosToIndex(boardPos.x, boardPos.y, Dimensions);

                _boardTiles[index] = null;    
            }
            
            OnTilesLinked?.Invoke(linkedTiles.Count, linkedTiles[0].TileType);
        }

        public void SwapTiles(Vector2Int pos1, Vector2Int pos2)
        {
            int index1 = BoardUtils.BoardPosToIndex(pos1.x, pos1.y, Dimensions);
            int index2 = BoardUtils.BoardPosToIndex(pos2.x, pos2.y, Dimensions);
            
            BoardTile t1 = _boardTiles[index1];
            BoardTile t2 = _boardTiles[index2];

            _boardTiles[index1] = t2;
            _boardTiles[index2] = t1;

            if (t1 != null)
            {
                t1.UpdateBoardPos(pos2);
            }

            if (t2 != null)
            {
                t2.UpdateBoardPos(pos1);
            }
        }

        public void AddTile(BoardTile newTile, Vector2Int boardPos)
        {
            int index = BoardUtils.BoardPosToIndex(boardPos.x, boardPos.y, Dimensions);

            if (index < 0 || index >= _boardTiles.Length)
            {
                return;
            }
            
            _boardTiles[index] = newTile;
        }

        public void AddTile(BoardTile newTile, int index)
        {
            if (index < 0 || index >= _boardTiles.Length)
            {
                return;
            }
            
            _boardTiles[index] = newTile;
        }

        public void SetBoardNeedReshuffle(bool reshuffle)
        {
            NeedToShuffle = reshuffle;
        }

        public void Lock()
        {
            _inputController.Lock();
        }

        public void Freeze()
        {
            Freezed = true;
        }

        private void OnLinkCompleted()
        {
            IReadOnlyList<BoardTile> linkedTiles = _linkController.Link;

            _inputController.Lock(); 
            
            _taskRunner.AddTasks(
                new LinkTilesTask(this, linkedTiles, _boardTilePool),
                new DropTilesTask(this, _boardTilePool, _configs),
                new ValidateBoardTask(this),
                new ShuffleBoardTask(this),
                UnlockBoard());
        }

        private IServiceTask UnlockBoard()
        {
            return new ExecuteActionTask(() =>
            {
                if (Freezed)
                {
                    return;
                }
                
                _inputController.Unlock();
            });
        }

        private void OnDestroy()
        {
            _bgTilePool.Dispose();
            _boardTilePool.Dispose();

            _linkController.OnLinkCompleted -= OnLinkCompleted;
            _linkController.Dispose();
            _linkController = null;
            
            _taskRunner.Terminate();

            DOTween.Kill(this);
        }
    }
}