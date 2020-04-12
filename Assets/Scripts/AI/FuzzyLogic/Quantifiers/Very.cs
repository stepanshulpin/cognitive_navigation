using System;

namespace AI.FuzzyLogic {
    public class Very : IQuantifier {
        public string Name {
            get {
                return "very";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Very;
            }
        }

        public double Apply(double membership) {
            return Math.Pow(membership, 2);
        }
    }
}
