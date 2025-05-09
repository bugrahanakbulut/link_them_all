using System;
using UnityEngine;
using UnityEngine.Pool;
using LinkThemAll.Game.Tile;
using System.Collections.Generic;

namespace LinkThemAll.Game.Board
{
    [Serializable]
    public class BoardTilePool : IDisposable
    {
        [SerializeField] private BoardTile _refTile;
        [SerializeField] private Transform _tileHolder;

        private IObjectPool<BoardTile> _tilePool;
       
        private readonly List<BoardTile> _activeObjects = new List<BoardTile>();

        public BoardTilePool()
        {
            _tilePool = new ObjectPool<BoardTile>(
                CreteNewTileBoard,
                TileBoardTakenFromPool,
                TileBoardReturnToPool);
        }

        public BoardTile GetTile()
        {
            return _tilePool.Get();
        }

        public void ReleaseAll()
        {
            int itemCount = _activeObjects.Count;

            for (int i = itemCount; i > 0; --i)
            {
                _tilePool.Release(_activeObjects[i]);
            }
            
            _activeObjects.Clear();
        }

        public void Dispose()
        {
            _tilePool.Clear();
        }

        public void Release(BoardTile tile) => _tilePool.Release(tile);

        private BoardTile CreteNewTileBoard()
        {
            return GameObject.Instantiate(_refTile, _tileHolder);
        }

        private void TileBoardTakenFromPool(BoardTile tile)
        {
        }

        private void TileBoardReturnToPool(BoardTile tile)
        {
            tile.SetActive(false);
        }
    }
}