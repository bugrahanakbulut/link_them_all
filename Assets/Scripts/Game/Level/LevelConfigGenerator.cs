#if UNITY_EDITOR

using System.IO;
using UnityEngine;

namespace LinkThemAll.Game.Level
{
    
    public class LevelConfigGenerator : MonoBehaviour
    {
        [ContextMenu("Generate Level Config Files")]
        private void GenerateLevelConfigFiles()
        {
            string folderPath = Path.Combine(Application.dataPath, "Resources/Levels");
            
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            for (int i = 0; i < 25; i++)
            {
                LevelConfig config = CreateLevelConfig(i);
                string fileName = $"level_{i}.json";
                string filePath = Path.Combine(folderPath, fileName);
                string json = JsonUtility.ToJson(config, prettyPrint: true);

                File.WriteAllText(filePath, json);
            }

            Debug.Log("Level configurations generated under Resources folder.");
        }    

        private LevelConfig CreateLevelConfig(int levelId)
        {
            int targetMove = 3 + (levelId * 2);
            int targetScore = targetMove * 3 + levelId;
            Vector2Int boardSize = new Vector2Int(5 + levelId / 5, 5 + levelId / 5);

            return new LevelConfig
            {
                LevelId = levelId,
                TargetMove = targetMove,
                TargetScore = targetScore,
                BoardSize = boardSize
            };
        }
    }
}

#endif