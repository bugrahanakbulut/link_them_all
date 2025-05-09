using System;
using System.Collections.Generic;
using DG.Tweening;
using LinkThemAll.Game.Level;
using UnityEngine;
using LinkThemAll.Game.Tile;
using LinkThemAll.Game.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Game.Board
{
    public static class BoardConstants
    {
        public const float TILE_WIDTH = 2.6f;
        public const float TILE_HEIGHT = 2.6f;

        public const int MIN_BOARD_WIDTH = 3;
        public const int MIN_BOARD_HEIGHT = 3;
    }
    
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private BackgroundPool _bgTilePool;
        [SerializeField] private BoardTilePool _boardTilePool;
        [SerializeField] private BoardTileConfigs _configs;
        [SerializeField] private InputController _inputController;
        [SerializeField] private LinkDrawer _linkDrawer;
        
        private List<ETileType> _tiles;
        private BoardTile[] _boardTiles;
        private IReadOnlyList<BoardTile> BoardTiles => _boardTiles;
        
        private Vector2Int _boardDimensions;
        public Vector2Int Dimensions => _boardDimensions;

        public LinkController LinkController { get; private set; }

        private TaskRunner _taskRunner;
        
        public Action<ETileType> OnTileLinked { get; set; }
        
        public void Initialize(LevelConfig config)
        {
            _taskRunner = new TaskRunner();
            _taskRunner.AddTask(new InitializeBoardTask(this, config));
            _taskRunner.AddTask(new DrawBoardBackgroundTask(_boardDimensions, _bgTilePool));
            _taskRunner.AddTask(new DrawBoardTilesTask(_tiles.AsReadOnly(), _boardTiles, _boardDimensions, _boardTilePool, _configs));
            _taskRunner.AddTask(new AdjustCameraTask(_boardDimensions));
        }
        
        public void InitializeBoard(Vector2Int dimensions, List<ETileType> tiles)
        {
            _tiles = tiles;
            _boardDimensions = dimensions;
            _boardTiles = new BoardTile[_boardDimensions.x * _boardDimensions.y];
            
            LinkController = new LinkController(_inputController, this);
            _linkDrawer.Initialize(LinkController);
            
            LinkController.OnLinkCompleted += OnLinkCompleted;
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
            
            int index = BoardUtils.GetIndexByBoardPos(boardPos.x, boardPos.y, _boardDimensions);
            return GetTileByIndex(index);
        }
        
        private void OnLinkCompleted()
        {
            IReadOnlyList<BoardTile> linkedTiles = LinkController.Link;
            
            _taskRunner.AddTask(new LinkTilesTask(this, linkedTiles, _boardTilePool));
            _taskRunner.AddTask(new DropTilesTask(this, _boardTilePool, linkedTiles));
        }

        public void TileLinked(BoardTile tile)
        {
            Vector2Int boardPos = tile.BoardPos;
            int index = BoardUtils.GetIndexByBoardPos(boardPos.x, boardPos.y, _boardDimensions);

            _boardTiles[index] = null;
            OnTileLinked?.Invoke(tile.TileType);
        }

        public void SwapTiles(Vector2Int pos1, Vector2Int pos2)
        {
            int index1 = BoardUtils.GetIndexByBoardPos(pos1.x, pos1.y, Dimensions);
            int index2 = BoardUtils.GetIndexByBoardPos(pos2.x, pos2.y, Dimensions);
            
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

        private void OnDestroy()
        {
            _bgTilePool.Dispose();
            _boardTilePool.Dispose();

            LinkController.OnLinkCompleted -= OnLinkCompleted;
            LinkController.Dispose();
            LinkController = null;

            DOTween.Kill(this);
        }
    }
}