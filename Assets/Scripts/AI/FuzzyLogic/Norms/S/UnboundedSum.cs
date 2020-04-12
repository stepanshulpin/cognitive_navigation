namespace AI.FuzzyLogic {
    public class UnboundedSum : SNorm {
        public override double Compute(double left, double right) {
            return left + right;
        }
    }
}
