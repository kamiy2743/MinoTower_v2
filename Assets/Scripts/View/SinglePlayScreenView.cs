using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace View
{
    public sealed class SinglePlayScreenView : MonoBehaviour
    {
        [SerializeField] Transform minoParent;
        [SerializeField] RectTransform minoSpawnPoint;

        [Inject] MinoSpawnerView _minoSpawnerView;
        
        public IObservable<Unit> OnCollisionGameOverArea => Observable.Timer(TimeSpan.FromSeconds(11)).AsUnitObservable();
        public IObservable<Unit> OnRetryButtonClicked => Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable();
        public IObservable<Unit> OnTitleButtonClicked => Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable();

        readonly List<MinoView> _minoViews = new List<MinoView>();
        

        public void RefreshMino()
        {
            Debug.Log("reflesh");
            for (int i = 0; i < _minoViews.Count; i++)
            {
                var minoView = _minoViews[0];
                _minoViews.RemoveAt(0);
                Destroy(minoView.gameObject);
            }
        }
        
        public async UniTask SpawnMinoAsync(Mino mino, CancellationToken ct)
        {
            RefreshMino();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("spawn");
            var minoView = await _minoSpawnerView.SpawnAsync(mino,minoSpawnPoint.position,  minoParent, ct);
            _minoViews.Add(minoView);
        }
        
        public void ShowResultView()
        {
            Debug.Log("result");
        }
    }
}