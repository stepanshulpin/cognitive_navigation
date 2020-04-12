using System;

namespace AI.FuzzyLogic {
    public class NilpotentMaximum : SNorm {
        public override double Compute(double left, double right) {
            if (left + right < 1.0) {
                return Math.Max(left, right);
            } else {
                return 1.0;
            }
        }
    }
}
