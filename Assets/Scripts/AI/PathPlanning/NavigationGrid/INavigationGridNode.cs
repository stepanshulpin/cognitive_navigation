using System;
using UnityEngine;

namespace AI.PathPlanning {
    public interface INavigationGridNode : IEquatable<INavigationGridNode> {
        Vector3 WorldPosition {
            get;
        }

        Vector2Int GridPosition {
            get;
        }

        bool IsWalkable {
            get;
        }

        int MovementCost {
            get;
            set;
        }

        void Free();

        void Occupy();
    }
}
