using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Level;
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
            
            _board.InitializeBoard(boardDim);

            return UniTask.CompletedTask;
        }
    }
}