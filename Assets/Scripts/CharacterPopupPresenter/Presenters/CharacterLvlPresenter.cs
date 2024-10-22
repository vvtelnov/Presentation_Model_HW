using System;
using CharacterPopupPresenter.IPresenters;
using Player;
using UniRx;

namespace CharacterPopupPresenter.Presenters
{
    public class CharacterLvlPresenter : ICharacterLvlPresenter, IDisposable
    {
        public IReadOnlyReactiveProperty<string> CurrentLevel => _currentLevel;
        
        private readonly ReactiveProperty<string> _currentLevel;
        private readonly PlayerLevel _playerLevel;

        public CharacterLvlPresenter(PlayerLevel playerLevel, uint currentLevel)
        {
            _currentLevel = new StringReactiveProperty(ConvertLevelToViewFormat(currentLevel));
            _playerLevel = playerLevel;

            SubscribeToModelChange();
        }
        
        public void Dispose()
        {
            UnsubscribeToModelChange();
        }
        
        private void SubscribeToModelChange()
        {
            _playerLevel.OnLevelUp += HandleIncreaseLevel;
        }
        
        private void UnsubscribeToModelChange()
        {
            _playerLevel.OnLevelUp -= HandleIncreaseLevel;

        }
        
        private string ConvertLevelToViewFormat(uint currentLevel)
        {
            return $"Level: {currentLevel}";
        }
        
        private void HandleIncreaseLevel(uint nextLevel)
        {
            _currentLevel.Value = ConvertLevelToViewFormat(nextLevel);
        }
    }
}