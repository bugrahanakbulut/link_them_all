using System;
using System.Collections.Generic;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;
using UnityEngine;

namespace LinkThemAll.Game.Board
{
    public class LinkController : IDisposable
    {
        private readonly InputController _inputController;
        private readonly BoardController _boardController;
        private readonly Camera _mainCamera;
        
        private readonly List<BoardTile> _link = new List<BoardTile>();
        public IReadOnlyList<BoardTile> Link => _link.AsReadOnly();

        private readonly Vector2Int[] _directions;
        private int _directionDivider;
        private int _directionAxisShift;
        
        public Action OnLinkStarted { get; set; }
        public Action OnLinkUpdated { get; set; }
        public Action OnLinkTerminated { get; set; }
        public Action OnLinkCompleted { get; set; }

        public LinkController(InputController inputController, BoardController boardController)
        {
            _directions = new[]
            {
                Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down,
            };
            
            _directionDivider = 360 / _directions.Length;
            _directionAxisShift = (int)(_directionDivider * 0.5f);
            
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
                return;
            }
            
            BoardTile tile = _boardController.GetTileByBoardPos(inputBoardPos);

            if (tile == null)
            {
                return;
            }
            
            _link.Add(tile);
            OnLinkStarted?.Invoke();
        }

        private void OnFingerUp()
        {
            if (_link.Count < BoardConstants.MIN_LINK_LENGTH)
            {
                OnLinkTerminated?.Invoke();
                return;
            }
            
            OnLinkCompleted?.Invoke();
            
            _link.Clear();
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
                OnLinkUpdated?.Invoke();
                return;
            }
            
            if (_link.Contains(candidateTile))
            {
                return;
            }

            if (lastSelectedTile.TileType == candidateTile.TileType)
            {
                _link.Add(candidateTile);
                OnLinkUpdated?.Invoke();
            }
        }
        
        private Vector2Int GetDirectionByAngle(float angle)
        {
            angle += _directionAxisShift;

            if (angle < 0)
            {
                angle += 360;
            }

            return _directions[((int)angle / _directionDivider)];
        }
    }
}