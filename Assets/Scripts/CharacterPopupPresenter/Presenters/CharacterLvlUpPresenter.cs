using System;
using CharacterPopupPresenter.IPresenters;
using Player;
using UI.CharacterPopupView;
using UniRx;

namespace CharacterPopupPresenter.Presenters
{
    public class CharacterLvlUpPresenter : ICharacterLvlUpPresenter, 
        IDisposable, IViewEventsListener
    {
        public IReadOnlyReactiveProperty<bool> CanLevelUp => _canLevelUp;

        private readonly ReactiveProperty<bool> _canLevelUp;
        private readonly PlayerLevel _playerLevel;
        private ICharacterLvlUpView _characterLvlUpView;

        public CharacterLvlUpPresenter(PlayerLevel playerLevel, bool canLevelUp)
        {
            _playerLevel = playerLevel;
            _canLevelUp = new BoolReactiveProperty(canLevelUp);

            SubscribeToModelChange();
        }
        
        public void SubscribeToViewEvents(IViewEventEmmiter view)
        {
            if (view is not ICharacterLvlUpView eventEmitterView)
            {
                throw new Exception("Wrong view instance passed");
            }
            
            _characterLvlUpView = eventEmitterView;
            
            _characterLvlUpView.OnLvlUpClicked += HandleLevelUp;
        }
        
        public void Dispose()
        {
            UnsubscribeToModelChange();
            UnsubscribeToViewEvents();
        }
        
        private void SubscribeToModelChange()
        {
            _playerLevel.OnCanLevelUp += HandleCanLevelUp;
        }
        
        private void UnsubscribeToModelChange()
        {
            _playerLevel.OnCanLevelUp -= HandleCanLevelUp;
        }
        
        private void UnsubscribeToViewEvents()
        {
            _characterLvlUpView.OnLvlUpClicked -= HandleLevelUp;
        }
        
        private void HandleCanLevelUp(bool canLvlUp)
        {
            _canLevelUp.Value = canLvlUp;
        }
        
        private void HandleLevelUp()
        {
            _playerLevel.LevelUp();
        }
    }
}