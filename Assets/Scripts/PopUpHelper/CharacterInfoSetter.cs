using Player;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace PopUpHelper
{
    public class CharacterInfoSetter : MonoBehaviour
    {
        [Title(title: "Change Player Info", 
            subtitle: "After changing these values popup changes automatically")]
        [SerializeField] private StringReactiveProperty _newName = new();
        [SerializeField] private StringReactiveProperty _newDescription = new();
        [Tooltip("1 - Uther; 2 - Garrosh; 3 - Jaine")]
        [SerializeField] private IntReactiveProperty _newProfileImageNumb = new();
        
        private IUserInfoModelSetter _userInfoSetter;  
        private IPlayerXpModelSetter _playerXpSetter;  
        private ICharacterStatsModelSetter _charStatsSetter;

        private CompositeDisposable _disposable;

        [Title(title: "Character's icons locator")]
        [SerializeField] private Sprite _utherIcon;
        [SerializeField] private Sprite _garroshIcon;
        [SerializeField] private Sprite _jaineIcon;

        [Inject]
        public void Construct(IUserInfoModelSetter userInfoSetter, IPlayerXpModelSetter playerXpSetter, ICharacterStatsModelSetter charInfoSetter)
        {
            _userInfoSetter = userInfoSetter;
            _playerXpSetter = playerXpSetter;
            _charStatsSetter = charInfoSetter;

            _disposable = new CompositeDisposable();
            
            _newName.SkipLatestValueOnSubscribe()
                .Subscribe(ChangeName)
                .AddTo(_disposable);
            
            _newDescription.SkipLatestValueOnSubscribe()
                .Subscribe(ChangeDescription)
                .AddTo(_disposable);
            
            _newProfileImageNumb.SkipLatestValueOnSubscribe()
                .Subscribe(ChangeCharImage)
                .AddTo(_disposable);
        }

        private void IncreaseStat(Stats stat, int increment = 1)
        {
            _charStatsSetter.ChangeStatValue(stat, increment);
        }

        private void DecreaseStat(Stats stat, int decrement = -1)
        {
            _charStatsSetter.ChangeStatValue(stat, decrement);
        }

        private void ChangeName(string charName)
        {
            if (charName is null)
            {
                // Переодически вызывается, когда изменяется значение
                Debug.LogError("The NewName field must not be null");
            }

            _userInfoSetter.ChangeName(charName);
        }

        private void ChangeDescription(string description)
        {
            if (description is null)
            {
                Debug.LogError("The newDescription field must not be null");
            }
            
            _userInfoSetter.ChangeDescription(description);
        } 
        
        private void ChangeCharImage(int imageNumb)
        {
            switch (imageNumb)
            {
                case 1:
                    _userInfoSetter.ChangeIcon(_utherIcon);
                    return;
                case 2:
                    _userInfoSetter.ChangeIcon(_garroshIcon);
                    return;
                case 3:
                    _userInfoSetter.ChangeIcon(_jaineIcon);
                    return;
                
                default:
                    Debug.LogError("The newProfileImageNumb field must be (> 0 && <= 3)");
                    return;
            }
        }


        [Title("Set XP")]
        [SerializeField]
        private int _xpValue;

        [Button(name: "Increase XP by value")]
        private void IncreaseXpBy_fieldXpValue()
        {
            if (_xpValue < 0)
                Debug.LogError("XpValue Cannot be less than 0");

            _playerXpSetter.AddExperience(_xpValue);
        }

        [Title("Increase stats by one")]
        [Button]
        private void IncreaseMoveSpeed()
        {
            IncreaseStat(Stats.MOVE_SPEED);
        }
        
        [Button]
        private void IncreaseStamina()
        {
            IncreaseStat(Stats.STAMINA);
        }
        
        [Button]
        private void IncreaseDexterity()
        {
            IncreaseStat(Stats.DEXTERITY);
        }
        
        [Button]
        private void IncreaseIntelligence()
        {
            IncreaseStat(Stats.INTELLIGENCE);
        }

        [Button]
        private void IncreaseDamage()
        {
            IncreaseStat(Stats.DAMAGE);
        }

        [Button]
        private void IncreaseRegeneration()
        {
            IncreaseStat(Stats.REGENERATION);
        }
        
        
        [Title("Decrease stats by one")]
        [Button]
        private void DecreaseMoveSpeed()
        {
            DecreaseStat(Stats.MOVE_SPEED);
        }
        
        [Button]
        private void DecreaseStamina()
        {
            DecreaseStat(Stats.STAMINA);
        }
        
        [Button]
        private void DecreaseDexterity()
        {
            DecreaseStat(Stats.DEXTERITY);
        }
        
        [Button]
        private void DecreaseIntelligence()
        {
            DecreaseStat(Stats.INTELLIGENCE);
        }

        [Button]
        private void DecreaseDamage()
        {
            DecreaseStat(Stats.DAMAGE);
        }

        [Button]
        private void DecreaseRegeneration()
        {
            DecreaseStat(Stats.REGENERATION);
        }
    }
}