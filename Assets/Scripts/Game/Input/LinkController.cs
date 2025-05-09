using System;
using System.Collections.Generic;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;
using UnityEngine;

namespace LinkThemAll.Game
{
    public class LinkController : IDisposable
    {
        private readonly InputController _inputController;
        private readonly BoardController _boardController;
        private readonly Camera _mainCamera;
        
        private readonly List<BoardTile> _link = new List<BoardTile>();

        public LinkController(InputController inputController, BoardController boardController)
        {
            _inputController = inputController;
            _boardController = boardController;
            _mainCamera = ServiceProvider.Get<ICameraService>().MainCamera;
            
            _inputController.OnFingerDown += OnFingerDown;
            _inputController.OnFingerUp += OnFingerUp;
            _inputController.OnInputMove += OnInputMove;
        }
        
        public void Dispose()
        {
            _inputController.OnFingerDown -= OnFingerDown;
            _inputController.OnFingerUp -= OnFingerUp;
            _inputController.OnInputMove -= OnInputMove;
        }

        private void OnFingerDown(Vector3 inputPos)
        {
            _link.Clear();
            
            Vector2Int inputBoardPos = BoardUtils.WorldPosToBoardPos(_mainCamera.ScreenToWorldPoint(inputPos));
            
            if (inputBoardPos.x < 0 || inputBoardPos.x >= _boardController.Dimensions.x ||
                inputBoardPos.y < 0 || inputBoardPos.y >= _boardController.Dimensions.y)
            {
                Debug.Log("[InputController]::Not able to select tile!");
                return;
            }
            
            BoardTile tile = _boardController.GetTileByBoardPos(inputBoardPos);

            if (tile == null)
            {
                return;
            }
            
            _link.Add(tile);
        }

        private void OnFingerUp()
        {
            if (_link.Count < 3)
            {
                return;
            }
            
            // link established
        }

        private void OnInputMove(Vector3 fingerPos)
        {
            if (_link.Count == 0)
            {
                return;
            }
            
            BoardTile lastSelectedTile = _link[^1];
            Vector2Int lastTileBoardPos = lastSelectedTile.BoardPos;
            Vector2Int inputBoardPos = BoardUtils.WorldPosToBoardPos(_mainCamera.ScreenToWorldPoint(fingerPos));

            Vector2Int deltaBoardPos = inputBoardPos - lastTileBoardPos;
            
            if (deltaBoardPos.magnitude == 0)
            {
                return;
            }

            float angle = Mathf.Atan2(deltaBoardPos.y, deltaBoardPos.x) * Mathf.Rad2Deg;

            Vector2Int targetBoardPos = lastTileBoardPos + GetDirectionByAngle(angle);

            BoardTile candidateTile = _boardController.GetTileByBoardPos(targetBoardPos);
            
            if (candidateTile == null)
            {
                return;
            }
            
            if (_link.Count > 1 && candidateTile == _link[^2])
            {
                _link.RemoveAt(_link.Count - 1);
                return;
            }

            if (lastSelectedTile.TileType == candidateTile.TileType)
            {
                _link.Add(candidateTile);
            }
        }
        
        private Vector2Int GetDirectionByAngle(float angle)
        {
            if (angle < 0)
            {
                angle += 360;
            }

            if (angle > 45 && angle < 135)
            {
                return Vector2Int.up;
            }

            if (angle > 135 && angle < 235)
            {
                return Vector2Int.left;
            }

            if (angle > 235 && angle < 315)
            {
                return Vector2Int.down;
            }

            return Vector2Int.right;
        }
    }
}