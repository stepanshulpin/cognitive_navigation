using System;

namespace AI.PathPlanning.AStar {
    public class AStarNode : IComparable<AStarNode> {
        public int GCost {
            get {
                return this.gCost;
            }

            set {
                this.gCost = value;
            }
        }

        public int HCost {
            get {
                return this.hCost;
            }

            set {
                this.hCost = value;
            }
        }

        public int FCost {
            get {
                return this.hCost + this.gCost;
            }
        }

        public AStarNode() {
            this.gCost = 0;
            this.hCost = 0;
        }

        public int CompareTo(AStarNode other) {
            int compare = this.FCost.CompareTo(other.FCost);
            if (compare == 0) {
                return this.HCost.CompareTo(other.HCost);
            }
            return compare;
        }

        private int gCost;

        private int hCost;
    }
}
