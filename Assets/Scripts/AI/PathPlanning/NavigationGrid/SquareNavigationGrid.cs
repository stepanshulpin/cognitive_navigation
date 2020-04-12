using UnityEngine;

namespace AI.PathPlanning {
    public class SquareNavigationGrid : NavigationGrid {
        public SquareNavigationGrid(float nodeSize, Vector2 gridSize, Vector3 gridCenter) :
            base(new Vector2Int(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize))) {
            this.nodeSize = nodeSize;
            this.size = gridSize;
            this.gridCenter = gridCenter;
            this.Initialize();
        }

        private void Initialize() {
            Vector3 gridTopLeft = this.gridCenter + Vector3.left * this.size.x / 2 + Vector3.back * this.size.y / 2;
            for (int x = 0; x < this.shape.x; ++x) {
                for (int y = 0; y < this.shape.y; ++y) {
                    Vector3 nodeShiftX = Vector3.right * (x + 0.5f) * this.nodeSize;
                    Vector3 nodeShiftY = Vector3.forward * (y + 0.5f) * this.nodeSize;
                    Vector3 nodeWorldPosition = gridTopLeft + nodeShiftX + nodeShiftY;
                    this.grid[x, y] = new NavigationGridNode(new Vector2Int(x, y), nodeWorldPosition);
                }
            }
        }

        public override INavigationGridNode GetNavigationNode(Vector3 worldPosition) {
            Vector2Int nodeIndex = this.GetNavigationNodeIndex(worldPosition);
            return this.grid[nodeIndex.x, nodeIndex.y];
        }

        public override void DrawWithGizmos() {
            Gizmos.DrawWireCube(this.gridCenter, new Vector3(this.size.x, 1.0f, this.size.y));
            Vector3 gizmosTileSize = new Vector3(this.nodeSize - 0.1f, 0.1f, this.nodeSize - 0.1f);
            foreach (INavigationGridNode navigationNode in this.grid) {
                Gizmos.color = navigationNode.IsWalkable ? Color.white : Color.red;
                Gizmos.DrawCube(navigationNode.WorldPosition, gizmosTileSize);
            }
        }

        private Vector2Int GetNavigationNodeIndex(Vector3 worldPosition) {
            float percentX = 0.5f + worldPosition.x / this.size.x;
            float percentY = 0.5f + worldPosition.z / this.size.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            int nodeX = Mathf.RoundToInt((this.shape.x - 1) * percentX);
            int nodeY = Mathf.RoundToInt((this.shape.y - 1) * percentY);
            return new Vector2Int(nodeX, nodeY);
        }

        private float nodeSize;

        private Vector2 size;

        private Vector3 gridCenter;
    }
}
