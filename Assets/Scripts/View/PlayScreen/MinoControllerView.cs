
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    sealed class MinoControllerView : MonoBehaviour
    {
        [SerializeField] Button rotateButton;
        [SerializeField] ObservableEventTrigger moveMinoEventTrigger;
        
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
        
        internal async UniTask MoveAndRotateMinoAsync(MinoView minoView, CancellationToken ct)
        {
            moveMinoEventTrigger.gameObject.SetActive(true);

            _currentActiveMino = minoView;
            await moveMinoEventTrigger.OnPointerUpAsObservable().First().ToUniTask(cancellationToken: ct);
            _currentActiveMino = null;
            
            moveMinoEventTrigger.gameObject.SetActive(false);
        }
    }
}
