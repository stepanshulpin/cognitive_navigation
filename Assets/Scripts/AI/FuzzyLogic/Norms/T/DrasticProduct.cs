using System;

namespace AI.FuzzyLogic {
    public class DrasticProduct : TNorm {
        public override double Compute(double left, double right) {
            if (Math.Abs(Math.Max(left, right) - 1.0) < 1e-8) {
                return Math.Min(left, right);
            } else {
                return 0.0;
            }
        }
    }
}
