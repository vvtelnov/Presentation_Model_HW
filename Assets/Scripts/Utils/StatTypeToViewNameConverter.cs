using System;
using System.Collections.Generic;
using Player;

namespace Utils
{
    public static class StatTypeToViewNameConverter
    {
        private static readonly Dictionary<Stats, string> _statTypeToViewNameMap = new()
        {
            { Stats.MOVE_SPEED,  "Move Speed"},
            { Stats.STAMINA,  "Stamina"},
            { Stats.DEXTERITY, "Dexterity"},
            { Stats.INTELLIGENCE,  "Intelligence"},
            { Stats.DAMAGE,  "Damage"},
            { Stats.REGENERATION, "Regeneration"},
        };

        public static string GetStatViewFormat(Stats statType)
        {
            return _statTypeToViewNameMap[statType];
        }

        public static Stats GetStatType(string stringStat)
        {
            foreach (KeyValuePair<Stats, string> pair in _statTypeToViewNameMap)
            {
                if (pair.Value == stringStat)
                {
                    return pair.Key;
                }
            }

            throw new ArgumentException("Passed string stat does not have a corresponding stat type");
        }
    }
}