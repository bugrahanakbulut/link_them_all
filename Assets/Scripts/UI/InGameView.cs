using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game;
using LinkThemAll.Game.Level;
using LinkThemAll.Services;
using TMPro;

namespace LinkThemAll.UI
{
    public class InGameView : ViewBase
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly ILevelService _levelService = ServiceProvider.Get<ILevelService>();
        private readonly IScoreService _scoreService = ServiceProvider.Get<IScoreService>();
        
        private const string LevelTemplate = "LEVEL {0}";
        private const string ScoreTemplate = "Score : {0} / {1}";
        
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
            int curLevel = _levelService.GetCurrentLevel() + 1;
            
            _levelText.SetText(string.Format(LevelTemplate, curLevel));
            
            OnScoreUpdated();

            _scoreService.OnScoreUpdated += OnScoreUpdated;
        }

        private void OnDisable()
        {
            _scoreService.OnScoreUpdated -= OnScoreUpdated;
        }

        private void OnScoreUpdated()
        {
            int levelScoreTarget = _levelService.GetLevelTarget();
            _scoreText.SetText(string.Format(ScoreTemplate, _scoreService.CurrentScore, levelScoreTarget));
        }
    }
}