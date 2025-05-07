using Cysharp.Threading.Tasks;

namespace LinkThemAll.Services.Task
{
    public interface IServiceTask
    {
        UniTask Execute();
    }
}