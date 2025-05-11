using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Level;
using LinkThemAll.Services.Task;
using UnityEngine.AddressableAssets;

namespace LinkThemAll.Game.Tasks
{
    public class UnloadBoardTask : IServiceTask
    {
        private readonly LevelController _levelController;

        public UnloadBoardTask(LevelController levelController)
        {
            _levelController = levelController;
        }

        public UniTask Execute()
        {
            Addressables.ReleaseInstance(_levelController.Board.gameObject);
            
            return UniTask.CompletedTask;
        }
    }
}