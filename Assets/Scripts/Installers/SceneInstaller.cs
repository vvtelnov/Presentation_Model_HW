using CharacterPopupPresenter;
using Player;
using PopUpHelper;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterStats>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerLevel>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserInfo>().AsSingle();
            Container.Bind<CharacterInitInfoSetter>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<CharacterInfoSetter>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PopupPresenterFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterStatIncreaser>().AsSingle();
        } 
    }
}