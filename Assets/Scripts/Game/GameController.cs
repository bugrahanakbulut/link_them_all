using UnityEngine;
using LinkThemAll.Game.Level;
using LinkThemAll.Game.Tasks;
using Cysharp.Threading.Tasks;
using LinkThemAll.Common.Tasks;
using LinkThemAll.Services;
using LinkThemAll.Services.Task;
using LinkThemAll.UI;

namespace LinkThemAll.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private ViewManager _viewManager;
        
        private LevelController _level;
        
        private readonly TaskRunner _taskRunner = new TaskRunner();
        
        public void Initialize()
        {
            _level = new LevelController(_levelConfigs);
            
            ServiceProvider.Add<ILevelService>(_level);
            ServiceProvider.Add<IMoveService>(_level);
        }
        
        public async UniTaskVoid StartGame()
        {
            _taskRunner.AddTask(new LoadGameTask(_level));
            _taskRunner.AddTask(new ExecuteActionTask(() =>
            {
                _viewManager.LoadView(ViewConstants.InGameView).Forget();
            }));
        }
        
        private void OnDestroy()
        {
            _level.Dispose();
            _taskRunner.Terminate();
        }
    }
}