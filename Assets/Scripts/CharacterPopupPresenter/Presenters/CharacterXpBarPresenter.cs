using System;
using CharacterPopupPresenter.IPresenters;
using Player;
using UniRx;

namespace CharacterPopupPresenter.Presenters
{
    public class CharacterXpBarPresenter : ICharacterXpBarPresenter, IDisposable
    {
        public IReadOnlyReactiveProperty<string> XpFullStr => _xpFullStr;
        public IReadOnlyReactiveProperty<bool> IsXpBarFull => _isXpBarFull;
        public IReadOnlyReactiveProperty<uint> CurrentXp => _currentXp;
        public IReadOnlyReactiveProperty<uint> MaxBarXp => _maxBarXp;
        
        private readonly ReactiveProperty<string> _xpFullStr;
        private readonly ReactiveProperty<bool> _isXpBarFull;
        private readonly ReactiveProperty<uint> _currentXp;
        private readonly ReactiveProperty<uint> _maxBarXp;
        
        private readonly PlayerLevel _playerLevel;

        public CharacterXpBarPresenter(PlayerLevel playerLevel, uint currentXp, uint maxBarXp)
        {
            _playerLevel = playerLevel;
            
            _xpFullStr = new StringReactiveProperty(ConvertXpToViewFormat(currentXp, maxBarXp));
            _isXpBarFull = new BoolReactiveProperty(currentXp == maxBarXp);
            _currentXp = new ReactiveProperty<uint>(currentXp);
            _maxBarXp = new ReactiveProperty<uint>(maxBarXp);

            SubscribeToModelChange();
        }
        
        public void Dispose()
        {
            UnsubscribeToModelChange();
        }
        
        private void SubscribeToModelChange()
        {
            _playerLevel.OnExperienceChanged += HandleXpChange;
            _playerLevel.OnMaxExperienceChanged += HandleMaxXpChange;
        }
        
        private void UnsubscribeToModelChange()
        {
            _playerLevel.OnExperienceChanged -= HandleXpChange;
            _playerLevel.OnMaxExperienceChanged -= HandleMaxXpChange;
        }
        
        private void HandleXpChange(uint xpNumb)
        {
            _currentXp.Value = xpNumb;
            _xpFullStr.Value = ConvertXpToViewFormat(xpNumb, _maxBarXp.Value);
            _isXpBarFull.Value = _currentXp.Value == _maxBarXp.Value;
        }
        
        private void HandleMaxXpChange(uint xpNumb)
        {
            _maxBarXp.Value = xpNumb;
        }
        
        private string ConvertXpToViewFormat(uint currentXp, uint maxBarXp)
        {
            return $"XP: {currentXp} / {maxBarXp}";
        }
    }
}