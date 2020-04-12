namespace AI.FuzzyLogic {
    public class EinsteinSum : SNorm {
        public override double Compute(double left, double right) {
            return (left + right) / (1.0 + left * right);
        }
    }
}
