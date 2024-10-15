using System;
using Player;
using PopupView;
using ScriptableObjects;
using UniRx;
using UnityEngine;

namespace CharacterPopupPresenter
{
    public class PopupPresenter : ICharacterPopupPresenter,
        IEventsubscriberPresenter,
        IDisposable
    {
        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        
        public IReadOnlyReactiveProperty<string> CurrentLevel => _currentLevel;
        public IReadOnlyReactiveProperty<string> XpFullStr => _xpFullStr;
        public IReadOnlyReactiveProperty<bool> IsXpBarFull => _isXpBarFull;
        public IReadOnlyReactiveProperty<bool> CanLevelUp => _canLevelUp;
        public IReadOnlyReactiveProperty<uint> CurrentXp => _currentXp;
        public IReadOnlyReactiveProperty<uint> MaxBarXp => _maxBarXp;
        public ReactiveDictionary<Stats, StringReactiveProperty> CharacterStatsMapToReactiveProperties { get; }

        public CharacterPopupElements PopupElements { get; private set; }
        
        private readonly ReactiveProperty<string> _name;
        private readonly ReactiveProperty<string> _description;
        private readonly ReactiveProperty<Sprite> _icon;
        private readonly ReactiveProperty<string> _currentLevel;
        private readonly ReactiveProperty<string> _xpFullStr;
        private readonly ReactiveProperty<bool> _isXpBarFull;
        private readonly ReactiveProperty<bool> _canLevelUp;
        private readonly ReactiveProperty<uint> _currentXp;
        private readonly ReactiveProperty<uint> _maxBarXp;

        private readonly PlayerLevel _playerLevel;
        private readonly UserInfo _userInfo;
        private readonly CharacterStats _characterStats;

        private IPopupEventEmitter _viewEventEmitter;

        
        public PopupPresenter(CharacterStat[] stats,
            string name,
            string description,
            Sprite icon,
            uint currentLevel,
            uint currentXp,
            uint maxBarXp,
            bool canLevelUp,
            PlayerLevel playerLevel, 
            UserInfo userInfo, 
            CharacterStats characterStats,
            CharacterPopupElements popupElements
            )
        {
            _playerLevel = playerLevel;
            _userInfo = userInfo;
            _characterStats = characterStats;
            PopupElements = popupElements;

            _name = new StringReactiveProperty(ConvertNameToViewFormat(name));
            _description = new StringReactiveProperty(description);
            _icon = new ReactiveProperty<Sprite>(icon);
            _currentLevel = new StringReactiveProperty(ConvertLevelToViewFormat(currentLevel));
            _xpFullStr = new StringReactiveProperty(ConvertXpToViewFormat(currentXp, maxBarXp));
            _isXpBarFull = new BoolReactiveProperty(currentXp == maxBarXp);
            _canLevelUp = new BoolReactiveProperty(canLevelUp);
            
            _currentXp = new ReactiveProperty<uint>(currentXp);
            _maxBarXp = new ReactiveProperty<uint>(maxBarXp);

            CharacterStatsMapToReactiveProperties = new ReactiveDictionary<Stats, StringReactiveProperty>();
            SetUpCharStatsForViewFormat(stats);

            SubscribeToModelChange();
        }

        void IEventsubscriberPresenter.SubscribeToViewEvents(IPopupEventEmitter viewEventEmitter)
        {
            _viewEventEmitter = viewEventEmitter;
            
            viewEventEmitter.OnClose += Dispose;
            viewEventEmitter.OnActionButtonClick += HandleLevelUp;
        }

        private void SubscribeToModelChange()
        {
            _userInfo.OnNameChanged += HandleNameChange;
            _userInfo.OnDescriptionChanged += HandleDescriptionChange;
            _userInfo.OnIconChanged += HandleIconChange;

            _playerLevel.OnLevelUp += HandleIncreaseLevel;
            _playerLevel.OnExperienceChanged += HandleXpChange;
            _playerLevel.OnMaxExperienceChanged += HandleMaxXpChange;
            _playerLevel.OnCanLevelUp += HandleCanLevelUp;

            _characterStats.OnStatAdded += HandleAddNewCharStat;
            _characterStats.OnStatRemoved += HandleRemoveCharStat;
            _characterStats.OnStatValueChanged += HandleStatValueChange;
        }
        
        private void UnsubscribeToModelChange()
        {
            _userInfo.OnNameChanged -= HandleNameChange;
            _userInfo.OnDescriptionChanged -= HandleDescriptionChange;
            _userInfo.OnIconChanged -= HandleIconChange;

            _playerLevel.OnLevelUp -= HandleIncreaseLevel;
            _playerLevel.OnExperienceChanged -= HandleXpChange;

            _characterStats.OnStatAdded -= HandleAddNewCharStat;
            _characterStats.OnStatRemoved -= HandleRemoveCharStat;
            _characterStats.OnStatValueChanged -= HandleStatValueChange;
        }
        
        private void HandleNameChange(string name)
        {
            _name.Value = ConvertNameToViewFormat(name);
        }

        private void HandleDescriptionChange(string description)
        {
            _description.Value = description;
        }

        private void HandleIconChange(Sprite icon)
        {
            _icon.Value = icon;
        }
        
        private void HandleXpChange(uint xpNumb)
        {
            _currentXp.Value = xpNumb;
            _xpFullStr.Value = ConvertXpToViewFormat(xpNumb, _maxBarXp.Value);
            _isXpBarFull.Value = _currentXp.Value == _maxBarXp.Value;
        }

        private void HandleCanLevelUp(bool canLvlUp)
        {
            _canLevelUp.Value = canLvlUp;
        }

        private void HandleMaxXpChange(uint xpNumb)
        {
            _maxBarXp.Value = xpNumb;
        }

        private void HandleLevelUp()
        {
            _playerLevel.LevelUp();
        }

        private void HandleIncreaseLevel(uint nextLevel)
        {
            _currentLevel.Value = ConvertLevelToViewFormat(nextLevel);
        }
        
        private void HandleAddNewCharStat(CharacterStat stat)
        {
            StringReactiveProperty viewFormat = new StringReactiveProperty($"{stat.Name}: {stat.Value}"); 

            CharacterStatsMapToReactiveProperties.Add(stat.StatType, viewFormat);
        }
        
        private void HandleRemoveCharStat(CharacterStat stat)
        {
            StringReactiveProperty value;
            
            if (!CharacterStatsMapToReactiveProperties.TryGetValue(stat.StatType, out value))
            {
                throw new Exception($"There is no {stat.StatType} in dict to remove");
            }
            
            value.Dispose();
            CharacterStatsMapToReactiveProperties.Remove(stat.StatType);
        }

        private void HandleStatValueChange(CharacterStat stat)
        {
            if (!CharacterStatsMapToReactiveProperties.TryGetValue(stat.StatType, out var value))
            {
                throw new Exception($"There is no {stat.StatType} in {CharacterStatsMapToReactiveProperties} dict");
            }
            
            value.Value = $"{stat.Name}: {stat.Value}";
        }

        private void SetUpCharStatsForViewFormat(CharacterStat[] stats)
        {
            foreach (var stat in stats)
            {
                HandleAddNewCharStat(stat);
            }
        }

        private string ConvertNameToViewFormat(string name)
        {
            return string.Concat("@", name);
        }
        
        private string ConvertLevelToViewFormat(uint currentLevel)
        {
            return $"Level: {currentLevel}";
        }
        
        private string ConvertXpToViewFormat(uint currentXp, uint maxBarXp)
        {
            return $"XP: {currentXp} / {maxBarXp}";
        }

        public void Dispose()
        {
            if (_viewEventEmitter is not null)
            {
                _viewEventEmitter.OnClose -= Dispose;
                _viewEventEmitter.OnActionButtonClick -= HandleLevelUp;
            }

            UnsubscribeToModelChange();
        }
    }
}