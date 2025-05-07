using System;
using UnityEngine;

namespace LinkThemAll.Services.Level
{
    [Serializable]
    public struct LevelConfig
    {
        public int LevelId;
        public int TargetMove;
        public int TargetScore;
        public Vector2Int BoardSize;
    }
    
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