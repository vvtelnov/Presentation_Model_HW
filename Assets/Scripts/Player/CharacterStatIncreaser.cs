using System;

namespace Player
{
    public class CharacterStatIncreaser : IDisposable
    {
        private CharacterStats _characterStats;
        private PlayerLevel _playerLevel;
        
        private uint _levelUpStatIncrement = 5;
        

        public CharacterStatIncreaser(CharacterStats characterStats, PlayerLevel playerLevel)
        {
            _characterStats = characterStats;
            _playerLevel = playerLevel;

            _playerLevel.OnLevelUp += IncrementAllStats;
        }

        void IDisposable.Dispose()
        {
            _playerLevel.OnLevelUp -= IncrementAllStats;
        }

        private void IncrementAllStats(uint _)
        {
            foreach (var iStat in _characterStats.GetStats())
            {
                _characterStats.IncreaseStat(iStat, _levelUpStatIncrement);
            }
        }
    }
}