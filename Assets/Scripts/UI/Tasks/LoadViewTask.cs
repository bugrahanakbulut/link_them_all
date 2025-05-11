using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.UI.Tasks
{
    public class LoadViewTask : IServiceTask
    {
        private readonly string _path;
        private readonly ViewManager _viewManager;

        public LoadViewTask(string path, ViewManager viewManager)
        {
            _path = path;
            _viewManager = viewManager;
        }

        public async UniTask Execute()
        {
            await _viewManager.LoadView(_path);
        }
    }
}