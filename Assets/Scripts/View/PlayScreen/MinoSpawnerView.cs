using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain;
using UnityEngine;

namespace View
{
    sealed class MinoSpawnerView : MonoBehaviour
    {
        [SerializeField] MinoView minoViewPrefab;
        [SerializeField] GameObject minoBlockPrefab;
        [SerializeField] float minoBlockScale;
        [SerializeField] Transform minoParent;
        [SerializeField] Transform minoSpawnPoint;

        internal async UniTask<MinoView> SpawnAsync(Mino mino, CancellationToken ct)
        {
            var minoView = MonoBehaviour.Instantiate(minoViewPrefab, parent: minoParent);
            minoView.transform.position = minoSpawnPoint.position;
            
            // mino.BlockPositionsをもとにブロックを作成
            {
                var center = new Vector2(0, 0);
                foreach (var position in mino.BlockPositions)
                {
                    center += position;
                }
                center /= mino.BlockPositions.Count;
            
                foreach (Vector2 blockPosition in mino.BlockPositions)
                {
                    var minoBlock = MonoBehaviour.Instantiate(minoBlockPrefab, parent: minoView.transform);
                    minoBlock.transform.localPosition = ((blockPosition - center) * minoBlockScale);
                    minoBlock.transform.localScale = new Vector3(minoBlockScale, minoBlockScale, 1);
                    
                    minoView.AddBlock(minoBlock.GetComponent<BoxCollider2D>());
                }
            }
            minoView.SetSimulation(false);

            // アニメーション
            {
                minoView.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                minoView.transform.rotation = Quaternion.Euler(0,0,90);
                
                var sequence = DOTween.Sequence();
                sequence.Join(minoView.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
                sequence.Join(minoView.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));

                await sequence.Play().WithCancellation(ct);
            }

            return minoView;
        }
    }
}