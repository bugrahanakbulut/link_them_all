using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class ShuffleBoardTask : IServiceTask
    {
        private readonly BoardController _boardController;

        private const float TILE_SHUFFLE_DURATION = 0.5f;

        public ShuffleBoardTask(BoardController boardController)
        {
            _boardController = boardController;
        }

        public UniTask Execute()
        {
            if (!_boardController.NeedToShuffle)
            {
                return UniTask.CompletedTask;
            }

            List<BoardTile> shuffledTiles = new List<BoardTile>(_boardController.BoardTiles);
            
            ShuffleBoard(shuffledTiles);

            InsertLinkIfNeeded(shuffledTiles);
            
            for (int index = 0, len = shuffledTiles.Count; index < len; index++)
            {
                BoardTile tile = shuffledTiles[index];
                Vector2Int newBoardPos = BoardUtils.IndexToBoardPos(index, _boardController.Dimensions);
                
                _boardController.AddTile(tile, index);
                
                tile.UpdateBoardPos(newBoardPos);
                tile.MoveTo(BoardUtils.BoardPosToWorldPos(newBoardPos.x, newBoardPos.y), TILE_SHUFFLE_DURATION).SetId(tile).SetEase(Ease.OutBack);
            }

            return UniTask.CompletedTask;
        }

        private void InsertLinkIfNeeded(List<BoardTile> tiles)
        {
            NativeArray<bool> result = new NativeArray<bool>(1, Allocator.Persistent);
            
            LinkFinder linkFinder = new LinkFinder(_boardController.Dimensions, tiles, result);
            
            linkFinder.Schedule().Complete();

            bool res = result[0];

            result.Dispose();
            
            if (res)
            {
                return;
            }

            ETileType linkTileType = FindTileTypeForLink(tiles);
            
            InsertLink(tiles, linkTileType);
        }

        private ETileType FindTileTypeForLink(List<BoardTile> currentTiles)
        {
            Dictionary<ETileType, int> counts = new Dictionary<ETileType, int>();

            foreach (BoardTile tile in currentTiles)
            {
                if (tile == null)
                {
                    continue;
                }

                ETileType tileType = tile.TileType;
                
                if (counts.ContainsKey(tileType))
                {
                    counts[tileType]++;
                }
                else
                {
                    counts.Add(tileType, 1);
                }
            }

            foreach (ETileType tileKey in counts.Keys)
            {
                if (counts[tileKey] > BoardConstants.MIN_LINK_LENGTH)
                {
                    return tileKey;
                }
            }
            
            Debug.LogError("[ShuffleBoardTask]::Impossible to reshuffle board with guaranteed link!");
            return ETileType.None;
        }

        private void InsertLink(List<BoardTile> tiles, ETileType linkType)
        {
            Vector2Int dimensions = _boardController.Dimensions;

            int randomx = Random.Range(1, dimensions.x - 2);
            int randomy = Random.Range(1, dimensions.y - 2);

            int insertIndex = -1;
            Vector2Int insertionAxis = GetInsertAxis();

            for (int i = 0, len = tiles.Count; i < len; i++)
            {
                if (insertIndex == 1)
                {
                    return;
                }
                
                var tile = tiles[i];
                if (tile == null)
                {
                    continue;
                }

                if (tile.TileType == linkType)
                {
                    int newBoardIndex = BoardUtils.BoardPosToIndex(
                        randomx + insertionAxis.x * insertIndex,
                        randomy + insertionAxis.y * insertIndex,
                        dimensions);

                    tiles[i] = tiles[newBoardIndex];
                    tiles[newBoardIndex] = tile;

                    insertIndex++;
                }
            }
        }

        private Vector2Int GetInsertAxis()
        {
            int random = Random.Range(0, 1);

            if (random == 0)
            {
                return Vector2Int.right;
            }

            return Vector2Int.up;
        }

        private void ShuffleBoard(List<BoardTile> currentTiles)
        {
            int tileCount = currentTiles.Count;

            for (int i = tileCount - 1; i > 0; i--)
            {
                int shuffledIndex = Random.Range(0, i);

                BoardTile tmp = currentTiles[i];
                currentTiles[i] = currentTiles[shuffledIndex];
                currentTiles[shuffledIndex] = tmp;
            }
        }
    }
}