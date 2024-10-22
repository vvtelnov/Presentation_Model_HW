using CharacterPopupPresenter.IPresenters;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI.CharacterPopupView
{
    public class CharacterLvlPopupView : BasePopupView<ICharacterLvlPresenter>
    {
        [SerializeField] private TMP_Text _currentLevel;

        private Sequence _lvlUpSequence;
        private Transform _levelTransform;
        
        protected override void Show(ICharacterLvlPresenter args)
        {
            args.CurrentLevel.SkipLatestValueOnSubscribe().Subscribe(OnLevelChanged).AddTo(disposable);
            
            _currentLevel.text = args.CurrentLevel.Value;
            _levelTransform = _currentLevel.transform;
        }
        
        
        private readonly Color _increaseColor = new Color32(0, 206, 44, 255);
        private readonly Color _decreaseColor = new Color32(255, 73, 85, 255);
        private readonly Color _textBaseColor = new Color32(25, 25, 25, 255);
        private readonly float _colorChangeDuration = 0.3f;
        private readonly float _scaleDuration = 0.3f;
        private readonly float _scaleEndValue = 1.25f;
        private readonly float _scaleDefaultValue = 1f;
        private readonly float _shakeDuration = 0.5f;
        private readonly float _shakeStrength = 2f;
        private readonly int _shakeVibrato = 20;
        
        private void OnLevelChanged(string newLevel)
        {
            Sequence statChangeSequence = DOTween.Sequence();
            
            statChangeSequence
                .Append(_currentLevel.DOColor(_increaseColor, _colorChangeDuration))
                .Append(_levelTransform.DOScale(_scaleEndValue, _scaleDuration))
                .Append(_levelTransform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato))
                .AppendInterval(2f)
                .Append(_levelTransform.DOScale(_scaleDefaultValue, _scaleDuration)).SetEase(Ease.OutSine)
                .Append(_currentLevel.DOColor(_textBaseColor, _colorChangeDuration));
        }
    }
}