using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using LinkThemAll.Game.Tile;
using System.Collections.Generic;

namespace LinkThemAll.Game.Board
{
    [BurstCompile(CompileSynchronously = true, DisableSafetyChecks = true)]
    public struct LinkFinder : IJob
    {
        [ReadOnly][DeallocateOnJobCompletion] public NativeArray<ETileType> Tiles;
        [ReadOnly] private Vector2Int _dimensions;
        [ReadOnly] private int _tileCount;

        public NativeArray<bool> Result;
        
        [DeallocateOnJobCompletion] private NativeArray<int> _visited;
        [DeallocateOnJobCompletion] private NativeArray<Vector2Int> _directions;
        
        public LinkFinder(Vector2Int dimensions, IReadOnlyList<BoardTile> tiles, NativeArray<bool> result) : this()
        {
            _tileCount = tiles.Count;

            _dimensions = dimensions;
            
            Result = result;
            
            Tiles = new NativeArray<ETileType>(_tileCount, Allocator.Persistent);
            
            _visited = new NativeArray<int>(_tileCount, Allocator.Persistent);

            _directions = new NativeArray<Vector2Int>(4, Allocator.Persistent);
            _directions[0] = Vector2Int.left;
            _directions[1] = Vector2Int.right;
            _directions[2] = Vector2Int.up;
            _directions[3] = Vector2Int.down;

            for (int i = 0; i < _tileCount; ++i)
            {
                BoardTile tile = tiles[i];

                Tiles[i] = tile != null ? tile.TileType : ETileType.None;
            }
        }

        public void Execute()
        {
            for (int y = 0; y < _dimensions.y; ++y)
            {
                for (int x = 0; x < _dimensions.x; ++x)
                {
                    int index = BoardUtils.BoardPosToIndex(x, y, _dimensions);

                    if (_visited[index] == 1)
                    {
                        continue;
                    }
                    
                    int linkLength = GetLinkLengthFor(x, y, index, Tiles[index]);
                    
                    if (linkLength >= BoardConstants.MIN_LINK_LENGTH)
                    {
                        Result[0] = true;
                        return;
                    }
                }
            }
        }
        
        private int GetLinkLengthFor(int x, int y, int index, ETileType linkType)
        {
            if (x >= _dimensions.x || x < 0 || y >= _dimensions.y || y < 0 || index < 0 || index >= _tileCount || _visited[index] == 1)
            {
                return 0;
            }
            
            ETileType tile = Tiles[index];

            if (tile != linkType)
            {
                return 0;
            }
            
            _visited[index] = 1;
            int curLinkLength = 0;
            
            foreach (Vector2Int dir in _directions)
            {
                int newX = x + dir.x;
                int newY = y + dir.y;
                
                int subLink = GetLinkLengthFor(newX, newY, BoardUtils.BoardPosToIndex(newX, newY, _dimensions), linkType);

                if (subLink > curLinkLength)
                {
                    curLinkLength = subLink;
                }
            }

            _visited[index] = 0;
            
            return 1 + curLinkLength;
        }
    }
}