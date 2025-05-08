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
    }
}