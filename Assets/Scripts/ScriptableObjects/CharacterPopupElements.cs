using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterPopupElements", menuName = "Data/New CharacterPopupElements")]
    public class CharacterPopupElements : ScriptableObject
    {
        [Title("Character Fields")] 
        [field: SerializeField] public TMP_Text Name;
        public TMP_Text CurrentLevel;
        public TMP_Text Description;
        public Image ProfilePicture;
        
        [Title("Character XP")]
        [SerializeField] public TMP_Text Xp;
        [SerializeField] public GameObject FullBar;
        [SerializeField] public GameObject NotFullBar;
        [SerializeField] public Image NotFullBarImage;


        [Title("Character Stats")]
        [SerializeField] public TMP_Text MoveSpeedStat;
        [SerializeField] public TMP_Text StaminaStat;
        [SerializeField] public TMP_Text DexterityStat;
        [SerializeField] public TMP_Text IntelligenceStat;
        [SerializeField] public TMP_Text DamageStat;
        [SerializeField] public TMP_Text RegenerationStat;
        
        [Title("SubmitBtn")]
        [SerializeField] public GameObject InactiveBtn;
        [SerializeField] public GameObject ActiveBtn;

        [Title("Buttons")]
        [SerializeField] public Button CloseBtn;
        [SerializeField] public Button SubmitBtn;
    }
}