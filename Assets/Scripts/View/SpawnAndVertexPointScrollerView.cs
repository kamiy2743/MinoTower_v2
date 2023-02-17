using UnityEngine;

namespace View
{
    sealed class SpawnAndVertexPointScrollerView
    {
        readonly RectTransform _spawnAndVertexPoint;
        readonly RectTransform _maxTowerVertexPoint;

        internal SpawnAndVertexPointScrollerView(RectTransform spawnAndVertexPoint, RectTransform maxTowerVertexPoint)
        {
            _spawnAndVertexPoint = spawnAndVertexPoint;
            _maxTowerVertexPoint = maxTowerVertexPoint;
        }
        
        internal void ScrollToTowerVertex(float towerVertexY)
        {
            var y = Mathf.Min(towerVertexY,_maxTowerVertexPoint.position.y);
            _spawnAndVertexPoint.position = new Vector2(_spawnAndVertexPoint.position.x, y);
        }
    }
}