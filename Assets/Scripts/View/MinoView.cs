
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

        readonly List<BoxCollider2D> _blocks = new List<BoxCollider2D>();
        internal void AddBlock(BoxCollider2D block) => _blocks.Add(block);

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
            foreach (var collider in _blocks)
            {
                collider.enabled = active;
            }
        }

        internal float GetVelocity()
        {
            return Mathf.Max(rigidbody.velocity.magnitude, rigidbody.angularVelocity);
        }

        internal void Sleep()
        {
            rigidbody.Sleep();
        }

        internal float GetVertexY()
        {
            var maxY = float.NegativeInfinity;
            foreach (var block in _blocks)
            {
                maxY = Mathf.Max(block.bounds.center.y + block.bounds.extents.y, maxY);
            }

            return maxY;
        }
    }
}
