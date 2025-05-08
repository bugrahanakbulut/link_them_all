using Cysharp.Threading.Tasks;
using LinkThemAll.Game.Board;
using LinkThemAll.Services.Level;
using LinkThemAll.Services.Task;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LinkThemAll.Game.Tasks
{
    public class LoadBoardTask : IServiceTask
    {
        private readonly LevelController _level;

        public LoadBoardTask(LevelController level)
        {
            _level = level;
        }

        private const string BOARD_PATH = "Board";
        
        public async UniTask Execute()
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
        }
    }
}