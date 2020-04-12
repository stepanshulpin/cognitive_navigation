using System.Collections.Generic;

namespace AI.PathPlanning {
    public interface IGraphPathPlanningStrategy {
        List<INavigationNode> FindPath(INavigationGraph navigationGraph, INavigationNode startNode, INavigationNode targetNode);
    }
}
