using System;

namespace AI.FuzzyLogic {
    public class BoundedDifference : TNorm {
        public override double Compute(double left, double right) {
            return Math.Max(0.0, left + right - 1.0);
        }
    }
}
