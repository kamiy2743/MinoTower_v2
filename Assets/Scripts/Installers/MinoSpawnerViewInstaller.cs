
using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public sealed class MinoSpawnerViewInstaller : MonoInstaller
     {
         [SerializeField] MinoView minoViewPrefab;
         [SerializeField] GameObject minoBlockPrefab;
         [SerializeField] float minoBlockScale;

         public override void InstallBindings()
         {
             Container
                 .Bind<MinoSpawnerView>()
                 .AsSingle()
                 .WithArguments(minoViewPrefab, minoBlockPrefab, minoBlockScale);
         }
     }
}