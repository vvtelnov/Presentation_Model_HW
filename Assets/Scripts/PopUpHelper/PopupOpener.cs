using System;
using System.Collections.Generic;
using CameraManipulator;
using CharacterPopupPresenter.IPresenters;
using CharacterPopupPresenter.PresentersFactory;
using Sirenix.OdinInspector;
using UI.CharacterPopupView;
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
        private RectTransform _popupCanvas;
        private ICameraToggler _cameraToggler;

        private ICharacterPresenterFactory _presenterFactory;
        private IViews[] _views;

        [Inject]
        public void Constructor(DiContainer container,
            RectTransform popupCanvas,
            ICameraToggler cameraToggler,
            ICharacterPresenterFactory presenterFactory
            )
        {
            _container = container;
            _popupCanvas = popupCanvas;
            _cameraToggler = cameraToggler;
            _presenterFactory = presenterFactory;
        }

        [Button(ButtonSizes.Large)]
        private void OpenPopup()
        {
            if (_popup is null)
            {
                _popup = _container.InstantiatePrefabForComponent<CharacterPopupView>(_characterPopupPrefab, _popupCanvas);
                _views = _popup.GetComponents<IViews>();
            }
            else
            {
                _popup.gameObject.SetActive(true);
            }

            _popup.Open();
            _popup.OnClose += ClosePopupHandler;

            HandleSwitchCameraLogic();
            ShowAllView();
        }

        private void ShowAllView()
        {
            var presenters = CreatePresenters();
            
            foreach (var view in _views)
            {
                view.OnShow(presenters);
            }
        }

        private void ClosePopupHandler()
        {
            foreach (var view in _views)
            {
                if (view is IViewHideable hideableView)
                    hideableView.OnHide();
            }
            
            ToGameCameraSwitchHandler();
            _popup.OnClose -= ClosePopupHandler;
        }

        private void HandleSwitchCameraLogic()
        {
            _cameraToggler.TurnOnUICamera();
        }

        private void ToGameCameraSwitchHandler()
        {
            _cameraToggler.TurnOnGameCamera();
        }
        
        private Dictionary<Type, IPresenter> CreatePresenters()
        {
            Dictionary<Type, IPresenter> presenters = new();
            
            foreach (var presenterType in _presenterFactory.СanCreatePresenters)
            {
                var presenter = _presenterFactory.CreatePresenter(presenterType);
                presenters.TryAdd(presenterType, presenter);
            }

            return presenters;
        }
    }
}