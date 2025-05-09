using System;
using System.Collections.Generic;
using LinkThemAll.Game.Tile;
using UnityEngine;

namespace LinkThemAll.Game.Board
{
    [Serializable]
    public class LinkDrawer : IDisposable
    {
        [Serializable]
        private struct LinkColorConfig
        {
            public ETileType TileType;
            public Color Color;
        }

        [SerializeField] private LinkColorConfig[] _colorConfig;
        [SerializeField] private LineRenderer _lineRenderer;
        
        private LinkController _linkController;
        
        public void Initialize(LinkController linkController)
        {
            Reset();

            _linkController = linkController;
            
            _linkController.OnLinkStarted += OnLinkStarted;
            _linkController.OnLinkUpdated += OnLinkUpdated;
            _linkController.OnLinkTerminated += OnLinkTerminated;
            _linkController.OnLinkCompleted += OnLinkCompleted;
        }

        public void Dispose()
        {
            Reset();
        }

        private void Reset()
        {
            _lineRenderer.enabled = false;

            if (_linkController == null)
            {
                return;
            }
            
            _linkController.OnLinkStarted -= OnLinkStarted;
            _linkController.OnLinkUpdated -= OnLinkUpdated;
            _linkController.OnLinkTerminated -= OnLinkTerminated;
            _linkController.OnLinkCompleted -= OnLinkCompleted;
        }

        private void OnLinkStarted()
        {
            _lineRenderer.positionCount = 0;
            _lineRenderer.enabled = true;
            
            IReadOnlyList<BoardTile> link = _linkController.Link;

            if (link.Count == 0)
            {
                return;
            }

            BoardTile tile = link[0];
            Color linkColor = GetLinkColor(tile.TileType);
            _lineRenderer.startColor = linkColor;
            _lineRenderer.endColor = linkColor;
        }

        private void OnLinkUpdated()
        {
            IReadOnlyList<BoardTile> link = _linkController.Link;

            int linkCount = link.Count;
            
            if (linkCount == 0)
            {
                return;
            }

            _lineRenderer.positionCount = linkCount;
            
            for (int i = 0; i < linkCount; ++i)
            {
                BoardTile tile = link[i];
                
                Vector2Int boardPos = tile.BoardPos;
                Vector3 worldPos = BoardUtils.BoardPosToWorldPos(boardPos.x, boardPos.y);
                worldPos.z = 0;
                _lineRenderer.SetPosition(i, worldPos);
            }
        }

        private void OnLinkTerminated()
        {
            _lineRenderer.enabled = false;
        }

        private void OnLinkCompleted()
        {
            _lineRenderer.enabled = false;
        }

        private Color GetLinkColor(ETileType tileType)
        {
            foreach (LinkColorConfig config in _colorConfig)
            {
                if (config.TileType == tileType)
                {
                    return config.Color;
                }
            }
            
            return Color.white;
        }
    }
}