using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Services.Task;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class ValidateBoardTask : IServiceTask
    {
        private readonly BoardController _boardController;

        public ValidateBoardTask(BoardController boardController)
        {
            _boardController = boardController;
        }

        public UniTask Execute()
        {
            NativeArray<bool> linkResult = new NativeArray<bool>(1, Allocator.Persistent);
            
            LinkFinder linkFinder = new LinkFinder(_boardController.Dimensions, _boardController.BoardTiles,  linkResult);
            
            linkFinder.Schedule().Complete();
            
            _boardController.SetBoardNeedReshuffle(!linkFinder.Result[0]);

            linkResult.Dispose();
            
            return UniTask.CompletedTask;
        }
    }
}