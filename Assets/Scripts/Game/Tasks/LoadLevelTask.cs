using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Game.Tasks
{
    public class LoadLevelTask : IServiceTask
    {
        public UniTask Execute()
        {
            return UniTask.CompletedTask;
        }
    }
}