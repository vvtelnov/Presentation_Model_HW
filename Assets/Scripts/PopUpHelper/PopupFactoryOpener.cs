using System;
using CharacterPopupPresenter;
using PopupView;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace PopUpHelper
{
    public class PopupFactoryOpener : MonoBehaviour
    {
        [SerializeField]
        private GameObject _characterPopupPrefab;
        
        private DiContainer _container;
        private IPopupPresenterFactory _presenterFactory;

        [Inject]
        public void Constructor(DiContainer container, IPopupPresenterFactory factory)
        {
            _container = container;
            _presenterFactory = factory;
        }
        
        [Button(ButtonSizes.Large)]
        private void OpenPopup()
        {
            var args = _presenterFactory.CreatePresenter();
            Open(args);
        }

        private void Open(ICharacterPopupPresenter args)
        {
            CharacterPopupView popup = _container.InstantiatePrefabForComponent<CharacterPopupView>(_characterPopupPrefab);

            if (popup is null)
            {
                throw new NullReferenceException("There is no view script (e.g. CharacterPopupView) on prefab");
            }

            if (args is IEventsubscriberPresenter presenter)
                SubscribePresenterToPopupEvents(presenter, popup);
            
            ((IPopupView)popup).Open(args);
        }

        private void SubscribePresenterToPopupEvents(IEventsubscriberPresenter presenter, IPopupEventEmitter popup)
        {
            presenter.SubscribeToViewEvents(popup);
        }
    }
}