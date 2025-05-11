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

        public LevelConfig(int levelId, int targetMove, int targetScore, Vector2Int boardSize)
        {
            LevelId = levelId;
            TargetMove = targetMove;
            TargetScore = targetScore;
            BoardSize = boardSize;
        }
    }
}