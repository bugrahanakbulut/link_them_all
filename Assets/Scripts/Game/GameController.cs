using LinkThemAll.Game.Tasks;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game
{
    public class GameController : MonoBehaviour
    {
        private readonly TaskRunner _taskRunner = new TaskRunner();
        
        public void StartGame()
        {
            _taskRunner.AddTask(new LoadLevelTask());
            _taskRunner.AddTask(new InitializeBoardTask());
        }

        private void OnDestroy()
        {
            _taskRunner.Terminate();
        }
    }
}