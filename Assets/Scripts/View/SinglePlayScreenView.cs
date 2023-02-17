using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        [SerializeField] Transform minoParent;
        
        [Space(20)]
        [SerializeField] RectTransform minoSpawnPoint;
        [SerializeField] RectTransform towerVertexPoint;
        [SerializeField] RectTransform maxTowerVertexPoint;
        [SerializeField] RectTransform spawnAndVertexPoint;
        [SerializeField] Transform groundPoint;
        
        [Space(20)]
        [SerializeField] Button rotateButton;
        [SerializeField] ObservableEventTrigger moveMinoEventTrigger;
        [SerializeField] ObservableTrigger2DTrigger gameOverAreaTrigger;

        [Inject] MinoSpawnerView _minoSpawnerView;

        CameraScrollerView _cameraScrollerView;
        SpawnAndVertexPointScrollerView _spawnAndVertexPointScrollerView;
        
        readonly Dictionary<MinoId, MinoView> _minoViews = new Dictionary<MinoId, MinoView>();
        MinoView _currentActiveMino = null;

        void Awake()
        {
            _cameraScrollerView = new CameraScrollerView(towerVertexPoint);
            _spawnAndVertexPointScrollerView = new SpawnAndVertexPointScrollerView(spawnAndVertexPoint, maxTowerVertexPoint);
            
            moveMinoEventTrigger.gameObject.SetActive(false);
            moveMinoEventTrigger.OnDragAsObservable()
                .Merge(moveMinoEventTrigger.OnPointerDownAsObservable().First())
                .Select(e => Camera.main.ScreenToWorldPoint(e.position))
                .Subscribe(position => _currentActiveMino?.SetX(position.x))
                .AddTo(this);

            rotateButton
                .BindToOnClick(_ => _currentActiveMino?.RotateZAsync(-45).ToObservable().AsUnitObservable())
                .AddTo(this);
        }
        
        public void RefreshMino()
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

        public async UniTask ScrollToTowerVertexAsync()
        {
            var towerVertexY = GetTowerVertexY();
            
            _spawnAndVertexPointScrollerView.ScrollToTowerVertex(towerVertexY);
            await _cameraScrollerView.ScrollToTowerVertexAsync(towerVertexY);
        }
        
        public async UniTask SpawnMinoAsync(Mino mino, CancellationToken ct)
        {
            var minoView = await _minoSpawnerView.SpawnAsync(mino,minoSpawnPoint.position,  minoParent, ct);
            _minoViews.Add(mino.Id, minoView);
        }

        public async UniTask MoveAndRotateMinoAsync(MinoId minoId, CancellationToken ct)
        {
            moveMinoEventTrigger.gameObject.SetActive(true);

            _currentActiveMino = _minoViews[minoId];
            await moveMinoEventTrigger.OnPointerUpAsObservable().First().ToUniTask(cancellationToken: ct);
            _currentActiveMino = null;
            
            moveMinoEventTrigger.gameObject.SetActive(false);
        }
        
        /// <return>AllMinoStopped</return>
        public async UniTask<bool> WaitMinoFallAsync(MinoId minoId, CancellationToken ct)
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
        
        public void ShowResultView()
        {
            Debug.Log("result");
        }
    }
}