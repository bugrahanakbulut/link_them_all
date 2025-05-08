using System;
using UnityEngine;

namespace LinkThemAll.Game.Level
{
    [Serializable]
    public struct LevelConfig
    {
        public int LevelId;
        public int TargetMove;
        public int TargetScore;
        public Vector2Int BoardSize;
    }
    
    [CreateAssetMenu(fileName = "LevelConfigs", menuName = "Configs/New Level Configs")]
    public class LevelConfigs : ScriptableObject
    {
        [SerializeField] private LevelConfig[] _levels;

        public LevelConfig TryGetLevelConfig(int id)
        {
            int definedLevelCount = _levels.Length;

            if (id < 0)
            {
                id = 0;
            }
            
            if (id >= definedLevelCount)
            {
                id = id % definedLevelCount;
            }

            return _levels[id];
        }
    }
}