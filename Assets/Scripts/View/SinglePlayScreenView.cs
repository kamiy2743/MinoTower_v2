using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UnityEngine;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        [SerializeField] PlayScreenView playScreenView;
        [SerializeField] ResultScreenView resultScreenView;
        
        public async UniTask ResetAsync(CancellationToken ct)
        {
            await UniTask.WhenAll(
                playScreenView.ResetAsync(ct),
                resultScreenView.HideAsync(ct)
            );
        }

        public async UniTask ScrollToTowerVertexAsync(CancellationToken ct) => await playScreenView.ScrollToTowerVertexAsync(ct);

        public async UniTask SpawnMinoAsync(Mino mino, CancellationToken ct) => await playScreenView.SpawnMinoAsync(mino, ct);

        public async UniTask MoveAndRotateMinoAsync(MinoId minoId, CancellationToken ct) => await playScreenView.MoveAndRotateMinoAsync(minoId, ct);

        /// <return>AllMinoStopped</return>
        public async UniTask<bool> WaitMinoFallAsync(MinoId minoId, CancellationToken ct) => await playScreenView.WaitMinoFallAsync(minoId, ct);

        public float GetResultHeight() => playScreenView.GetResultHeight();
        
        public async UniTask ShowResultScreenAsync(float resultHeight, CancellationToken ct)
        {
            resultScreenView.SetResultHeight(resultHeight);
            await resultScreenView.ShowAsync(ct);
        }

        /// <return>RetryGame</return>
        public async UniTask<bool> WaitRetryOrBackToTitleAsync(CancellationToken ct) => await resultScreenView.WaitRetryOrBackToTitleAsync(ct);
    }
}