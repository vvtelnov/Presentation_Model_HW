using System;
using CharacterPopupPresenter;
using DG.Tweening;
using Player;
using ScriptableObjects;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PopupView
{
    public class CharacterPopupView : MonoBehaviour, IPopupView, IPopupEventEmitter
    {
        public event Action OnActionButtonClick;
        public event Action OnClose;
        
        [Title("Character Fields")]
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _currentLevel;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _profilePicture;
        
        [Title("Character XP")]
        [SerializeField] private TMP_Text _xp;
        [SerializeField] private GameObject _fullBar;
        [SerializeField] private GameObject _notFullBar;
        [SerializeField] private Image _notFullBarImage;


        [Title("Character Stats")]
        [SerializeField] private TMP_Text _moveSpeedStat;
        [SerializeField] private TMP_Text _staminaStat;
        [SerializeField] private TMP_Text _dexterityStat;
        [SerializeField] private TMP_Text _intelligenceStat;
        [SerializeField] private TMP_Text _damageStat;
        [SerializeField] private TMP_Text _regenerationStat;
        
        [Title("SubmitBtn")]
        [SerializeField] private GameObject _inactiveBtn;
        [SerializeField] private GameObject _activeBtn;

        [Title("Buttons")]
        [SerializeField] private Button _closeBtn;
        [SerializeField] private Button _submitBtn;

        private uint _maxBarXp;
        private uint _currentXp;
        private bool _isXpBarFull;
        private bool _canLevelUp;

        private Transform _profilePictureTransform;
        private Transform _levelTransform;
        private Transform _moveSpeedStatTransform;
        private Transform _staminaStatTransform;
        private Transform _dexterityStatTransform;
        private Transform _intelligenceStatTransform;
        private Transform _damageStatTransform;
        private Transform _regenerationStatTransform;

        private CompositeDisposable _disposable;
        
        private Color _increseColor = new Color32(0, 206, 44, 255);
        private Color _decreseColor = new Color32(255, 73, 85, 255);
        private Color _textBaseColor = new Color32(25, 25, 25, 255);
        
        void IPopupView.Open(ICharacterPopupPresenter args)
        {
            SetPopupElements(args.PopupElements);
            
            _disposable = new CompositeDisposable();
            
            args.Name.SkipLatestValueOnSubscribe().Subscribe(OnNameChanged).AddTo(_disposable);
            args.Description.SkipLatestValueOnSubscribe().Subscribe(OnDescriptionChanged).AddTo(_disposable);
            args.CurrentLevel.SkipLatestValueOnSubscribe().Subscribe(OnLevelChanged).AddTo(_disposable);
            args.XpFullStr.SkipLatestValueOnSubscribe().Subscribe(OnXpChanged).AddTo(_disposable);
            args.Icon.SkipLatestValueOnSubscribe().Subscribe(OnProfilePictureChanged).AddTo(_disposable);
            args.MaxBarXp.SkipLatestValueOnSubscribe().Subscribe(OnMaxBarXpChanged).AddTo(_disposable);
            args.CurrentXp.SkipLatestValueOnSubscribe().Subscribe(OnCurrentXpChange).AddTo(_disposable);
            args.IsXpBarFull.Subscribe(UpdateIsXpBarFull).AddTo(_disposable);
            args.CanLevelUp.Subscribe(UpdateCanLevelUp).AddTo(_disposable);
            
            _name.text = args.Name.Value;
            _description.text = args.Description.Value;
            _currentLevel.text = args.CurrentLevel.Value;
            _xp.text = args.XpFullStr.Value;
            _profilePicture.sprite = args.Icon.Value;
            _maxBarXp = args.MaxBarXp.Value;
            _currentXp = args.CurrentXp.Value;
            UpdateXpBarFillingWidth();

            // Вот это нарушает OCP, но я не придумал как это обойти и решил сделать по KISS
            args.CharacterStatsMapToReactiveProperties[Stats.MOVE_SPEED].SkipLatestValueOnSubscribe().Subscribe(UpdateSpeedStat).AddTo(_disposable);
            args.CharacterStatsMapToReactiveProperties[Stats.STAMINA].SkipLatestValueOnSubscribe().Subscribe(UpdateStaminaStat).AddTo(_disposable);
            args.CharacterStatsMapToReactiveProperties[Stats.DEXTERITY].SkipLatestValueOnSubscribe().Subscribe(UpdateDexterityStat).AddTo(_disposable);
            args.CharacterStatsMapToReactiveProperties[Stats.INTELLIGENCE].SkipLatestValueOnSubscribe().Subscribe(UpdateIntelligenceStat).AddTo(_disposable);
            args.CharacterStatsMapToReactiveProperties[Stats.DAMAGE].SkipLatestValueOnSubscribe().Subscribe(UpdateDamageStat).AddTo(_disposable);
            args.CharacterStatsMapToReactiveProperties[Stats.REGENERATION].SkipLatestValueOnSubscribe().Subscribe(UpdateRegenerationStat).AddTo(_disposable);
            
            _moveSpeedStat.text = args.CharacterStatsMapToReactiveProperties[Stats.MOVE_SPEED].Value;
            _staminaStat.text = args.CharacterStatsMapToReactiveProperties[Stats.STAMINA].Value;
            _dexterityStat.text = args.CharacterStatsMapToReactiveProperties[Stats.DEXTERITY].Value;
            _intelligenceStat.text = args.CharacterStatsMapToReactiveProperties[Stats.INTELLIGENCE].Value;
            _damageStat.text = args.CharacterStatsMapToReactiveProperties[Stats.DAMAGE].Value;
            _regenerationStat.text = args.CharacterStatsMapToReactiveProperties[Stats.REGENERATION].Value;

            _profilePictureTransform = _profilePicture.transform;
            _levelTransform = _currentLevel.transform;
            _moveSpeedStatTransform = _moveSpeedStat.transform;
            _staminaStatTransform = _staminaStat.transform;
            _dexterityStatTransform = _dexterityStat.transform;
            _intelligenceStatTransform = _intelligenceStat.transform;
            _damageStatTransform = _damageStat.transform;
            _regenerationStatTransform = _regenerationStat.transform;

            _closeBtn.onClick.AddListener(((IPopupView)this).Close);
        }


        void IPopupView.Close()
        {
            _closeBtn.onClick.RemoveListener(((IPopupView)this).Close);
            if (_canLevelUp)
            {
                _submitBtn.onClick.RemoveListener(OnLevelUpClicked);
            }
            
            OnClose?.Invoke();
            
            Destroy(gameObject);
        }

        private void SetPopupElements(CharacterPopupElements popupElements)
        {
            //TODO: Попробовал перенести эти поля в SO и от туда их сюда биндить.
            // Но в таком случае вьюшка получает ссылка на поля префаба, а не вновь созданного объекта из этого префаба.
            // Как это нужно было сделать ? 
            
            
            // _name = popupElements.Name;
            // _currentLevel = popupElements.CurrentLevel;
            // _description = popupElements.Description;
            // _profilePicture = popupElements.ProfilePicture;
            // _xp = popupElements.Xp;
            // _fullBar = popupElements.FullBar;
            // _notFullBar = popupElements.NotFullBar;
            // _notFullBarImage = popupElements.NotFullBarImage;
            // _moveSpeedStat = popupElements.MoveSpeedStat;
            // _staminaStat = popupElements.StaminaStat;
            // _dexterityStat = popupElements.DexterityStat;
            // _intelligenceStat = popupElements.IntelligenceStat;
            // _damageStat = popupElements.DamageStat;
            // _regenerationStat = popupElements.RegenerationStat;
            // _inactiveBtn = popupElements.InactiveBtn;
            // _activeBtn = popupElements.ActiveBtn;
            // _closeBtn = popupElements.CloseBtn;
            // _submitBtn = popupElements.SubmitBtn;
        }

        private void OnNameChanged(string newName)
        {
            _name.text = newName;
        }

        private void OnDescriptionChanged(string newDescription)
        {
            _description.text = newDescription;
        }

        private Sequence _lvlUpSequence;
        private void OnLevelChanged(string newLevel)
        {
            _lvlUpSequence?.Kill();
            _lvlUpSequence = DOTween.Sequence();

            _lvlUpSequence
                .Append(_profilePictureTransform.DOMoveY(_profilePictureTransform.position.y + 60, 0.8f)).SetEase(Ease.InSine)
                .Append(_profilePictureTransform.DOScale(1.36f, 0.2f)).SetEase(Ease.OutSine)
                .Append(_profilePictureTransform.DORotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(5)).SetEase(Ease.OutSine)
                .Append(_profilePictureTransform.DOScale(1f, 0.01f))
                .Insert(1.1f,_profilePictureTransform.DOMoveY(_profilePictureTransform.position.y, 0.01f)).SetEase(Ease.OutSine);

            _lvlUpSequence
                .Insert(0f, _levelTransform.DOScale(2f, 0.3f)).SetEase(Ease.OutSine)
                .Insert(0.1f, _levelTransform.DOMoveX(_levelTransform.position.x + 310, 0.3f))
                .Insert(0, _currentLevel.DOColor(_increseColor, 0.1f))
                .AppendInterval(2f)
                .Append( _currentLevel.DOColor(_textBaseColor, 2f)).SetEase(Ease.OutSine)
                .Append(_levelTransform.DOMoveX(_levelTransform.position.x, 0.02f))
                .Append(_levelTransform.DOScale(1f, 0.5f)).SetEase(Ease.InSine);
            
            _currentLevel.text = newLevel;
        }

        private void OnXpChanged(string newXpFullStr)
        {
            _xp.text = newXpFullStr;
        }

        private void OnProfilePictureChanged(Sprite newImage)
        {
            _profilePicture.sprite = newImage;
        }

        private void OnMaxBarXpChanged(uint newMaxValue)
        {
            _maxBarXp = newMaxValue;
        }

        private void OnCurrentXpChange(uint newCurrXp)
        {
            _currentXp = newCurrXp;
            UpdateXpBarFillingWidth();
        }

        private void UpdateIsXpBarFull(bool isXpBarFull)
        {
            if (isXpBarFull)
            {
                _fullBar.SetActive(true); 
                _notFullBar.SetActive(false);

                return;
            }

            _fullBar.SetActive(false); 
            _notFullBar.SetActive(true);
            UpdateXpBarFillingWidth();
        }

        private void UpdateCanLevelUp(bool canLvlUp)
        {
            if (canLvlUp)
            {
                _inactiveBtn.SetActive(false);
                _activeBtn.SetActive(true);
                
                _submitBtn.onClick.AddListener(OnLevelUpClicked);

                return;
            }
            
            _inactiveBtn.SetActive(true);
            _activeBtn.SetActive(false);
        }

        private void OnLevelUpClicked()
        {
            OnActionButtonClick?.Invoke();
        }

        private void UpdateSpeedStat(string newStatStr)
        {
            _moveSpeedStat.text = newStatStr;
            PlayStatUpdateAnimation(_moveSpeedStatTransform, _moveSpeedStat);
        }
        private void UpdateStaminaStat(string newStatStr)
        {
            _staminaStat.text = newStatStr;
            PlayStatUpdateAnimation(_staminaStatTransform, _staminaStat);
        }
        private void UpdateDexterityStat(string newStatStr)
        {
            _dexterityStat.text = newStatStr;
            PlayStatUpdateAnimation(_dexterityStatTransform, _dexterityStat);
        }
        private void UpdateIntelligenceStat(string newStatStr)
        {
            _intelligenceStat.text = newStatStr;
            PlayStatUpdateAnimation(_intelligenceStatTransform, _intelligenceStat);
        }
        private void UpdateDamageStat(string newStatStr)
        {
            _damageStat.text = newStatStr;
            PlayStatUpdateAnimation(_damageStatTransform, _damageStat);
        }
        private void UpdateRegenerationStat(string newStatStr)
        {
            _regenerationStat.text = newStatStr;
            PlayStatUpdateAnimation(_regenerationStatTransform, _regenerationStat);
        }


        private void PlayStatUpdateAnimation(Transform stat, TMP_Text statTextElem)
        {
            Sequence statChangeSequence = DOTween.Sequence();

            statChangeSequence
                .Append(statTextElem.DOColor(_increseColor, 0.3f))
                .Append(stat.DOScale(1.25f, 0.3f))
                .Append(stat.DOShakePosition(0.5f, 2f, 20))
                .AppendInterval(2f)
                .Append(stat.DOScale(1f, 0.3f)).SetEase(Ease.OutSine)
                .Append(statTextElem.DOColor(_textBaseColor, 0.3f));
        }

        private void UpdateXpBarFillingWidth()
        {
            float percentageOfFilling = (float)_currentXp / _maxBarXp;

            _notFullBarImage.fillAmount = percentageOfFilling;
        }
    }
}