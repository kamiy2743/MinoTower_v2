using System;
using UnityEngine;

namespace View
{
    public sealed class SpriteRectFitter : MonoBehaviour
    {
        [SerializeField] RectTransform targetRect;
        [SerializeField] bool fitPosition;
        [SerializeField] bool fitScale;
        
        void Awake() => Fit();

        [ContextMenu("Fit")]
        void Fit()
        {
            var corners = new Vector3[4];
            targetRect.GetWorldCorners(corners);
            var cornerMax = corners[2];
            var cornerMin = corners[0];
            
            var scale = cornerMax - cornerMin;
            var position = cornerMin + (scale / 2);

            if (fitPosition) transform.position = new Vector3(position.x, position.y, 0);
            if (fitScale) transform.localScale = new Vector3(scale.x, scale.y, 1);
        }
    }
}
