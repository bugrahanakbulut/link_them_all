using System;
using DG.Tweening;
using UnityEngine;

namespace LinkThemAll.Game.Tile
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private ETileType _tile;
        public ETileType TileType => _tile;
        
        private Vector2Int _boardPosition;
        public Vector2Int BoardPos => _boardPosition;
        
        public void Initialize(ETileType tile, Sprite sprite, Vector2Int boardPosition)
        {
            _tile = tile;
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.color = Color.white;
            _boardPosition = boardPosition;
            
            #if UNITY_EDITOR
            _gameObject.name = $"{boardPosition.x}_{boardPosition.y}_{tile}";
            #endif
        }

        public void SetPosition(Vector3 worldPosition)
        {
            _transform.position = worldPosition;
        }

        public void SetActive(bool isActive)
        {
            _gameObject.SetActive(isActive);
        }

        public void UpdateBoardPos(Vector2Int newBoardPos)
        {
            _boardPosition = newBoardPos;
        }

        public Tweener FadeIn(float duration, float from = 0)
        {
            return _spriteRenderer.DOFade(1, duration).From(from);
        }

        public Tween FadeOut(float duration)
        {
            return _spriteRenderer.DOFade(0, duration);
        }

        public Tween MoveTo(Vector3 position, float duration)
        {
            return _transform.DOMove(position, duration);
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }
    }
}