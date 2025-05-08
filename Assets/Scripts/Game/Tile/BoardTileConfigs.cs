using System;
using UnityEngine;

namespace LinkThemAll.Game.Tile
{
    [Serializable]
    public struct BoardTileConfig
    {
        public ETileType Type;
        public Sprite Sprite;
    }
    
    [CreateAssetMenu(fileName = "TileConfigs", menuName = "Configs/New Tile Configs")]
    public class BoardTileConfigs : ScriptableObject
    {
        [SerializeField] private BoardTileConfig[] _configs;

        public Sprite GetTileSprite(ETileType type)
        {
            foreach (BoardTileConfig config in _configs)
            {
                if (config.Type == type)
                {
                    return config.Sprite;
                }
            }

            return null;
        }
    }
}