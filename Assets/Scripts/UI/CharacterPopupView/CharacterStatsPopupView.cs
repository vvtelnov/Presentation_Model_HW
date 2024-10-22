using CharacterPopupPresenter.IPresenters;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI.CharacterPopupView
{
    public class CharacterStatsPopupView : BasePopupView<ICharacterStatsPresenter>
    {
        [SerializeField]
        private TMP_Text[] _stats; 
        
        protected override void Show(ICharacterStatsPresenter args)
        {
            for (int i = 0; i < _stats.Length; i++)
            {
                var stat = _stats[i];
                
                args.Stats[i]
                    .SkipLatestValueOnSubscribe()
                    .Subscribe(str => UpdateStat(stat, str))
                    .AddTo(disposable);
                
                stat.text = args.Stats[i].Value;
            }
        }
        
        private void UpdateStat(TMP_Text stat, string newStatValue)
        {
            stat.text = newStatValue;
            PlayStatUpdateAnimation(stat.transform, stat);
        }
        
        private readonly Color _increseColor = new Color32(0, 206, 44, 255);
        private readonly Color _decreaseColor = new Color32(255, 73, 85, 255);
        private readonly Color _textBaseColor = new Color32(25, 25, 25, 255);
        private readonly float _colorChangeDuration = 0.3f;
        private readonly float _scaleDuration = 0.3f;
        private readonly float _scaleEndValue = 1.25f;
        private readonly float _scaleDefaultValue = 1f;
        private readonly float _shakeDuration = 0.5f;
        private readonly float _shakeStrength = 2f;
        private readonly int _shakeVibrato = 20;
        
        private void PlayStatUpdateAnimation(Transform stat, TMP_Text statTextElem)
        {
            Sequence statChangeSequence = DOTween.Sequence();
            
            statChangeSequence
                .Append(statTextElem.DOColor(_increseColor, _colorChangeDuration))
                .Append(stat.DOScale(_scaleEndValue, _scaleDuration))
                .Append(stat.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato))
                .AppendInterval(2f)
                .Append(stat.DOScale(_scaleDefaultValue, _scaleDuration)).SetEase(Ease.OutSine)
                .Append(statTextElem.DOColor(_textBaseColor, _colorChangeDuration));
        }
    }
}