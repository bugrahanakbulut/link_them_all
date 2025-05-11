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