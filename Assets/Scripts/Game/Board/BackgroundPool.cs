using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace LinkThemAll.Game.Board
{
    [Serializable]
    public class BackgroundPool : IDisposable
    {
        [SerializeField] private GameObject _referenceObject;

        [SerializeField] private Transform _bgHolder;
        
        private readonly IObjectPool<GameObject> _tilePool;

        private readonly List<GameObject> _activeObjects = new List<GameObject>();

        public BackgroundPool()
        {
            _tilePool = new ObjectPool<GameObject>(
                CreateNewBackgroundTile,
                TileTakenFromPool,
                TileReturnToPool);
        }

        public GameObject GetTile()
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
            
            _tilePool.Clear();
        }

        public void Dispose()
        {
            _tilePool.Clear();
        }

        private GameObject CreateNewBackgroundTile()
        {
            return GameObject.Instantiate(_referenceObject, _bgHolder);
        }

        private void TileTakenFromPool(GameObject gameObject)
        {
            gameObject.SetActive(true);
            
            _activeObjects.Add(gameObject);
        }

        private void TileReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
            
            _activeObjects.Remove(gameObject);
        }
    }
}