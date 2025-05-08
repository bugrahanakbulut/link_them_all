using System;
using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Common.Tasks
{
    public class ExecuteActionTask : IServiceTask
    {
        private readonly Action _action;
        
        public ExecuteActionTask(Action action)
        {
            _action = action;
        }

        public UniTask Execute()
        {
            try
            {
                _action?.Invoke();
            }
            catch
            {
                // ignored
            }

            return UniTask.CompletedTask;
        }
    }
}