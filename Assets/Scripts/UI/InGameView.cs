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
        [SerializeField] private TextMeshProUGUI _moveText;
        [SerializeField] private TextMeshProUGUI _fpstracker;
        
        
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly ILevelService _levelService = ServiceProvider.Get<ILevelService>();
        private readonly IScoreService _scoreService = ServiceProvider.Get<IScoreService>();
        private readonly IMoveService _moveService = ServiceProvider.Get<IMoveService>();
        
        private const string LevelTemplate = "LEVEL {0}";
        private const string ScoreTemplate = "Score : {0} / {1}";
        private const string MoveTemplate = "Move Count : \n {0}"; 
        private const string FpsTrackerTemplate = "FPS : {0:###}";
        
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

        private void Update()
        {
            _fpstracker.SetText(string.Format(FpsTrackerTemplate, 1f / Time.deltaTime));
        }

        private void OnEnable()
        {
            int curLevel = _levelService.GetCurrentLevel() + 1;
            
            _levelText.SetText(string.Format(LevelTemplate, curLevel));
            
            OnScoreUpdated();
            OnMoveCountUpdated();
            
            _scoreService.OnScoreUpdated += OnScoreUpdated;
            _moveService.OnMoveCountUpdated += OnMoveCountUpdated;
        }

        private void OnDisable()
        {
            _scoreService.OnScoreUpdated -= OnScoreUpdated;
            _moveService.OnMoveCountUpdated -= OnMoveCountUpdated;
        }

        private void OnMoveCountUpdated()
        {
            _moveText.SetText(string.Format(MoveTemplate, _moveService.RemainingMoveCount));
        }

        private void OnScoreUpdated()
        {
            int levelScoreTarget = _levelService.GetLevelTargetScore();
            _scoreText.SetText(string.Format(ScoreTemplate, Math.Min(levelScoreTarget, _scoreService.CurrentScore), levelScoreTarget));
        }
    }
}