using UnityEngine;

namespace View
{
    sealed class SpawnAndVertexPointScrollerView : MonoBehaviour
    {
        [SerializeField] Transform spawnAndVertexPoint;
        
        internal void ScrollToTowerVertex(float towerVertexY)
        {
            spawnAndVertexPoint.position = new Vector2(spawnAndVertexPoint.position.x, towerVertexY);
        }
    }
}