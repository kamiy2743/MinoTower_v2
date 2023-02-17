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
            await _singlePlayScreenView.ResetAsync(ct);
        }

        async UniTask GameCycleAsync(CancellationToken ct)
        {
            // タワーの頂上まで画面をスクロール
            await _singlePlayScreenView.ScrollToTowerVertexAsync(ct);
            
            // ミノをスポーンさせる
            var mino = _minoFactory.CreateRandom();
            await _singlePlayScreenView.SpawnMinoAsync(mino, ct);

            // ミノの操作を待機
            await _singlePlayScreenView.MoveAndRotateMinoAsync(mino.Id, ct);

            // ミノが停止するか、土台から落ちるまで待機
            var allMinoStopped = await _singlePlayScreenView.WaitMinoFallAsync(mino.Id, ct);
            if (allMinoStopped)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct);
                GameCycleAsync(ct).Forget();
                return;
            }
            
            // ゲームオーバー
            var resultHeight = _singlePlayScreenView.GetResultHeight();
            await _singlePlayScreenView.ShowResultScreenAsync(resultHeight, ct);

            // リトライボタンかタイトルボタンが押されるのを待つ
            var retryGame = await _singlePlayScreenView.WaitRetryOrBackToTitleAsync(ct);
            if (retryGame)
            {
                await ResetAsync(ct);
                GameCycleAsync(ct).Forget();
            }
            else
            {
                await ResetAsync(ct);
                GameCycleAsync(ct).Forget();
            }
        }

        public void Dispose()
        {
            _cts.Dispose();
        }
    }
}