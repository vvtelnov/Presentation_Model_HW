using System;
using CameraManipulator;
using CharacterPopupPresenter;
using PopupView;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace PopUpHelper
{
    public class PopupOpener : MonoBehaviour
    {
        [SerializeField]
        private GameObject _characterPopupPrefab;

        private CharacterPopupView _popup;
        
        private DiContainer _container;
        private IPopupPresenterFactory _presenterFactory;
        private RectTransform _popupCanvas;

        private ICameraToggler _cameraToggler;
        

        [Inject]
        public void Constructor(DiContainer container,
            IPopupPresenterFactory factory, 
            RectTransform popupCanvas,
            ICameraToggler cameraToggler
            )
        {
            _container = container;
            _presenterFactory = factory;
            _popupCanvas = popupCanvas;
            _cameraToggler = cameraToggler;
        }
        
        [Button(ButtonSizes.Large)]
        private void OpenPopup()
        {
            var args = _presenterFactory.CreatePresenter();
            Open(args);
        }

        private void Open(ICharacterPopupPresenter args)
        {
            if (_popup is null)
            {
                _popup = _container.InstantiatePrefabForComponent<CharacterPopupView>(_characterPopupPrefab, _popupCanvas);
            }
            else
            {
                _popup.gameObject.SetActive(true);
            }
            
            

            if (args is IEventsubscriberPresenter presenter)
                SubscribePresenterToPopupEvents(presenter, _popup);
            
            HandleSwitchCameraLogic();
            
            _popup.Open(args);
        }

        private void SubscribePresenterToPopupEvents(IEventsubscriberPresenter presenter, IPopupEventEmitter popup)
        {
            presenter.SubscribeToViewEvents(popup);
        }

        private void HandleSwitchCameraLogic()
        {
            _cameraToggler.TurnOnUICamera();
            
            _popup.OnClose += ToGameCameraSwitchHandler;
        }

        private void ToGameCameraSwitchHandler()
        {
            _cameraToggler.TurnOnGameCamera();
            
            _popup.OnClose -= ToGameCameraSwitchHandler;
        }
    }
}