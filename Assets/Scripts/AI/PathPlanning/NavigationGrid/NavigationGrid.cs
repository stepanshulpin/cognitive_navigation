using System.Collections.Generic;
using UnityEngine;

namespace AI.PathPlanning {
    public abstract class NavigationGrid : INavigationGrid {
        public int NodesCount {
            get {
                return this.shape.x * this.shape.y;
            }
        }

        public Vector2Int Shape {
            get {
                return this.shape;
            }
        }

        public INavigationGridNode[,] Nodes {
            get {
                return this.grid;
            }
        }

        public NavigationGrid(Vector2Int shape) {
            this.shape = shape;
            this.grid = new NavigationGridNode[shape.x, shape.y];
        }

        public abstract INavigationGridNode GetNavigationNode(Vector3 worldPosition);

        public abstract void DrawWithGizmos();

        public List<INavigationGridNode> GetNavigationNodeNeighbours(INavigationGridNode navigationNode) {
            List<INavigationGridNode> neighbourNodes = new List<INavigationGridNode>();
            Vector2Int possibleNeighbourIndex = new Vector2Int();
            for (int x = -1; x <= 1; ++x) {
                for (int y = -1; y <= 1; ++y) {
                    if (x == 0 && y == 0) {
                        continue;
                    } else {
                        possibleNeighbourIndex.x = navigationNode.GridPosition.x + x;
                        possibleNeighbourIndex.y = navigationNode.GridPosition.y + y;
                        if (this.IsInRange(possibleNeighbourIndex.x, 0, this.shape.x - 1) &&
                            this.IsInRange(possibleNeighbourIndex.y, 0, this.shape.y - 1)) {
                            neighbourNodes.Add(this.grid[possibleNeighbourIndex.x, possibleNeighbourIndex.y]);
                        }
                    }
                }
            }
            return neighbourNodes;
        }

        protected INavigationGridNode[,] grid;

        protected Vector2Int shape;

        private bool IsInRange(int value, int minimum, int maximum) {
            return minimum <= value && value <= maximum;
        }
    }
}
