using System;
using CharacterPopupPresenter.IPresenters;
using Player;
using UniRx;
using UnityEngine;

namespace CharacterPopupPresenter.Presenters
{
    public class CharacterInfoPresenter : ICharacterInfoPresenter, IDisposable
    {
        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        
        private readonly ReactiveProperty<string> _name;
        private readonly ReactiveProperty<string> _description;
        private readonly ReactiveProperty<Sprite> _icon;
        
        private readonly UserInfo _userInfo;

        
        public CharacterInfoPresenter(UserInfo userInfo, 
            string name,
            string description,
            Sprite icon)
        {
            _userInfo = userInfo;
            
            _name = new StringReactiveProperty(ConvertNameToViewFormat(name));
            _description = new StringReactiveProperty(description);
            _icon = new ReactiveProperty<Sprite>(icon);
            
            SubscribeToModelChange();
        }
        
        public void Dispose()
        {
            UnsubscribeToModelChange();
        }

        private void SubscribeToModelChange()
        {
            _userInfo.OnNameChanged += HandleNameChange;
            _userInfo.OnDescriptionChanged += HandleDescriptionChange;
            _userInfo.OnIconChanged += HandleIconChange;
        }

        private void UnsubscribeToModelChange()
        {
            _userInfo.OnNameChanged -= HandleNameChange;
            _userInfo.OnDescriptionChanged -= HandleDescriptionChange;
            _userInfo.OnIconChanged -= HandleIconChange;
        }

        private void HandleNameChange(string name)
        {
            _name.Value = ConvertNameToViewFormat(name);
        }
        
        private string ConvertNameToViewFormat(string name)
        {
            return string.Concat("@", name);
        }
        
        private void HandleDescriptionChange(string description)
        {
            _description.Value = description;
        }

        private void HandleIconChange(Sprite icon)
        {
            _icon.Value = icon;
        }
    }
}