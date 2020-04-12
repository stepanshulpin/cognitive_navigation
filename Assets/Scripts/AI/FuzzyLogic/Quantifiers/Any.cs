namespace AI.FuzzyLogic {
    public class Any : IQuantifier {
        public string Name {
            get {
                return "any";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Any;
            }
        }

        public double Apply(double termMembership) {
            return 1.0;
        }
    }
}
