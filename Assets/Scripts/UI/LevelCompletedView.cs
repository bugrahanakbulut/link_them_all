using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game;
using LinkThemAll.Services;
using UnityEngine;
using UnityEngine.UI;

namespace LinkThemAll.UI
{
    public class LevelCompletedView : ViewBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Button _continueButton;
        
        public override async UniTask Show()
        {
            _continueButton.interactable = false;
            
            _gameObject.SetActive(true);
            
            await _canvasGroup.DOFade(1, 0.5f).From(0).SetId(this).ToUniTask();
            
            _continueButton.interactable = true;
        }

        public override async UniTask Hide()
        {
            await _canvasGroup.DOFade(0, 0.5f).SetId(this).ToUniTask();
            _gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnClickedContinue);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnClickedContinue);
        }

        private void OnClickedContinue()
        {
            _continueButton.interactable = false;
            
            ServiceProvider.Get<IGameService>().StartGame();
        }
    }
}