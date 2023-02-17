using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UnityEngine;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        [SerializeField] PlayScreenView playScreenView;

        public void RefreshMino() => playScreenView.RefreshMino();

        public async UniTask ScrollToTowerVertexAsync(CancellationToken ct) => await playScreenView.ScrollToTowerVertexAsync(ct);

        public async UniTask SpawnMinoAsync(Mino mino, CancellationToken ct) => await playScreenView.SpawnMinoAsync(mino, ct);

        public async UniTask MoveAndRotateMinoAsync(MinoId minoId, CancellationToken ct) => await playScreenView.MoveAndRotateMinoAsync(minoId, ct);

        /// <return>AllMinoStopped</return>
        public async UniTask<bool> WaitMinoFallAsync(MinoId minoId, CancellationToken ct) => await playScreenView.WaitMinoFallAsync(minoId, ct);
        
        /// <return>RetryGame</return>
        public async UniTask<bool> WaitRetryOrBackToTitleAsync(CancellationToken ct)
        {
            var retryObservable = Observable.Timer(TimeSpan.FromSeconds(2));
            var backToTitleObservable = Observable.Timer(TimeSpan.FromSeconds(21));

            var result = await UniTask.WhenAny(
                retryObservable.ToUniTask(cancellationToken: ct),
                backToTitleObservable.ToUniTask(cancellationToken: ct)
            );

            return result.winArgumentIndex == 0;
        }
        
        public void ShowResultScreen()
        {
            Debug.Log("result");
        }
    }
}