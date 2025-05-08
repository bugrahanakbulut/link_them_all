using UnityEngine;

namespace LinkThemAll.Game.Board
{
    public static class BoardUtils
    {
        public static Vector3 BoardPosToWorldPos(int x, int y)
        {
            return new Vector3(x * BoardConstants.TILE_WIDTH, y * BoardConstants.TILE_HEIGHT, 0);
        }

        public static int GetIndexByBoardPos(int x, int y, Vector2Int boardDimensions)
        {
            return x + y * boardDimensions.x;
        }

        public static Vector2Int WorldPosToBoardPos(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt((worldPos.x + BoardConstants.TILE_WIDTH * 0.5f) / BoardConstants.TILE_WIDTH);
            int y = Mathf.FloorToInt((worldPos.y + BoardConstants.TILE_HEIGHT * 0.5f) / BoardConstants.TILE_HEIGHT);
            return new Vector2Int(x, y);
        }
    }
}