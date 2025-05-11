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

        public override UniTask Show()
        {
            _gameObject.SetActive(true);
            return _canvasGroup.DOFade(1, 0.5f).From(0).SetId(this).ToUniTask();
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
            ServiceProvider.Get<IGameService>().StartGame();
        }
    }
}