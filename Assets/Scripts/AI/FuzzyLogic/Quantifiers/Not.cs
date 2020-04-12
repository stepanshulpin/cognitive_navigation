namespace AI.FuzzyLogic {
    public class Not : IQuantifier {
        public string Name {
            get {
                return "not";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Not;
            }
        }

        public double Apply(double membership) {
            return 1.0 - membership;
        }
    }
}
