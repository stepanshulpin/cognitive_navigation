using System;

namespace AI.FuzzyLogic {
    public class BoundedSum : SNorm {
        public override double Compute(double left, double right) {
            return Math.Min(1.0, left + right);
        }
    }
}

