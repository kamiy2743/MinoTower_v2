using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace View
{
    sealed class CameraScrollerView
    {
        readonly Transform _cameraTransform;
        readonly float _towerVertexPointY;

        internal CameraScrollerView(Transform towerVertexPoint)
        {
            _cameraTransform = Camera.main.transform;
            
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, towerVertexPoint.position);
            _towerVertexPointY = Camera.main.ScreenToWorldPoint(screenPoint).y;
        }

        internal async UniTask ScrollToTowerVertexAsync(float towerVertexY)
        {
            var cameraY = Mathf.Clamp(towerVertexY - _towerVertexPointY, 0, float.PositiveInfinity);
            await _cameraTransform.DOMoveY(cameraY, 0.5f);
        }
    }
}
