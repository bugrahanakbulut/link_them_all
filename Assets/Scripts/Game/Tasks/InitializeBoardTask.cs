using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Level;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class InitializeBoardTask : IServiceTask
    {
        private readonly LevelController _levelController;

        public InitializeBoardTask(LevelController levelController)
        {
            _levelController = levelController;
        }

        public UniTask Execute()
        {
            LevelConfig config = _levelController.GetCurrentLevelConfig();

            Vector2Int boardDim = config.BoardSize;
            int boardSize = boardDim.x * boardDim.y;

            ETileType[] tiles = new ETileType[boardSize];

            for (int i = 0; i < boardSize; ++i)
            {
                tiles[i] = (ETileType) Random.Range(0, 3);
            }
            
            _levelController.Board.InitializeBoard(boardDim, tiles);

            return UniTask.CompletedTask;
        }
    }
}