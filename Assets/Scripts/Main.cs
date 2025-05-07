using LinkThemAll.Game;
using UnityEngine;
using LinkThemAll.Services;
using LinkThemAll.Services.Level;

namespace LinkThemAll
{
    [DefaultExecutionOrder(-1000)]
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        
        private void Awake()
        {
            InitializeServices();
            
            _gameController.StartGame();
        }

        private void InitializeServices()
        {
            ServiceProvider.Add<ILevelService>(new LevelService());
            ServiceProvider.Add(new GameService());
        }
    }
}