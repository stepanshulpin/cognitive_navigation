using System;

namespace AI.FuzzyLogic {
    public class NormalizedSum : SNorm {
        public override double Compute(double left, double right) {
            return (left + right) / Math.Max(1.0, left + right);
        }
    }
}
