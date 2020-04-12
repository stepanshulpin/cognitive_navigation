using System.Collections.Generic;
using UnityEngine;

namespace AI.PathPlanning {
    public interface INavigationGraph {
        int NodesCount {
            get;
        }

        IEnumerable<INavigationNode> Nodes {
            get;
        }

        void Initialize();

        INavigationNode GetNavigationNode(Vector3 worldPosition);

        INavigationNode GetNavigationNode(int nodeIndex);

        List<INavigationNode> GetNavigationNodeNeighbours(INavigationNode navigationNode);

        List<INavigationNode> GetNavigationNodeNeighbours(int nodeIndex);

        void DrawWithGizmos();
    }
}
