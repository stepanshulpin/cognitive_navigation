namespace AI.FuzzyLogic {
    public class EinsteinProduct : TNorm {
        public override double Compute(double left, double right) {
            return (left * right) / (2.0 - (left + right - left * right));
        }
    }
}
