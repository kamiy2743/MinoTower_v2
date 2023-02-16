
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    public sealed class MinoView : MonoBehaviour
    {
        [SerializeField] new Rigidbody2D rigidbody;

        readonly List<Transform> _blocks = new List<Transform>();
        internal void AddBlock(Transform block) => _blocks.Add(block);

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
            rigidbody.isKinematic = !active;
        }

        internal bool IsSleeping()
        {
            return rigidbody.IsSleeping();
        }

        internal float GetVertexY()
        {
            var maxY = float.NegativeInfinity;
            foreach (var block in _blocks)
            {
                maxY = Mathf.Max(block.position.y + (block.localScale.y * 0.5f), maxY);
            }

            return maxY;
        }
    }
}
