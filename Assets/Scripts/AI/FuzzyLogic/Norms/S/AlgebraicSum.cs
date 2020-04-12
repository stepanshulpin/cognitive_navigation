namespace AI.FuzzyLogic {
    public class AlgebraicSum : SNorm {
        public override double Compute(double left, double right) {
            return left + right - left * right;
        }
    }
}
