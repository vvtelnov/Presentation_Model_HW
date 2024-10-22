using System;
using System.Collections.Generic;
using System.Linq;

namespace Player
{
    public enum Stats
    {
        MOVE_SPEED = 0,    
        STAMINA = 1,
        DEXTERITY = 2,
        INTELLIGENCE = 3,
        DAMAGE = 4,
        REGENERATION = 5,
    }

    public sealed class CharacterStats : ICharacterStatsModelSetter
    {
        public event Action<CharacterStat> OnStatAdded;
        public event Action<CharacterStat> OnStatRemoved;
        public event Action<CharacterStat> OnStatValueChanged;

        private readonly HashSet<CharacterStat> _stats = new();

        public void AddStat(CharacterStat stat)
        {
            if (this._stats.Add(stat))
            {
                this.OnStatAdded?.Invoke(stat);
            }
        }

        public void RemoveStat(CharacterStat stat)
        {
            if (this._stats.Remove(stat))
            {
                this.OnStatRemoved?.Invoke(stat);
            }
        }

        public void IncreaseStat(CharacterStat stat, uint increaseValue)
        {
            uint newValue = stat.Value + increaseValue;
            stat.ChangeValue(newValue);
            OnStatValueChanged?.Invoke(stat);
        }

        public void DecreaseStat(CharacterStat stat, uint decreaseValue)
        {
            var newValue = (uint)Math.Max((int)(stat.Value - decreaseValue), 0);
            stat.ChangeValue(newValue);
            OnStatValueChanged?.Invoke(stat);
        }

        public CharacterStat GetStat(Stats statType)
        {
            foreach (var stat in this._stats)
            {
                if (stat.StatType == statType)
                {
                    return stat;
                }
            }

            throw new Exception($"Stat {statType} is not found!");
        }

        public void ChangeStatValue(Stats statType, int value)
        {
            CharacterStat stat = GetStat(statType);

            if ((stat.Value + value) < 0)
            {
                throw new Exception($"You tried to change stat {statType} value to value bellow zero");
            }
            
            stat.ChangeValue((uint)(stat.Value + value));
            OnStatValueChanged?.Invoke(stat);
        }

        public CharacterStat[] GetStats()
        {
            return this._stats.ToArray();
        }
    }
}