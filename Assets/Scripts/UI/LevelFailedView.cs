using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LinkThemAll.UI
{
    public class LevelFailedView : ViewBase
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _gameObject;

        public override UniTask Show()
        {
            _gameObject.SetActive(true);
            return _canvasGroup.DOFade(1, 1).From(0).SetId(this).ToUniTask();
        }

        public override async UniTask Hide()
        {
            await _canvasGroup.DOFade(0, 1f).SetId(this).ToUniTask();
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
            
        }
    }
}