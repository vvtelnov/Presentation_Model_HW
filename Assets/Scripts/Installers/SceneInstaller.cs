using CameraManipulator;
using CharacterPopupPresenter.PresentersFactory;
using Player;
using PopUpHelper;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private CharacterInitInfoSetter _characterInitInfoSetter;
        
        [SerializeField]
        private CharacterInfoSetter _characterInfoSetter;
        
        [SerializeField]
        private RectTransform _popupCanvas;
        
        [SerializeField]
        private Camera _gameCamera;
        
        [SerializeField]
        private Camera _uiCamera;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterStats>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterStatIncreaser>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerLevel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserInfo>().AsSingle();
            // Container.Bind<CharacterInitInfoSetter>().FromComponentInHierarchy().AsSingle();
            // Container.Bind<CharacterInfoSetter>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterInitInfoSetter>().FromInstance(_characterInitInfoSetter).AsSingle().NonLazy();
            Container.Bind<CharacterInfoSetter>().FromInstance(_characterInfoSetter).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacterPopupPresentersFactory>().AsSingle();
            Container.Bind<RectTransform>().FromInstance(_popupCanvas).AsSingle();

            BindCamera();
        }

        private void BindCamera()
        {
            Container.Bind<ICameraToggler>().To<CameraToggler>().AsSingle();
            
            Container.Bind<Camera>().WithId("GameCamera").FromInstance(_gameCamera).AsCached();
            Container.Bind<Camera>().WithId("UICamera").FromInstance(_uiCamera).AsCached();
        }
    }
}