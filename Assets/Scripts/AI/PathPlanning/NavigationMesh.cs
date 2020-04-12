using System.Collections.Generic;
using AI.PathPlanning;
using AI.PathPlanning.AStar;
using UnityEngine;

public class NavigationMesh : MonoBehaviour {

    public LayerMask unwalkableMask;

    public float navigationNodeSize;


    public void FindPath(Vector3 from, Vector3 to) {
        INavigationGridNode startNode = this.navigationGrid.GetNavigationNode(from);
        INavigationGridNode targetNode = this.navigationGrid.GetNavigationNode(to);
        Debug.Log("Find path");
        this.path = this.pathPlanningStrategy.FindPath(this.navigationGrid, startNode, targetNode);
    }

    private void Awake() {
        Bounds navigationMeshBounds = this.GetComponent<Renderer>().bounds;
        Vector2 navigationGridWorldSize = new Vector2(navigationMeshBounds.size.x, navigationMeshBounds.size.z);
        this.navigationGrid = new SquareNavigationGrid(this.navigationNodeSize, navigationGridWorldSize, this.transform.position);
        foreach (INavigationGridNode navigationNode in this.navigationGrid.Nodes) {
            bool containsObstacle = (Physics.CheckSphere(navigationNode.WorldPosition, this.navigationNodeSize / 2, this.unwalkableMask));
            if (navigationNode.IsWalkable && containsObstacle) {
                navigationNode.Occupy();
            } else if (!navigationNode.IsWalkable && !containsObstacle) {
                navigationNode.Free();
            }
        }
        IGridDistance distance = new ManhattanDistance();
        this.pathPlanningStrategy = new AStarGridPathPlanningStrategy(distance);
    }

    private void OnDrawGizmos() {
        if (this.navigationGrid != null) {
            this.navigationGrid.DrawWithGizmos();
        }
        if (this.path != null) {
            Gizmos.color = Color.blue;
            foreach (INavigationGridNode node in this.path) {
                Gizmos.DrawCube(node.WorldPosition + Vector3.up, navigationNodeSize * Vector3.one);
            }
        }
    }

    private INavigationGrid navigationGrid = null;

    private IGridPathPlanningStrategy pathPlanningStrategy = null;

    private List<INavigationGridNode> path = null;
}
