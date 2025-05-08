using System;
using System.Collections.Generic;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;
using UnityEngine;

namespace LinkThemAll.Game.InputController
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private BoardController _boardController;

        private Camera _mainCamera;
        private bool _haveSelectedTile;
        private readonly List<BoardTile> _selectedTiles = new List<BoardTile>();

        private void Awake()
        {
            _mainCamera = ServiceProvider.Get<ICameraService>().MainCamera;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FingerDown();
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                FingerUp();
                return;
            }

            if (_haveSelectedTile)
            {
                FingerMove();
            }
        }

        private void FingerDown()
        {
            Vector2Int inputBoardPos = BoardUtils.WorldPosToBoardPos(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
            
            if (inputBoardPos.x < 0 || inputBoardPos.x >= _boardController.Dimensions.x ||
                inputBoardPos.y < 0 || inputBoardPos.y >= _boardController.Dimensions.y)
            {
                Debug.Log("[InputController]::Not able to select tile!");
                return;
            }

            int selectedTileIndex = BoardUtils.GetIndexByBoardPos(inputBoardPos.x, inputBoardPos.y, _boardController.Dimensions); 
            BoardTile selectedTile = _boardController.GetTileByIndex(selectedTileIndex);

            if (selectedTile == null)
            {
                Debug.Log("[InputController]::Selected tile is null!");
                return;
            }

            _haveSelectedTile = true;
            _selectedTiles.Add(selectedTile);
        }

        private void FingerUp()
        {
            _haveSelectedTile = false;
        }

        private void FingerMove()
        {
            if (_selectedTiles.Count == 0)
            {
                return;
            }

            BoardTile lastSelectedTile = _selectedTiles[^1];
            
            Vector2Int lastTileBoardPos = lastSelectedTile.BoardPos;
            Vector2Int inputBoardPos = BoardUtils.WorldPosToBoardPos(_mainCamera.ScreenToWorldPoint(Input.mousePosition));

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

            if (_selectedTiles.Count > 1 && candidateTile == _selectedTiles[^2])
            {
                _selectedTiles.RemoveAt(_selectedTiles.Count - 1);
                return;
            }

            if (lastSelectedTile.TileType == candidateTile.TileType)
            {
                _selectedTiles.Add(candidateTile);
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