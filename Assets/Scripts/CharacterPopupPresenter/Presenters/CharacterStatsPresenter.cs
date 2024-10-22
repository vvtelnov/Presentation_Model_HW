using System;
using CharacterPopupPresenter.IPresenters;
using Player;
using UniRx;

namespace CharacterPopupPresenter.Presenters
{
    public class CharacterStatsPresenter : ICharacterStatsPresenter, IDisposable
    {
        public IReadOnlyReactiveCollection<StringReactiveProperty> Stats => _stats;

        private readonly ReactiveCollection<StringReactiveProperty> _stats;
        
        private readonly CharacterStats _characterStats;
        private readonly bool _isStaticOrder;

        public CharacterStatsPresenter(CharacterStats characterStats, CharacterStat[] stats)
        {
            _characterStats = characterStats;
            _stats = new ReactiveCollection<StringReactiveProperty>();

            SetUpCharStatsForViewFormat(stats);
            SubscribeToModelChange();
        }

        public void Dispose()
        {
            UnsubscribeToModelChange();
        }
        
        private void SubscribeToModelChange()
        {
            _characterStats.OnStatAdded += AddNewCharStat;
            _characterStats.OnStatRemoved += HandleRemoveCharStat;
            _characterStats.OnStatValueChanged += HandleStatValueChange;
        }
        
        private void UnsubscribeToModelChange()
        {
            _characterStats.OnStatAdded -= AddNewCharStat;
            _characterStats.OnStatRemoved -= HandleRemoveCharStat;
            _characterStats.OnStatValueChanged -= HandleStatValueChange;
        }
        
        private void SetUpCharStatsForViewFormat(CharacterStat[] stats)
        {
            foreach (var stat in stats)
            {
                AddNewCharStat(stat);
            }
        }

        private void AddNewCharStat(CharacterStat stat)
        {
            StringReactiveProperty viewFormat = new StringReactiveProperty($"{stat.Name}: {stat.Value}");

            int index = (int)stat.StatType;
            _stats.Insert(index, viewFormat);
            
        }
        
        private void HandleRemoveCharStat(CharacterStat stat)
        {
            int statIndex = (int)stat.StatType;
            StringReactiveProperty value = _stats[statIndex];
            
            value.Dispose();
            _stats.RemoveAt(statIndex);
        }

        private void HandleStatValueChange(CharacterStat stat)
        {
            int index = (int)stat.StatType;
            _stats[index].Value = $"{stat.Name}: {stat.Value}";
        }
    }
}