using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace View
{
    sealed class CameraScrollerView : MonoBehaviour
    {
        [SerializeField] Transform towerVertexPoint;
        
        Transform _cameraTransform;
        float _towerVertexPointY;

        void Awake()
        {
            _cameraTransform = Camera.main.transform;
            
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, towerVertexPoint.position);
            _towerVertexPointY = Camera.main.ScreenToWorldPoint(screenPoint).y;
        }

        internal async UniTask ScrollToTowerVertexAsync(float towerVertexY, CancellationToken ct)
        {
            var cameraY = Mathf.Clamp(towerVertexY - _towerVertexPointY, 0, float.PositiveInfinity);
            await _cameraTransform.DOMoveY(cameraY, 0.5f).WithCancellation(ct);
        }
    }
}
