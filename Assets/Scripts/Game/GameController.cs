using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Level;
using UnityEngine;
using LinkThemAll.Services.Task;
using UnityEngine.AddressableAssets;

namespace LinkThemAll.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;

        private LevelController _level;
        
        private readonly TaskRunner _taskRunner = new TaskRunner();
        
        private const string BOARD_PATH = "Board";

        public void Initialize()
        {
            _level = new LevelController(_levelConfigs);
        }
        
        public async UniTaskVoid StartGame()
        {
            await LoadBoard();
        }
        
        private void OnDestroy()
        {
            _taskRunner.Terminate();
        }

        private async UniTask LoadBoard()
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

                _level.SetBoard(board);
                _level.LoadCurrentLevel();
            }
            catch
            {
                // ignore
            }
        }
    }
}