using System.Collections.Generic;
using LinkThemAll.Game.Tile;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LinkThemAll.Game.Board
{
    [BurstCompile(CompileSynchronously = true, DisableSafetyChecks = true)]
    public struct LinkFinder : IJob
    {
        private readonly Vector2Int Dimensions;
        
        [ReadOnly] public NativeArray<ETileType> Tiles;
        
        public NativeArray<bool> Result; 

        public LinkFinder(Vector2Int dimensions, IReadOnlyList<BoardTile> tiles, NativeArray<bool> result) : this()
        {
            Dimensions = dimensions;
            Result = result;

            int tileCount = tiles.Count;
            
            Tiles = new NativeArray<ETileType>(tileCount, Allocator.Persistent);

            for (int i = 0; i < tileCount; ++i)
            {
                BoardTile tile = tiles[i];

                Tiles[i] = tile != null ? tile.TileType : ETileType.None;
            }
        }

        public void Execute()
        {
            for (int y = 0; y < Dimensions.y; ++y)
            {
                for (int x = 0; x < Dimensions.x; ++x)
                {
                    if (GetLinkLengthFor(x, y) > BoardConstants.MIN_LINK_LENGTH)
                    {
                        Result[0] = true;
                        return;
                    }
                }
            }
        }

        private int GetLinkLengthFor(int x, int y)
        {
            ETileType tile = Tiles[BoardUtils.BoardPosToIndex(x, y, Dimensions)];

            if (tile == ETileType.None)
            {
                return 0;
            }

            int linkFromRight = 0;
            int linkFromAbove = 0;
            
            if (x + 1 < Dimensions.x)
            {
                ETileType tileRight = Tiles[BoardUtils.BoardPosToIndex(x + 1, y, Dimensions)];

                if (tileRight == tile)
                {
                    linkFromRight = 1 + GetLinkLengthFor(x + 1, y);
                }
            }

            if (y + 1 < Dimensions.y)
            {
                ETileType tileAbove = Tiles[BoardUtils.BoardPosToIndex(x, y + 1, Dimensions)];

                if (tileAbove == tile)
                {
                    linkFromAbove = 1 + GetLinkLengthFor(x, y + 1);
                }
            }

            return Mathf.Max(linkFromAbove, linkFromRight);
        }
    }
}