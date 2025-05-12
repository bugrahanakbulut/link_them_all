using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game;
using LinkThemAll.Services;
using UnityEngine;
using UnityEngine.UI;

namespace LinkThemAll.UI
{
    public class LevelFailedView : ViewBase
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _gameObject;

        public override async UniTask Show()
        {
            _tryAgainButton.interactable = false;
            
            _gameObject.SetActive(true);
            await _canvasGroup.DOFade(1, 0.5f).From(0).SetId(this).ToUniTask();
            
            _tryAgainButton.interactable = true;
        }

        public override async UniTask Hide()
        {
            await _canvasGroup.DOFade(0, 0.5f).SetId(this).ToUniTask();
            _gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _tryAgainButton.onClick.AddListener(OnClickedTryAgain);
        }

        private void OnDisable()
        {
            _tryAgainButton.onClick.RemoveListener(OnClickedTryAgain);
        }

        private void OnClickedTryAgain()
        {
            _tryAgainButton.interactable = false;
            
            ServiceProvider.Get<IGameService>().StartGame();
        }
    }
}