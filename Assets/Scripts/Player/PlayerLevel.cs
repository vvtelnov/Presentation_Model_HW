using System;

namespace Player
{
    public sealed class PlayerLevel : IPlayerXpModelSetter
    {
        public event Action<uint> OnLevelUp;
        public event Action<bool> OnCanLevelUp;
        public event Action<uint> OnExperienceChanged;
        public event Action<uint> OnMaxExperienceChanged;

        public uint CurrentLevel { get; set; } = 1;

        public uint CurrentExperience { get; set; }

        public uint RequiredExperience
        {
            get { return 100 * (this.CurrentLevel + 1); }
        }

        public bool CanLevelUp
        {
            get { return this.CurrentExperience == this.RequiredExperience; }
        }

        public void AddExperience(int range)
        {
            var xp = Math.Min(this.CurrentExperience + range, this.RequiredExperience);

            if (xp < 0)
                throw new InvalidOperationException("Xp Cannot be less than 0");

            this.CurrentExperience = (uint)xp;
            this.OnExperienceChanged?.Invoke((uint)xp);
            
            this.OnCanLevelUp?.Invoke(CanLevelUp);
        }

        public void LevelUp()
        {
            if (this.CanLevelUp)
            {
                this.CurrentExperience = 0;
                this.CurrentLevel++;
                this.OnLevelUp?.Invoke(this.CurrentLevel);
                this.OnExperienceChanged?.Invoke(CurrentExperience);
                this.OnMaxExperienceChanged?.Invoke(RequiredExperience);
                this.OnCanLevelUp?.Invoke(CanLevelUp);
            }
        }
    }
}