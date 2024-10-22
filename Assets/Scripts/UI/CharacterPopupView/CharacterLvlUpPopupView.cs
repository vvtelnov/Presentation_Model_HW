using System;
using CharacterPopupPresenter.IPresenters;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterPopupView
{
    public class CharacterLvlUpPopupView : BasePopupView<ICharacterLvlUpPresenter>, 
        ICharacterLvlUpView, IViewEventEmmiter, IViewHideable
    {
        public event Action OnLvlUpClicked;
        
        [Title("SubmitBtn")]
        [SerializeField] private GameObject _inactiveBtn;
        [SerializeField] private GameObject _activeBtn;

        [Title("Buttons")]
        [SerializeField] private Button _lvlUpButton;
        
        private bool _canLevelUp;
        
        void IViewHideable.OnHide()
        {
            if (_canLevelUp)
            {
                _lvlUpButton.onClick.RemoveListener(OnLevelUpClicked);
            }
        }

        protected override void Show(ICharacterLvlUpPresenter args)
        {
            args.CanLevelUp.Subscribe(UpdateCanLevelUp).AddTo(disposable);
        }
        
        protected override void SubscribePresenterToViewEvents(IViewEventsListener presenter)
        {
            presenter.SubscribeToViewEvents(this);
        }
        
        private void UpdateCanLevelUp(bool canLvlUp)
        {
            if (canLvlUp)
            {
                _inactiveBtn.SetActive(false);
                _activeBtn.SetActive(true);
                
                _lvlUpButton.onClick.AddListener(OnLevelUpClicked);

                return;
            }
            
            _inactiveBtn.SetActive(true);
            _activeBtn.SetActive(false);
        }

        private void OnLevelUpClicked()
        {
            OnLvlUpClicked?.Invoke();
            _lvlUpButton.onClick.RemoveListener(OnLevelUpClicked);
        }
    }
}