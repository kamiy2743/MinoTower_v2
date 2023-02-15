using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        public IObservable<Unit> OnCollisionGameOverArea => Observable.Timer(TimeSpan.FromSeconds(21)).AsUnitObservable();
        public IObservable<Unit> OnRetryButtonClicked => Observable.Timer(TimeSpan.FromSeconds(2)).AsUnitObservable();
        public IObservable<Unit> OnTitleButtonClicked => Observable.Timer(TimeSpan.FromSeconds(2)).AsUnitObservable();

        public async UniTask RefreshMinoAsync(CancellationToken ct)
        {
            Debug.Log("reflesh");
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: ct);
        }
        
        public async UniTask SpawnMinoAsync(CancellationToken ct)
        {
            Debug.Log("spawn");
            await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: ct);
        }
        
        public void ShowResultView()
        {
            Debug.Log("result");
        }
    }
}