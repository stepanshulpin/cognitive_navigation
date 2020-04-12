using System;

namespace AI.FuzzyLogic {
    public class HamacherSum : SNorm {
        public override double Compute(double left, double right) {
            if (Math.Abs(left * right - 1.0f) < 1e-8) {
                return 1.0;
            } else {
                return (left + right - 2 * left * right) / (1.0 - left * right);
            }
        }
    }
}
