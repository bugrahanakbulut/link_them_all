using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class DropTilesTask : IServiceTask
    {
        private readonly BoardController _boardController;
        private readonly BoardTilePool _boardTilePool;
        private readonly BoardTileConfigs _configs;

        private const float TILE_MOVE_DURATION = 0.18f;
        private const float TILE_FADE_IN_DURATION = 0.18f;
        
        public DropTilesTask(BoardController boardController, BoardTilePool boardTilePool, BoardTileConfigs configs)
        {
            _boardController = boardController;
            _boardTilePool = boardTilePool;
            _configs = configs;
        }

        public async UniTask Execute()
        {
            Vector2Int boardPos = Vector2Int.zero;
            Vector2Int dimensions = _boardController.Dimensions;

            Sequence sequence = DOTween.Sequence();
            sequence.SetId(_boardController);

            for (int y = 1; y < dimensions.y; ++y)
            {
                for (int x = 0; x < dimensions.x; ++x)
                {
                    boardPos.x = x;
                    boardPos.y = y;

                    BoardTile tile = _boardController.GetTileByBoardPos(boardPos);

                    if (tile == null)
                    {
                        continue;
                    }

                    DropTile(tile, sequence);
                }
            }

            float delay = 0;
            
            for (int y = 0; y < dimensions.y; ++y)
            {
                bool spawned = false;
                
                for (int x = 0; x < dimensions.x; ++x)
                {
                    boardPos.x = x;
                    boardPos.y = y;

                    BoardTile tile = _boardController.GetTileByBoardPos(boardPos);

                    if (tile != null)
                    {
                        continue;
                    }

                    GenerateTileAt(x, y, sequence, delay);

                    spawned = true;
                }

                if (spawned)
                {
                    delay += TILE_MOVE_DURATION;
                }
            }

            await sequence;
        }

        private void DropTile(BoardTile tile, Sequence sequence)
        {
            Vector2Int boardPos = tile.BoardPos;
            Vector2Int dropPos;
            
            int dropCount = 0;

            for (int i = boardPos.y-1; i >= 0; --i)
            {
                dropPos = new Vector2Int(boardPos.x, i);
                BoardTile tileBelow = _boardController.GetTileByBoardPos(dropPos);

                if (tileBelow == null)
                {
                    dropCount++;
                }
                else
                {
                    break;
                }
            }

            if (dropCount == 0)
            {
                return;
            }
            
            dropPos = new Vector2Int(boardPos.x, boardPos.y - dropCount);

            _boardController.SwapTiles(boardPos, dropPos);

            sequence.Join(tile.MoveTo(BoardUtils.BoardPosToWorldPos(dropPos.x, dropPos.y), TILE_MOVE_DURATION * dropCount).SetEase(Ease.OutCubic));
        }

        private void GenerateTileAt(int x, int y, Sequence sequence, float delay)
        {
            ETileType tileType = (ETileType)Random.Range(0, 3);
            Vector2Int generatedTilePos = new Vector2Int(x, y);
            int dropAmount = _boardController.Dimensions.y + 1 - y;
                    
            BoardTile newTile = _boardTilePool.GetTile();
            newTile.SetActive(true);
            newTile.Initialize(tileType, _configs.GetTileSprite(tileType), new Vector2Int(x, y));
            newTile.SetPosition(BoardUtils.BoardPosToWorldPos(generatedTilePos.x, _boardController.Dimensions.y + 1));
                    
            sequence.Insert(0, newTile.FadeIn(TILE_FADE_IN_DURATION).SetDelay(delay));
            sequence.Insert(0, newTile.MoveTo(BoardUtils.BoardPosToWorldPos(generatedTilePos.x, generatedTilePos.y), TILE_MOVE_DURATION * dropAmount).SetEase(Ease.OutCubic).SetDelay(delay, false));
                    
            _boardController.TileGenerated(newTile, generatedTilePos);
        }
    }
}