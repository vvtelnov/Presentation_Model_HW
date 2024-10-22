using CharacterPopupPresenter.IPresenters;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterPopupView
{
    public class CharacterXpBarPopupView : BasePopupView<ICharacterXpBarPresenter>
    {
        [Title("Character XP")]
        [SerializeField] private TMP_Text _xp;
        [SerializeField] private GameObject _fullBar;
        [SerializeField] private GameObject _notFullBar;
        [SerializeField] private Image _notFullBarImage;
        
        private uint _maxBarXp;
        private uint _currentXp;
        private bool _isXpBarFull;
        
        protected override void Show(ICharacterXpBarPresenter args)
        {
            args.XpFullStr.SkipLatestValueOnSubscribe().Subscribe(OnXpChanged).AddTo(disposable);
            args.MaxBarXp.SkipLatestValueOnSubscribe().Subscribe(OnMaxBarXpChanged).AddTo(disposable);
            args.CurrentXp.SkipLatestValueOnSubscribe().Subscribe(OnCurrentXpChange).AddTo(disposable);
            args.IsXpBarFull.Subscribe(UpdateIsXpBarFull).AddTo(disposable);
            
            _xp.text = args.XpFullStr.Value;
            _maxBarXp = args.MaxBarXp.Value;
            _currentXp = args.CurrentXp.Value;
            UpdateXpBarFillingWidth();
        }
        
        private void OnCurrentXpChange(uint newCurrXp)
        {
            _currentXp = newCurrXp;
            UpdateXpBarFillingWidth();
        }
        
        private void OnXpChanged(string newXpFullStr)
        {
            _xp.text = newXpFullStr;
        }
        
        private void OnMaxBarXpChanged(uint newMaxValue)
        {
            _maxBarXp = newMaxValue;
        }
        
        private void UpdateIsXpBarFull(bool isXpBarFull)
        {
            if (isXpBarFull)
            {
                _fullBar.SetActive(true); 
                _notFullBar.SetActive(false);

                return;
            }

            _fullBar.SetActive(false); 
            _notFullBar.SetActive(true);
            UpdateXpBarFillingWidth();
        }
        
        private void UpdateXpBarFillingWidth()
        {
            float percentageOfFilling = (float)_currentXp / _maxBarXp;

            _notFullBarImage.fillAmount = percentageOfFilling;
        }
    }
}