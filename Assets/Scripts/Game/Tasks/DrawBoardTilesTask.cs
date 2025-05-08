using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class DrawBoardTilesTask : IServiceTask
    {
        public IReadOnlyList<ETileType> _tiles;
        private Vector2Int _boardDimensions;
        private readonly BoardTilePool _boardTilePool;
        private readonly BoardTileConfigs _configs;

        public DrawBoardTilesTask(IReadOnlyList<ETileType> tiles, Vector2Int boardDimensions, BoardTilePool boardTilePool, BoardTileConfigs configs)
        {
            _tiles = tiles;
            _boardDimensions = boardDimensions;
            _boardTilePool = boardTilePool;
            _configs = configs;
        }

        public UniTask Execute()
        {
            for (int y = 0; y < _boardDimensions.y; ++y)
            {
                for (int x = 0; x < _boardDimensions.x; ++x)
                {
                    BoardTile tile = _boardTilePool.GetTile();
                    ETileType tileType = _tiles[BoardUtils.GetIndexByBoardPos(x, y, _boardDimensions)];
                    
                    tile.Initialize(
                        tileType,
                        _configs.GetTileSprite(tileType),
                        new Vector2Int(x, y));
                    tile.SetPosition(BoardUtils.GetWorldPosByBoardPos(x, y));
                    tile.SetActive(true);
                }
            }
            
            return UniTask.CompletedTask;
        }
    }
}