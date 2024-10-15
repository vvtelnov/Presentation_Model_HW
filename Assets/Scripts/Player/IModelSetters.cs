using UnityEngine;

namespace Player
{
    public interface IUserInfoModelSetter
    {
        public void ChangeName(string name);
        public void ChangeDescription(string description);
        public void ChangeIcon(Sprite icon);
    }
    
    public interface IPlayerXpModelSetter
    {
        public uint CurrentLevel { get; set; }
        public uint CurrentExperience { get; set; }
        public void AddExperience(int range);
        public void LevelUp();
    }
    
    public interface ICharacterStatsModelSetter
    {
        public void AddStat(CharacterStat stat);
        public void ChangeStatValue(Stats statType, int value);
        public void IncreaseStat(CharacterStat stat, uint increaseValue);
        public void DecreaseStat(CharacterStat stat, uint decreaseValue);

    }
}