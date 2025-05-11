using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class ShuffleBoardTask : IServiceTask
    {
        private readonly BoardController _boardController;
        private readonly List<int> _availableIndexes = new List<int>();

        private const float TILE_SHUFFLE_DURATION = 0.5f;

        public ShuffleBoardTask(BoardController boardController)
        {
            _boardController = boardController;

            int tileCount = _boardController.Dimensions.x * _boardController.Dimensions.y;

            for (int i = 0; i < tileCount; ++i)
            {
                _availableIndexes.Add(i);
            }
        }

        public UniTask Execute()
        {
            if (!_boardController.NeedToShuffle || _boardController.Freezed)
            {
                return UniTask.CompletedTask;
            }

            List<BoardTile> currentTiles = new List<BoardTile>(_boardController.BoardTiles);
            
            int tileCount = currentTiles.Count;
            
            ETileType linkTileType = FindTileTypeForLink(currentTiles);

            BoardTile[] shuffledTiles = new BoardTile[tileCount];

            InsertLink(currentTiles, shuffledTiles, linkTileType);

            for (int i = 0; i < tileCount; ++i)
            {
                BoardTile t = shuffledTiles[i];

                if (t != null)
                {
                    continue;
                }

                int selectedTileIndex = Random.Range(0, currentTiles.Count);

                shuffledTiles[i] = currentTiles[selectedTileIndex];
                currentTiles.RemoveAt(selectedTileIndex);
            }
            
            for (int index = 0, len = shuffledTiles.Length; index < len; index++)
            {
                BoardTile tile = shuffledTiles[index];
                Vector2Int newBoardPos = BoardUtils.IndexToBoardPos(index, _boardController.Dimensions);
                
                _boardController.AddTile(tile, index);
                
                tile.UpdateBoardPos(newBoardPos);
                tile.MoveTo(BoardUtils.BoardPosToWorldPos(newBoardPos.x, newBoardPos.y), TILE_SHUFFLE_DURATION).SetId(tile).SetEase(Ease.OutBack);
            }

            return UniTask.CompletedTask;
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
                
                if (!counts.TryAdd(tileType, 1))
                {
                    counts[tileType]++;
                }
            }

            foreach (ETileType tileKey in counts.Keys)
            {
                if (counts[tileKey] >= BoardConstants.MIN_LINK_LENGTH)
                {
                    return tileKey;
                }
            }
            
            Debug.LogError("[ShuffleBoardTask]::Impossible to reshuffle board with guaranteed link!");
            return ETileType.None;
        }

        private void InsertLink(List<BoardTile> tiles, BoardTile[] shuffledTiles, ETileType linkType)
        {
            Vector2Int dimensions = _boardController.Dimensions;

            int randomx = Random.Range(1, dimensions.x - 2);
            int randomy = Random.Range(1, dimensions.y - 2);

            int insertIndex = -1;
            Vector2Int insertionAxis = GetInsertAxis();

            for (int i = 0, len = tiles.Count; i < len; i++)
            {
                if (insertIndex == 2)
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

                    shuffledTiles[newBoardIndex] = tile;
                    tiles.RemoveAt(i);
                    i--;
                    
                    insertIndex++;
                }
            }
        }

        private Vector2Int GetInsertAxis()
        {
            int random = Random.Range(0, 2);
            
            if (random == 0)
            {
                return Vector2Int.right;
            }

            return Vector2Int.up;
        }
    }
}