using System;

namespace AI.FuzzyLogic {
    public class HamacherProduct : TNorm {
        public override double Compute(double left, double right) {
            if (Math.Abs(left + right) < 1e-8) {
                return 0.0;
            } else {
                return (left * right) / (left + right - left * right);
            }
        }
    }
}

