using LinkThemAll.Common.Tasks;
using UnityEngine;
using LinkThemAll.Game.Tasks;
using LinkThemAll.Services.Task;
using LinkThemAll.Services.Level;

namespace LinkThemAll.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;

        private LevelController _level;
        
        private readonly TaskRunner _taskRunner = new TaskRunner();
        
        public void Initialize()
        {
            _level = new LevelController(_levelConfigs);
        }
        
        public void StartGame()
        {
            _taskRunner.AddTask(new LoadBoardTask(_level));
            _taskRunner.AddTask(new InitializeBoardTask(_level));
            _taskRunner.AddTask(new ExecuteActionTask(DrawBoard));
        }
        
        private void OnDestroy()
        {
            _taskRunner.Terminate();
        }

        private void DrawBoard()
        {
            _taskRunner.AddTask(_level.Board.DrawBoardBackgroundTask());
            _taskRunner.AddTask(_level.Board.DrawTiles());
            _taskRunner.AddTask(_level.Board.AdjustCamera());
        }
    }
}