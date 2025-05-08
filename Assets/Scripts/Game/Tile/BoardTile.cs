using UnityEngine;

namespace LinkThemAll.Game.Tile
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private ETileType _tile;
        private Vector2Int _boardPosition;
        
        public void Initialize(ETileType tile, Sprite sprite, Vector2Int boardPosition)
        {
            _tile = tile;
            _spriteRenderer.sprite = sprite;
            _boardPosition = boardPosition;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            _transform.position = worldPosition;
        }

        public void SetActive(bool isActive)
        {
            _gameObject.SetActive(isActive);
        }
    }
}