using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Domain
{
    public sealed record Mino
    {
        readonly Vector2Int[] _blockPositions;
        public IReadOnlyCollection<Vector2Int> BlockPositions => _blockPositions;

        public Mino(int blockCount, Random random)
        {
            _blockPositions = new Vector2Int[blockCount];
            var currentPosition = new Vector2Int(0, 0);

            for (int i = 0; i < blockCount; i++)
            {
                _blockPositions[i] = currentPosition;

                // 上下左右ランダムに追加
                switch (random.NextInt(1, 4))
                {
                    case 1:
                        currentPosition += Vector2Int.right;
                        break;
                    case 2:
                        currentPosition += Vector2Int.left;
                        break;
                    case 3:
                        currentPosition += Vector2Int.up;
                        break;
                    case 4:
                        currentPosition += Vector2Int.down;
                        break;
                }
            }
        }
    }
}