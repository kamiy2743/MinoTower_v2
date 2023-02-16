
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace View
{
    public sealed class MinoView : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody;

        internal void SetX(float x)
        {
            transform.position = new Vector3(x, transform.position.y, 0);
        }

        internal async UniTask RotateZAsync(float angle)
        {
           await transform
               .DORotate( new Vector3(0, 0, transform.eulerAngles.z + angle), 0.2f)
               .SetEase(Ease.Linear);
        }
        
        internal void SetSimulation(bool active)
        {
            _rigidbody.isKinematic = !active;
        }

        internal bool IsSleeping()
        {
            return _rigidbody.IsSleeping();
        }
    }
}
