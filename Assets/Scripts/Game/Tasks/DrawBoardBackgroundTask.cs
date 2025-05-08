using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class DrawBoardBackgroundTask : IServiceTask
    {
        private readonly BackgroundPool _pool;
        private readonly Vector2Int _boardDimensions;

        public DrawBoardBackgroundTask(Vector2Int boardDimensions, BackgroundPool pool)
        {
            _boardDimensions = boardDimensions;
            _pool = pool;
        }

        public UniTask Execute()
        {
            for (int y = 0; y < _boardDimensions.y; ++y)
            {
                for (int x = 0; x < _boardDimensions.x; ++x)
                {
                    GameObject tile = _pool.GetTile();
                    tile.transform.position = BoardUtils.GetWorldPosByBoardPos(x, y);
                }
            }
            
            return UniTask.CompletedTask;
        }

        
    }
}