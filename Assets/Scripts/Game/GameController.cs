using UnityEngine;
using LinkThemAll.Game.Level;
using LinkThemAll.Game.Tasks;
using Cysharp.Threading.Tasks;
using LinkThemAll.Common.Tasks;
using LinkThemAll.Services;
using LinkThemAll.Services.Task;
using LinkThemAll.UI;
using LinkThemAll.UI.Tasks;

namespace LinkThemAll.Game
{
    public interface IGameService : IService
    {
        UniTaskVoid StartGame();
    }
    
    public class GameController : MonoBehaviour, IGameService
    {
        [SerializeField] private ViewManager _viewManager;
        
        private LevelController _level;
        
        private readonly TaskRunner _taskRunner = new TaskRunner();

        private void OnDestroy()
        {
            if (_level != null)
            {
                _level.OnLevelCompleted -= OnLevelCompleted;
                _level.OnLevelFailed -= OnLevelFailed;
                _level.Dispose();
            }
            
            _taskRunner.Terminate();
        }

        public void Initialize()
        {
            _level = new LevelController();
            
            ServiceProvider.Add<ILevelService>(_level);
            ServiceProvider.Add<IMoveService>(_level);
            ServiceProvider.Add<IGameService>(this);

            _level.OnLevelFailed += OnLevelFailed;
            _level.OnLevelCompleted += OnLevelCompleted;
        }

        public async UniTaskVoid StartGame()
        {
            _taskRunner.AddTask(new LoadGameTask(_level));
            _taskRunner.AddTask(new ExecuteActionTask(() =>
            {
                _viewManager.LoadView(ViewConstants.InGameView).Forget();
                _viewManager.UnloadView(ViewConstants.LevelFailedView).Forget();
                _viewManager.UnloadView(ViewConstants.LevelCompletedView).Forget();
            }));
        }

        private void OnLevelCompleted()
        {
            _level.LockBoard();
            
            _taskRunner.AddTask(new WaitForSecondsTask(0.5f));
            _taskRunner.AddTask(new LoadViewTask(ViewConstants.LevelCompletedView, _viewManager));
            _taskRunner.AddTask(new ExecuteActionTask(() =>
            {
                _level.Reset();
                _viewManager.UnloadView(ViewConstants.InGameView).Forget();
            }));
            _taskRunner.AddTask(new UnloadBoardTask(_level));
        }

        private void OnLevelFailed()
        {
            _level.FreezeBoard();
            
            _taskRunner.AddTask(new LoadViewTask(ViewConstants.LevelFailedView, _viewManager));
            _taskRunner.AddTask(new UnloadBoardTask(_level));
            _taskRunner.AddTask(new ExecuteActionTask(() =>
            {
                _level.Reset();
                _viewManager.UnloadView(ViewConstants.InGameView).Forget();
            }));
        }

        public void Dispose()
        {
            //
        }
    }
}