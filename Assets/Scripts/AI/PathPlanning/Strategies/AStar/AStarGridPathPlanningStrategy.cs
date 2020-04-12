using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.PathPlanning.AStar {
    public class AStarGridPathPlanningStrategy : IGridPathPlanningStrategy {
        public AStarGridPathPlanningStrategy(IGridDistance distance) {
            this.distance = distance;
        }

        public List<INavigationGridNode> FindPath(INavigationGrid navigationGrid, INavigationGridNode startNode, INavigationGridNode targetNode) {
            ExtendedHeap<AStarNode, INavigationGridNode> openSet = new ExtendedHeap<AStarNode, INavigationGridNode>(navigationGrid.NodesCount);
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            HeapKeyValuePair<AStarNode, INavigationGridNode> currentNode;
            AStarNode currentAStarNode;
            INavigationGridNode currentGridNode;
            openSet.Push(new AStarNode(), startNode);
            HeapKeyValuePair<AStarNode, INavigationGridNode> closestNode = openSet.PeekKeyValuePair();
            closestNode.Key.HCost = this.distance.Calculate(startNode, targetNode);
            while (openSet.Count > 0) {
                currentNode = openSet.PopKeyValuePair();
                currentAStarNode = currentNode.Key;
                currentGridNode = currentNode.Value;
                closedSet.Add(currentGridNode.GridPosition);
                if (currentAStarNode.HCost < closestNode.Key.HCost) {
                    closestNode = currentNode;
                }
                if (currentGridNode.Equals(targetNode)) {
                    Debug.Log("Path found");
                    return this.RetracePath(navigationGrid, targetNode, cameFrom);
                }
                foreach (INavigationGridNode neighbour in navigationGrid.GetNavigationNodeNeighbours(currentGridNode)) {
                    if (neighbour.IsWalkable && !closedSet.Contains(neighbour.GridPosition)) {
                        int transitionCost = neighbour.MovementCost + currentAStarNode.GCost + this.distance.Calculate(currentGridNode, neighbour);
                        HeapKeyValuePair<AStarNode, INavigationGridNode> visitedNode;
                        int visitedNodeIndex;
                        if (openSet.Find(neighbour, out visitedNode, out visitedNodeIndex)) {
                            if (transitionCost < visitedNode.Key.GCost) {
                                visitedNode.Key.GCost = transitionCost;
                                visitedNode.Key.HCost = this.distance.Calculate(neighbour, targetNode);
                                cameFrom[neighbour.GridPosition] = currentGridNode.GridPosition;
                                openSet.KeyIncreased(visitedNodeIndex);
                            }
                        } else {
                            AStarNode key = new AStarNode();
                            key.GCost = transitionCost;
                            key.HCost = this.distance.Calculate(neighbour, targetNode);
                            cameFrom.Add(neighbour.GridPosition, currentGridNode.GridPosition);
                            openSet.Push(key, neighbour);
                        }
                    }
                }
            }
            Debug.Log("Target unreachable. Navigating to closest node " + closestNode.Value.GridPosition.ToString());
            return this.RetracePath(navigationGrid, closestNode.Value, cameFrom);
        }

        private List<INavigationGridNode> RetracePath(INavigationGrid navigationGrid, INavigationGridNode finishNode, Dictionary<Vector2Int, Vector2Int> cameFrom) {
            List<INavigationGridNode> path = new List<INavigationGridNode>();
            INavigationGridNode currentNode = finishNode;
            Vector2Int parentNodePosition;
            while (cameFrom.TryGetValue(currentNode.GridPosition, out parentNodePosition)) {
                path.Add(currentNode);
                currentNode = navigationGrid.Nodes[parentNodePosition.x, parentNodePosition.y];
            }
            path.Reverse();
            return path;
        }

        private IGridDistance distance;
    }
}
