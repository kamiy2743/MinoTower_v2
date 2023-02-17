using UnityEditor;
using UnityEngine;

namespace View
{
    public sealed class SpriteResolutionFitter : MonoBehaviour
    {
        [SerializeField] Vector2 referenceResolution;
        
        void Awake() => Fit();

        [ContextMenu("Fit")]
        void Fit()
        {
            var currentResolutionString = UnityStats.screenRes.Split("x");
            var currentResolution = new Vector2(float.Parse(currentResolutionString[0]), float.Parse(currentResolutionString[1]));

            var referenceAspect = referenceResolution.x / referenceResolution.y;
            var currentAspect = currentResolution.x / currentResolution.y;
            var spriteScale = currentAspect / referenceAspect;

            if (spriteScale >= 1)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(spriteScale, spriteScale, 1);
            }
        }
    }
}
