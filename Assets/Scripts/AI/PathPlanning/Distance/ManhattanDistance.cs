using System;
using UnityEngine;

namespace AI.PathPlanning {
    public class ManhattanDistance : IGridDistance {
        public ManhattanDistance() : this(Vector2Int.one) {
        }

        public ManhattanDistance(Vector2Int weight) {
            this.weight = weight;
        }

        public int Calculate(INavigationGridNode nodeA, INavigationGridNode nodeB) {
            return this.weight.x * Math.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x) +
                this.weight.y * Math.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);
        }

        private Vector2Int weight;
    }
}
