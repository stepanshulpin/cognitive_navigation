using System;

namespace AI.FuzzyLogic {
    public class Minimum : TNorm {
        public override double Compute(double left, double right) {
            return Math.Min(left, right);
        }
    }
}
