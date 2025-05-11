using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.UI.Tasks
{
    public class LoadViewTask : IServiceTask
    {
        private readonly string _path;
        private readonly ViewManager _viewManager;

        public LoadViewTask(string path, ViewManager _viewManager)
        {
            _path = path;
            this._viewManager = _viewManager;
        }

        public async UniTask Execute()
        {
            await _viewManager.LoadView(_path);
        }
    }
}