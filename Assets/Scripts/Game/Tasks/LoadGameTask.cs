using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Level;
using LinkThemAll.Services.Task;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LinkThemAll.Game.Tasks
{
    public class LoadGameTask : IServiceTask
    {
        private readonly LevelController _level;
        
        private const string BOARD_PATH = "Board";
        private const string LEVEL_FILE_NAME_TEMPLATE = "level_{0}";
        
        private static readonly LevelConfig _defaultConfig = new LevelConfig(0, 7, 25, new Vector2Int(8, 8));

        public LoadGameTask(LevelController level)
        {
            _level = level;
        }

        public async UniTask Execute()
        {
            try
            {
                GameObject boardObject = await Addressables.InstantiateAsync(BOARD_PATH);

                if (boardObject == null)
                {
                    Debug.LogError("Board Object could not be loaded!");
                    return;
                }
            
                BoardController board = boardObject.GetComponent<BoardController>();

                if (board == null)
                {
                    Debug.LogError("Board Controller could not be found!");
                    return;
                }
                
                LevelConfig config = LoadLevelConfig(_level.LoadCurrentLevel());

                _level.InitializeLevel(board, config);
            }
            catch
            {
                // ignore
            }
        }

        private LevelConfig LoadLevelConfig(int levelId)
        {
            Object[] texts = Resources.LoadAll("Levels");
            
            int levelCount = texts.Length;

            if (levelCount == 0)
            {
               Debug.LogError("No defined level!");
               return _defaultConfig;
            }

            int loadingLevel = levelId % levelCount;

            string levelFileName = string.Format(LEVEL_FILE_NAME_TEMPLATE, loadingLevel);

            foreach (Object o in texts)
            {
                if (o.name == levelFileName)
                {
                    TextAsset textAsset = (TextAsset) o;
                    return JsonUtility.FromJson<LevelConfig>(textAsset.text);
                }
            }

            return _defaultConfig;
        }
    }
}