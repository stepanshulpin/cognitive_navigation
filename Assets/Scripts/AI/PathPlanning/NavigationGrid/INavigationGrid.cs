using System.Collections.Generic;
using UnityEngine;

namespace AI.PathPlanning {
    public interface INavigationGrid {
        int NodesCount {
            get;
        }

        Vector2Int Shape {
            get;
        }

        INavigationGridNode[,] Nodes {
            get;
        }

        INavigationGridNode GetNavigationNode(Vector3 worldPosition);

        List<INavigationGridNode> GetNavigationNodeNeighbours(INavigationGridNode navigationNode);

        void DrawWithGizmos();
    }
}
