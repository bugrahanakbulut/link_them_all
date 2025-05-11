using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class DrawBoardTilesTask : IServiceTask
    {
        private Vector2Int _boardDimensions;
        private readonly BoardTilePool _boardTilePool;
        private readonly BoardTileConfigs _configs;
        
        private BoardTile[] _boardTiles;
        
        public DrawBoardTilesTask(
            BoardTile[] boardTiles,
            Vector2Int boardDimensions,
            BoardTilePool boardTilePool,
            BoardTileConfigs configs)
        {
            _boardDimensions = boardDimensions;
            _boardTilePool = boardTilePool;
            _configs = configs;
            _boardTiles = boardTiles;
        }

        public UniTask Execute()
        {
            for (int y = 0; y < _boardDimensions.y; ++y)
            {
                for (int x = 0; x < _boardDimensions.x; ++x)
                {
                    int index = BoardUtils.BoardPosToIndex(x, y, _boardDimensions);
                    
                    BoardTile tile = _boardTilePool.GetTile();
                    ETileType tileType = (ETileType) Random.Range(0, (int)ETileType.Tile_Count);
                    
                    tile.Initialize(tileType, _configs.GetTileSprite(tileType), new Vector2Int(x, y));
                    tile.SetPosition(BoardUtils.BoardPosToWorldPos(x, y));
                    tile.SetActive(true);
                    
                    _boardTiles[index] = tile;
                }
            }
            
            return UniTask.CompletedTask;
        }
    }
}