using UnityEngine;

namespace AI.PathPlanning {
    public interface INavigationNode {
        Vector3 WorldPosition {
            get;
        }

        int Index {
            get;
        }

        bool IsWalkable {
            get;
            set;
        }

        int MovementCost {
            get;
            set;
        }
    }
}
