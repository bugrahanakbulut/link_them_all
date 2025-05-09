using UnityEngine;
using LinkThemAll.Game;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;

namespace LinkThemAll
{
    [DefaultExecutionOrder(-1000)]
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private CameraService _cameraService;
        
        private void Awake()
        {
            ServiceProvider.Add<ICameraService>(_cameraService);
            
            _gameController.Initialize();
            _gameController.StartGame();
        }
    }
}