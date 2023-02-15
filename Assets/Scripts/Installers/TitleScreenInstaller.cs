using Model;
using Presenter;
using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public sealed class TitleScreenInstaller : MonoInstaller
    {
        [SerializeField] TitleScreenView titleScreenView;
        
        public override void InstallBindings()
        {
            Container.Bind<TitleScreenModel>().AsSingle();
            Container.BindInstance(titleScreenView).AsSingle();
            Container.BindInterfacesTo<TitleScreenPresenter>().AsSingle();
        }
    }
}
