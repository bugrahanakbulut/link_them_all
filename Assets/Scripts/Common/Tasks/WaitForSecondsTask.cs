using System;
using Cysharp.Threading.Tasks;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Common.Tasks
{
    public class WaitForSecondsTask : IServiceTask
    {
        private readonly float _duration;

        public WaitForSecondsTask(float duration)
        {
            _duration = duration;
        }

        public async UniTask Execute()
        {
            if (_duration == 0)
            {
                return;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));
        }
    }
}