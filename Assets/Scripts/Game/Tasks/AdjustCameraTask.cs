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

        private const float WIDTH_OFFSET = 1.1f;

        public AdjustCameraTask(Vector2Int boardDimensions)
        {
            _boardDimensions = boardDimensions;
        }

        public UniTask Execute()
        {
            Vector3 leftBottom = BoardUtils.BoardPosToWorldPos(0, 0);
            Vector3 rightTop = BoardUtils.BoardPosToWorldPos(_boardDimensions.x - 1, _boardDimensions.y - 1);

            Vector3 center = (leftBottom + rightTop) * 0.5f;

            Camera mainCam = ServiceProvider.Get<ICameraService>().MainCamera;
            Vector3 camPos = new Vector3(center.x, center.y, -10);

            float screenAspectRation = (float)Screen.height / Screen.width;
            
            float boardWidth = (_boardDimensions.x * BoardConstants.TILE_WIDTH) * WIDTH_OFFSET;
            float boardHeight = _boardDimensions.y * BoardConstants.TILE_HEIGHT;

            float sizeForHeight = boardHeight * 0.75f;
            float sizeForWidth = boardWidth * screenAspectRation * 0.5f;
            
            mainCam.orthographicSize = Mathf.Max(sizeForHeight, sizeForWidth);

            mainCam.transform.position = camPos;
            
            return UniTask.CompletedTask;
        }
    }
}