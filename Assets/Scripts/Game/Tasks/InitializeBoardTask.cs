using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Game.Tasks
{
    public class InitializeBoardTask : IServiceTask
    {
        public UniTask Execute()
        {
            return UniTask.CompletedTask;
        }
    }
}