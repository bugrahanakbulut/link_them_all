using System.Collections.Generic;
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
        private readonly IReadOnlyList<BoardTile> _linkedTiles;
        
        private const float TILE_MOVE_DURATION = 0.18f;
        
        public DropTilesTask(BoardController boardController, BoardTilePool boardTilePool, IReadOnlyList<BoardTile> linkedTiles)
        {
            _boardController = boardController;
            _boardTilePool = boardTilePool;
            _linkedTiles = linkedTiles;
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
    }
}