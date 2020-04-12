using System;

namespace AI.FuzzyLogic {
    public class DrasticSum : SNorm {
        public override double Compute(double left, double right) {
            if (Math.Abs(Math.Min(left, right)) < 1e-8) {
                return Math.Max(left, right);
            } else {
                return 1.0;
            }
        }
    }
}
