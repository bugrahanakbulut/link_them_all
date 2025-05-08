using UnityEngine;

namespace LinkThemAll.Services.CameraService
{
    public interface ICameraService : IService
    {
        Camera MainCamera { get; }
    }
    
    public class CameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private Camera _mainCamera;

        public Camera MainCamera => _mainCamera;
        
        public void Dispose()
        {
            _mainCamera = null;
        }
    }
}