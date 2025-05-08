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
        
        private ETileType[] _tiles;
        
        private Vector2Int _boardDimensions;

        public void InitializeBoard(Vector2Int dimensions, ETileType[] tiles)
        {
            _tiles = tiles;
            _boardDimensions = dimensions;
        }

        public IServiceTask DrawBoardBackgroundTask()
        {
            return new DrawBoardBackgroundTask(_boardDimensions, _bgTilePool);
        }

        public IServiceTask DrawTiles()
        {
            return new DrawBoardTilesTask(_tiles, _boardDimensions, _boardTilePool, _configs);
        }

        public IServiceTask AdjustCamera()
        {
            return new AdjustCameraTask(_boardDimensions);
        }
        
        private void OnDestroy()
        {
            _bgTilePool.Dispose();
        }
    }
}