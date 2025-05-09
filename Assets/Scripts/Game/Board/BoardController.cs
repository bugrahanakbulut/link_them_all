using System.Collections.Generic;
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
    }
    
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private BackgroundPool _bgTilePool;
        [SerializeField] private BoardTilePool _boardTilePool;
        [SerializeField] private BoardTileConfigs _configs;
        [SerializeField] private InputController _inputController;
        
        private List<ETileType> _tiles;
        private BoardTile[] _boardTiles;
        
        private Vector2Int _boardDimensions;
        public Vector2Int Dimensions => _boardDimensions;

        public LinkController LinkController {get; private set;}

        public void InitializeBoard(Vector2Int dimensions, List<ETileType> tiles)
        {
            _tiles = tiles;
            _boardDimensions = dimensions;

            LinkController = new LinkController(_inputController, this);
        }

        public IServiceTask DrawBoardBackgroundTask()
        {
            return new DrawBoardBackgroundTask(_boardDimensions, _bgTilePool);
        }

        public IServiceTask DrawTiles()
        {
            _boardTiles = new BoardTile[_boardDimensions.x * _boardDimensions.y];
            return new DrawBoardTilesTask(_tiles.AsReadOnly(), _boardTiles, _boardDimensions, _boardTilePool, _configs);
        }

        public IServiceTask AdjustCamera()
        {
            return new AdjustCameraTask(_boardDimensions);
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
            int index = BoardUtils.GetIndexByBoardPos(boardPos.x, boardPos.y, _boardDimensions);
            return GetTileByIndex(index);
        }
        
        private void OnDestroy()
        {
            _bgTilePool.Dispose();
            _boardTilePool.Dispose();

            LinkController = null;
        }
    }
}