using Model;
using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public sealed class SinglePlayScreenInstaller : MonoInstaller
    {
        [SerializeField] SinglePlayScreenView singlePlayScreenView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SinglePlayScreenModel>().AsSingle();
            Container.BindInstance(singlePlayScreenView).AsSingle();
        }
    }
}
