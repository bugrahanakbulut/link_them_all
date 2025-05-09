using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Level;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class InitializeBoardTask : IServiceTask
    {
        private readonly BoardController _board;
        private readonly LevelConfig _levelConfig;

        public InitializeBoardTask(BoardController board, LevelConfig levelConfig)
        {
            _board = board;
            _levelConfig = levelConfig;
        }

        public UniTask Execute()
        {
            Vector2Int configBoardSize = _levelConfig.BoardSize;
            Vector2Int boardDim = new Vector2Int(
                Mathf.Max(configBoardSize.x, BoardConstants.MIN_BOARD_WIDTH), 
                Mathf.Max(configBoardSize.y, BoardConstants.MIN_BOARD_HEIGHT));
            
            int boardSize = boardDim.x * boardDim.y;

            List<ETileType> tiles = new List<ETileType>(boardSize);

            for (int i = 0; i < boardSize; ++i)
            {
                tiles.Add((ETileType)Random.Range(0, 3));
            }
            
            _board.InitializeBoard(boardDim, tiles);

            return UniTask.CompletedTask;
        }
    }
}