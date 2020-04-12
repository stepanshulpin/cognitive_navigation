using System;

namespace AI.FuzzyLogic {
    public class NilpotentMinimum : TNorm {
        public override double Compute(double left, double right) {
            if (left + right > 1.0) {
                return Math.Min(left, right);
            } else {
                return 0.0;
            }
        }
    }
}
