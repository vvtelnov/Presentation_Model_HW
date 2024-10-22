using System;
using System.Collections.Generic;
using CharacterPopupPresenter.IPresenters;
using CharacterPopupPresenter.Presenters;
using Player;

namespace CharacterPopupPresenter.PresentersFactory
{
    public class CharacterPopupPresentersFactory : ICharacterPresenterFactory
    {
        public List<Type> СanCreatePresenters { get; } = new List<Type>
        {
            typeof(ICharacterInfoPresenter),
            typeof(ICharacterLvlPresenter),
            typeof(ICharacterLvlUpPresenter),
            typeof(ICharacterXpBarPresenter),
            typeof(ICharacterStatsPresenter)
        };
        
        private readonly PlayerLevel _playerLevel;
        private readonly UserInfo _userInfo;
        private readonly CharacterStats _characterStats;

        public CharacterPopupPresentersFactory(PlayerLevel playerLevel, UserInfo userInfo, CharacterStats characterStats)
        {
            _playerLevel = playerLevel;
            _userInfo = userInfo;
            _characterStats = characterStats;
        }

        public IPresenter CreatePresenter(Type presenterType)
        {
            if (presenterType == typeof(ICharacterInfoPresenter))
                return CreateInfoPresenter();

            if (presenterType == typeof(ICharacterLvlPresenter))
                return CreateLvlPresenter();

            if (presenterType == typeof(ICharacterLvlUpPresenter))
                return CreateLvlUpPresenter();

            if (presenterType == typeof(ICharacterXpBarPresenter))
                return CreateXpBarPresenter();

            if (presenterType == typeof(ICharacterStatsPresenter))
                return CreateStatsPresenter();

            throw new ArgumentException($"Presenter of type {presenterType} is not supported.");
        }

        public ICharacterInfoPresenter CreateInfoPresenter()
        {
            return new CharacterInfoPresenter(userInfo: _userInfo,
                name: _userInfo.Name,
                description: _userInfo.Description,
                icon: _userInfo.Icon
                );
        }
        
        public ICharacterLvlPresenter CreateLvlPresenter()
        {
            return new CharacterLvlPresenter(_playerLevel, _playerLevel.CurrentLevel);
        }
        
        public ICharacterLvlUpPresenter CreateLvlUpPresenter()
        {
            return new CharacterLvlUpPresenter(_playerLevel, _playerLevel.CanLevelUp);
        }
        
        public ICharacterXpBarPresenter CreateXpBarPresenter()
        {
            return new CharacterXpBarPresenter(_playerLevel, 
                _playerLevel.CurrentExperience, 
                _playerLevel.RequiredExperience
                );
        }
        
        public ICharacterStatsPresenter CreateStatsPresenter()
        {
            var stats = _characterStats.GetStats();
            
            return new CharacterStatsPresenter(_characterStats, stats);
        }
    }
}