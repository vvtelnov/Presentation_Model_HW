using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Data/New DataInstaller", fileName = "DataInstaller")]
    public class DataInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CharacterPopupElements _popupElements;

        public override void InstallBindings()
        {
            Container.Bind<CharacterPopupElements>().FromInstance(_popupElements).AsSingle();
        }
    }
}