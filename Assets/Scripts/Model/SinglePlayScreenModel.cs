using System;
using System.Threading;
using View;
using Zenject;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UnityEngine;

namespace Model
{
    public sealed class SinglePlayScreenModel : IInitializable, IDisposable
    {
        readonly SinglePlayScreenView _singlePlayScreenView;
        readonly MinoFactory _minoFactory;
        
        readonly CancellationTokenSource _cts = new CancellationTokenSource();

        [Inject]
        SinglePlayScreenModel(SinglePlayScreenView singlePlayScreenView)
        {
            _singlePlayScreenView = singlePlayScreenView;
            _minoFactory = new MinoFactory(DateTime.Now.GetHashCode());
        }

        public void Initialize() => InitializeAsync().Forget();
        async UniTask InitializeAsync()
        {
            await ResetAsync(_cts.Token);
            GameCycleAsync(_cts.Token).Forget();
        }

        async UniTask ResetAsync(CancellationToken ct)
        {
            _singlePlayScreenView.RefreshMino();
        }

        async UniTask GameCycleAsync(CancellationToken ct)
        {
            var mino = _minoFactory.CreateRandom();
            await _singlePlayScreenView.SpawnMinoAsync(mino, ct);

            var continueGameCycle = await WaitingToFallAsync(ct);
            if (continueGameCycle)
            {
                GameCycleAsync(ct).Forget();
                return;
            }
            
            // GameOver
            _singlePlayScreenView.ShowResultView();

            var retryGame = await WaitingRetryOrBackToTitleAsync(ct);
            if (retryGame)
            {
                await ResetAsync(ct);
                GameCycleAsync(ct).Forget();
            }
            else
            {
                Debug.Log("title");
                BackToTitle();
            }
        }

        /// <return>ContinueGameCycle</return>
        async UniTask<bool> WaitingToFallAsync(CancellationToken ct)
        {
            var waitingAllMinoStopTask = UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: ct);
            var gameOverObservable = _singlePlayScreenView.OnCollisionGameOverArea.First();

            var result = await UniTask.WhenAny(
                waitingAllMinoStopTask,
                gameOverObservable.ToUniTask(cancellationToken: ct)
            );

            return result == 0;
        }

        /// <return>RetryGame</return>
        async UniTask<bool> WaitingRetryOrBackToTitleAsync(CancellationToken ct)
        {
            var retryObservable = _singlePlayScreenView.OnRetryButtonClicked.First();
            var backToTitleObservable = _singlePlayScreenView.OnTitleButtonClicked.First();

            var result = await UniTask.WhenAny(
                retryObservable.ToUniTask(cancellationToken: ct),
                backToTitleObservable.ToUniTask(cancellationToken: ct)
            );

            return result.winArgumentIndex == 0;
        }

        void BackToTitle()
        {
            
        }

        public void Dispose()
        {
            _cts.Dispose();
        }
    }
}