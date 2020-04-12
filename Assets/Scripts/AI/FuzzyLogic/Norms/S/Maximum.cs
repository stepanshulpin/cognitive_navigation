using System;

namespace AI.FuzzyLogic {
    public class Maximum : SNorm {
        public override double Compute(double left, double right) {
            return Math.Max(left, right);
        }
    }
}
