using UnityEngine;

namespace View
{
    sealed class SpawnAndVertexPointScrollerView
    {
        readonly Transform _spawnAndVertexPoint;

        internal SpawnAndVertexPointScrollerView(Transform spawnAndVertexPoint)
        {
            _spawnAndVertexPoint = spawnAndVertexPoint;
        }
        
        internal void ScrollToTowerVertex(float towerVertexY)
        {
            _spawnAndVertexPoint.position = new Vector2(_spawnAndVertexPoint.position.x, towerVertexY);
        }
    }
}