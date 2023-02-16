using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        [SerializeField] Transform minoParent;
        [SerializeField] RectTransform minoSpawnPoint;
        [SerializeField] Button rotateButton;
        [SerializeField] ObservableEventTrigger moveMinoEventTrigger;

        [Inject] MinoSpawnerView _minoSpawnerView;

        readonly Dictionary<MinoId, MinoView> _minoViews = new Dictionary<MinoId, MinoView>();
        MinoView _currentActiveMino = null;

        void Awake()
        {
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
        public async UniTask<bool> WaitingToMinoFallAsync(CancellationToken ct)
        {
            var waitingAllMinoStopTask = UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: ct);
            var gameOverObservable = Observable.Timer(TimeSpan.FromSeconds(21));

            var result = await UniTask.WhenAny(
                waitingAllMinoStopTask,
                gameOverObservable.ToUniTask(cancellationToken: ct)
            );

            return result == 0;
        }
        
        /// <return>RetryGame</return>
        public async UniTask<bool> WaitingRetryOrBackToTitleAsync(CancellationToken ct)
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