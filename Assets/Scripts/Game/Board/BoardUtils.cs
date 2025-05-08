using UnityEngine;

namespace LinkThemAll.Game.Board
{
    public static class BoardUtils
    {
        public static Vector3 GetWorldPosByBoardPos(int x, int y)
        {
            return new Vector3(x * BoardConstants.TILE_WIDTH, y * BoardConstants.TILE_HEIGHT);
        }

        public static int GetIndexByBoardPos(int x, int y, Vector2Int boardDimensions)
        {
            return x + y * boardDimensions.x;
        }
    }
}