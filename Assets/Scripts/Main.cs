using UnityEngine;
using LinkThemAll.Game;

namespace LinkThemAll
{
    [DefaultExecutionOrder(-1000)]
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        
        private void Awake()
        {
            _gameController.Initialize();
            _gameController.StartGame();
        }
    }
}