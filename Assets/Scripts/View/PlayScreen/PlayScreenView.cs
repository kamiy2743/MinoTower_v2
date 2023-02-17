using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace View
{
    sealed class PlayScreenView : MonoBehaviour
    {
        [SerializeField] CameraScrollerView cameraScrollerView;
        [SerializeField] SpawnAndVertexPointScrollerView spawnAndVertexPointScrollerView;
        [SerializeField] MinoSpawnerView minoSpawnerView;
        [SerializeField] MinoControllerView minoControllerView;
        
        [SerializeField] Transform groundPoint;
        [SerializeField] ObservableTrigger2DTrigger gameOverAreaTrigger;
        
        readonly Dictionary<MinoId, MinoView> _minoViews = new Dictionary<MinoId, MinoView>();

        internal async UniTask ResetAsync(CancellationToken ct)
        {
            RefreshMino();
            await ScrollToTowerVertexAsync(ct);
        }

        void RefreshMino()
        {
            foreach (var minoId in _minoViews.Keys.ToList())
            {
                var minoView = _minoViews[minoId];
                _minoViews.Remove(minoId);
                Destroy(minoView.gameObject);
            }
        }
        
        float GetTowerVertexY()
        {
            var maxY = groundPoint.position.y;
            foreach (var minoView in _minoViews.Values)
            {
                maxY = Mathf.Max(minoView.GetVertexY(), maxY);
            }

            return maxY;
        }

        internal float GetResultHeight()
        {
            return GetTowerVertexY() - groundPoint.position.y;
        }

        internal async UniTask ScrollToTowerVertexAsync(CancellationToken ct)
        {
            var towerVertexY = GetTowerVertexY();
            
            spawnAndVertexPointScrollerView.ScrollToTowerVertex(towerVertexY);
            await cameraScrollerView.ScrollToTowerVertexAsync(towerVertexY, ct);
        }
        
        internal async UniTask SpawnMinoAsync(Mino mino, CancellationToken ct)
        {
            var minoView = await minoSpawnerView.SpawnAsync(mino, ct);
            _minoViews.Add(mino.Id, minoView);
        }

        internal async UniTask MoveAndRotateMinoAsync(MinoId minoId, CancellationToken ct)
        {
            await minoControllerView.MoveAndRotateMinoAsync(_minoViews[minoId], ct);
        }
        
        /// <return>AllMinoStopped</return>
        internal async UniTask<bool> WaitMinoFallAsync(MinoId minoId, CancellationToken ct)
        {
            var minoView = _minoViews[minoId];
            minoView.SetSimulation(true);

            var gameOverObservable = gameOverAreaTrigger.OnTriggerStay2DAsObservable()
                .Where(c => c.gameObject.GetComponentInParent<MinoView>())
                .First();

            var result = await UniTask.WhenAny(
                WaitAllMinoStopTask.Start(_minoViews.Values, ct),
                gameOverObservable.ToUniTask(cancellationToken: ct)
            );

            return result == 0;
        }
    }
}
