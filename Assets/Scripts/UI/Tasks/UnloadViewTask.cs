using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.UI.Tasks
{
    public class UnloadViewTask : IServiceTask
    {
        private readonly string _path;
        private readonly ViewManager _viewManager;

        public UnloadViewTask(string path, ViewManager _viewManager)
        {
            _path = path;
            this._viewManager = _viewManager;
        }

        public async UniTask Execute()
        {
            await _viewManager.UnloadView(_path);
        }
    }
}