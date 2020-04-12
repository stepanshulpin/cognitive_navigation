using System;
using UnityEngine;

namespace AI.PathPlanning {
    public class NavigationGridNode : INavigationGridNode {
        public Vector2Int GridPosition {
            get {
                return this.gridPosition;
            }
        }

        public bool IsWalkable {
            get {
                return this.isWalkable;
            }
        }

        public int MovementCost {
            get {
                return this.movementCost;
            }

            set {
                this.movementCost = value;
            }
        }

        public Vector3 WorldPosition {
            get {
                return this.worldPosition;
            }
        }

        public NavigationGridNode(Vector2Int gridPosition, Vector3 worldPosition) {
            this.gridPosition = gridPosition;
            this.worldPosition = worldPosition;
            this.isWalkable = true;
            this.movementCost = 0;
        }

        public NavigationGridNode(Vector2Int gridPosition, Vector3 worldPosition, bool isWalkable) {
            this.gridPosition = gridPosition;
            this.worldPosition = worldPosition;
            this.isWalkable = isWalkable;
            this.movementCost = 0;
        }

        public NavigationGridNode(Vector2Int gridPosition, Vector3 worldPosition, bool isWalkable, int movementCost) {
            this.gridPosition = gridPosition;
            this.worldPosition = worldPosition;
            this.isWalkable = isWalkable;
            this.movementCost = movementCost;
        }

        public void Free() {
            this.isWalkable = true;
        }

        public void Occupy() {
            this.isWalkable = false;
        }

        public bool Equals(INavigationGridNode other) {
            return this.GridPosition == other.GridPosition;
        }

        private Vector2Int gridPosition;

        private Vector3 worldPosition;

        private bool isWalkable;

        private int movementCost;
    }
}
