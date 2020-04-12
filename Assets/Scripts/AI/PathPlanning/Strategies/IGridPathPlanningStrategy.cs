using System.Collections.Generic;

namespace AI.PathPlanning {
    public interface IGridPathPlanningStrategy {
        List<INavigationGridNode> FindPath(INavigationGrid navigationGrid, INavigationGridNode startNode, INavigationGridNode targetNode);
    }
}
