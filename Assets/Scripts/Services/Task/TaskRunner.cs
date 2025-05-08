using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LinkThemAll.Services.Task
{
    public class TaskRunner
    {
        public bool Running { get; private set; }

        private bool _paused;
        private IServiceTask _curTask;
        private Queue<IServiceTask> _taskQueue = new Queue<IServiceTask>();
        private CancellationTokenSource _cts;

        public TaskRunner(CancellationTokenSource cts = null)
        {
            if (cts == null)
            {            
                cts = new CancellationTokenSource();
            }

            _cts = cts;
        }

        public void AddTask(IServiceTask task)
        {
            _taskQueue.Enqueue(task);

            if (!Running)
            {
                StartExecution();
            }
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }
        
        public void Terminate()
        {
            if (_cts == null)
            {
                return;
            }
            
            _cts.Cancel();
            _cts.Dispose();
        }

        private async UniTaskVoid StartExecution()
        {
            Running = true;

            while (_taskQueue.Count > 0)
            {
                if (_paused)
                {
                    await UniTask.WaitUntil(() => !_paused);
                    continue;
                }
                
                IServiceTask serviceTask = _taskQueue.Dequeue();

                try
                {
                    await serviceTask.Execute().AttachExternalCancellation(_cts.Token);
                }
                catch
                {
                    // 
                }
            }

            Running = false;
        }
    }
}