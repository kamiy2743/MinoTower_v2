﻿using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain;
using UnityEngine;
using Zenject;

namespace View
{
    public sealed class MinoSpawnerView
    {
        readonly MinoView _minoViewPrefab;
        readonly GameObject _minoBlockPrefab;
        readonly float _minoBlockScale;
        
        [Inject]
        MinoSpawnerView(
            MinoView minoViewPrefab, 
            GameObject minoBlockPrefab, 
            float minoBlockScale)
        {
            _minoViewPrefab = minoViewPrefab;
            _minoBlockPrefab = minoBlockPrefab;
            _minoBlockScale = minoBlockScale;
        }

        internal async UniTask<MinoView> SpawnAsync(Mino mino, Vector2 spawnPoint, Transform parent, CancellationToken ct)
        {
            var minoView = MonoBehaviour.Instantiate(_minoViewPrefab, parent: parent);
            minoView.transform.position = spawnPoint;
            
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
                    var minoBlock = MonoBehaviour.Instantiate(_minoBlockPrefab, parent: minoView.transform);
                    minoBlock.transform.localPosition = (blockPosition - center) * _minoBlockScale;
                    minoBlock.transform.localScale = new Vector3(_minoBlockScale, _minoBlockScale, 1);
                }
            }

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