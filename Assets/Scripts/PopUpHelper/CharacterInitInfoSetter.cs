using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace PopUpHelper
{
    public class CharacterInitInfoSetter : MonoBehaviour
    {
        [Title("Input Character stats")]
        [SerializeField] private uint _moveSpeed;    
        [SerializeField] private uint _stamina;
        [SerializeField] private uint _dexterity;
        [SerializeField] private uint _intelligence;    
        [SerializeField] private uint _damage;    
        [SerializeField] private uint _regeneration;

        [Title("Input Character Info")]
        [SerializeField] private string _name;
        [SerializeField] private Sprite _profilePicture;
        [TextArea(3, 10), SerializeField] private string _description;

        [Title("Input Character XP")]
        [SerializeField]
        private uint _lvl;
        [SerializeField, PropertyRange(0, 1000)]
        private uint _xp;

        private readonly Dictionary<Stats, uint> _initStats = new();
        
        [Inject]
        public void SetInitInfo(IPlayerXpModelSetter playerLevelSetter, 
            IUserInfoModelSetter userInfoSetter, 
            ICharacterStatsModelSetter characterStatsSetter
            )
        {
            playerLevelSetter.CurrentLevel = _lvl;
            playerLevelSetter.CurrentExperience = _xp;
            
            userInfoSetter.ChangeName(_name);
            userInfoSetter.ChangeDescription(_description);
            userInfoSetter.ChangeIcon(_profilePicture);
            
            
            characterStatsSetter.AddStat(new CharacterStat (Stats.MOVE_SPEED, _moveSpeed));
            characterStatsSetter.AddStat(new CharacterStat (Stats.STAMINA, _stamina));
            characterStatsSetter.AddStat(new CharacterStat (Stats.DEXTERITY, _dexterity));
            characterStatsSetter.AddStat(new CharacterStat (Stats.INTELLIGENCE, _intelligence));
            characterStatsSetter.AddStat(new CharacterStat (Stats.DAMAGE, _damage));
            characterStatsSetter.AddStat(new CharacterStat (Stats.REGENERATION, _regeneration));
        }
    }
}