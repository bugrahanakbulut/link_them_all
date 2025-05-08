using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;
using LinkThemAll.Services.Task;
using UnityEngine;

namespace LinkThemAll.Game.Tasks
{
    public class AdjustCameraTask : IServiceTask
    {
        private readonly Vector2Int _boardDimensions;

        public AdjustCameraTask(Vector2Int boardDimensions)
        {
            _boardDimensions = boardDimensions;
        }

        public UniTask Execute()
        {
            Vector3 leftBottom = BoardUtils.GetWorldPosByBoardPos(0, 0);
            Vector3 rightTop = BoardUtils.GetWorldPosByBoardPos(_boardDimensions.x - 1, _boardDimensions.y - 1);

            Vector3 center = (leftBottom + rightTop) * 0.5f;

            Camera mainCam = ServiceProvider.Get<ICameraService>().MainCamera;
            mainCam.transform.position = new Vector3(center.x, center.y, -10);

            if (_boardDimensions.x > _boardDimensions.y)
            {
                mainCam.orthographicSize = _boardDimensions.x * BoardConstants.TILE_WIDTH;
            }
            else
            {
                mainCam.orthographicSize = _boardDimensions.y * BoardConstants.TILE_HEIGHT;
            }
            
            return UniTask.CompletedTask;
        }
    }
}