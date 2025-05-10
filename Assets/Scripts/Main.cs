using System;
using UnityEngine;
using LinkThemAll.Game;
using LinkThemAll.Services;
using LinkThemAll.Services.CameraService;
using Random = UnityEngine.Random;

namespace LinkThemAll
{
    [DefaultExecutionOrder(-1000)]
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private CameraService _cameraService;
        
        private void Awake()
        {
            Random.InitState(DateTime.Now.Second);
            
            ServiceProvider.Add<ICameraService>(_cameraService);
            
            _gameController.Initialize();
            _gameController.StartGame();
        }
    }
}